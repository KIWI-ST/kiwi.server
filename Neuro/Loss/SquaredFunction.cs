using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEURO.Loss
{
    /// <summary>
    /// 二分之一平方差损失计算函数
    /// Returns summary squared error of the last layer divided by 2.
    /// </summary>
    public class SquaredFunction:ILoss
    {
        public double Error(double[] predOutput, double[] deiredOutput)
        {
            return 0.0;
        }
    }
}
