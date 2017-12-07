using GeoAPI.Geometries;
using OsmSharp;
using System.Collections.Generic;

namespace Engine.GIS.File
{
    /// <summary>
    /// 读取成功后的回调委托
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="ways"></param>
    /// <param name="relations"></param>
    public delegate void ReadCompleteHandle(List<OsmGeo> nodes, List<OsmGeo> ways, List<OsmGeo> relations);

    public interface IOsmReader
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="geoType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Read(IPolygon polygon);
        /// <summary>
        /// 写入裁剪后写入文件
        /// </summary>
        /// <param name="polygon"></param>
        void Flush(IPolygon polygon);
        /// <summary>
        /// 读取完成后的回调事件
        /// </summary>
        event ReadCompleteHandle OnComplete;
    }
}
