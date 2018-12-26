using Engine.GIS.GeoType;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Collections.Generic;

namespace Engine.GIS.File
{
    public class ShpReader
    {

        #region 属性

        bool _isReaded = false;

        ShapefileDataReader _reader = null;

        GBound _bound = null;

        Envelope _envelope = null;

        DbaseFileHeader _header = null;

        FeatureCollection _feaures = new FeatureCollection();

        public FeatureCollection FeaureCollection { get => _feaures; }

        public GBound Bounds { get => _bound; }

        #endregion

        public ShpReader(string shpfile)
        {
            _reader = new ShapefileDataReader(shpfile, GeometryFactory.Default);
            //1.边界读取
            _envelope = _reader.ShapeHeader.Bounds;
            //2.边界转换
            _bound = _toBound(_envelope);
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
                return _feaures;
            while (_reader.Read())
            {
                Feature feature = new Feature { Geometry = _reader.Geometry };
                AttributesTable attrs = new AttributesTable();
                for (int i = 0; i < _header.NumFields; i++)
                    attrs.Add(_header.Fields[i].Name, _reader.GetValue(i));
                feature.Attributes = attrs;
                _feaures.Add(feature);
                _isReaded = true;
            }
            return _feaures;
        }

        public void AddFeature(IGeometry geo, AttributesTable table)
        {
            Feature f = new Feature(geo, table);
            _feaures.Add(f);
        }

        public void Write(string shpfile)
        {
            //var header = ShapefileDataWriter.GetHeader(_feaures.Features.First(), _feaures.Features.Count);
            //var shapeWriter = new ShapefileDataWriter(shpfile, new GeometryFactory()) { Header = header };
            //shapeWriter.Write(_feaures.Features);
        }

    }
}
