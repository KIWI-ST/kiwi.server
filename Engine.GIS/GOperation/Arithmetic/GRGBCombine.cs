using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine.GIS.GOperation.Arithmetic
{
    public class GRGBCombine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataR"></param>
        /// <param name="dataG"></param>
        /// <param name="dataB"></param>
        /// <returns></returns>
        public static Bitmap Run(byte[,] dataR, byte[,] dataG, byte[,] dataB)
        {
            //
            int width = dataB.GetLength(0);
            int height = dataB.GetLength(1);
            //二维数组转成一维
            byte[] rawValuesB = new byte[width * height];
            byte[] rawValuesR = new byte[width * height];
            byte[] rawValuesG = new byte[width * height];
            int index = 0;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    rawValuesB[index] = dataB[j, i];
                    rawValuesR[index] = dataR[j, i];
                    rawValuesG[index] = dataG[j, i];
                    index++;
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
                    //b g r 顺序排列
                    pixelValues[posScan] = rawValuesB[posReal];
                    pixelValues[posScan + 1] = rawValuesG[posReal];
                    pixelValues[posScan + 2] = rawValuesR[posReal];
                    posReal++;
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
