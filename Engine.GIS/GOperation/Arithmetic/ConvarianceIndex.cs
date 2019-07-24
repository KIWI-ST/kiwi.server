using System;
using System.Linq;

namespace Engine.GIS.GOperation.Arithmetic
{
    public class ConvarianceIndex
    {
        public static float CalcuteConvariance(float[] x, float[] y)
        {
            //0.统计length
            int length = 0;
            if (x.Length != y.Length)
                return -9999;
            else
                length = x.Length;
            //1.计算协方差
            float ex = x.Sum() / (length - 1);
            float ey = y.Sum() / (length - 1);
            float exy = 0.0f;
            for (int i = 0; i < length; i++)
                exy += x[i] * y[i];
            exy = exy / (length - 1);
            //2.计算相关系数
            float covxy = exy - ex * ey;
            return covxy;
        }

        public static float Variance(float[] x)
        {
            int length = x.Length;
            float ex = x.Sum() / length;
            float vx = 0.0f;
            for (int i = 0; i < length; i++)
                vx += (x[i]-ex);
            return vx / length;
        }

        public static float CalcuteConvarianceIndex(float[] x, float[] y)
        {
            float cov = CalcuteConvariance(x, y);
            double input = Variance(x) * Variance(y);
            return cov / (float)Math.Sqrt(input);
        }

    }
}
