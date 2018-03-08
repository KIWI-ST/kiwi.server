namespace Engine.GIS.GeoType
{
    /// <summary>
    /// 瓦片
    /// @author yellow  date 2017/11/9
    /// </summary>
    public class GTileElement
    {
        /// <summary>
        /// X 方向coordinate 编号
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y 方向coordinate 编号
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 缩放层级
        /// </summary>
        public int Z { get; set; }
        /// <summary>
        /// 瓦片区域矩形边界
        /// </summary>
        public GBound Bound { get; set; }
    }
}
