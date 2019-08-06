using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.Brain.Method;
using Engine.Brain.Method.DeepQNet;

namespace Engine.Brain.Utils
{
    public partial class NP
    {
        /// <summary>
        /// 模型保存实体
        /// </summary>
        public class ModelEntity
        {
            public string ModelName { get; set; }

            List<byte[]> ModelStream { get; set; }
        }

        /// <summary>
        /// support models in this library
        /// </summary>
        public static class SupportModel
        {
            /// <summary>
            /// get convnet support only
            /// </summary>
            public static List<string> ConvSupportCollection {
                get {
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IConvNet))).Select(s => s.ToString().Split('.').Last());
                    return typeNames.ToList();
                }
            }

            /// <summary>
            /// get deep reinforcement neural net support , for DQN
            /// </summary>
            public static List<string> ReinforceSupportCollection
            {
                get
                {
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISupportNet))).Select(s => s.ToString().Split('.').Last());
                    return typeNames.ToList();
                }
            }
        }
    }
}
