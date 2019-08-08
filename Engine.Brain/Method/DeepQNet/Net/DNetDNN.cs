using System;
using System.IO;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Engine.Brain.Extend;
using Engine.Brain.Utils;

namespace Engine.Brain.Method.DeepQNet.Net
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
    public class DNetDNN : ISupportNet
    {
        ActivationNetwork _network;

        BackPropagationLearning _teacher;

        double _learningRate;

        string _dqnFilename = DateTime.Now.ToFileTimeUtc().ToString() + ".ann";

        public DNetDNN(int[] featureNum, int actionNum, double learningRate = 0.002)
        {
            int input = featureNum.Product();
            _learningRate = learningRate;
            _network = new ActivationNetwork(new SeluFunction(), input, input / 2, input / 4, actionNum, actionNum);
            new GaussianWeights(_network).Randomize();
            //https://github.com/accord-net/framework/blob/a5a2ea8b59173dd4e695da8017ba06bc45fc6b51/Samples/Neuro/Deep%20Learning/ViewModel/LearnViewModel.cs#L289
            _teacher = new BackPropagationLearning(_network)
            {
                LearningRate = learningRate,
                Momentum = 0.9
            };
        }

        public double Train(float[][] inputs, float[][] outputs)
        {

            int samples = inputs.GetLength(0);
            double loss = _teacher.RunEpoch(NP.FloatArrayToDoubleArray(inputs), NP.FloatArrayToDoubleArray(outputs)) / samples;
            return loss;
        }

        public string PersistencNative(string modelFilename = null)
        {
            string filePath = modelFilename ?? Directory.GetCurrentDirectory() + @"\tmp\";
            string fileName = filePath + _dqnFilename;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (File.Exists(fileName))
                File.Delete(fileName);
            _network.Save(fileName);
            return fileName;
        }

        public byte[] PersistenceMemory()
        {
            MemoryStream memory = new MemoryStream();
            _network.Save(memory);
            memory.Seek(0, SeekOrigin.Begin);
            //
            byte[] bytes = new byte[memory.Length];
            memory.Read(bytes, 0, bytes.Length);
            memory.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public void Accept(ISupportNet sourceNet)
        {
            byte[] bytes = sourceNet.PersistenceMemory();
            Stream stream = new MemoryStream(bytes);
            _network = Network.Load(stream) as ActivationNetwork;
            _teacher = new BackPropagationLearning(_network)
            {
                LearningRate = _learningRate,
                Momentum = 0.9
            };
        }

        public float[] Predict(float[] input)
        {
            double[] dInput = input.toDouble();
            double[] output = _network.Compute(dInput);
            return output.toFloat();
        }
    }
}