using Engine.Brain.Entity;
using Engine.Brain.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TensorFlow;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loss">loss value</param>
    /// <param name="totalReward">rewards</param>
    /// <param name="accuracy">train accuracy</param>
    /// <param name="epochesTime"></param>
    public delegate void UpdateLearningLossHandler(float loss, float totalReward, float accuracy,float progress,string epochesTime);

    public class Memory
    {
        public float[] STATE { get; set; }
        public float[] STATE_ { get; set; }
        public float[] Action { get; set; }
        public float Q { get; set; }
        public float Reward { get; set; }
    }

    /// <summary>
    /// DQN学习机
    /// </summary>
    public class DQN
    {
        /// <summary>
        /// 回调区
        /// </summary>
        public event UpdateLearningLossHandler OnLearningLossEventHandler;
        /// <summary>
        /// 记忆区
        /// </summary>
        private List<Memory> _memoryList = new List<Memory>();
        /// <summary>
        /// 决策网络
        /// </summary>
        private DNet _actorNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private DNet _criticNet;
        /// <summary>
        /// 观察环境
        /// </summary>
        private IDEnv _env;
        //
        readonly int _memoryCapacity = 500;
        //拷贝net参数
        readonly int _everycopy = 128;
        //学习轮次
        readonly int _epoches = 2000;
        //一次学习样本数
        readonly int _batchSize = 32;
        //一轮学习次数
        readonly int _forward = 512;
        //q值积累权重
        readonly float _alpah = 0.5f;
        //q值印象权重
        readonly float _gamma = 0;
        //输入feature长度
        readonly int _featuresNumber;
        //输入action长度
        readonly int _actionsNumber;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public DQN(IDEnv env)
        {
            //environment
            _env = env;
            //
            _actionsNumber = _env.ActionNum;
            //
            _featuresNumber = _env.FeatureNum;
            //决策
            _actorNet = new DNet(_featuresNumber, _actionsNumber);
            //训练
            _criticNet = new DNet(_featuresNumber, _actionsNumber);
        }
        /// <summary>
        /// 控制记忆容量
        /// </summary>
        public void Remember(float[] state, float[] action, float q, float reward, float[] state_)
        {
            //容量上限,取消容量上限限制
            if (_memoryList.Count >= _memoryCapacity)
                _memoryList.RandomRemove();
            //预学习N步，记录在memory里
            _memoryList.Add(new Memory()
            {
                STATE = state,
                STATE_ = state_,
                Action = action,
                Q = q,
                Reward = reward
            });
        }
        /// <summary>
        /// 输出每一个 state 对应的 action 值
        /// </summary>
        /// <returns></returns>
        public (int action, float q) ChooseAction(float[] state)
        {
            //默认使用actorNet
            int offset = 0;
            float[] input = new float[(_featuresNumber + _actionsNumber)*_actionsNumber];
            for(int i = 0; i < _actionsNumber; i++)
            {
                Array.ConstrainedCopy(state, 0, input, offset, _featuresNumber);
                offset += _featuresNumber;
                Array.ConstrainedCopy(NP.ToOneHot(i, _actionsNumber), 0, input, offset, _actionsNumber);
                offset += _actionsNumber;
            }
            TFTensor input_tensor = TFTensor.FromBuffer(new TFShape(_actionsNumber, _featuresNumber + _actionsNumber), input, 0, input.Length);
            float[,] predicts = (float[,])_actorNet.Predict(input_tensor);
            float[] array = NP.Pad(predicts);
            input_tensor.Dispose();
            return (NP.Argmax(array), NP.Max(array));
        }
        /// <summary>
        /// 随机抽取样本
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        private List<Memory> CreateRawDataBatch(int batchSize)
        {
            List<Memory> list = new List<Memory>();
            for (int i = 0; i < batchSize; i++)
                list.Add(_memoryList.RandomTake());
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input_features_tensor"></param>
        /// <param name="input_qvalue_tensor"></param>
        /// <param name="batchSize"></param>
        private (TFTensor input_features_tensor, TFTensor input_qvalue_tensor) MakeBatch(List<Memory> list)
        {
            //batchSize个样本
            int batchSize = list.Count;
            //
            TFTensor input_features_tensor, input_qvalue_tensor;
            //feature input
            float[] input_features = new float[batchSize * (_featuresNumber + _actionsNumber)];
            //qvalue input
            float[] input_qValue = new float[batchSize];
            //写入偏移位
            int offset = 0;
            //q value
            float q = 0f;
            //
            for (int i = 0; i < batchSize; i++)
            {
                //input features assign
                Array.ConstrainedCopy(list[i].STATE, 0, input_features, offset, _featuresNumber);
                offset += _featuresNumber;
                //input actions assign
                Array.ConstrainedCopy(list[i].Action, 0, input_features, offset, _actionsNumber);
                offset += _actionsNumber;
                //input qvalue assign
                input_qValue[i] = (1 - _alpah) * list[i].Reward + _alpah * (list[i].Q + _gamma * q);
            }
            input_features_tensor = TFTensor.FromBuffer(new TFShape(batchSize, _featuresNumber + _actionsNumber), input_features, 0, input_features.Length);
            input_qvalue_tensor = TFTensor.FromBuffer(new TFShape(batchSize, 1), input_qValue, 0, input_qValue.Length);
            return (input_features_tensor, input_qvalue_tensor);
        }
        /// <summary>
        /// 探索算法
        /// </summary>
        /// <param name="step"></param>
        /// <param name="ep_min"></param>
        /// <param name="ep_max"></param>
        /// <param name="ep_decay"></param>
        /// <param name="eps_total"></param>
        /// <returns></returns>
        public float EpsilonCalcute(int step, float ep_min = 0.01f, float ep_max = 1f, float ep_decay = 0.0001f, int eps_total = 1000)
        {
            return Math.Max(ep_min, ep_max - (ep_max - ep_min) * step / eps_total);
        }
        /// <summary>
        /// 获取当前actor下的action和reward
        /// </summary>
        /// <param name="step"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public (int action, float q) EpsilonGreedy(int step, float[] state)
        {
            var epsion = EpsilonCalcute(step);
            if ((float)new Random().NextDouble() < epsion)
                return (_env.RandomAction(), 0);
            else
            {
                var (action, q) = ChooseAction(state);
                return (action, q);
            }
        }
        /// <summary>
        /// 经验回放
        /// </summary>
        /// <returns></returns>
        public (float loss,TimeSpan span) Replay()
        {
            DateTime now = DateTime.Now;
            //batch of memory
            List<Memory> rawBatchList = CreateRawDataBatch(_batchSize);
            var (input_feature_tensor, input_qvalue_tensor) = MakeBatch(rawBatchList);
            //loss计算
            float loss = _criticNet.Train(input_feature_tensor, input_qvalue_tensor);
            return (loss,DateTime.Now-now);
        }
        /// <summary>
        /// 计算分类精度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private float Accuracy()
        {
            const int batchSize = 128;
            var (states, rawLabels) = _env.RandomEval(batchSize);
            float[] actions = new float[batchSize];
            float[] labels = new float[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                actions[i] = ChooseAction(states[i]).action;
                labels[i] = rawLabels[i];
            }
            var accuracy = NP.CalcuteAccuracy(labels, actions);
            return accuracy;
        }
        /// <summary>
        /// 预存储记忆
        /// </summary>
        /// <param name="rememberSize"></param>
        public void PreRemember(int rememberSize)
        {
            float[] state = _env.Reset();
            for (int i = 0; i < rememberSize; i++)
            {
                int action = _env.RandomAction();
                var (nextState, reward) = _env.Step(action);
                Remember(state, NP.ToOneHot(action, _env.ActionNum), 0, reward, nextState);
                state = nextState;
            }
        }
        /// <summary>
        /// 批次训练
        /// </summary>
        /// <param name="batchSize"></param>
        public void Learn()
        {
            PreRemember(_memoryCapacity);
            float[] state = _env.Step(-1).state;
            for (int e = 0; e <= _epoches; e++)
            {
                DateTime now = DateTime.Now;
                float loss = 0, accuracy = 0, totalRewards = 0;
                for (int step = 0; step <=_forward; step++)
                {
                    TimeSpan span;
                    //对状态进行epsilon_greedy选择
                    var (action, q) = EpsilonGreedy(step, state);
                    //play
                    var (nextState, reward) = _env.Step(action);
                    Remember(state, NP.ToOneHot(action, _env.ActionNum), q, reward, nextState);
                    //train
                    (loss, span)= Replay();
                    //
                    state = nextState;
                    totalRewards += reward;
                    //
                    if (step % _everycopy == 0)
                        _actorNet.Accept(_criticNet);
                }
                accuracy = Accuracy();
                OnLearningLossEventHandler?.Invoke(loss, totalRewards, accuracy, (float)e / _epoches, (DateTime.Now - now).TotalSeconds.ToString());
            }
        }

    }
}
