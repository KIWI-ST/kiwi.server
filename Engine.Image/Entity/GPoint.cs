/// <summary>
/// 标识地理位置的Point
/// </summary>
namespace Engine.Image.Entity
{
    public class GPoint
    {
        double _x, _y, _z, _w;

        public double X { get { return _x; } set { _x = value; } }

        public double Y { get { return _y; } set { _y = value; } }

        public GPoint(double x, double y, double z = 0, double w = 1)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        public GPoint Clone()
        {
            return new GPoint(this.X, this.Y, this._z, this._w);
        }
    }
}
