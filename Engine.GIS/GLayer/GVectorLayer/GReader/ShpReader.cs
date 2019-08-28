using System.Collections.Generic;
using Engine.GIS.GeoType;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Engine.GIS.File
{
    public class ShpReader
    {

        #region 属性

        bool _isReaded = false;

        readonly ShapefileDataReader _reader = null;

        readonly Envelope _envelope = null;

        readonly DbaseFileHeader _header = null;

        public FeatureCollection FeaureCollection { get; } = new FeatureCollection();

        public GBound Bounds { get; } = null;

        #endregion

        public ShpReader(string shpfile)
        {
            _reader = new ShapefileDataReader(shpfile, GeometryFactory.Default);
            //1.边界读取
            _envelope = _reader.ShapeHeader.Bounds;
            //2.边界转换
            Bounds = _toBound(_envelope);
            //3.顶点
            _header = _reader.DbaseHeader;
        }

        public GBound _toBound(Envelope envelope)
        {
            List<Coordinate> coords = new List<Coordinate>()
            {
                new Coordinate(envelope.MinX,envelope.MinY),
                new Coordinate(envelope.MaxX,envelope.MaxY),
            };
           return new GBound(coords);
        }

        /// <summary>
        /// 显式读取shpfile
        /// </summary>
        public FeatureCollection Read()
        {
            if (_isReaded)
                return FeaureCollection;
            while (_reader.Read())
            {
                Feature feature = new Feature { Geometry = _reader.Geometry };
                AttributesTable attrs = new AttributesTable();
                for (int i = 0; i < _header.NumFields; i++)
                    attrs.Add(_header.Fields[i].Name, _reader.GetValue(i));
                feature.Attributes = attrs;
                FeaureCollection.Add(feature);
                _isReaded = true;
            }
            return FeaureCollection;
        }

        public void AddFeature(IGeometry geo, AttributesTable table)
        {
            Feature f = new Feature(geo, table);
            FeaureCollection.Add(f);
        }

        public void Write(string shpfile)
        {
            //var header = ShapefileDataWriter.GetHeader(_feaures.Features.First(), _feaures.Features.Count);
            //var shapeWriter = new ShapefileDataWriter(shpfile, new GeometryFactory()) { Header = header };
            //shapeWriter.Write(_feaures.Features);
        }

    }
}
