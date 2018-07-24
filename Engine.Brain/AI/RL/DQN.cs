using Engine.Brain.Entity;
using Engine.Brain.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using TensorFlow;

namespace Engine.Brain.AI.RL
{
    public delegate void UpdateLearningLossHandler(float loss, float totalReward, float accuracy);

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

        //memory容量
        readonly int _memoryCapacity = 800;
        //拷贝net参数
        readonly int _everycopy = 128;
        //学习轮次
        readonly int _epoches = 200;
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
            int count = _memoryList.Count;
            if (count >= _memoryCapacity)
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
        public (int action, float q) ChooseAction(float[] state, DNet net = null)
        {
            //默认使用actorNet
            net = net == null ? _actorNet : net;
            float[] input = new float[_featuresNumber + _actionsNumber];
            Array.ConstrainedCopy(state, 0, input, 0, _featuresNumber);
            float[] arrays = new float[_actionsNumber];
            for (int i = 0; i < _actionsNumber; i++)
            {
                float[] action = NP.ToOneHot(i, _actionsNumber);
                Array.ConstrainedCopy(action, 0, input, _featuresNumber, _actionsNumber);
                TFTensor input_tensor = TFTensor.FromBuffer(new TFShape(1, _featuresNumber + _actionsNumber), input, 0, input.Length);
                float[,] predicts = (float[,])net.Predict(input_tensor);
                arrays[i] = predicts[0, 0];
            }
            return (NP.Argmax(arrays), NP.Max(arrays));
        }
        //随机抽取样本
        private List<Memory> CreateRawDataBatch(int batchSize)
        {
            int count = _memoryList.Count;
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
            //renturn tenor 
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
                int action; float q;
                (action, q) = ChooseAction(state);
                return (action, q);
            }
        }
        /// <summary>
        /// 经验回放
        /// </summary>
        /// <returns></returns>
        public (float loss, float accuracy) Replay()
        {
            if (_memoryList.Count < _batchSize)
                return (-10000.0f, 0f);
            //batch of memory
            List<Memory> rawBatchList = CreateRawDataBatch(_batchSize);
            //create input tensor
            TFTensor input_feature_tensor, input_qvalue_tensor;
            (input_feature_tensor, input_qvalue_tensor) = MakeBatch(rawBatchList);
            //loss计算
            float loss = _criticNet.Train(input_feature_tensor, input_qvalue_tensor);
            //}{debug
            var prediction = _criticNet.Predict(input_feature_tensor);
            //使用函数判断精度
            float[,] predict = (float[,])_actorNet.Predict(input_feature_tensor);
            List<Memory> accuracyList = _memoryList.Where(p => p.Reward == 1.0f).ToList();
            float accuracy = Accuracy(accuracyList);
            return (loss, accuracy);
        }
        /// <summary>
        /// 计算分类精度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private float Accuracy(List<Memory> list)
        {
            int count = list.Count;
            float[] labels = new float[count];
            float[] actions = new float[count];
            for (int i = 0; i < list.Count; i++)
            {
                actions[i] = ChooseAction(list[i].STATE).action;
                labels[i] = NP.Argmax(list[i].Action);
            }
            var accuracy = NP.CalcuteAccuracy(labels, actions);
            return accuracy;
        }
        /// <summary>
        /// 预存储记忆
        /// </summary>
        /// <param name="rememberSize"></param>
        public void PreRemember(int rememberSize = 256)
        {
            float[] state = _env.Reset();
            for (int i = 0; i < rememberSize; i++)
            {
                int action = _env.RandomAction();
                float[] nextState;
                float reward;
                (nextState, reward) = _env.Step(action);
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
            float[] state = _env.Reset();
            for (int e = 0; e < _epoches; e++)
            {
                float totalRewards = 0;
                for (int step = 0; step < _forward; step++)
                {
                    float loss, accuracy, q, reward;
                    int action;
                    //对状态进行epsilon_greedy选择
                    (action, q) = EpsilonGreedy(step, state);
                    //play
                    float[] nextState;
                    (nextState, reward) = _env.Step(action);
                    //加入要经验记忆中
                    Remember(state, NP.ToOneHot(action, _env.ActionNum), q, reward, nextState);
                    //
                    (loss, accuracy) = Replay();
                    state = nextState;
                    totalRewards += reward;
                    //
                    if (step % _everycopy == 0)
                        _actorNet.Accept(_criticNet);
                    OnLearningLossEventHandler?.Invoke(loss, totalRewards, accuracy);
                }
            }
        }

    }
}
