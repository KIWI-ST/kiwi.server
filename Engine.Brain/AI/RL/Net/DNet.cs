using Accord.Neuro;
using Accord.Neuro.ActivationFunctions;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using System.IO;

namespace Engine.Brain.AI.RL
{
    public class DNet
    {
        DeepBeliefNetwork _network;

        BackPropagationLearning _teacher;

        public DNet(int featureNum, int actionNum)
        {
            //
            int num = featureNum + actionNum;
            //
            _network = new DeepBeliefNetwork(new BernoulliFunction(), num, 10,1);
            //
            new GaussianWeights(_network).Randomize();
            _network.UpdateVisibleWeights();
            //https://github.com/accord-net/framework/blob/a5a2ea8b59173dd4e695da8017ba06bc45fc6b51/Samples/Neuro/Deep%20Learning/ViewModel/LearnViewModel.cs#L289
            _teacher = new BackPropagationLearning(_network)
            {
                LearningRate = 0.1,
                Momentum = 0.5
            };
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            double loss = _teacher.RunEpoch(inputs, outputs);
            _network.UpdateVisibleWeights();
            return loss;
        }

        private string Save()
        {
            string filePath = Directory.GetCurrentDirectory() + @"\ai\";
            string fileName = filePath + "dqn.ann";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (File.Exists(fileName))
                File.Delete(fileName);
            _network.Save(fileName);
            return fileName;
        }

        public void Accept(DNet sourceNet)
        {
            _network = DeepBeliefNetwork.Load(sourceNet.Save());
            _teacher = new BackPropagationLearning(_network)
            {
                LearningRate = 0.1,
                Momentum = 0.5
            };
        }

        public double[] Predict(double[] input)
        {
            double[] output = _network.Compute(input);
            return output;
        }

    }
}
