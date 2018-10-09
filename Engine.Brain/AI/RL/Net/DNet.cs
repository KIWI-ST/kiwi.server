using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.IO;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// Selu激活函数
    /// </summary>
    [Serializable]
    public class SeluFunction : IActivationFunction
    {
        /// <summary>
        /// 
        /// </summary>
        const double alpha = 1.6732632423543772848170429916717;
        /// <summary>
        /// 
        /// </summary>
        const double scale = 1.0507009873554804934193349852946;
        /// <summary>
        /// dx
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Derivative(double x)
        {
            double y = Function(x);
            return Derivative2(y);
        }
        /// <summary>
        /// dy
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double Derivative2(double y)
        {
            if (y > 0)
                return scale;
            else
                return scale * alpha * Math.Exp(y);
        }
        /// <summary>
        /// y=
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Function(double x)
        {
            if (x >= 0.0)
                return scale * x;
            else
                return scale * alpha * (Math.Exp(x) - 1);
        }
    }
    /// <summary>
    /// DQN State Prediction NeuralNetwork 
    /// </summary>
    public class DNet: INet
    {
        ActivationNetwork _network;

        BackPropagationLearning _teacher;

        double _learningRate;

        public DNet(int[] featureNum, int actionNum,double learningRate = 0.001)
        {
            //
            int num = featureNum.Product() + actionNum;
            //
            _learningRate = learningRate;
            //
            _network = new ActivationNetwork(new SeluFunction(), num, num*2, num,num,num, actionNum*2, actionNum, actionNum/2,1);
            //
            new GaussianWeights(_network).Randomize();
            //https://github.com/accord-net/framework/blob/a5a2ea8b59173dd4e695da8017ba06bc45fc6b51/Samples/Neuro/Deep%20Learning/ViewModel/LearnViewModel.cs#L289
            _teacher = new BackPropagationLearning(_network) {
                LearningRate = learningRate,
                Momentum = 0.9
            };
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            int samples = inputs.GetLength(0);
            double loss = _teacher.RunEpoch(inputs, outputs) / samples;
            return loss;
        }

        public string Persistence()
        {
            string filePath = Directory.GetCurrentDirectory() + @"\tmp\";
            string fileName = filePath + "dqn.ann";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (File.Exists(fileName))
                File.Delete(fileName);
            _network.Save(fileName);
            return fileName;
        }

        public void Accept(INet sourceNet)
        {
            _network = Network.Load(sourceNet.Persistence()) as ActivationNetwork;
            _teacher = new BackPropagationLearning(_network)
            {
                LearningRate = _learningRate,
                Momentum = 0.9
            };
        }

        public double[] Predict(double[] input)
        {
            double[] output = _network.Compute(input);
            return output;
        }

    }
}