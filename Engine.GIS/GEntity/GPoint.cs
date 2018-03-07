namespace Engine.GIS.Entity
{
    /// <summary>
    /// 标识地理位置的Point
    /// </summary>
    public class GPoint
    {
        /// <summary>
        /// 
        /// </summary>
        private double _x;
        /// <summary>
        /// 
        /// </summary>
        private double _y;
        /// <summary>
        /// 
        /// </summary>
        private double _z;
        /// <summary>
        /// 
        /// </summary>
        private double _w;
        /// <summary>
        /// 
        /// </summary>
        public double X { get { return _x; } set { _x = value; } }
        /// <summary>
        /// 
        /// </summary>
        public double Y { get { return _y; } set { _y = value; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public GPoint(double x, double y, double z = 0, double w = 1)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GPoint Clone()
        {
            return new GPoint(_x, _y, _z, _w);
        }
    }
}
