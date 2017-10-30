using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using OsmSharp;
using OsmSharp.Geo;
using OsmSharp.Streams;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.GIS.Read
{
    public class OsmReaderPBF : IOsmReaderPBF
    {
        FileStream _steam;

        List<OsmGeo> _nodeCollection = new List<OsmGeo>();

        List<OsmGeo> _wayCollection = new List<OsmGeo>();

        List<OsmGeo> _relationCollection = new List<OsmGeo>();

        public event ReadCompleteHandle OnComplete;

        PBFOsmStreamSource _source;

        public OsmReaderPBF(string path) 
        {
            _steam = File.OpenRead(path);
            _source = new PBFOsmStreamSource(_steam);
        }

        public void Read(IPolygon polygon)
        {
            //1.矩形区域裁剪
            var polygonSource = _source.FilterSpatial(polygon, true);
            //2.筛选道路数据
            var featureSrouce = polygonSource.ToFeatureSource();
            //3.筛选lineString类型的geometry
            var lineFeatures = featureSrouce.Where(p => p.Geometry is LineString);
            //4.单独存放成shp文件
            var featureCollection = new FeatureCollection();
            var attributesTable = new AttributesTable();
            foreach (var feature in lineFeatures)
                featureCollection.Add(new Feature(feature.Geometry, attributesTable));
            var header = ShapefileDataWriter.GetHeader(featureCollection.Features.First(), featureCollection.Features.Count);
            var shapeWriter = new ShapefileDataWriter("luxembourg.shp", new GeometryFactory())
            {
                Header = header
            };
            shapeWriter.Write(featureCollection.Features);
            //读取完毕
            OnComplete(_nodeCollection,_wayCollection,_relationCollection);
        }

    }
}
