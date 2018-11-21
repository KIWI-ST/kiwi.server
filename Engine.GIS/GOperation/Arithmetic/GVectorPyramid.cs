using Engine.GIS.GeoType;
using Engine.GIS.GProject;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;


namespace Engine.GIS.GOperation
{
    /// <summary>
    /// 矢量切割进度
    /// </summary>
    /// <param name="vectorName"></param>
    /// <param name="process"></param>
    public delegate void VectorCutHandler(string vectorName, int process);

    /// <summary>
    /// 矢量金字塔
    /// </summary>
    public class VectorPyramid
    {
        /// <summary>
        /// 构造函数
        /// example var layer = new WebMercatorGridLayer(）
        /// </summary>
        /// <param name="bound"></param>
        public VectorPyramid(GBound bound = null)
        {
            if (bound == null)
                bound = new GBound(new List<Coordinate>() { new Coordinate(-180, 90), new Coordinate(180, -90) });
            for (int i = 0; i <= 19; i++)
                Build(bound, i);
        }
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
        /// <summary>
        /// 格网瓦片缓存，缩放层级 : 瓦片集合
        /// </summary>
        public Dictionary<int, List<GTileElement>> TileDictionary { get; } = new Dictionary<int, List<GTileElement>>();

        /// <summary>
        /// web墨卡托投影
        /// </summary>
        public WebMercatorProjection Projection { get; } = new WebMercatorProjection();

        /// <summary>
        /// 构建当前缩放层级的瓦片
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="zoom"></param>
        void Build(GBound bound, int zoom)
        {
            int _tileSize = Projection.TileSize;
            //构建裁剪边界的矩形，用于整体裁剪
            IGeometry _boundGeometry = bound.ToInsertPolygon();
            //瓦片多尺度缓存
            if (TileDictionary.ContainsKey(zoom))
                TileDictionary[zoom].Clear();
            else
                TileDictionary[zoom] = new List<GTileElement>();
            //1.获取坐上右下坐标
            Coordinate p0 = bound.Min;
            Coordinate p1 = bound.Max;
            //2.分尺度计算格网位置
            //2.1 转换成尺度下的pixel
            Coordinate min = Projection.LatlngToPoint(p0, zoom);
            Coordinate max = Projection.LatlngToPoint(p1, zoom);
            //2.2 计算pixel下边界范围
            GBound pixelBound = new GBound(new List<Coordinate>() { min, max });
            //2.3 通过pixelbound计算range
            GBound range = new GBound(new List<Coordinate>() {
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
                    coordinates.Add(Projection.PointToLatLng(new Coordinate(i * 256, j * 256), zoom));
                    coordinates.Add(Projection.PointToLatLng(new Coordinate(i * 256 + 256, j * 256 + 256), zoom));
                    //
                    GTileElement tile = new GTileElement()
                    {
                        X = i,
                        Y = j,
                        Z = zoom,
                        Bound = new GBound(coordinates)
                    };
                    TileDictionary[zoom].Add(tile);
                }
            }
        }
    }
}
