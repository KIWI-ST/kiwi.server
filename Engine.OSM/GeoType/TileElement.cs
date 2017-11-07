namespace Engine.GIS.GeoType
{
    /// <summary>
    /// 瓦片元素
    /// </summary>
    public class TileElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Bound Bound { get; set; }
    }
}
