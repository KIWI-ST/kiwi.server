using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Linq;

namespace Engine.GIS.Read
{
    public class ShpReader
    {
        ShapefileReader _reader = null;
        IGeometryCollection _geometryCollection;

        public IGeometryCollection GeometryCollection { get => _geometryCollection; }

        public ShpReader(string path)
        {
            _reader = new ShapefileReader(path,GeometryFactory.Default);
        }

        public void Read()
        {
            _geometryCollection = _reader.ReadAll();
        }

        public IGeometry LoadPolygon(IPoint point)
        {
            var result = (from geo in _geometryCollection
                             where geo.OgcGeometryType == OgcGeometryType.Polygon
                             where geo.Contains(point)
                             select geo).Single();
            return result;
        }



    }
}
