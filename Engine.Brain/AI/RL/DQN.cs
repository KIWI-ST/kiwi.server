using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
using TensorFlow;

namespace Engine.Brain.AI.RL
{


    public class Memory
    {
        public float[] STATE { get; set; }
        public float[] STATE_ { get; set; }
        public float[] Action { get; set; }
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
        /// 应用学习结果，观察步骤间隔
        /// </summary>
        readonly int _learnInterval = 300;
        /// <summary>
        /// 评估网络
        /// </summary>
        private DNet _evalNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private DNet _targetNet;
        /// <summary>
        /// 环境地质
        /// </summary>
        private DEnv _env;

        public int MemoryCapacity { get; } = 200;

        int _featuresNumber;

        int _actionsNumber;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features_num">环境要素个数</param>
        /// <param name="actions_num">操作枚举</param>
        public DQN(int features_num, int actions_num, DEnv env)
        {
            _actionsNumber = actions_num;
            _featuresNumber = features_num;
            _evalNet = new DNet(_featuresNumber, _actionsNumber);
            _targetNet = new DNet(_featuresNumber, _actionsNumber);
            _env = env;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Remember(float[] state, float[] action, float reward, float[] state_)
        {
            //预学习N步，记录在memory里
            _memoryList.Add(new Memory()
            {
                STATE = state,
                STATE_ = state_,
                Action = action,
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
            var index = new Random().Next(MemoryCapacity - batchSize);
            var list = _memoryList.GetRange(index, batchSize);
            //feature input
            float[] input_features = new float[batchSize * (_featuresNumber + _actionsNumber)];
            //qvalue input
            float[] input_qValue = new float[batchSize];
            //写入偏移位
            int offset = 0;
            for (int i = 0; i < batchSize; i++)
            {
                //input features assign
                Array.ConstrainedCopy(list[i].STATE, 0, input_features, offset, _featuresNumber);
                offset += _featuresNumber;
                //input actions assign
                Array.ConstrainedCopy(list[i].Action, 0, input_features, offset, _actionsNumber);
                offset += _actionsNumber;
                //input qvalue assign
                input_qValue[i] = list[i].Reward;
            }
            input_features_tensor = TFTensor.FromBuffer(new TFShape(batchSize, _featuresNumber + _actionsNumber), input_features, 0, input_features.Length);
            input_qvalue_tensor = TFTensor.FromBuffer(new TFShape(batchSize, 1), input_qValue, 0, input_qValue.Length);
        }
        /// <summary>
        /// 批次训练
        /// </summary>
        /// <param name="batchSize"></param>
        public void Learn(int batchSize = 5)
        {
            //1.从memory里获取batchSize个训练样本
            TFTensor input_features_tensor, input_qvalue_tensor;
            //2.训练evalNet
            //3.查看计数，超过_learnInterval，则同步evalNet和targetNet
            MakeBatch(out input_features_tensor, out input_qvalue_tensor, batchSize);
            _evalNet.Train(input_features_tensor, input_qvalue_tensor);
            var s = _evalNet.History;
            MakeBatch(out input_features_tensor, out input_qvalue_tensor, batchSize);
            var s23 =_evalNet.Predict(input_features_tensor);
        }

    }
}
