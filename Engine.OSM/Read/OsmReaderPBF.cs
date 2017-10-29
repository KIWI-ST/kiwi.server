using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using OsmSharp;
using OsmSharp.Geo;
using OsmSharp.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.OSM.Read
{

    public delegate void ReadCompleteHandle(List<OsmGeo> nodes, List<OsmGeo> ways, List<OsmGeo> relations);

    public class OsmReaderPBF : OsmReader, IOsmReaderPBF
    {

        List<OsmGeo> _nodeCollection = new List<OsmGeo>();

        List<OsmGeo> _wayCollection = new List<OsmGeo>();

        List<OsmGeo> _relationCollection = new List<OsmGeo>();

        public event ReadCompleteHandle OnComplete;

        PBFOsmStreamSource _source;

        public OsmReaderPBF(string path) : base(path)
        {
            _source = new PBFOsmStreamSource(_steam);
        }

        public void Read()
        {
            var filterWay = from osmGeo in _source where osmGeo.Type == OsmGeoType.Way select osmGeo;
            var features = filterWay.ToFeatureSource();
            var featureCollection = new FeatureCollection();
            var attributesTable = new AttributesTable();
            foreach (var feature in features)
            { 
                featureCollection.Add(new Feature(feature.Geometry, attributesTable));
            }
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
