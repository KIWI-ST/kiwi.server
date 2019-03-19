using NEURO;
using System.Linq;

namespace Neuro.Trainer
{
    /// <summary>
    /// 基于动量法的神经网络训练器
    /// </summary>
    public class MomentuTrainer
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
        public MomentuTrainer(INetwork network, ILoss loss)
        {
            _network = network;
            _loss = loss;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public double Train(double[] input, double[] output)
        {
            _network.Compute(input);
            double error = ComputeError(output);
            ComputeDelta(input);
            UpdateNetworkWeights();
            return error;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double TrainEpochs()
        {
            return 0.0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desiredOuput"></param>
        /// <returns></returns>
        public double ComputeError(double[] desiredOuput)
        {
            //pred output
            double e, sum, error = 0;
            //calculate each neuron error at last layer first
            ILayer lastLayer = _network.Layers.Last();
            for (int i = 0; i < lastLayer.Neurons.Count; i++)
            {
                INeuron neuron = lastLayer.Neurons[i];
                double output = neuron.Output;
                e = desiredOuput[i] - output; //使用
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
        public void ComputeDelta(double[] input)
        {
            ILayer firstLayer = _network.Layers.First();
            //cache frequently used values
            double cachedMomentum = learningRate * momentum;
            double cached1mMomentum = learningRate * (1 - momentum);
            //for each neuron of the layer
            for (int i = 0; i < firstLayer.Neurons.Count; i++)
            {
                INeuron neuron = firstLayer.Neurons[i];
                double cachedError = neuron.Error * cached1mMomentum;
                for (int k = 0; k < neuron.W.Length; k++)
                {
                    neuron.Dw[k] = cachedMomentum * neuron.Dw[k] + cachedError * input[k];
                }
                neuron.Db = cachedMomentum * neuron.Db + cachedError;
            }
            for (int k = 1; k < _network.Layers.Count; k++)
            {
                ILayer layerPrev = _network.Layers[k - 1];
                ILayer layer = _network.Layers[k];
                for (int i = 0; i < layer.Neurons.Count; i++)
                {
                    INeuron neuron = layer.Neurons[i];
                    double cachedError = neuron.Error * cached1mMomentum;
                    for (int j = 0; j < neuron.W.Length; j++)
                    {
                        neuron.Dw[j] = cachedMomentum * neuron.Dw[j] + cachedError * layerPrev.Neurons[j].Output;
                    }
                    neuron.Db = cachedMomentum * neuron.Db + cachedError;
                }
            }
        }
        /// <summary>
        /// 更新权重
        /// </summary>
        private void UpdateNetworkWeights()
        {
            _network.Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron =>
                {
                    neuron.UpdateWeights();
                });
            });
        }

    }
}
