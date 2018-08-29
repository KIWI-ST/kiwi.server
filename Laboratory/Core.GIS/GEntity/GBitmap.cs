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
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L10
        /// </summary>
        private double Kappa = 24389 / 27;
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L9
        /// </summary>
        private double Epsilon = 216 / 24389;
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L11
        /// </summary>
        private double[] WhiteReference = new double[3] { 95.047, 100.000, 108.883 };
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
        /// 初始化
        /// </summary>
        public GBitmap() { }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="ch"></param>
        public GBitmap(int w, int h, int ch)
        {
            NumCh = ch;
            Width = w;
            Height = h;
            ImageFileName = "";

            for (int i = 0; i < NumCh; i++)
                Bitplane.Add(new Bitplane(Width, Height));
        }
        #endregion

        #region 图像操作
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
        /// 返回bitmap对象
        /// </summary>
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
        /// <summary>
        /// 图像卷积操作,返回指定点和mask的模版的卷积计算结果
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="mask"></param>
        /// <returns>byte</returns>
        public byte Convolution(int centerX, int centerY, int[] mask)
        {
            Bitmap bmp = this.GetBitmap();
            int d = (int)Math.Sqrt(mask.Length);
            if (centerX - d < 0 || centerY - d < 0)
                return bmp.GetPixel(centerX, centerY).R;
            else
            {
                int halfd = (int)Math.Floor(d / 2.0);
                Rectangle rect = new Rectangle(centerX - halfd, centerY - halfd, d, d);
                Bitmap rectBmp = bmp.Clone(rect, bmp.PixelFormat);
                GBitmap bitmap3 = new GBitmap(rectBmp);
                double v = 0;
                for (int i = 0; i < mask.Length; i++)
                    v += bitmap3.Bitplane[0].GetPixel(i % d, (int)1 / d);
                return Convert.ToByte(v / mask.Length);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/LabConverter.cs#L41
        /// </summary>
        private double PivotXyz(double n)
        {
            return n > Epsilon ? CubicRoot(n) : (Kappa * n + 16) / 116;
        }
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L70
        /// </summary>
        private double PivotRgb(double n)
        {
            return (n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92) * 100.0;
        }
        /// <summary>
        /// 
        /// </summary>
        private static double CubicRoot(double n)
        {
            return Math.Pow(n, 1.0 / 3.0);
        }
        /// <summary>
        /// 限定n在rgb值范围
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L62
        /// </summary>
        private double ToRgb(double n)
        {
            var result = 255.0 * n;
            if (result < 0) return 0;
            if (result > 255) return 255;
            return result;
        }
        /// <summary>
        /// 转换rbg到xyz空间
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L27
        /// </summary>
        private double[] ConvertRgbToXyz(double r, double g, double b)
        {
            double R = PivotRgb(r / 255.0);
            double G = PivotRgb(g / 255.0);
            double B = PivotRgb(b / 255.0);
            // Observer. = 2°, Illuminant = D65
            double X = R * 0.4124 + G * 0.3576 + B * 0.1805;
            double Y = R * 0.2126 + G * 0.7152 + B * 0.0722;
            double Z = R * 0.0193 + G * 0.1192 + B * 0.9505;
            //
            return new double[3] { X, Y, Z };
        }
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L39
        /// </summary>
        private double[] ConvertXyzToRgb(double x, double y, double z)
        {
            // (Observer = 2°, Illuminant = D65)
            x = x / 100.0;
            y = y / 100.0;
            z = z / 100.0;
            double r = x * 3.2406 + y * -1.5372 + z * -0.4986;
            double g = x * -0.9689 + y * 1.8758 + z * 0.0415;
            double b = x * 0.0557 + y * -0.2040 + z * 1.0570;
            r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
            g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
            b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b;
            return new double[3] { ToRgb(r), ToRgb(g), ToRgb(b) };
        }
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/LabConverter.cs#L7
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/XyzConverter.cs#L11
        /// </summary>
        private double[] ConvertRgbToLab(double r,double g,double b)
        {
            double[] xyz = ConvertRgbToXyz(r, g, b);
            double x = PivotXyz(xyz[0] / WhiteReference[0]);
            double y = PivotXyz(xyz[1] / WhiteReference[1]);
            double z = PivotXyz(xyz[2] / WhiteReference[2]);
            double L = Math.Max(0, 116 * y - 16);
            double A = 500 * (x - y);
            double B = 200 * (y - z);
            return new double[3] { L, A, B };
        }
        /// <summary>
        /// https://github.com/THEjoezack/ColorMine/blob/0b445272239ab816e616c719f844b3dba18bfdbc/ColorMine/ColorSpaces/Conversions/LabConverter.cs#L41
        /// </summary>
        private double[] ConvertLabToRgb(double l,double a,double b)
        {
            double y = (l + 16.0) / 116.0;
            double x = a/ 500.0 + y;
            double z = y - b / 200.0;
            double x3 = x * x * x;
            double z3 = z * z * z;
            double X = WhiteReference[0] * (x3 > Epsilon ? x3 : (x - 16.0 / 116.0) / 7.787);
            double Y = WhiteReference[1] * (l > (Kappa * Epsilon) ? Math.Pow(((l + 16.0) / 116.0), 3) : l / Kappa);
            double Z = WhiteReference[2] * (z3 > Epsilon ? z3 : (z - 16.0 / 116.0) / 7.787);
            return ConvertXyzToRgb(X, Y, Z);
        }
        #endregion

        #region 格式转换
        /// <summary>
        /// 转换rgb到xyz空间
        /// </summary>
        internal void RGBtoXYZ()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double R = Bitplane[2].GetPixel(x, y);
                    double G = Bitplane[1].GetPixel(x, y);
                    double B = Bitplane[0].GetPixel(x, y);
                    double[] xyz = ConvertRgbToXyz(R, G, B);
                    Bitplane[2].SetPixel(x, y, xyz[0]);
                    Bitplane[1].SetPixel(x, y, xyz[1]);
                    Bitplane[0].SetPixel(x, y, xyz[2]);
                }
        }
        /// <summary>
        /// 转换xyz到rbg
        /// </summary>
        internal void XYZtoRGB()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double X = Bitplane[2].GetPixel(x, y);
                    double Y = Bitplane[1].GetPixel(x, y);
                    double Z = Bitplane[0].GetPixel(x, y);
                    double[] rgb = ConvertXyzToRgb(X, Y, Z);
                    Bitplane[2].SetPixel(x, y, rgb[0]);
                    Bitplane[1].SetPixel(x, y, rgb[1]);
                    Bitplane[0].SetPixel(x, y, rgb[2]);
                }
        }
        /// <summary>
        /// 
        /// </summary>
        internal void LABtoRGB()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double L = Bitplane[2].GetPixel(x, y);
                    double A = Bitplane[1].GetPixel(x, y);
                    double B = Bitplane[0].GetPixel(x, y);
                    double[] rgb = ConvertLabToRgb(L, A, B);
                    Bitplane[2].SetPixel(x, y, rgb[0]);
                    Bitplane[1].SetPixel(x, y, rgb[1]);
                    Bitplane[0].SetPixel(x, y, rgb[2]);
                }
        }
        /// <summary>
        /// 
        /// </summary>
        internal void RGBtoLAB()
        {
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    double R = Bitplane[2].GetPixel(x, y);
                    double G = Bitplane[1].GetPixel(x, y);
                    double B = Bitplane[0].GetPixel(x, y);
                    double[] lab = ConvertRgbToLab(R, G, B);
                    Bitplane[2].SetPixel(x, y, lab[0]);
                    Bitplane[1].SetPixel(x, y, lab[1]);
                    Bitplane[0].SetPixel(x, y, lab[2]);
                }
        }
        #endregion
    }
}
