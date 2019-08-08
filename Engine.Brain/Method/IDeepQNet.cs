using Engine.Brain.Method.DeepQNet;

namespace Engine.Brain.Method
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loss">loss value</param>
    /// <param name="totalReward">rewards</param>
    /// <param name="accuracy">train accuracy</param>
    /// <param name="epochesTime"></param>
    public delegate void UpdateLearningLossHandler(double loss, double totalReward, double accuracy, double progress, string epochesTime);

    /// <summary>
    /// 强化学习模型
    /// </summary>
    public interface IDeepQNet : IMachineLarning
    {
        /// <summary>
        /// event
        /// </summary>
        event UpdateLearningLossHandler OnLearningLossEventHandler;

        /// <summary>
        /// input state and get the best action
        /// besides : the raw action value has been converted to typed value
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        int Predict(float[] state);

        /// <summary>
        /// 使用环境进行训练
        /// </summary>
        void Learn();

        /// <summary>
        /// 如果要训练DQN，需要先执行PrepareTrain，设置必要参数
        /// </summary>
        /// <param name="env"></param>
        /// <param name="epochs"></param>
        /// <param name="gamma"></param>
        void PrepareLearn(IEnv env, int epochs = 3000, float gamma = 0.0f);

        /// <summary>
        /// store in memory
        /// </summary>
        /// <returns></returns>
        (byte[] actorBuffer, byte[] cirticBuffer, string innerTypeName, int actionsNumber, int featuresNumber,int[] actionKeys) PersistencMemory();
    }
}
