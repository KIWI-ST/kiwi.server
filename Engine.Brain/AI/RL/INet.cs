using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.AI.RL
{
    public interface INet
    {
        /// <summary>
        /// train the network
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns>loss</returns>
        double Train(double[][] inputs, double[][] outputs);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Copy();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        double[] Predict(double[] input);
        /// <summary>
        /// copy sourceNet parameters to this Net
        /// </summary>
        /// <param name="sourceNet"></param>
        void Accept(INet sourceNet);
    }
}
