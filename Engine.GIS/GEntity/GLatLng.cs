namespace Engine.GIS.Entity
{
    /// <summary>
    /// 经纬度
    /// </summary>
    public class GLatLng
    {
        /// <summary>
        /// 创建经纬度实例
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        public GLatLng(double lat, double lng)
        {
            _lat = lat;
            _lng = lng;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        private double _lat;
        /// <summary>
        /// 经度
        /// </summary>
        private double _lng;
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get { return _lat; } }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get { return _lng; } }
    }
}
