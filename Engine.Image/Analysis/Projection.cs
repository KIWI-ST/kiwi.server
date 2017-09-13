using Engine.Image.Entity;
using System;

namespace Engine.Image.Analysis
{
    public interface IProjection
    {
        GPoint Porject(double lat, double lng);
        LatLng unPorject(double x, double y);
    }

    /// <summary>
    /// 墨卡托投影
    /// </summary>
    public class MercatorProjection : IProjection
    {
        //长半轴
        double R = 6378137;
        //短半轴
        double R_MINOR = 6356752.314245179;
        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <returns></returns>
        public GPoint Porject(double lat, double lng)
        {
            double d = Math.PI / 180;
            double r = R;
            double y = lat * d;
            double tmp = R_MINOR / r;
            double e = Math.Sqrt(1 - tmp * tmp);
            double con = e * Math.Sin(y);
            double ts = Math.Tan(Math.PI / 4 - y / 2) / Math.Pow((1 - con) / (1 + con), e / 2);
            y = -r * Math.Log(Math.Max(ts, 1E-10));
            return new GPoint(lng * d * r, y);
        }
        /// <summary>
        /// 反投影
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public LatLng unPorject(double x, double y)
        {
            double d = 180 / Math.PI;
            double r = R;
            double tmp = R_MINOR / r;
            double e = Math.Sqrt(1 - tmp * tmp);
            double ts = Math.Exp(-y / r);
            double phi = Math.PI / 2 - 2 * Math.Atan(ts);
            double dphi = 0.1;
            double con;
            for (int i = 0;i < 15 && Math.Abs(dphi) > 1e-7; i++)
            {
                con = e * Math.Sin(phi);
                con = Math.Pow((1 - con) / (1 + con), e / 2);
                dphi = Math.PI / 2 - 2 * Math.Atan(ts * con) - phi;
                phi += dphi;
            }
            return new LatLng(phi * d, x * d / r);
        }
    }

    public class SphericalMercatorProjection:IProjection
    {
         //长半轴
        double R = 6378137;
        //短半轴
        double R_MINOR = 6356752.314245179;
        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <returns></returns>
        public GPoint Porject(double lat, double lng)
        {
            double d = Math.PI / 180;
            double max = 1 - 1E-15;
            double sin = Math.Max(Math.Min(Math.Sin(lat * d), max), -max);
            return new GPoint(this.R * lng * d, this.R * Math.Log((1 + sin) / (1 - sin)) / 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public LatLng unPorject(double x, double y)
        {
            var d = 180 / Math.PI;
            return new LatLng((2 * Math.Atan(Math.Exp(y / this.R)) - (Math.PI / 2)) * d, x * d / this.R);
        }
    }

}
