namespace NEURO
{
    public interface ILoss
    {
        /// <summary>
        /// 适合RNN的loss计算方式
        /// </summary>
        /// <param name="probs"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        double Error(double[] predOutput, double[] deiredOutput);
    }
}
