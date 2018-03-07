using Engine.GIS.GeoType;
using GeoAPI.Geometries;
using System.Collections.Generic;

namespace Engine.GIS.GOperation.Arithmetic
{
    /// <summary>
    /// @author yellow
    /// cohen 裁剪算法
    /// </summary>
    class CohenSutherland
    {
        /// <summary>
        /// 0001
        /// </summary>
        static byte LEFT = 1;
        /// <summary>
        /// 0002
        /// </summary>
        static byte RIGHT = 2;
        /// <summary>
        /// 0003
        /// </summary>
        static byte BOTTOM = 4;
        /// <summary>
        /// 0004
        /// </summary>
        static byte TOP = 8;
        /// <summary>
        /// 给当前坐标编码
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bound"></param>
        /// <returns></returns>
        private static byte Encode(double x, double y, GBound bound)
        {
            double xl = bound.Left, xr = bound.Right, yt = bound.Top, yb = bound.Bottom;
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
        /// <summary>
        /// 裁剪线段
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="bound"></param>
        /// <returns></returns>
        private static List<Coordinate> ClipLine(Coordinate start, Coordinate end, GBound bound)
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
        /// <summary>
        /// 裁剪线
        /// </summary>
        /// <param name="subjectPolyline"></param>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static List<Coordinate> GetIntersectedPolyline(Coordinate[] subjectPolyline, GBound bound)
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
