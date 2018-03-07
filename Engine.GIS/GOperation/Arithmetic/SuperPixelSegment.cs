using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Engine.GIS.GOperation.Arithmetic
{

    public class SLICPKG
    {
        public Bitmap BMP { get; set; }
        public string CENTER { get; set; }
    }

    public class Center
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double L { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double COUNT { get; set; }

        public Center(double X, double Y, double L, double A, double B, double COUNT)
        {
            this.X = X;
            this.Y = Y;
            this.L = L;
            this.A = A;
            this.B = B;
            this.COUNT = COUNT;
        }
    }

    class Bitplane
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public double[,] PixelData { get; set; }

        public Bitplane(Bitplane bitplane)
        {
            this.Width = bitplane.Width;
            this.Height = bitplane.Height;

            for (int y = 0; y < this.Height; ++y)
                for (int x = 0; x < this.Width; ++x)
                    SetPixel(x, y, bitplane.GetPixel(x, y));
        }

        public Bitplane(int w, int h)
        {
            Width = w;
            Height = h;

            PixelData = new double[Height, Width];
        }

        public void max()
        {
            for (int y = 0; y < this.Height; ++y)
                for (int x = 0; x < this.Width; ++x)
                    SetPixel(x, y, double.MaxValue);
        }

        public double GetPixel(int x, int y)
        {
            return PixelData[y, x];
        }

        public void SetPixel(int x, int y, double value)
        {
            PixelData[y, x] = value;
        }

        internal void setAllTo(double v)
        {
            for (int y = 0; y < this.Height; ++y)
                for (int x = 0; x < this.Width; ++x)
                    SetPixel(x, y, v);
        }
    }

    class Bitmap3
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumCh { get; set; }
        public string ImageFileName { get; set; }

        public List<Bitplane> Bitplane = new List<Bitplane>();

        public Bitmap3(Bitmap bmp)
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

        public Bitmap3(int w, int h, int ch)
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
    }

    public class SuperPixelSegment
    {
        public static Center[] ReadCenter(string centerText)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Center[]>(centerText);
        }

        public static SLICPKG Run(Bitmap bmp, double numberOfCenters, double m, Color edgeColor)
        {
            Bitmap3 image = new Bitmap3(bmp);
            Bitmap[] processedImages = new Bitmap[2];
            // Convert RGB TO LAB
            image.RGBtoLAB();
            // Create centers
            double S = Math.Sqrt((image.Width * image.Height) / numberOfCenters);
            Center[] centers = CreateCenters(image, numberOfCenters, S);
            Bitplane labels = new Bitplane(image.Width, image.Height);
            labels.setAllTo(-1);

            for (int iteration = 0; iteration < 10; iteration++)
            {
                Bitplane lenghts = new Bitplane(image.Width, image.Height);
                lenghts.max();

                int i = 0;
                foreach (Center center in centers)
                {
                    for (int k = (int)Math.Round(center.X - S); k < (int)Math.Round(center.X + S); k++)
                        for (int l = (int)Math.Round(center.Y - S); l < (int)Math.Round(center.Y + S); l++)
                            if (k >= 0 && k < image.Width && l >= 0 && l < image.Height)
                            {
                                double L = image.Bitplane[2].GetPixel(k, l);
                                double A = image.Bitplane[1].GetPixel(k, l);
                                double B = image.Bitplane[0].GetPixel(k, l);

                                double Dc = Math.Sqrt(Math.Pow(L - center.L, 2) + Math.Pow(A - center.A, 2) + Math.Pow(B - center.B, 2));
                                double Ds = Math.Sqrt(Math.Pow(l - center.Y, 2) + Math.Pow(k - center.X, 2));
                                double lenght = Math.Sqrt(Math.Pow(Dc, 2) + Math.Pow(Ds / 2, 2) * Math.Pow(m, 2));

                                if (lenght < lenghts.GetPixel(k, l))
                                {
                                    lenghts.SetPixel(k, l, lenght);
                                    labels.SetPixel(k, l, i);
                                }
                            }
                    i++;
                }
                centers = CalculateNewCenters(image, centers, labels);
            }
            //image.GetBitmap().Save(@"D:\Workspace\bmp\o.jpg");
            //image = drawAverage(image, centers, labels);
            //image.LABtoRGB();
            //processedImages[0] = image.GetBitmap(); // Segmented
            image = DrawEdges(image, centers, labels, edgeColor);
            image.LABtoRGB();
            processedImages[0] = image.GetBitmap(); // Segmented with Edge
            string centerText = Newtonsoft.Json.JsonConvert.SerializeObject(centers);
            //
            return new SLICPKG()
            {
                BMP = processedImages[0],
                CENTER = centerText,
            };
        }

        private static Bitmap3 DrawEdges(Bitmap3 image, Center[] centers, Bitplane labels, Color edgeColor)
        {
            Bitplane edges = new Bitplane(image.Width, image.Height);
            Bitmap3 newImage = new Bitmap3(image.Width, image.Height, image.NumCh);

            for (int y = 0; y < image.Height; ++y)
                for (int x = 0; x < image.Width; ++x)
                {
                    if (y == 0 || x == 0)
                    {
                        newImage.Bitplane[2].SetPixel(x, y, (int)Math.Round(image.Bitplane[2].GetPixel(x, y)));
                        newImage.Bitplane[1].SetPixel(x, y, (int)Math.Round(image.Bitplane[1].GetPixel(x, y)));
                        newImage.Bitplane[0].SetPixel(x, y, (int)Math.Round(image.Bitplane[0].GetPixel(x, y)));
                        continue;
                    }
                    double p = labels.GetPixel(x, y);
                    double p1 = labels.GetPixel(x, y - 1);
                    double p2 = labels.GetPixel(x - 1, y - 1);
                    double p3 = labels.GetPixel(x - 1, y);

                    bool edge = false;

                    if (((p1 != p) && (edges.GetPixel(x, y - 1) != 1)) || ((p2 != p) && (edges.GetPixel(x - 1, y - 1) != 1)) || ((p3 != p) && (edges.GetPixel(x - 1, y) != 1)))
                    {
                        edge = true;
                        edges.SetPixel(x, y, 1.0);
                    }

                    if (edge)
                    {
                        newImage.Bitplane[2].SetPixel(x, y, edgeColor.R);
                        newImage.Bitplane[1].SetPixel(x, y, edgeColor.G);
                        newImage.Bitplane[0].SetPixel(x, y, edgeColor.B);
                    }
                    else
                    {
                        newImage.Bitplane[2].SetPixel(x, y, (int)Math.Round(image.Bitplane[2].GetPixel(x, y)));
                        newImage.Bitplane[1].SetPixel(x, y, (int)Math.Round(image.Bitplane[1].GetPixel(x, y)));
                        newImage.Bitplane[0].SetPixel(x, y, (int)Math.Round(image.Bitplane[0].GetPixel(x, y)));
                    }
                }

            return newImage;
        }

        private static Bitmap3 DrawAverage(Bitmap3 image, Center[] centers, Bitplane labels)
        {
            Bitmap3 newImage = new Bitmap3(image.Width, image.Height, image.NumCh);

            for (int y = 0; y < image.Height; ++y)
                for (int x = 0; x < image.Width; ++x)
                    if (labels.GetPixel(x, y) != -1)
                    {
                        newImage.Bitplane[2].SetPixel(x, y, centers[(int)Math.Floor(labels.GetPixel(x, y))].L);
                        newImage.Bitplane[1].SetPixel(x, y, centers[(int)Math.Floor(labels.GetPixel(x, y))].A);
                        newImage.Bitplane[0].SetPixel(x, y, centers[(int)Math.Floor(labels.GetPixel(x, y))].B);
                    }

            return newImage;
        }

        private static Center[] CalculateNewCenters(Bitmap3 image, Center[] centers, Bitplane labels)
        {
            Center[] newCenters = new Center[centers.Length];

            for (int i = 0; i < centers.Length; i++)
                newCenters[i] = new Center(0, 0, 0, 0, 0, 0);

            for (int y = 0; y < image.Height; ++y)
                for (int x = 0; x < image.Width; ++x)
                {
                    int centerIndex = (int)Math.Floor(labels.GetPixel(x, y));
                    if (centerIndex != -1)
                    {
                        double L = image.Bitplane[2].GetPixel(x, y);
                        double A = image.Bitplane[1].GetPixel(x, y);
                        double B = image.Bitplane[0].GetPixel(x, y);

                        newCenters[centerIndex].X += x;
                        newCenters[centerIndex].Y += y;
                        newCenters[centerIndex].L += L;
                        newCenters[centerIndex].A += A;
                        newCenters[centerIndex].B += B;
                        newCenters[centerIndex].COUNT += 1;
                    }
                }

            // Normalize
            foreach (Center center in newCenters)
            {
                if (center.COUNT != 0)
                {
                    center.X = Math.Round(center.X / center.COUNT);
                    center.Y = Math.Round(center.Y / center.COUNT);
                    center.L /= center.COUNT;
                    center.A /= center.COUNT;
                    center.B /= center.COUNT;
                }
            }

            return newCenters;
        }

        private static Center[] CreateCenters(Bitmap3 image, double numberOfCenters, double S)
        {
            List<Center> centers = new List<Center>();
            for (double x = S; x < image.Width - S / 2; x += S)
                for (double y = S; y < image.Height - S / 2; y += S)
                {
                    int xx = (int)Math.Floor(x);
                    int yy = (int)Math.Floor(y);

                    double L = image.Bitplane[2].GetPixel(xx, yy);
                    double A = image.Bitplane[1].GetPixel(xx, yy);
                    double B = image.Bitplane[0].GetPixel(xx, yy);

                    centers.Add(new Center(xx, yy, L, A, B, 0));
                }
            return centers.ToArray();
        }
    }

}
