using NEURO.Utils;
using System;

namespace NEURO.Neuron
{
    /// <summary>
    /// 每一个神经元都重复 
    /// wX+b
    /// x为一次输入，
    /// </summary>
    public class Neuron : INeuron
    {
        /// <summary>
        /// activation function
        /// </summary>
        IActivation _function;
        /// <summary>
        /// weights
        /// </summary>
        public double[] W { get; private set; }
        /// <summary>
        /// derivative wegiths
        /// </summary>
        public double[] Dw { get; set; }
        /// <summary>
        /// bias
        /// </summary>
        public double B { get; private set; }
        /// <summary>
        /// delta b
        /// </summary>
        public double Db { get; set; }
        /// <summary>
        /// the neuron's output
        /// </summary>
        public double Output { get; private set; }
        /// <summary>
        /// the error of neuron
        /// </summary>
        public double Error { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IActivation Function { get { return _function; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDimension"></param>
        /// <param name="function"></param>
        public Neuron(int inputDimension, IActivation function)
        {
            //allocate weights and derivative wegiths
            W = new double[inputDimension];
            Dw = new double[inputDimension];
            //set activation function
            _function = function;
        }
        /// <summary>
        /// randomize weights and bias
        /// </summary>
        public void Randomize()
        {
            //init weights
            for (int i = 0; i < W.Length; i++)
                W[i] = NP.RandomByNormalDistribute();
            //init bias
            B = NP.RandomByNormalDistribute();
        }
        /// <summary>
        /// forward compute
        /// </summary>
        /// <returns></returns>
        public double Compute(double[] inputs)
        {
            if (inputs.Length != W.Length) throw new ArgumentException("Wrong length of the input vector.");
            double sum = 0.0;
            for (int i = 0; i < inputs.Length; i++)
                sum += W[i] * inputs[i];
            sum += B;
            Output = _function.Function(sum);
            return Output;
        }
        /// <summary>
        /// update weights
        /// </summary>
        public void UpdateWeights()
        {
            //update weights
            for (int i = 0; i < W.Length; i++)
                W[i] += Dw[i];
            //update bias
            B += Db;
        }
    }
}
