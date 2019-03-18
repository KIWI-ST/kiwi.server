using System;
using System.Collections.Generic;

namespace NEURO.Layer
{
    public class ActivationLayer: ILayer
    {
        /// <summary>
        /// Layer's neurons.
        /// </summary>
        protected List<INeuron> neurons;
        /// <summary>
        /// Layer's output vector. The calculation way of layer's output vector is determined by neurons, which comprise the layer
        /// </summary>
        public double[] Output { get; private set; }
        /// <summary>
        /// Compute output vector of the layer.
        /// </summary>
        public double[] Compute(double[] input)
        {
            // local variable to avoid mutlithread conflicts
            double[] output = new double[neurons.Count];
            // compute each neuron
            for (int i = 0; i < neurons.Count; i++)
                output[i] = neurons[i].Compute(input);
            // assign output property as well (works correctly for single threaded usage)
            Output = output;
            return Output;
        }
        /// <summary>
        /// Randomize neurons of the layer.
        /// <summary>
        public void Randomize()
        {
            foreach (INeuron neuron in neurons)
                neuron.Randomize();
        }
        /// <summary>
        /// Initializes a new instance of the ActivationLayer class.
        /// The new layer is randomized after it is created.
        /// </summary>
        public ActivationLayer(int inputDimension, int outputDimension, IActivation function)
        {
            // the input count
            inputDimension = Math.Max(1, inputDimension);
            // create layer output neurons count
            outputDimension = Math.Max(1, outputDimension);
            // create collection of neurons
            // create each neuron
            for (int i = 0; i < outputDimension; i++)
                neurons[i] = new Neuron.Neuron(inputDimension, function);
        }
    }
}
