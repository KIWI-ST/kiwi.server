using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Core.GIS.GEntity
{
    /// <summary>
    /// 封装常用方法的图像基类
    /// </summary>
    public class GBitmap
    {

        #region 属性
        /// <summary>
        /// bitmap宽
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// bitmap高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// bitmap波段
        /// </summary>
        public int NumCh { get; set; }
        /// <summary>
        /// 源文件名
        /// </summary>
        public string ImageFileName { get; set; }
        #endregion

        #region 初始化
        /// <summary>
        /// 
        /// </summary>
        public List<Bitplane> Bitplane = new List<Bitplane>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        public GBitmap(Bitmap bmp)
        {
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                  ImageLockMode.ReadOnly, bmp.PixelFormat);

            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed: NumCh = 1; break;
                case PixelFormat.Format16bppGrayScale: NumCh = 2; break;
                case PixelFormat.Format24bppRgb: NumCh = 3; break;
                case PixelFormat.Format32bppArgb: NumCh = 4; break;
                default: NumCh = 1; break;
            }

            byte[] pixels = new byte[bmp.Width * bmp.Height * NumCh];
            Marshal.Copy(bd.Scan0, pixels, 0, pixels.Length);
            bmp.UnlockBits(bd);

            Width = bmp.Width;
            Height = bmp.Height;

            for (int i = 0; i < NumCh; i++)
                Bitplane.Add(new Bitplane(Width, Height));

            int pos = 0;
            for (int j = 0; j < Height; ++j)
                for (int i = 0; i < Width; ++i)
                    for (int ch = 0; ch < NumCh; ++ch)
                        Bitplane[ch].SetPixel(i, j, pixels[pos++]);

            bmp.Dispose();
        }
        #endregion

        #region

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

        #endregion




      

        internal void RGBtoXYZ()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double R = Bitplane[2].GetPixel(x, y);
                    double G = Bitplane[1].GetPixel(x, y);
                    double B = Bitplane[0].GetPixel(x, y);

                    Rgb myRgb = new Rgb(R, G, B);
                    Xyz myXYZ = myRgb.To<Xyz>();

                    Bitplane[2].SetPixel(x, y, myXYZ.X);
                    Bitplane[1].SetPixel(x, y, myXYZ.Y);
                    Bitplane[0].SetPixel(x, y, myXYZ.Z);
                }
        }

        internal void XYZtoRGB()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double X = Bitplane[2].GetPixel(x, y);
                    double Y = Bitplane[1].GetPixel(x, y);
                    double Z = Bitplane[0].GetPixel(x, y);

                    Xyz myXYZ = new Xyz(X, Y, Z);
                    Rgb myRGB = myXYZ.To<Rgb>();

                    Bitplane[2].SetPixel(x, y, myRGB.R);
                    Bitplane[1].SetPixel(x, y, myRGB.G);
                    Bitplane[0].SetPixel(x, y, myRGB.B);

                }
        }

        internal void LABtoRGB()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double L = Bitplane[2].GetPixel(x, y);
                    double A = Bitplane[1].GetPixel(x, y);
                    double B = Bitplane[0].GetPixel(x, y);

                    Lab myLAB = new Lab(L, A, B);
                    Rgb myRGB = myLAB.To<Rgb>();

                    Bitplane[2].SetPixel(x, y, myRGB.R);
                    Bitplane[1].SetPixel(x, y, myRGB.G);
                    Bitplane[0].SetPixel(x, y, myRGB.B);
                }
        }

        internal void RGBtoLAB()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double R = Bitplane[2].GetPixel(x, y);
                    double G = Bitplane[1].GetPixel(x, y);
                    double B = Bitplane[0].GetPixel(x, y);

                    Rgb myRGB = new Rgb(R, G, B);
                    Lab myLAB = myRGB.To<Lab>();

                    Bitplane[2].SetPixel(x, y, myLAB.L);
                    Bitplane[1].SetPixel(x, y, myLAB.A);
                    Bitplane[0].SetPixel(x, y, myLAB.B);
                }
        }

        public GBitmap(int w, int h, int ch)
        {
            NumCh = ch;
            Width = w;
            Height = h;
            ImageFileName = "";

            for (int i = 0; i < NumCh; i++)
                Bitplane.Add(new Bitplane(Width, Height));
        }

        public Bitmap GetBitmap()
        {
            Bitmap bmp;
            switch (NumCh)
            {
                case 1: bmp = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed); break;
                case 2: bmp = new Bitmap(Width, Height, PixelFormat.Format16bppGrayScale); break;
                case 3: bmp = new Bitmap(Width, Height, PixelFormat.Format24bppRgb); break;
                case 4: bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb); break;
                default: bmp = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed); break;
            }
            byte[] pixels = new byte[Width * Height * NumCh];

            int pos = 0;
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                    for (int ch = 0; ch < NumCh; ++ch)
                        pixels[pos++] = (byte)Bitplane[ch].GetPixel(x, y);


            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            Marshal.Copy(pixels, 0, bd.Scan0, pixels.Length);

            bmp.UnlockBits(bd);

            return bmp;
        }

        public static byte Run(Bitmap bmp, int centerX, int centerY, int[] mask)
        {
            int d = (int)Math.Sqrt(mask.Length);
            if (centerX - d < 0 || centerY - d < 0)
                return bmp.GetPixel(centerX, centerY).R;
            else
            {
                int halfd = (int)Math.Floor(d / 2.0);
                Rectangle rect = new Rectangle(centerX - halfd, centerY - halfd, d, d);
                Bitmap rectBmp = bmp.Clone(rect, bmp.PixelFormat);
                Bitmap3 bitmap3 = new Bitmap3(rectBmp);
                double v = 0;
                for (int i = 0; i < mask.Length; i++)
                    v += bitmap3.Bitplane[0].GetPixel(i % d, (int)1 / d);
                return Convert.ToByte(v / mask.Length);
            }
        }
    }
}
