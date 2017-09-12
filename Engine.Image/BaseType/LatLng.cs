using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Image.BaseType
{
    public class LatLng
    {
        double _lat;
        double _lng;
        public double Lat { get { return _lat; } }
        public double Lng { get { return _lng; } }
        public LatLng(double lat, double lng)
        {
            _lat = lat;
            _lng = lng;
        }
    }
}
