namespace NEURO
{
    /// <summary>
    /// 激活函数
    /// </summary>
    public interface IActivation
    {
        /// <summary>
        /// Calculates function value.
        /// </summary>
        double Function(double x);

        /// <summary>
        /// Calculates function derivative.
        /// The method calculates function derivative at point x, as f'(x)
        /// <summary>
        double Derivative(double x);

        /// <summary>
        /// Calculates function derivative.
        /// 
        /// The method calculates the same derivative value as the method, 
        /// but it takes not the input x value itself, but the function value, 
        /// which was calculated previously with the help of method.
        /// 
        /// Some applications require as function value, as derivative value,
        /// so they can save the amount of calculations using this method to calculate derivative.
        /// </summary>
        double Derivative2(double y);
    }
}
