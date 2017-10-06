using System;
using tf = TensorFlow;

namespace Engine.TensorFlow.Models
{

    public static class NumpyRandom
    {
        private static readonly Random Rng = new Random();

        public static double Randn()
        {
            var u1 = 1.0 - Rng.NextDouble();
            var u2 = 1.0 - Rng.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return randStdNormal;
        }
    }

    public class TraceModel
    {

        public void Run()
        {



            // Parameters
            var learningRate = 0.01;
            var trainingEpochs = 1000;
            var displayStep = 50;

            // Training Data
            var trainX = new[]
            {
                3.3, 4.4, 5.5, 6.71, 6.93, 4.168, 9.779, 6.182, 7.59, 2.167, 7.042, 10.791, 5.313,
                7.997, 5.654, 9.27, 3.1
            };
            var trainY = new[]
            {
                1.7, 2.76, 2.09, 3.19, 1.694, 1.573, 3.366, 2.596, 2.53, 1.221,
                2.827, 3.465, 1.65, 2.904, 2.42, 2.94, 1.3
            };
            var nSamples = trainX.Length;
            
            using (var graph = new tf.TFGraph())
            {
                using (var session = new tf.TFSession(graph))
                {
                    // Graph Input
                    var x = graph.Placeholder(tf.TFDataType.Double);
                    var y = graph.Placeholder(tf.TFDataType.Double);

                    // Set model weights
                    var w = graph.Variable(graph.Const(NumpyRandom.Randn()), "weight");
                    var b = graph.Variable(graph.Const(NumpyRandom.Randn()), "bias");

                    // Construct a linear model
                    var pred = graph.Add(graph.Mul(x, w), b);

                    // Mean squared error
                    var cost = graph.ReduceSum(graph.Div(graph.Pow(graph.Sub(pred, y), graph.Const(2)), graph.Mul(graph.Const(2), graph.Const(nSamples))));

                    // Gradient descent
                    //  Note, minimize() knows to modify w and b because Variable objects are trainable=True by default

                    // TODO: stuck here, bindings are missing

                    //var optimizer = .GradientDescentOptimizer(learning_rate).minimize(cost)
                }
                //graph.Save()
            }
          




        }


    }
}
