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
        public int Action { get; set; }
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
        private List<Memory> _memory = new List<Memory>();
        /// <summary>
        /// 记忆库容量
        /// </summary>
        readonly int _memoryCapacity = 300;
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
        /// 
        /// </summary>
        public DQN(int inputWidth, int inputHeight, int actions_num)
        {
            var n_features = inputHeight * inputWidth;
            var n_actions = actions_num;
            _evalNet = new DNet(n_features, n_actions);
            _targetNet = new DNet(n_features, n_actions);
            //1.增加记忆库记忆
            //2.训练
        }
        /// <summary>
        /// 
        /// </summary>
        public void Remember(float[] s,int a, float r, float[] s_)
        {
            //预学习N步，记录在memory里
            _memory.Add(new Memory()
            {
                STATE = s,
                STATE_ = s_,
                Action = a,
                Reward = r
            });
            //记忆计数+1
            _memoryCount++;
        }
        /// <summary>
        /// 输出每一个 state 对应的q值
        /// </summary>
        /// <returns></returns>
        public float ChooseAction(float[] state)
        {
            TFShape shape = new TFShape(1, state.Length);
            TFTensor tensor = TFTensor.FromBuffer(shape, state, 0, state.Length);
            float[] predicts =(float[]) _evalNet.Predict(tensor);
            //模拟argmax
            float predict =  NP.Argmax(predicts);
            //使用eval net计算action结果
            return predict;
        }

        private List<double[]> RandomMakeBatch()
        {
            return null;
        }

        public void Learn()
        {
            //样本格式
            const int batchSize = 10;
            //特征个数
            const int featureCount = 64;
            //构建memory
            if (_memoryCount < _memoryCapacity)
            {
                var inputs = NP.CreateInputs(oneDimensionCount:featureCount,batchSize:batchSize);
                var labels = NP.CreateLabels();
                var tensor = NP.CreateTensorWithRandomFloat(new TFShape(10, 64));
                var q_eval = _evalNet.Predict(tensor);                                                                            
            }
            //1.从memeory生成随机批次的训练数据
            //2.使用Eval网络计算 q_eval,q_next
            //q_next is the target NeuralNetwork
            var q_next = _targetNet.Predict(null);
            //根据输入的batchsize，构建二维表，用于计算cost
            var q_real = _targetNet.Predict(null); 
            //4.从memory得到reward

        }





    }
}
