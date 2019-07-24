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
        double Train(float[][] inputs, float[][] outputs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        float[] Predict(params object[] inputs);

        /// <summary>
        /// persistence in local dir
        /// </summary>
        string PersistencNative(string modelFilename = null);
    }
}
