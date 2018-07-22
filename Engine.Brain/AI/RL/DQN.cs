using Engine.Brain.AI.RL.Env;
using Engine.Brain.Entity;
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
        /// 一轮学习长度
        /// </summary>
        readonly int Epoches = 100;
        /// <summary>
        /// 应用学习结果，观察步骤间隔
        /// </summary>
        readonly int EveryCopyStep = 128;

        readonly int batchSize = 64;
        readonly int forward = 512;
        readonly float alpah = 0.5f;
        readonly float gamma = 0;

        /// <summary>
        /// 评估网络
        /// </summary>
        private DNet _actorNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private DNet _criticNet;

        int _featuresNumber;

        int _actionsNumber;

        IDEnv _env;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features_num">环境要素个数</param>
        /// <param name="actions_num">操作枚举</param>
        public DQN(IDEnv env)
        {
            //
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
        /// 
        /// </summary>
        public void Remember(float[] state, float[] action, float q, float reward, float[] state_)
        {
            int count = _memoryList.Count;
            if ( count > 500)
            {
                int randomIndex = new Random().Next(count);
                _memoryList.RemoveAt(randomIndex);
            }
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
        public (int action,float q) ChooseAction(float[] state)
        {
            float[] input = new float[_featuresNumber + _actionsNumber];
            Array.ConstrainedCopy(state, 0, input, 0, _featuresNumber);
            float maxReward = -10000;
            int actionIndex = -1;
            for (int i = 1; i < _actionsNumber; i++)
            {
                float[] action = NP.ToOneHot(i, _actionsNumber);
                Array.ConstrainedCopy(action, 0, input, _featuresNumber, _actionsNumber);
                TFTensor input_tensor = TFTensor.FromBuffer(new TFShape(1, _featuresNumber + _actionsNumber), input, 0, input.Length);
                float[,] predicts = (float[,])_actorNet.Predict(input_tensor);
                if (maxReward < predicts[0, 0])
                {
                    maxReward = predicts[0, 0];
                    actionIndex = i;
                }
            }
            return (actionIndex,maxReward);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input_features_tensor"></param>
        /// <param name="input_qvalue_tensor"></param>
        /// <param name="batchSize"></param>
        private (TFTensor input_features_tensor, TFTensor input_qvalue_tensor) MakeBatch(int batchSize)
        {
            TFTensor input_features_tensor, input_qvalue_tensor;
            //随机从记忆里抽取batchSize个样本
            var list = new List<Memory>();
            int count = _memoryList.Count;
            for (int i = 0; i < batchSize; i++)
            {
                var index = new Random().Next(count);
                list.Add(_memoryList[index]);
            }
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
                input_qValue[i] = (1 - alpah) * list[i].Reward + alpah * (list[i].Q + gamma * q);
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
                int action;float q;
                (action, q) = ChooseAction(state);
                return (action,q);
            }
        }

        /// <summary>
        /// 经验回放
        /// </summary>
        /// <returns></returns>
        public (float loss, float accuracy) Replay()
        {
            if (_memoryList.Count < batchSize)
                return (-10000.0f, 0f);
            TFTensor input_feature_tensor, input_qvalue_tensor;
            (input_feature_tensor, input_qvalue_tensor) = MakeBatch(batchSize);
            //loss计算
            float loss = _criticNet.Train(input_feature_tensor, input_qvalue_tensor);
            //使用函数判断精度
            float[,] predict = (float[,])_actorNet.Predict(input_feature_tensor);
            float accuracy = NP.CalcuteAccuracy(predict, (float[,])input_qvalue_tensor.GetValue());
            return (loss, accuracy);
        }
        /// <summary>
        /// 批次训练
        /// </summary>
        /// <param name="batchSize"></param>
        public void Learn()
        {
            float[] state = _env.Reset();
            for (int e = 0; e < Epoches; e++)
            {
                float totalRewards = 0;
                for (int step = 0; step < forward; step++)
                {
                    float loss, accuracy,q;
                    int action; 
                    //对状态进行epsilon_greedy选择
                    (action, q) = EpsilonGreedy(step, state);
                    //play
                    float[] nextState; float reward;
                    (nextState, reward) = _env.Step(action);
                    //加入要经验记忆中
                    Remember(state, NP.ToOneHot(action, _env.ActionNum), q, reward, nextState);
                    (loss, accuracy) = Replay();
                    totalRewards += reward;
                    state = nextState;
                    //拷贝模型参数
                    if (step % EveryCopyStep == 0)
                        _actorNet.Accept(_criticNet);
                    OnLearningLossEventHandler?.Invoke(loss, totalRewards, accuracy);
                }
            }
        }

    }
}
