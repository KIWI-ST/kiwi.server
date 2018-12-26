using System;
using System.Threading.Tasks;

namespace Engine.Brain.Model.DL.Language
{
    [Serializable]
    public abstract class Layer
    {
        private readonly Random random = new Random();

        protected const double rmsDecay = 0.95;

        public static int BufferSize;

        public static double LearningRate;

        public abstract int Count();

        public abstract double[][] Forward(double[][] buffer, bool reset);

        public abstract double[][] Backward(double[][] grads);

        protected abstract void ResetState();

        protected abstract void ResetParameters();

        protected abstract void ResetGradients();

        protected abstract void ResetCaches();

        protected abstract void Update();

        /// <summary>
        /// Prevent gradient explosions.
        /// </summary>
        protected static double Clip(double x)
        {
            if (x < -1.0) return -1.0;
            if (x > 1.0) return 1.0;
            return x;
        }

        /// <summary>
        /// Random weight initialisation.
        /// </summary>
        protected double RandomWeight()
        {
            return (0.5 - random.NextDouble()) * 0.2;
        }

        /// <summary>
        /// Squashing function returning values between zero and plus one.
        /// </summary>
        protected static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        /// <summary>
        /// Squashing function returning values between minus one and plus one.
        /// </summary>
        protected static double Tanh(double x)
        {
            return Math.Tanh(x);
        }

        /// <summary>
        /// Derivative of sigmoid function.
        /// </summary>
        protected static double dSigmoid(double x)
        {
            return (1 - x) * x;
        }

        /// <summary>
        /// Derivative of hyperbolic tangent function.
        /// </summary>
        protected static double dTanh(double x)
        {
            return 1 - x * x;
        }
    }
}
