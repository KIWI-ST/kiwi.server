using Engine.Image.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Image.Analysis
{
    public class Transformation
    {
        double _a, _b, _c, _d;
        public Transformation(double a, double b, double c, double d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public GPoint Transform(GPoint point, double scale=1)
        {
            GPoint _point = point.Clone();
            _point.X = scale * (_a * point.X + _b);
            _point.Y = scale*(_c * point.Y + _d);
            return _point;
        }

        public GPoint unTransfrom(GPoint point, double scale=1)
        {
            return new GPoint((point.X / scale - this._b) / this._a, (point.Y / scale - this._d) / this._c);
        }

        static double R = 6378137;
        static double _t3857_scale = 0.5 / (Math.PI * R);
        /// <summary>
        /// 默认3857
        /// </summary>
        public static Transformation T3857 = new Transformation(_t3857_scale,0.5, (-1)*_t3857_scale,0.5);

    }

}
