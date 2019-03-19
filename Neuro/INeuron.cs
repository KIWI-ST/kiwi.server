namespace NEURO
{
    public interface INeuron
    {
        /// <summary>
        /// 执行backprop
        /// </summary>
        void UpdateWeights();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        double Compute(double[] inputs);
        /// <summary>
        /// 
        /// </summary>
        void Randomize();
        /// <summary>
        /// 
        /// </summary>
        double Output { get; }
        /// <summary>
        /// 
        /// </summary>
        double Error { get; set; }
        /// <summary>
        /// 
        /// </summary>
        double[] W { get; }
        /// <summary>
        /// 
        /// </summary>
        double[] Dw { get; set; }
        /// <summary>
        /// 
        /// </summary>
        double B { get; }
        /// <summary>
        /// 
        /// </summary>
        double Db { get; set; }
        /// <summary>
        /// 
        /// </summary>
        IActivation Function { get; }
    }
}
