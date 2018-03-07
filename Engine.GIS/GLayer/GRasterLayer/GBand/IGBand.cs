using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GIS.GLayer.GRasterLayer.GBand
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
    }
}
