using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// 定义基本环境编写接口
    /// </summary>
    public interface IDEnv
    {
        /// <summary>
        /// const action of 1
        /// </summary>
        float[] DummyActions { get; }
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
        (float[] state, float reward) Step(int action);
        /// <summary>
        /// crate an action located in action range
        /// </summary>
        /// <returns></returns>
        int RandomAction();
        /// <summary>
        /// 重置环境
        /// </summary>
        float[] Reset();
    }
}
