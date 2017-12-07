using Engine.GIS.GeoType;
using NetTopologySuite.Features;

namespace Engine.GIS.File
{
    /// <summary>
    /// shapefile读取
    /// </summary>
    public interface IShpReader
    {
        /// <summary>
        /// 边界获取
        /// </summary>
        Bound Bounds { get; }
        /// <summary>
        /// shp 读取
        /// </summary>
        FeatureCollection Read();
    }
}
