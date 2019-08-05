namespace Engine.Brain.Method
{
    /// <summary>
    /// deep convolution neural network
    /// expecially suitable for deep feature extract
    /// </summary>
    public interface IConvNet : INeuralNet
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
