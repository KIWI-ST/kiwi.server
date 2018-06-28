using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace Engine.Brain.AI
{
    public class DQN
    {
        /// <summary>
        /// 学习环境
        /// </summary>
        DEnv _env;

        public DQN()
        {
            //构建学习环境，观察目录结构并学习
            _env = new DEnv(@"C:\Users\81596\Desktop\To_PPang");
            //创建三个neuralnetwork
            

        }

    }
}
