using GeoAPI.Geometries;

namespace Engine.GIS.GProject
{
    /// <summary>
    /// 计算投影都的像素坐标相对不同缩放层级下容器的像素坐标
    /// @author yellow date 2017/11/9
    /// </summary>
    public class GTransformation
    {
        double _a, _b, _c, _d;

        public GTransformation(double a, double b, double c, double d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public Coordinate Transform(Coordinate point, double scale)
        {
            Coordinate p0 = new Coordinate();
            p0.X = scale * (_a * point.X + _b);
            p0.Y = scale * (_c * point.Y + _d);
            return p0;
        }

        public Coordinate UnTransform(Coordinate point, double scale)
        {
            Coordinate p0 = new Coordinate();
            p0.X = (point.X / scale - _b) / _a;
            p0.Y = (point.Y / scale - _d) / _c;
            return p0;
        }

    }
}
