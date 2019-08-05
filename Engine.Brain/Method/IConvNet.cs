namespace Engine.Brain.Method
{
    /// <summary>
    /// deep convolution neural network
    /// expecially suitable for deep feature extract
    /// </summary>
    public interface IDConvNet : INet
    {

        /// <summary>
        /// remove softmax, convert it to Extract Feature Network
        /// </summary>
        void ConvertToExtractNetwork();

        /// <summary>
        /// predicts
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        float[][] Predicts(float[][] inputs);
    }
}
