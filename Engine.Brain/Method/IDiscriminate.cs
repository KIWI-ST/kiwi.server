namespace Engine.Brain.Method
{
    /// <summary>
    /// 判别模型
    /// </summary>
    public interface IDiscriminate : IMachineLarning
    {
        /// <summary>
        /// Train Machine Learning Model
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        double Train(float[][] inputs, int[] outputs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        int Predict(float[] input);
    }
}
