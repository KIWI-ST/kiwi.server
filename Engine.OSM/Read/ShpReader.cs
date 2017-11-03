using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using System.IO;

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
