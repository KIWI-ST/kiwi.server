using Engine.Image.Eneity.GBand;

namespace Engine.Image.Eneity.GLayer
{
    /// <summary>
    /// gdal图层表示
    /// </summary>
    public interface IGdalLayer
    {
        /// <summary>
        /// y方向长度
        /// </summary>
        int YSize { get; }
        /// <summary>
        /// x方向长度
        /// </summary>
        int XSize { get; }
        /// <summary>
        /// gdal波段集合
        /// </summary>
        Container<IGdalBand> BandCollection { get; }
        /// <summary>
        /// 创建layer
        /// </summary>
        void ReadFromFile(string filePath);
        /// <summary>
        /// 保存图像
        /// </summary>
        void SaveToFile(string filePath, byte[] byteData);
    }
}
