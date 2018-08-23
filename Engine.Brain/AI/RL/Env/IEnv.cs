using System.Collections.Generic;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// 定义基本环境编写接口
    /// </summary>
    public interface IEnv
    {
        /// <summary>
        /// 验证数据集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        (List<double[]> states, int[] labels) RandomEval(int batchSize = 64);
        /// <summary>
        /// number of actions
        /// </summary>
        int ActionNum { get; }
        /// <summary>
        /// number of features
        /// </summary>
        int FeatureNum { get; }
        /// <summary>
        /// get sate/reward/q/sate next(state_)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        (double[] state, double reward) Step(int action);
        /// <summary>
        /// crate an action located in action range
        /// </summary>
        /// <returns></returns>
        int RandomAction();
        /// <summary>
        /// 重置环境
        /// </summary>
        double[] Reset();
    }
}