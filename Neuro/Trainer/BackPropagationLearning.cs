using Neuro.Abstract;
using Neuro.Layers;
using Neuro.Networks;
using Neuro.Neurons;
using System;

namespace Neuro.Learning
{
    /// <summary>
    /// 
    /// </summary>
    public class BackPropagationLearning: ILearning
    {
        // network to teach
        private ActivationNetwork network;
        // learning rate
        private double learningRate = 0.1;
        // momentum
        private double momentum = 0.0;

        // neuron's loss
        private readonly double[][] neuronLoss = null;
        // weight's updates
        private readonly double[][][] weightsUpdates = null;
        // threshold's updates
        private readonly double[][] biasesUpdates = null;

        /// <summary>
        /// Learning rate, [0, 1].
        /// </summary>
        /// 
        /// <remarks><para>The value determines speed of learning.</para>
        /// 
        /// <para>Default value equals to <b>0.1</b>.</para>
        /// </remarks>
        ///
        public double LearningRate
        {
            get { return learningRate; }
            set
            {
                learningRate = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        /// <summary>
        /// Momentum, [0, 1]. Default value equals to 0.0
        /// The value determines the portion of previous weight's update
        /// to use on current iteration. Weight's update values are calculated on
        /// each iteration depending on neuron's error. The momentum specifies the amount
        /// of update to use from previous iteration and the amount of update
        /// to use from current iteration. If the value is equal to 0.1, for example,
        /// then 0.1 portion of previous update and 0.9 portion of current update are used
        /// to update weight's value.
        /// 
        /// </summary>
        public double Momentum
        {
            get { return momentum; }
            set
            {
                momentum = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackPropagationLearning"/> class.
        /// </summary>
        public BackPropagationLearning(ActivationNetwork network)
        {
            this.network = network;
            // create error and deltas arrays
            neuronLoss = new double[network.Layers.Length][];
            weightsUpdates = new double[network.Layers.Length][][];
            biasesUpdates = new double[network.Layers.Length][];
            // initialize errors and deltas arrays for each layer
            for (int i = 0; i < network.Layers.Length; i++)
            {
                IActivationLayer layer = network.Layers[i];
                neuronLoss[i] = new double[layer.Neurons.Length];
                weightsUpdates[i] = new double[layer.Neurons.Length][];
                biasesUpdates[i] = new double[layer.Neurons.Length];
                // for each neuron
                for (int j = 0; j < weightsUpdates[i].Length; j++)
                    weightsUpdates[i][j] = new double[layer.InputsCount];
            }
        }

        /// <summary>
        /// Runs learning iteration.
        /// Returns squared error (difference between current network's output and
        /// desired output) divided by 2.
        /// </summary>
        public double Run(double[] input, double[] output)
        {
            // compute the network's output
            // store ouput in layer.Neurons[i].Output
            network.Compute(input);
            // calculate network error
            double error = CalculateError(output);
            // calculate weights updates
            CalculateUpdates(input);
            // update the network
            UpdateNetwork();
            // return loss
            return error;
        }

        /// <summary>
        /// Runs learning epoch.
        /// </summary>
        public double RunEpoch(double[][] input, double[][] output)
        {
            //cost
            double error = 0.0;
            // run learning procedure for all samples
            for (int i = 0; i < input.Length; i++)
                error += Run(input[i], output[i]);
            // return summary error
            return error;
        }

        /// <summary>
        /// Calculates error values for all neurons of the network.
        /// Returns summary squared error of the last layer divided by 2.
        /// </summary>
        private double CalculateError(double[] desiredOutput)
        {
            // current and the next layers
            ActivationLayer layer, layerNext;
            // current and the next errors arrays
            double[] errors, errorsNext;
            // error values
            double error = 0, e, sum;
            // neuron's output value
            double output;
            // layers count
            int layersCount = network.Layers.Length;
            // assume, that all neurons of the network have the same activation function
            IActivation function = (network.Layers[0].Neurons[0] as ActNeuron).ActivationFunction;
            // calculate error values for the last layer first
            layer = network.Layers[layersCount - 1];
            errors = neuronLoss[layersCount - 1];
            for (int i = 0; i < layer.Neurons.Length; i++)
            {
                output = layer.Neurons[i].Output;
                // error of the neuron
                e = desiredOutput[i] - output;
                // error multiplied with activation function's derivative
                errors[i] = e * function.Derivative2(output);
                // squre the error and sum it
                error += (e * e);
            }
            // calculate error values for other layers
            for (int j = layersCount - 2; j >= 0; j--)
            {
                layer = network.Layers[j];
                layerNext = network.Layers[j + 1];
                errors = neuronLoss[j];
                errorsNext = neuronLoss[j + 1];
                // for all neurons of the layer
                for (int i = 0; i < layer.Neurons.Length; i++)
                {
                    sum = 0.0;
                    // for all neurons of the next layer
                    for (int k = 0; k < layerNext.Neurons.Length; k++)
                    {
                        sum += errorsNext[k] * layerNext.Neurons[k].Weights[i];
                    }
                    errors[i] = sum * function.Derivative2(layer.Neurons[i].Output);
                }
            }
            // return squared error of the last layer divided by 2
            return error / 2.0;
        }

        /// <summary>
        /// Calculate weights updates.
        /// </summary>
        private void CalculateUpdates(double[] input)
        {
            // current neuron
            ActNeuron neuron;
            // current and previous layers
            ActivationLayer layer, layerPrev;
            // layer's weights updates
            double[][] layerWeightsUpdates;
            // layer's thresholds updates
            double[] layerThresholdUpdates;
            // layer's error
            double[] errors;
            // neuron's weights updates
            double[] neuronWeightUpdates;
            // error value
            // double		error;
            // 1 - calculate updates for the first layer
            layer = network.Layers[0];
            errors = neuronLoss[0];
            layerWeightsUpdates = weightsUpdates[0];
            layerThresholdUpdates = biasesUpdates[0];
            // cache for frequently used values
            double cachedMomentum = learningRate * momentum;
            double cached1mMomentum = learningRate * (1 - momentum);
            double cachedError;
            // for each neuron of the layer
            for (int i = 0; i < layer.Neurons.Length; i++)
            {
                neuron = layer.Neurons[i];
                cachedError = errors[i] * cached1mMomentum;
                neuronWeightUpdates = layerWeightsUpdates[i];
                // for each weight of the neuron
                for (int j = 0; j < neuronWeightUpdates.Length; j++)
                {
                    // calculate weight update
                    neuronWeightUpdates[j] = cachedMomentum * neuronWeightUpdates[j] + cachedError * input[j];
                }
                // calculate treshold update
                layerThresholdUpdates[i] = cachedMomentum * layerThresholdUpdates[i] + cachedError;
            }
            // 2 - for all other layers
            for (int k = 1; k < network.Layers.Length; k++)
            {
                layerPrev = network.Layers[k - 1];
                layer = network.Layers[k];
                errors = neuronLoss[k];
                layerWeightsUpdates = weightsUpdates[k];
                layerThresholdUpdates = biasesUpdates[k];
                // for each neuron of the layer
                for (int i = 0; i < layer.Neurons.Length; i++)
                {
                    neuron = layer.Neurons[i];
                    cachedError = errors[i] * cached1mMomentum;
                    neuronWeightUpdates = layerWeightsUpdates[i];
                    // for each synapse of the neuron
                    for (int j = 0; j < neuronWeightUpdates.Length; j++)
                    {
                        // calculate weight update
                        neuronWeightUpdates[j] = cachedMomentum * neuronWeightUpdates[j] + cachedError * layerPrev.Neurons[j].Output;
                    }
                    // calculate treshold update
                    layerThresholdUpdates[i] = cachedMomentum * layerThresholdUpdates[i] + cachedError;
                }
            }
        }

        /// <summary>
        /// Update network'sweights.
        /// </summary>
        /// 
        private void UpdateNetwork()
        {
            // current neuron
            ActNeuron neuron;
            // current layer
            ActivationLayer layer;
            // layer's weights updates
            double[][] layerWeightsUpdates;
            // layer's thresholds updates
            double[] layerThresholdUpdates;
            // neuron's weights updates
            double[] neuronWeightUpdates;
            // for each layer of the network
            for (int i = 0; i < network.Layers.Length; i++)
            {
                layer = network.Layers[i];
                layerWeightsUpdates = weightsUpdates[i];
                layerThresholdUpdates = biasesUpdates[i];
                // for each neuron of the layer
                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    neuron = layer.Neurons[j] as ActNeuron;
                    neuronWeightUpdates = layerWeightsUpdates[j];
                    // for each weight of the neuron
                    for (int k = 0; k < neuron.Weights.Length; k++)
                    {
                        // update weight
                        neuron.Weights[k] += neuronWeightUpdates[k];
                    }
                    // update treshold
                    neuron.Threshold += layerThresholdUpdates[j];
                }
            }
        }
    }
}
