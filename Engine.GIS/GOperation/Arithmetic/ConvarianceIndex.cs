using System;
using System.Linq;

namespace Engine.GIS.GOperation.Arithmetic
{
    public class ConvarianceIndex
    {
        public static double CalcuteConvariance(double[] x, double[] y)
        {
            //0.统计length
            int length = 0;
            if (x.Length != y.Length)
                return -9999;
            else
                length = x.Length;
            //1.计算协方差
            double ex = x.Sum() / (length - 1);
            double ey = y.Sum() / (length - 1);
            double exy = 0.0;
            for (int i = 0; i < length; i++)
                exy += x[i] * y[i];
            exy = exy / (length - 1);
            //2.计算相关系数
            double covxy = exy - ex * ey;
            return covxy;
        }

        public static double Variance(double[] x)
        {
            int length = x.Length;
            double ex = x.Sum() / length;
            double vx = 0.0;
            for (int i = 0; i < length; i++)
                vx += (x[i]-ex);
            return vx / length;
        }

        public static double CalcuteConvarianceIndex(double[] x, double[] y)
        {
            double cov = CalcuteConvariance(x, y);
            return cov / Math.Sqrt(Variance(x) * Variance(y));
        }

    }
}
