using System.Drawing;

namespace Engine.GIS.GLayer.GRasterLayer.GBand
{
    public interface IGBand
    {

        /// <summary>
        /// reset cursor
        /// </summary>
        void ResetCursor();
        /// <summary>
        /// 获取band所在layer的索引
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 游标方式读取band pixel value
        /// </summary>
        /// <returns>未拉伸值（原始图像值）</returns>
        (int x, int y, int value) Next();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        byte GetRawPixel(int x, int y);
        /// <summary>
        /// 最小值
        /// </summary>
        double Min { get; }
        /// <summary>
        /// 最大值
        /// </summary>
        double Max { get; }
        /// <summary>
        /// 
        /// </summary>
        string BandName { get; set; }
        /// <summary>
        ///  图像宽度
        /// </summary>
        int Width { get; }
        /// <summary>
        ///  图像高度
        /// </summary>
        int Height { get; }
        /// <summary>
        /// 图像byte二维数组
        /// </summary>
        /// <returns></returns>
        byte[,] GetByteData();
        /// <summary>
        /// get raw byte data
        /// </summary>
        /// <returns></returns>
        byte[] GetByteBuffer();
        /// <summary>
        /// raw byte value
        /// </summary>
        /// <returns></returns>
        byte[] GetRawByteBuffer();
        /// <summary>
        /// 获取bitmap
        /// </summary>
        /// <returns></returns>
        Bitmap GetBitmap();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double[] GetPixelDoubleByMask(int x, int y, int row = 3, int col = 3);
    }
}
