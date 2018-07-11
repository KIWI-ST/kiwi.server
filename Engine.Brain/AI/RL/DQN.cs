using TensorFlow;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// DQN学习机
    /// </summary>
    public class DQN
    {
        /// <summary>
        /// 记忆库容量
        /// </summary>
        readonly int memoryCapacity = 300;

        /// <summary>
        /// 应用学习结果，观察步骤间隔
        /// </summary>
        readonly int learnInterval = 300;

        /// <summary>
        /// 
        /// </summary>
        public DQN(int inputWidth, int inputHeight, int actions_num)
        {
            var n_features = inputHeight * inputWidth;
            var n_actions = actions_num;
            var evalNet = new DNet(n_features, n_actions);
            var targetNet = new DNet(n_features, n_actions);
            //1.增加记忆库记忆
            //2.训练
        }
        /// <summary>
        /// 
        /// </summary>
        public void Learn()
        {
            //预学习N步，记录在memory里
        }

    }
}
