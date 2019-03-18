using Neuro.Abstract;
using System;

namespace Neuro.Activation
{
    /// <summary>
    ///  Selu激活函数
    /// </summary>
    [Serializable]
    public class SeluFunction : IActivation
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
}
