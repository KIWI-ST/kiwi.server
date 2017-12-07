using Engine.GIS.File;
using Engine.GIS.GeoType;
using Engine.GIS.Utils;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.Grid
{
    /// <summary>
    /// 返回切割矢量的进度
    /// </summary>
    /// <param name="vectorName"></param>
    /// <param name="process"></param>
    public delegate void VectorCutHandler(string vectorName, int process);

    /// <summary>
    /// 基于WebMercator投影的格网计算
    /// @author yellow date 2017/11/3
    /// 构建格网切割矢量文件，功能：
    /// -1.给定box区域，构建多尺度的格网
    /// -2.可更新区域格网
    /// </summary>
    public class WebMercatorGrid : IWebMercatorGrid
    {

        #region 事件，属性

        /// <summary>
        /// 事件处理函数
        /// </summary>
        event VectorCutHandler _onVectorCutProcess;

        /// <summary>
        /// 矢量切割进度事件
        /// </summary>
       public event VectorCutHandler OnVectorCutProcess
        {
            add
            {
                lock (_onVectorCutProcess)
                    _onVectorCutProcess += value;
            }

            remove
            {
                lock (_onVectorCutProcess)
                    _onVectorCutProcess -= value;
            }
        }

        #endregion

        #region 投影处理

        /// <summary>
        /// 转换factory
        /// </summary>
        CoordinateTransformationFactory _transformFactory = new CoordinateTransformationFactory();
        //原始坐标系统，wgs84
        IGeographicCoordinateSystem _sourceCoord = GeographicCoordinateSystem.WGS84;
        //投影坐标系
        ICoordinateSystem _targetCoord = ProjectedCoordinateSystem.WebMercator;
        /// <summary>
        /// 投影
        /// </summary>
        ICoordinateTransformation _projectTransform;
        /// <summary>
        /// 逆投影
        /// </summary>
        ICoordinateTransformation _unProjectTransform;

        public WebMercatorGrid()
        {
            _projectTransform = _transformFactory.CreateFromCoordinateSystems(_sourceCoord, _targetCoord);
            _unProjectTransform = _transformFactory.CreateFromCoordinateSystems(_targetCoord, _sourceCoord);
        }

        public Coordinate Project(Coordinate coordinate)
        {
            return _projectTransform.MathTransform.Transform(coordinate);
        }

        public IList<Coordinate> Project(List<Coordinate> coordinates)
        {
            return _projectTransform.MathTransform.TransformList(coordinates);
        }

        public Coordinate UnProject(Coordinate coordinate)
        {
            return _unProjectTransform.MathTransform.Transform(coordinate);
        }

        public IList<Coordinate> UnProject(List<Coordinate> coordinates)
        {
            return _unProjectTransform.MathTransform.TransformList(coordinates);
        }

        #endregion

        #region 格网计算

        Dictionary<int, List<TileElement>> _tileDictionary = new Dictionary<int, List<TileElement>>();

        Transformation _transformation = new Transformation(0.5 / (Math.PI * 6378137), 0.5, -0.5 / (Math.PI * 6378137), 0.5);

        IGeometry _boundGeometry;

        public void Build(Bound bound, int zoom)
        {
            //构建裁剪边界的矩形，用于整体裁剪
            _boundGeometry = bound.ToInsertPolygon();
            //瓦片多尺度缓存
            if (_tileDictionary.ContainsKey(zoom))
                _tileDictionary[zoom].Clear();
            else
                _tileDictionary[zoom] = new List<TileElement>();
            //1.获取坐上右下坐标
            Coordinate p0 = bound.Min;
            Coordinate p1 = bound.Max;
            //2.分尺度计算格网位置
            //2.1 转换成尺度下的pixel
            Coordinate min = LatlngToPoint(p0, zoom);
            Coordinate max = LatlngToPoint(p1, zoom);
            //2.2 计算pixel下边界范围
            Bound pixelBound = new Bound(new List<Coordinate>() { min, max });
            //2.3 通过pixelbound计算range
            Bound range = new Bound(new List<Coordinate>() {
                new Coordinate( (int)Math.Floor(pixelBound.Min.X / _tileSize), (int)Math.Floor(pixelBound.Min.Y / _tileSize)),
                 new Coordinate( (int)Math.Ceiling(pixelBound.Max.X / _tileSize)-1, (int)Math.Ceiling(pixelBound.Max.Y / _tileSize)-1),
            });
            //2.3统计区域内瓦片的编号，边界经纬度等信息
            for (int j = Convert.ToInt32(range.Min.Y); j <= Convert.ToInt32(range.Max.Y); j++)
            {
                for (int i = Convert.ToInt32(range.Min.X); i <= Convert.ToInt32(range.Max.X); i++)
                {
                    //反算每块瓦片的边界经纬度
                    List<Coordinate> coordinates = new List<Coordinate>();
                    coordinates.Add(PointToLatLng(new Coordinate(i * 256, j * 256), zoom));
                    coordinates.Add(PointToLatLng(new Coordinate(i * 256 + 256, j * 256 + 256), zoom));
                    //
                    TileElement tile = new TileElement()
                    {
                        X = i,
                        Y = j,
                        Z = zoom,
                        Bound = new Bound(coordinates)
                    };
                    _tileDictionary[zoom].Add(tile);
                }
            }
            //
            var s = _tileDictionary;
            //裁剪瓦片 并生成对应的道路矩阵
        }
        /// <summary>
        /// 转换当前的经纬度坐标到屏幕像素坐标
        /// </summary>
        private Coordinate LatlngToPoint(Coordinate latlng, int zoom)
        {
            Coordinate c0 = Project(latlng);
            double scale = Scale(zoom);
            //转换经纬度坐标到当前缩放层级的像素坐标
            Coordinate c1 = _transformation.Transform(c0, scale);
            return c1;
        }
        /// <summary>
        /// 屏幕像素坐标转经纬度坐标
        /// </summary>
        private Coordinate PointToLatLng(Coordinate point, int zoom)
        {
            double scale = Scale(zoom);
            Coordinate c0 = _transformation.UnTransform(point, scale);
            Coordinate c1 = UnProject(c0);
            return c1;
        }
        /// <summary>
        /// 获取当前比例尺地图的总分辨率
        /// </summary>
        private double Scale(int zoom)
        {
            return _tileSize * Math.Pow(2, zoom);
        }

        double _tileSize = 256;

        #endregion

        #region 裁剪并绘制矢量瓦片

        /// <summary>
        /// 按瓦片切割，一张瓦片里切割全部的矢量
        /// </summary>
        /// <param name="pShpReader"></param>
        /// <param name="outputDir"></param>
        public void CutShapeOrderByGrid(IShpReader pShpReader, string outputDir)
        {
            FeatureCollection featureCollection = pShpReader.Read();
            foreach (int zoom in _tileDictionary.Keys)
            {
                var tileCollection = _tileDictionary[zoom];
                foreach (var tile in tileCollection)
                {
                    try
                    {
                        //
                        Bitmap bmp = new Bitmap((int)_tileSize, (int)_tileSize);
                        Graphics g = Graphics.FromImage(bmp);
                        Pen pen = new Pen(Color.Black, 3);
                        //
                        for (int i = 0; i < featureCollection.Count; i++)
                        {
                            IFeature f = featureCollection[i];
                            //点
                            if (f.Geometry.OgcGeometryType == OgcGeometryType.Point)
                            {
                                Coordinate point = f.Geometry.Coordinate;
                                if (tile.Bound.PointInPolygon(point))
                                {
                                    //2.2.1 计算点的像素坐标
                                    Coordinate pixel = LatlngToPoint(point, zoom);
                                    //
                                    double deltaX = pixel.X / _tileSize - tile.X;
                                    double deltaY = pixel.Y / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    g.DrawLine(pen, x, x, x, y);
                                }
                                continue;
                            }
                            //线
                            else if (f.Geometry.OgcGeometryType == OgcGeometryType.LineString)
                            {
                                //2.1瓦片裁剪道路
                                List<Coordinate> clipLine = CohenSutherland.GetIntersectedPolyline(f.Geometry.Coordinates, tile.Bound);
                                if (clipLine.Count == 0) continue;
                                int x0 = -1000, y0 = -1000;
                                //2.2 绘制clipLine
                                foreach (Coordinate point in clipLine)
                                {
                                    //2.2.1 计算点的像素坐标
                                    Coordinate pixel = LatlngToPoint(point, zoom);
                                    //
                                    double deltaX = pixel.X / _tileSize - tile.X;
                                    double deltaY = pixel.Y / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    if (x0 == -1000 && y0 == -1000)
                                    {
                                        x0 = x;
                                        y0 = y;
                                        continue;
                                    }
                                    else
                                    {
                                        g.DrawLine(pen, x0, y0, x, y);
                                        x0 = x;
                                        y0 = y;
                                    }
                                }
                            }
                            //面
                            else if (f.Geometry.OgcGeometryType == OgcGeometryType.Polygon)
                            {
                                List<Coordinate> clipPolygon = SutherlandHodgman.GetIntersectedPolygon(f.Geometry.Coordinates, tile.Bound);
                                if (clipPolygon.Count < 3) continue;
                                int x0 = -1000, y0 = -1000;
                                //2.2 绘制clipLine
                                foreach (Coordinate point in clipPolygon)
                                {
                                    //2.2.1 计算点的像素坐标
                                    Coordinate pixel = LatlngToPoint(point, zoom);
                                    //
                                    double deltaX = pixel.X / _tileSize - tile.X;
                                    double deltaY = pixel.Y / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    if (x0 == -1000 && y0 == -1000)
                                    {
                                        x0 = x;
                                        y0 = y;
                                        continue;
                                    }
                                    else
                                    {
                                        g.DrawLine(pen, x0, y0, x, y);
                                        x0 = x;
                                        y0 = y;
                                    }
                                }
                            }
                        }
                        //2.3 保存bmp到指定路径
                        if (!System.IO.Directory.Exists(outputDir + @"\" + zoom))
                            System.IO.Directory.CreateDirectory(outputDir + @"\" + zoom);
                        //根据geometry id存储，获取不到geometry的id，所以只能自定内部序号
                        bmp.Save(outputDir + @"\" + zoom + @"\" + tile.X + "_" + tile.Y + "_" + tile.Z + ".jpg");
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 按矢量切割，一个feature被所有瓦片切割
        /// </summary>
        /// <param name="pShpReader"></param>
        /// <param name="outputDir"></param>
        public void CutShapeOrderByFeature(IShpReader pShpReader, string outputDir)
        {
            FeatureCollection featureCollection = pShpReader.Read();
            for (int i = 0; i < featureCollection.Count; i++)
            {
                IFeature f = featureCollection[i];
                foreach (int zoom in _tileDictionary.Keys)
                {
                    var tileCollection = _tileDictionary[zoom];
                    foreach (var tile in tileCollection)
                    {
                        try
                        {
                            //2.1瓦片裁剪道路
                            List<Coordinate> clipLine = CohenSutherland.GetIntersectedPolyline(f.Geometry.Coordinates, tile.Bound);
                            if (clipLine.Count == 0) continue;
                            //2.2 绘制clipLine
                            Bitmap bmp = new Bitmap((int)_tileSize, (int)_tileSize);
                            Graphics g = Graphics.FromImage(bmp);
                            Pen pen = new Pen(Color.Black, 3);
                            //
                            int x0 = -1000, y0 = -1000;
                            foreach (Coordinate point in clipLine)
                            {
                                //2.2.1 计算点的像素坐标
                                Coordinate pixel = LatlngToPoint(point, zoom);
                                //
                                double deltaX = pixel.X / _tileSize - tile.X;
                                double deltaY = pixel.Y / _tileSize - tile.Y;
                                int x = Convert.ToInt32(deltaX * _tileSize);
                                int y = Convert.ToInt32(deltaY * _tileSize);
                                if (x0 == -1000 && y0 == -1000)
                                {
                                    x0 = x;
                                    y0 = y;
                                    continue;
                                }
                                else
                                {
                                    g.DrawLine(pen, x0, y0, x, y);
                                    x0 = x;
                                    y0 = y;
                                }
                            }
                            //2.3 保存bmp到指定路径
                            if (!System.IO.Directory.Exists(outputDir + @"\" + zoom))
                                System.IO.Directory.CreateDirectory(outputDir + @"\" + zoom);
                            //根据geometry id存储，获取不到geometry的id，所以只能自定内部序号
                            bmp.Save(outputDir + @"\" + zoom + @"\" + tile.X + "_" + tile.Y + "_" + tile.Z + "_" + f.Attributes.GetValues()[0] + ".jpg");
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
