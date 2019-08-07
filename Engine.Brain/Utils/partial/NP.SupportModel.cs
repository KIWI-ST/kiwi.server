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
            /// <summary>
            /// 模型类型，与需要与类名一致
            /// </summary>
            public string TypeName { get; set; }

            /// <summary>
            /// 模型名称
            /// </summary>
            public string ModelName { get; set; }

            /// <summary>
            /// 模型的byte数据
            /// </summary>
            public byte[] ModelStream { get; set; }
        }

        /// <summary>
        /// 每个保存点需要带有的信息
        /// </summary>
        public class CheckpointEntity
        {
            List<ModelEntity> Model { get;set }
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
