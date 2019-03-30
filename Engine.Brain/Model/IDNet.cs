using System.IO;

namespace Engine.Brain.Model
{
    /// <summary>
    /// DNN结构网络
    /// </summary>
    public interface IDNet
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
        /// <param name="input"></param>
        /// <returns></returns>
        double[] Predict(double[] input);
        /// <summary>
        /// persistence in local dir
        /// </summary>
        string PersistencNative();
        /// <summary>
        /// persistence in memory
        /// </summary>
        Stream PersistenceMemory();
        /// <summary>
        /// copy sourceNet parameters to this Net
        /// </summary>
        /// <param name="sourceNet"></param>
        void Accept(IDNet sourceNet);
    }


}
