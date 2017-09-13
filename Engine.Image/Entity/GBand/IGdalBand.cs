using OSGeo.GDAL;
using System.Drawing;

namespace Engine.Image.Eneity.GBand
{
    /// <summary>
    /// 存储band数据
    /// </summary>
    public interface IGdalBand
    {
        /// <summary>
        /// 波段号
        /// </summary>
        int BandIndex { get; }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="bandIndex"></param>
        /// <param name="rawData"></param>
        /// <param name="xCount"></param>
        /// <param name="yCount"></param>
        void SetData(int bandIndex, int xCount, int yCount, Band pBand);
        /// <summary>
        /// 获取bitmap2
        /// </summary>
        Bitmap GetBitmap();
        /// <summary>
        /// 获取byte流
        /// </summary>
        byte[,] GetByteData();
        /// <summary>
        /// 横向扫描线
        /// </summary>
        int Width { get; }
        /// <summary>
        /// 纵向扫描线
        /// </summary>
        int Height { get; }
        /// <summary>
        /// 获取byte流
        /// </summary>
        byte[] GetByteBuffer();
    }
}
