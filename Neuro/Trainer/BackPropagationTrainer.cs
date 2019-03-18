using NEURO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuro.Trainer
{
    public class BackPropagationTrainer
    {
        //
        double learningRate = 0.1;
        //
        double momentum = 0.0;
        /// <summary>
        /// 
        /// </summary>
        INetwork _network;
        /// <summary>
        /// 
        /// </summary>
        ILoss _loss;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        /// <param name="loss"></param>
        public BackPropagationTrainer(INetwork network, ILoss loss)
        {
            _network = network;
            _loss = loss;
        }

        public double Train(double[] input, double[] output)
        {
            _network.Compute(input);
            double error = ComputeError(output);

            return error;
        }

        public double TrainEpochs()
        {
            return 0.0;
        }

        public double ComputeError(double[] desiredOuput)
        {
            //pred output
            double e, sum, error = 0;
            //calculate each neuron error at last layer first
            ILayer lastLayer = _network.Layers.Last();
            for(int i = 0; i < lastLayer.Neurons.Count; i++)
            {
                INeuron neuron = lastLayer.Neurons[i];
                double output = neuron.Output;
                e = desiredOuput[i] - output;
                neuron.Error = e * neuron.Function.Derivative2(output);
                error += (e * e);
            }
            //calculate error for other layers
            for (int k = _network.Layers.Count - 2; k >= 0; k--)
            {
                ILayer layer = _network.Layers[k];
                ILayer nextLayer = _network.Layers[k + 1];
                for (int i = 0; i < layer.Neurons.Count; i++)
                {
                    sum = 0.0;
                    for (int j = 0; j < nextLayer.Neurons.Count; j++)
                    {
                        sum += nextLayer.Neurons[j].Error * nextLayer.Neurons[j].W[i];
                    }
                    layer.Neurons[i].Error = sum * layer.Neurons[i].Function.Derivative2(layer.Neurons[i].Output);
                }
            }
            return error / 2.0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public ComputeDeltaWeights(double[] input)
        {
            ILayer firstLayer = _network.Layers.First();

        }

    }
}
