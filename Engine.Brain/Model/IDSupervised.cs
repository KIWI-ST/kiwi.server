namespace Engine.Brain.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDSupervised
    {
        /// <summary>
        /// Train Machine Learning Model
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        double Train(float[][] inputs, int[] outputs);

        /// <summary>
        /// Predict By Model
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        int[] Predict(float[][] inputs);
    }
}
