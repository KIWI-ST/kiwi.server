using System.Collections.Generic;
using System.IO;

namespace NEURO
{
    public interface INetwork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
       INetwork Load(string fileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        INetwork Load(Stream stream);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        void Save(Stream stream);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        void Save(string fileName);
        /// <summary>
        /// layers
        /// </summary>
        List<ILayer> Layers { get; }
        /// <summary>
        /// forward compute
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        double[] Compute(double[] input);
    }
}
