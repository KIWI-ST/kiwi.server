using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace Engine.GIS.GeoType
{
    /// <summary>
    /// @author yellow
    /// @date 2017/11/3
    /// 记录box边界
    /// </summary>
    public class Bound
    {
        /// <summary>
        /// 左侧
        /// </summary>
        double _left;
        /// <summary>
        /// 底部
        /// </summary>
        double _bottom;
        /// <summary>
        /// 右侧
        /// </summary>
        double _right;
        /// <summary>
        /// 顶部
        /// </summary>
        double _top;
        /// <summary>
        /// box区域最小坐标
        /// </summary>
        Coordinate _min;
        /// <summary>
        /// box区域最大坐标
        /// </summary>
        Coordinate _max;
        /// <summary>
        /// box区域最小坐标
        /// </summary>
        public Coordinate Min { get => _min; }
        /// <summary>
        /// box区域最大坐标
        /// </summary>
        public Coordinate Max { get => _max; }
        /// <summary>
        /// 左侧
        /// </summary>
        public double Left { get => _left; }
        /// <summary>
        /// 底部
        /// </summary>
        public double Bottom { get => _bottom; }
        /// <summary>
        /// 右侧
        /// </summary>
        public double Right { get => _right; }
        /// <summary>
        /// 顶部
        /// </summary>
        public double Top { get => _top; }
        /// <summary>
        /// 根据点构造box区域（extent）
        /// </summary>
        /// <param name="coordinates"></param>
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
            Coordinate[] coordinates = new Coordinate[5] { new Coordinate(_left, _top), new Coordinate(_left, _bottom), new Coordinate(_right, _bottom), new Coordinate(_right, _top), new Coordinate(_left, _top) };
            LinearRing ring = new LinearRing(coordinates);
            Polygon polygon = new Polygon(ring);
            return polygon;
        }
    }
}
