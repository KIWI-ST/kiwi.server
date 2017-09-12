using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Image.BaseType
{
    public class Point
    {
        double _x, _y, _z, _w;
        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public Point(double x, double y, double z = 0, double w = 1)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        public Point Clone() {
            return new Point(this.X, this.Y, this._z, this._w);
        }
    }
}
