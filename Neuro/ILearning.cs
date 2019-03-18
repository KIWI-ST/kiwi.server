namespace NEURO
{
    public interface ILearning
    {
        /// <summary>
        /// run learning iteration
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        double Run(double[] input, double[] output);

        /// <summary>
        /// run learning epoch
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        double RunEpoch(double[][] input, double[][] output);
    }
}
