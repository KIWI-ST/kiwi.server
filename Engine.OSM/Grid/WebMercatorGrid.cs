using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using Engine.GIS.Utils;
using System.Drawing;

namespace Engine.GIS.Grid
{

    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Bound Bound { get; set; }
    }

    public class Bound
    {
        double _left, _bottom, _right, _top;

        Coordinate _min, _max;

        public Coordinate Min { get => _min; }

        public Coordinate Max { get => _max; }

        public double Left { get => _left;}

        public double Bottom { get => _bottom;}

        public double Right { get => _right;}

        public double Top { get => _top;}

        public Bound(List<Coordinate> coordinates)
        {
            foreach (Coordinate p in coordinates)
                Extend(p);
        }
        /// <summary>
        /// 计算外轮廓
        /// </summary>
        private void Extend(Coordinate point)
        {
            if (_min == null && _max == null)
            {
                _min = point.Clone() as Coordinate;
                _max = point.Clone() as Coordinate;
            }
            else
            {
                _min.X = Math.Min(point.X, _min.X);
                _max.X = Math.Max(point.X, _max.X);
                _min.Y = Math.Min(point.Y, _min.Y);
                _max.Y = Math.Max(point.Y, _max.Y);
            }
            _left = _min.X;
            _bottom = _min.Y;
            _right = _max.X;
            _top = _max.Y;
        }
        //转换成多边形，便于裁剪计算
        public Coordinate[] ToClipPolygon()
        {
            return new Coordinate[4] {
                new Coordinate(_left,_top),
                new Coordinate(_left,_bottom),
                new Coordinate(_right,_bottom),
                new Coordinate(_right,_top)
            };
        }
        //转换成内判断矩形，用于筛选
        public Polygon ToInsertPolygon()
        {
            Coordinate[] coordinates = new Coordinate[5] { new Coordinate(_left, _top), new Coordinate(_left, _bottom), new Coordinate(_right, _bottom), new Coordinate(_right, _top) , new Coordinate(_left, _top) };
            LinearRing ring = new LinearRing(coordinates);
            Polygon polygon = new Polygon(ring);
            return polygon;
        }

    }

    public class Transformation
    {
        double _a, _b, _c, _d;

        public Transformation(double a, double b, double c, double d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public Coordinate Transform(Coordinate point, double scale)
        {
            Coordinate p0 = new Coordinate();
            p0.X = scale * (_a * point.X + _b);
            p0.Y = scale * (_c * point.Y + _d);
            return p0;
        }

        public Coordinate UnTransform(Coordinate point, double scale)
        {
            Coordinate p0 = new Coordinate();
            p0.X = (point.X / scale - _b) / _a;
            p0.Y = (point.Y / scale - _d) / _c;
            return p0;
        }

    }


    /// <summary>
    /// 基于WebMercator投影的格网计算
    /// @author yellow date 2017/11/3
    /// 构建格网切割矢量文件，功能：
    /// -1.给定box区域，构建多尺度的格网
    /// -2.可更新区域格网
    /// </summary>
    public class WebMercatorGrid : IWebMercatorGrid
    {

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

        Dictionary<int, List<Tile>> _tileDictionary = new Dictionary<int, List<Tile>>();

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
                _tileDictionary[zoom] = new List<Tile>();
            //1.获取坐上右下坐标
            Coordinate p0 = bound.Min;
            Coordinate p1 = bound.Max;
            //2.分尺度计算格网位置
            //2.1 转换成尺度下的pixel
            Coordinate min = LatlngToPoint(p0, zoom);
            Coordinate max = LatlngToPoint(p1, zoom);
            //2.2 计算pixel下边界范围
            Bound pixelBound = new Bound(new List<Coordinate>() {min,max});
            //2.3 通过pixelbound计算range
            Bound range = new Bound(new List<Coordinate>() {
                new Coordinate( (int)Math.Floor(pixelBound.Min.X / _tileSize), (int)Math.Floor(pixelBound.Min.Y / _tileSize)),
                 new Coordinate( (int)Math.Ceiling(pixelBound.Max.X / _tileSize)-1, (int)Math.Ceiling(pixelBound.Max.Y / _tileSize)-1),
            });
            //2.3统计区域内瓦片的编号，边界经纬度等信息
            for (int j = Convert.ToInt32(range.Min.Y); j <= Convert.ToInt32(range.Max.Y); j++)
            {
                for (int i = Convert.ToInt32(range.Min.X); i <=Convert.ToInt32(range.Max.X); i++)
                {
                    //反算每块瓦片的边界经纬度
                    List<Coordinate> coordinates = new List<Coordinate>();
                    coordinates.Add(PointToLatLng(new Coordinate(i * 256, j * 256), zoom));
                    coordinates.Add(PointToLatLng(new Coordinate(i * 256 + 256, j * 256 + 256), zoom));
                    //
                    Tile tile = new Tile()
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

        public void CutShape(IGeometryCollection geometries,string outputDir)
        {
            int idx = 0;
            //1.筛选在区域内的geometry，即与矩形相交
            var result = from geo in geometries where geo.OgcGeometryType == OgcGeometryType.LineString select geo;
            //2.裁剪
            foreach (var geometry in result)
            {
                idx++;
                foreach (int zoom in _tileDictionary.Keys)
                {
                    var tileCollection = _tileDictionary[zoom];
                    foreach(var tile in tileCollection)
                    {
                        try
                        {
                            //2.1瓦片裁剪道路
                            List<Coordinate> clipLine = CohenSutherland.GetIntersectedPolyline(geometry.Coordinates, tile.Bound);
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
                            bmp.Save(outputDir + @"\" + zoom + @"\" + tile.X + "_" + tile.Y + "_" + tile.Z + "_" + idx + ".jpg");
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            //
        }
        #endregion

    }
}
