using System.Drawing;

namespace Core.GIS.GEntity
{
    public interface IGBand
    {
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
        /// byte数据流
        /// </summary>
        /// <returns></returns>
        byte[] GetByteBuffer();
        /// <summary>
        /// 获取bitmap
        /// </summary>
        /// <returns></returns>
        Bitmap GetBitmap();
        /// <summary>
        /// 卷积操作
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="mask"></param>
        byte Convolution(int centerX, int centerY, int[] mask);
    }
}
