using System.Collections.Generic;
using TensorFlow;

namespace Engine.Brain.AI.RL
{


    public class Memory
    {
        public double[] STATE { get; set; }
        public double[] STATE_ { get; set; }
        public int Action { get; set; }
        public double Reward { get; set; }
    }

    /// <summary>
    /// DQN学习机
    /// </summary>
    public class DQN
    {
        /// <summary>
        /// 记忆区
        /// </summary>
        private List<Memory> memory = new List<Memory>();
        /// <summary>
        /// 记忆库容量
        /// </summary>
        readonly int memoryCapacity = 300;
        /// <summary>
        /// 记忆计数
        /// </summary>
        private int memoryCount = 0;
        /// <summary>
        /// 应用学习结果，观察步骤间隔
        /// </summary>
        readonly int learnInterval = 300;
        /// <summary>
        /// 评估网络
        /// </summary>
        private DNet evalNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private DNet targetNet;
        /// <summary>
        /// 
        /// </summary>
        public DQN(int inputWidth, int inputHeight, int actions_num)
        {
            var n_features = inputHeight * inputWidth;
            var n_actions = actions_num;
            evalNet = new DNet(n_features, n_actions);
            targetNet = new DNet(n_features, n_actions);
            //1.增加记忆库记忆
            //2.训练
        }
        /// <summary>
        /// 
        /// </summary>
        public void Remember(double[] s,int a,double r,double[] s_)
        {
            //预学习N步，记录在memory里
            memory.Add(new Memory()
            {
                STATE = s,
                STATE_ = s_,
                Action = a,
                Reward = r
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ChooseAction()
        {
            //使用eval net计算action结果
            return 0;
        }

        private List<double[]> RandomMakeBatch()
        {
            return null;
        }

        public void Learn()
        {
            const int batchSize = 10;


            if (memoryCount > memoryCapacity)
            {

            }

            //1.从memeory生成随机批次的训练数据
            //2.使用Eval网络计算 q_eval,q_next
            //3.q_target
            //4.从memory得到reward
        }





    }
}
