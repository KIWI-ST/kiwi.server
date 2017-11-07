using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Linq;

namespace Engine.GIS.Read
{
    public class ShpReader
    {
        ShapefileDataReader _reader = null;

        FeatureCollection _feaures = new FeatureCollection();

        public FeatureCollection FeaureCollection { get => _feaures;}

        public ShpReader(string path)
        {
            _reader = new ShapefileDataReader(path, GeometryFactory.Default);
        }

        public void Read()
        {
            var header = _reader.DbaseHeader;
            while (_reader.Read())
            {
                Feature feature = new Feature { Geometry = _reader.Geometry};
                AttributesTable attrs = new AttributesTable();
                for (int i = 0; i < header.NumFields; i++)
                    attrs.AddAttribute(header.Fields[i].Name, _reader.GetValue(i));
                feature.Attributes = attrs;
                _feaures.Add(feature);
            }
        }

    }
}
