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
            public static List<string> ConvNetCollection {
                get {
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDConvNet))).Select(s => s.ToString().Split('.').Last());
                    return typeNames.ToList();
                }
            }
            /// <summary>
            /// all the ConvNet constructed with w,h,c and output classNum
            /// </summary>
            /// <param name="name"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="channel"></param>
            /// <param name="classNum"></param>
            public static IDConvNet creator(string name, int width, int height, int channel, int classNum)
            {
                return null;
            }
        }
    }
}
