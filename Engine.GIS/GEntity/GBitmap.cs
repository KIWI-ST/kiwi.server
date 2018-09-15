using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine.GIS.GEntity
{
    /// <summary>
    /// 提供bitmap操作基类
    /// </summary>
    public class GBitmap
    {
        /// <summary>
        /// 将一个字节数组转换为24位真彩色图
        /// </summary>
        /// <param name="imageArray">字节数组</param>
        /// <param name="width">图像的宽度</param>
        /// <param name="height">图像的高度</param>
        /// <returns>位图对象</returns>
        public static Bitmap ToGrayBitmap(byte[,] imageArray, int width, int height)
        {
            //将用户指定的imageArray二维数组转换为一维数组rawValues
            byte[] rawValues = new byte[width * height];
            int index = 0;
            int imgwidth = imageArray.GetLength(0);
            int imgheight = imageArray.GetLength(1);
            for (int i = 0; i < imgheight; i++)
                for (int j = 0; j < imgwidth; j++)
                {
                    rawValues[index++] = imageArray[j, i];
                }
            //申请目标位图的变量，并将其内存区域锁定
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            //获得图像的参数
            int stride = bmpData.Stride; //扫描线的宽度
            // int offset = stride - width;  转换为8位灰度图时
            int offset = stride - width * 3; //显示宽度与扫描线宽度的间隙，
            //与8位灰度图不同width*3很重要，因为此时一个像素占3字节
            IntPtr iptr = bmpData.Scan0; //获得 bmpData的内存起始位置
            int scanBytes = stride * height; //用Stride宽度,表示内存区域的大小
            //下面把原始的显示大小字节数组转换为内存中的实际存放的字节数组
            int posScan = 0, posReal = 0; //分别设置两个位置指针指向源数组和目标数组
            byte[] pixelValues = new byte[scanBytes]; //为目标数组分配内存
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    pixelValues[posScan] = pixelValues[posScan + 1] = pixelValues[posScan + 2] = rawValues[posReal++];
                    posScan += 3;
                }
                posScan += offset; //行扫描结束，要将目标位置指针移过那段间隙
            }
            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData); //解锁内存区域
            return bmp;
        }
        /// <summary>
        /// 将一个字节数组转换为24位真彩色图
        /// </summary>
        /// <param name="imageArray">字节数组</param>
        /// <param name="width">图像的宽度</param>
        /// <param name="height">图像的高度</param>
        /// <returns>位图对象</returns>
        public static Bitmap ToGrayBitmap(double[,] imageArray, int width, int height)
        {
            //将用户指定的imageArray二维数组转换为一维数组rawValues
            byte[] rawValues = new byte[width * height];
            int index = 0;
            int imgwidth = imageArray.GetLength(0);
            int imgheight = imageArray.GetLength(1);
            for (int i = 0; i < imgheight; i++)
                for (int j = 0; j < imgwidth; j++)
                {
                    rawValues[index++] = Convert.ToByte(imageArray[j, i] * 255) ;
                }
            //申请目标位图的变量，并将其内存区域锁定
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            //获得图像的参数
            int stride = bmpData.Stride; //扫描线的宽度
            // int offset = stride - width;  转换为8位灰度图时
            int offset = stride - width * 3; //显示宽度与扫描线宽度的间隙，
            //与8位灰度图不同width*3很重要，因为此时一个像素占3字节
            IntPtr iptr = bmpData.Scan0; //获得 bmpData的内存起始位置
            int scanBytes = stride * height; //用Stride宽度,表示内存区域的大小
            //下面把原始的显示大小字节数组转换为内存中的实际存放的字节数组
            int posScan = 0, posReal = 0; //分别设置两个位置指针指向源数组和目标数组
            byte[] pixelValues = new byte[scanBytes]; //为目标数组分配内存
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    pixelValues[posScan] = pixelValues[posScan + 1] = pixelValues[posScan + 2] = rawValues[posReal++];
                    posScan += 3;
                }
                posScan += offset; //行扫描结束，要将目标位置指针移过那段间隙
            }
            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData); //解锁内存区域
            return bmp;
        }
    }
}
