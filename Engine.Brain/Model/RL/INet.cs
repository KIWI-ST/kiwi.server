using System.IO;

namespace Engine.Brain.AI.RL
{
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
        /// persistence in local dir
        /// </summary>
        string PersistencNative();
        /// <summary>
        /// persistence in memory
        /// </summary>
        Stream PersistenceMemory();
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
        void Accept(IDNet sourceNet);
    }

    public interface IDCnnNet:IDNet
    {
        void ToCharacteristicNetwork();
    }

}
