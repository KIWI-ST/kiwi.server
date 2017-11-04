using GeoAPI.Geometries;
using System.Collections.Generic;
using System.Windows;
using Engine.GIS.Grid;

namespace Engine.GIS.Utils
{
    class Extents
    {
        public double Left = 0;
        public double Right = 0;
        public double Bottom = 0;
        public double Top = 0;
    }

    class CohenSutherland
    {
        static byte LEFT = 1;//0001
        static byte RIGHT = 2;//0002
        static byte BOTTOM = 4;//0003
        static byte TOP = 8;//0004

        private static byte Encode(double x, double y, Bound bound)
        {
            double xl = bound.Left,xr = bound.Right,yt = bound.Top,yb = bound.Bottom;
            byte c = 0;
            if (x < xl)
                c |= LEFT;
            if (x > xr)
                c |= RIGHT;
            if (y < yb)
                c |= BOTTOM;
            if (y > yt)
                c |= TOP;
            return c;
        }

        private static List<Coordinate> ClipLine(Coordinate start, Coordinate end, Bound bound)
        {
            double xl = bound.Left,
                        xr = bound.Right,
                        yt = bound.Top,
                        yb = bound.Bottom;
            //
            List<Coordinate> coords = new List<Coordinate>();
            double x1 = start.X, y1 = start.Y, x2 = end.X, y2 = end.Y;
            byte code1 = Encode(x1, y1, bound);
            byte code2 = Encode(x2, y2, bound);
            byte code;
            double x = 0, y = 0;
            while (code1 != 0 || code2 != 0)
            {
                //1.线在窗口外,返回一个空数组
                if ((code1 & code2) != 0)
                    return coords;
                code = code1;
                //找窗口外的点
                if (code1 == 0) code = code2;
                //点在左边
                if ((LEFT & code) != 0)
                {
                    x = xl;
                    y = y1 + (y2 - y1) * (xl - x1) / (x2 - x1);
                }
                //点在右边
                else if ((RIGHT & code) != 0)
                {
                    x = xr;
                    y = y1 + (y2 - y1) * (xr - x1) / (x2 - x1);
                }
                //点在下面
                else if ((BOTTOM & code) != 0)
                {
                    y = yb;
                    x = x1 + (x2 - x1) * (yb - y1) / (y2 - y1);
                }
                else if ((TOP & code) != 0)
                {
                    y = yt;
                    x = x1 + (x2 - x1) * (yt - y1) / (y2 - y1);
                }
                //
                if (code == code1)
                {
                    x1 = x;
                    y1 = y;
                    code1 = Encode(x, y, bound);
                }
                else
                {
                    x2 = x;
                    y2 = y;
                    code2 = Encode(x, y, bound);
                }
            }
            //
            coords.Add(new Coordinate(x1, y1));
            coords.Add(new Coordinate(x2, y2));
            return coords;
        }

       

        public static List<Coordinate> GetIntersectedPolyline(Coordinate[] subjectPolyline, Bound bound)
        {
            List<Coordinate> clipLines = new List<Coordinate>();
            for (int i = 0; i < subjectPolyline.Length - 1; i++)
            {
                Coordinate p0 = subjectPolyline[i];
                Coordinate p1 = subjectPolyline[i + 1];
                List<Coordinate> cliped = ClipLine(p0, p1, bound);
                clipLines.AddRange(cliped);
            }
            return clipLines;
        }

    }
}
