using Neuro.Abstract;
using System;

namespace Neuro.Activation
{
    /// <summary>
    /// Relu 激活函数
    /// </summary>
    [Serializable]
    public class ReluFunction : IActivation, ICloneable
    {
        /// <summary>
        /// Calculates function value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Derivative(double x)
        {
            double y = Function(x);
            return Derivative2(y);
        }

        /// <summary>
        /// Calculates function derivative by current y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double Derivative2(double y)
        {
            if (y > 0)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Calculates function value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Function(double x)
        {
            return (x >= 0) ? 1 : 0;
        }

        /// <summary>
        /// A new object that is a copy of this instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new ReluFunction();
        }

    }
}
