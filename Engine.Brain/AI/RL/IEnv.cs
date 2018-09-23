using System.Collections.Generic;
using System.Drawing;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// 定义基本环境编写接口
    /// </summary>
    public interface IEnv
    {
        /// <summary>
        /// indicate the action value can be assigned by multi-action value
        /// e.g. 
        /// only support [0001] while the value is true
        /// support [1001] while the value is false
        /// </summary>
        bool IsSingleAction { get; }
        /// <summary>
        /// get action - rawValue dictionary map
        /// </summary>
        int[] RandomSeedKeys { get; }
        /// <summary>
        /// the environment memory
        /// </summary>
        Dictionary<int, List<Point>> Memory { get; }
        /// <summary>
        /// 验证数据集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        (List<double[]> states, double[][] labels) RandomEval(int batchSize = 64);
        /// <summary>
        /// number of actions
        /// </summary>
        int ActionNum { get; }
        /// <summary>
        /// number of features
        /// </summary>
        int FeatureNum { get; }
        /// <summary>
        /// get sate/reward/q/sate next(state_) (one hot)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        (double[] state, double reward) Step(double[] action);
        /// <summary>
        /// crate an action located in action range
        /// </summary>
        /// <returns></returns>
        double[] RandomAction();
        /// <summary>
        /// 重置环境
        /// </summary>
        double[] Reset();
    }
}