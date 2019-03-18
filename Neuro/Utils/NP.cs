using System;

namespace NEURO.Utils
{
    /// <summary>
    /// 模拟np操作库，实现数据处理相关功能
    /// @author yellow 
    /// @date 2018/12/22
    /// </summary>
    public class NP
    {

        /// <summary>
        /// 基于hash的random，比系统自带的random更符合随机特性
        /// </summary>
        /// <returns></returns>
        public static double Random()
        {
            return new Random(Guid.NewGuid().GetHashCode()).NextDouble();
        }

        /// <summary>
        /// 生成符合正太分布的随机数，默认e =0, d=1
        /// </summary>
        /// <returns></returns>
        public static double RandomByNormalDistribute()
        {
            double u1 = Random();
            double u2 = Random();
            return NormalDistribute(u1,u2);
        }

        /// <summary>
        /// 标准正态分部期望0，方差1
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <param name="e">期望，</param>
        /// <param name="d">方差</param>
        /// <returns></returns>
        public static double NormalDistribute(double u1, double u2, double e = 0, double d = 1)
        {
            double result = e + Math.Sqrt(d) * Math.Sqrt((-2) * (Math.Log(u1) / Math.Log(Math.E))) * Math.Cos(2 * Math.PI * u2);
            return result;
        }

    }
}
