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

        /// <summary>
        /// 显式读取shpfile
        /// </summary>
        public void Read(string shpfile)
        {
            _reader = new ShapefileDataReader(shpfile, GeometryFactory.Default);
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

        public void AddFeature(IGeometry geo, AttributesTable table)
        {
            Feature f = new Feature(geo, table);
            _feaures.Add(f);
        }

        public void Write(string shpfile)
        {
            var header = ShapefileDataWriter.GetHeader(_feaures.Features.First(), _feaures.Features.Count);
            var shapeWriter = new ShapefileDataWriter(shpfile, new GeometryFactory()){ Header = header };
            shapeWriter.Write(_feaures.Features);
        }

    }
}
