using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine.Image.Analysis
{
    /// <summary>
    /// bitmap与byte流的转换
    /// </summary>
    public class BitmapAndByte
    {
        /// <summary>
        /// 线性拉伸，将值分部在0-255之间
        /// </summary>
        /// <param name="array"></param>
        public static byte[]  LinerStretch(double[] array,int classNum=25)
        {
            byte[] values=new byte[array.Length];
            //1.挑选出最大最小值
            double min = array[0];
            double max=array[0];
            for (int count = 0; count < array.Length; count++)
            {
                if (min >=array[count])
                    min = array[count];
                if (max <= array[count])
                    max = array[count];
            }
            //2.将值拉伸至0-255区间
            double interval = (max - min)!=0?(max-min):1;
            //
            for (int count = 0; count < array.Length; count++)
            {
                values[count] = Convert.ToByte(255 * (array[count] - min) / interval);
                int num = values[count] / classNum;
                values[count] = (byte)(num * classNum);
            }
            return values;
        }
        /// <summary>
        /// 将一维图像转换为二维图像(width-x height-y)
        /// </summary>
        public static Bitmap ToGrayBitmap(byte[] source, int width, int height)
        {
            byte[,] array = new byte[width, height];
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    array[i, j] = source[j * width + i];
            //复用
            return ToGrayBitmap(array, width, height);
        }
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
        /// 剪裁 -- 用GDI+
        /// </summary>
        /// <param name="b">原始Bitmap</param>
        /// <param name="StartX">开始坐标X</param>
        /// <param name="StartY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public static Bitmap CutBitmap(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        public static Bitmap ToRgbBitmap(byte[,] dataR, byte[,] dataG, byte[,] dataB)
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
