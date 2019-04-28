using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.Brain.Model;

namespace Engine.Brain.Utils
{
    public partial class NP
    {
        public static class Model
        {
            /// <summary>
            /// get convnet support only
            /// </summary>
            public static List<string> ConvSupportCollection {
                get {
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDConvNet))).Select(s => s.ToString().Split('.').Last());
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
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDSupportDQN))).Select(s => s.ToString().Split('.').Last());
                    return typeNames.ToList();
                }
            }
        }
    }
}
