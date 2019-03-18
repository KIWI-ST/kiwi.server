using Neuro.Abstract;
using System;

namespace Neuro.Activation
{
    public class SigmoidFunction : IActivation, ICloneable
    {
        /// <summary>
        /// Sigmoid's alpha value.
        /// </summary>
        public double Alpha { get; set; } = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigmoidFunction"/> class.
        /// </summary>
        public SigmoidFunction() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigmoidFunction"/> class.
        /// </summary>
        public SigmoidFunction(double alpha)
        {
            Alpha = alpha;
        }

        /// <summary>
        /// Calculates function value.
        /// </summary>
        public double Function(double x)
        {
            return (1 / (1 + Math.Exp(-Alpha * x)));
        }

        /// <summary>
        /// Calculates function derivative.
        /// </summary>
        public double Derivative(double x)
        {
            double y = Function(x);

            return (Alpha * y * (1 - y));
        }

        /// <summary>
        /// Calculates function derivative.
        /// </summary>
        public double Derivative2(double y)
        {
            return (Alpha * y * (1 - y));
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return new SigmoidFunction(Alpha);
        }

    }
}
