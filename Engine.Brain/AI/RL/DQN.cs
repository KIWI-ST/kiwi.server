using Engine.Brain.AI.RL.Env;
using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using TensorFlow;

namespace Engine.Brain.AI.RL
{


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
        /// 
        /// </summary>
        private double _epsilon = 0.9;
        /// <summary>
        /// 记忆区
        /// </summary>
        private List<Memory> _memoryList = new List<Memory>();

        /// <summary>
        /// 记忆计数
        /// </summary>
        private int _memoryCount = 0;
        /// <summary>
        /// 一轮学习长度
        /// </summary>
        readonly int Epoches = 2000;
        /// <summary>
        /// 应用学习结果，观察步骤间隔
        /// </summary>
        readonly int EveryCopyStep = 128;

        readonly int BatchSize = 64;

        readonly int Forward = 512;

        float alpah = 0.5f;

        float gamma = 0;

        /// <summary>
        /// 评估网络
        /// </summary>
        private DNet _evalNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private DNet _targetNet;

        public int MemoryCapacity { get; } = 3000;

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
            _env = env;
            _actionsNumber = _env.ActionNum;
            _featuresNumber = _env.FeatureNum;
            _evalNet = new DNet(_featuresNumber, _actionsNumber);
            _targetNet = new DNet(_featuresNumber, _actionsNumber);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Remember(float[] state, float[] action,  float q, float reward, float[] state_)
        {
            //预学习N步，记录在memory里
            _memoryList.Add(new Memory()
            {
                STATE = state,
                STATE_ = state_,
                Action = action,
                Q = q,
                Reward = reward
            });
            //记忆计数+1
            _memoryCount++;
        }
        /// <summary>
        /// 输出每一个 state 对应的q值
        /// </summary>
        /// <returns></returns>
        public int[] ChooseAction(float[] state, float[] action)
        {
            TFTensor feature_tensor = TFTensor.FromBuffer(new TFShape(1, state.Length), state, 0, state.Length);
            TFTensor action_tensor = TFTensor.FromBuffer(new TFShape(1, action.Length), action, 0, action.Length);
            float[,] predicts = (float[,])_evalNet.Predict(feature_tensor);
            //模拟argmax
            int[] predict = NP.Argmax(predicts);
            //使用eval net计算action结果
            return predict;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input_features_tensor"></param>
        /// <param name="input_qvalue_tensor"></param>
        /// <param name="batchSize"></param>
        private void MakeBatch(out TFTensor input_features_tensor, out TFTensor input_qvalue_tensor, int batchSize)
        {
            //随机从记忆里抽取batchSize个样本
            var list = new List<Memory>();
            for(int i = 0; i < batchSize; i++)
            {
                var index = new Random().Next(MemoryCapacity);
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
                input_qValue[i] = (1-alpah)*list[i].Reward + alpah*(list[i].Q + gamma * q);
            }
            input_features_tensor = TFTensor.FromBuffer(new TFShape(batchSize, _featuresNumber + _actionsNumber), input_features, 0, input_features.Length);
            input_qvalue_tensor = TFTensor.FromBuffer(new TFShape(batchSize, 1), input_qValue, 0, input_qValue.Length);
        }

        public float EpsilonCalcute(int step, float ep_min = 0.01f,float ep_max = 1f,float ep_decay= 0.0001f,int eps_total = 1000)
        {
            return Math.Max(ep_min, ep_max - (ep_max - ep_min) * step / eps_total);
        }

        public (int action,float q) EpsilonGreedy(int step,float[] state)
        {
            var epsion = EpsilonCalcute(step);
            if((float)new Random().NextDouble()<epsion)
                return (_env.RandomAction(),0);
            else
            {
                int count = _featuresNumber + _actionsNumber;
                float[] input_feature = new float[count];
                int offset = 0;
                Array.ConstrainedCopy(state, 0, input_feature, offset, _featuresNumber);
                offset += _featuresNumber;
                Array.ConstrainedCopy(_env.DummyActions, 0, input_feature, offset, _env.DummyActions.Length);
                TFTensor feature_tensor = TFTensor.FromBuffer(new TFShape(1, count), input_feature, 0, count);
                float[,] predicts = (float[,])_evalNet.Predict(feature_tensor);
                //模拟argmax
                int[] predict = NP.Argmax(predicts);
                //使用eval net计算action结果
                float[] qs = NP.Max(predicts);
                return (predict[0], qs[0]);
            }
        }

        /// <summary>
        /// 经验回放
        /// </summary>
        /// <returns></returns>
        public float Replay()
        {
            return 0.0f;
        }

        /// <summary>
        /// 批次训练
        /// </summary>
        /// <param name="batchSize"></param>
        public void Learn()
        {
            float[] state = _env.Reset();
            for(int e=0;e< Epoches; e++)
            {
                float totalRewards = 0;
                for(int step = 0; step < Forward; step++)
                {
                    float loss;
                    int action;float q, eps;
                    //对状态进行epsilon_greedy选择
                    (action, q) = EpsilonGreedy(step, state);
                    eps = EpsilonCalcute(e);
                    //play
                    float[] nextState;float reward;
                    (nextState, reward) = _env.Step(action);
                    //加入要经验记忆中
                    Remember(state, NP.ToOneHot(action,_env.ActionNum), q, reward, nextState);
                    //
                    loss = Replay();
                    totalRewards += reward;
                    state = nextState;
                }
            }
        }

    }
}
