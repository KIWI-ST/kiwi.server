using System;
using System.Drawing;


namespace Engine.GIS.GOperation.Arithmetic
{
    public class GConvolution
    {
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
                double v = 0;
                for (int i = 0; i < mask.Length; i++)
                    v += rectBmp.GetPixel(i % d, (int)1 / d).ToArgb();
                return Convert.ToByte(v / mask.Length);
            }
        }
    }
}
