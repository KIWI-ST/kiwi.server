using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuro.Loss
{
    public class CrossEntropyLoss
    {

        public void Backward()
        {

        }

        /// <summary>
        /// 计算一次输出的 cross entropy
        /// </summary>
        /// <param name="target"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public double Measure(double[] target, double[] actual)
        {
            double error = 0.0;
            for (int i = 0; i < target.Length; i++)
                error -= (target[i] * Math.Log(actual[i] + 1e-15)) + ((1 - target[i]) * Math.Log((1 + 1e-15) - actual[i]));
            return error;
        }

    }
}
