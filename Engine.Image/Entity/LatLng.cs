/// <summary>
/// 经纬度表示
/// </summary>
namespace Engine.Image
{
    public class LatLng
    {
        double _lat,_lng;

        public double Lat { get { return _lat; } }

        public double Lng { get { return _lng; } }

        public LatLng(double lat, double lng)
        {
            _lat = lat;
            _lng = lng;
        }
    }
}
