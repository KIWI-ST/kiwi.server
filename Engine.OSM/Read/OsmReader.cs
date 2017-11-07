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
using Engine.GIS.Entity;

namespace Engine.GIS.Read
{
    public class OsmReader : IOsmReader
    {
        FileStream _steam;

        List<OsmGeo> _nodeCollection = new List<OsmGeo>();

        List<OsmGeo> _wayCollection = new List<OsmGeo>();

        List<OsmGeo> _relationCollection = new List<OsmGeo>();

        public event ReadCompleteHandle OnComplete;

        PBFOsmStreamSource _source;

        public OsmReader(string path)
        {
            _steam = File.OpenRead(path);
            _source = new PBFOsmStreamSource(_steam);
        }

        public void Read(IPolygon polygon)
        {
            //1.矩形区域裁剪
            //var polygonSource = _source.FilterSpatial(polygon, true);
            //}{debug 裁剪矩形包含 80000 多个点，裁剪巨慢，直接使用广东深区域的道路作为学习样本转化
            var waySource = from osmGeo in _source
                            where osmGeo.Type == OsmGeoType.Way || osmGeo.Type == OsmGeoType.Node
                            select osmGeo;
            //2.筛选道路数据
            var featureSrouce = waySource.ToFeatureSource().Where(p =>
            p.Geometry is LineString
            //{
            //    try
            //    {
            //        if (p is ILinearRing)
            //            return false;
            //        //return (p as ILineString).CoordinateSequence.Count > 3;
            //        else if (p is ILineString)
            //            return true;
            //        return false;
            //    }
            //    catch{
            //        return false;
            //    }
            //}
            );
            //3.筛选lineString类型的geometry
            //var lineFeatures = featureSrouce.Where(p => {
            //    p.Geometry.OgcGeometryType == OgcGeometryType.LineString && p.Geometry.
            //    return true;
            //});
            //4.单独存放成shp文件
            var featureCollection = new FeatureCollection();
            var attributesTable = new AttributesTable();
            try {
                foreach (var feature in featureSrouce)
                    featureCollection.Add(new Feature(feature.Geometry, attributesTable));
            }
            catch
            {

            }
            var header = ShapefileDataWriter.GetHeader(featureCollection.Features.First(), featureCollection.Features.Count);
            string targetPath = Directory.GetCurrentDirectory() + @"\DATA\shp\guangdong.shp";
            var shapeWriter = new ShapefileDataWriter(targetPath, new GeometryFactory())
            {
                Header = header
            };
            shapeWriter.Write(featureCollection.Features);
            //读取完毕
            OnComplete(_nodeCollection, _wayCollection, _relationCollection);
        }

        public void Flush(IPolygon polygon)
        {
            string targetPath = Directory.GetCurrentDirectory() + @"\DATA\street\target_guangdong.osm.pbf";
            var filtered = _source.FilterSpatial(polygon, true);// left, top, right, bottom
            using (var stream = new FileInfo(targetPath).Open(FileMode.Create, FileAccess.ReadWrite))
            {
                var target = new PBFOsmStreamTarget(stream);
                target.RegisterSource(filtered);
                target.Pull();
            }
        }


    }
}
