using System.Collections.Generic;

namespace NEURO
{
    public interface ILayer
    {
        /// <summary>
        /// 
        /// </summary>
        List<INeuron> Neurons { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        double[] Compute(double[] inputs);
    }
}
