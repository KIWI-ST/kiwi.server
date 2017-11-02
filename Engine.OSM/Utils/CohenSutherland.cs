using System.Collections.Generic;
using System.Windows;

namespace Engine.GIS.Utils
{

    public class Extents
    {
        public double Left = 0;
        public double Right = 0;
        public double Bottom = 0;
        public double Top = 0;
    }

    class CohenSutherland
    {
        //方位编码
        static byte INSIDE = 0;//0000
        static byte LEFT = 1;//0001
        static byte RIGHT = 2;//0002
        static byte BOTTOM = 4;//0003
        static byte TOP = 8;//0004

        private static byte ComputeOutCode(Extents extents, double x, double y)
        {
            byte code = INSIDE;
            if (x < extents.Left)           // to the left of clip window
                code |= LEFT;
            else if (x > extents.Right)     // to the right of clip window
                code |= RIGHT;
            if (y < extents.Bottom)         // below the clip window
                code |= BOTTOM;
            else if (y > extents.Top)       // above the clip window
                code |= TOP;
            return code;
        }

        private static List<Point> CohenSutherlandLineClip(Point p0, Point p1, Extents extents)
        {
            double x0 = p0.X;
            double y0 = p0.Y;
            double x1 = p1.X;
            double y1 = p1.Y;
            //
            byte outcode0 = CohenSutherland.ComputeOutCode(extents, x0, y0);
            byte outcode1 = CohenSutherland.ComputeOutCode(extents, x1, y1);
            bool accept = false;
            while (true)
            {
                // Bitwise OR is 0. Trivially accept and get out of loop
                if ((outcode0 | outcode1) == 0)
                {
                    accept = true;
                    break;
                }
                // Bitwise AND is not 0. Trivially reject and get out of loop
                else if ((outcode0 & outcode1) != 0)
                {
                    break;
                }
                else
                {
                    // failed both tests, so calculate the line segment to clip
                    // from an outside point to an intersection with clip edge
                    double x, y;
                    // At least one endpoint is outside the clip rectangle; pick it.
                    byte outcodeOut = (outcode0 != 0) ? outcode0 : outcode1;
                    // Now find the intersection point;
                    // use formulas y = y0 + slope * (x - x0), x = x0 + (1 / slope) * (y - y0)
                    if ((outcodeOut & TOP) != 0)
                    {   // point is above the clip rectangle
                        x = x0 + (x1 - x0) * (extents.Top - y0) / (y1 - y0);
                        y = extents.Top;
                    }
                    else if ((outcodeOut & BOTTOM) != 0)
                    { // point is below the clip rectangle
                        x = x0 + (x1 - x0) * (extents.Bottom - y0) / (y1 - y0);
                        y = extents.Bottom;
                    }
                    else if ((outcodeOut & RIGHT) != 0)
                    {  // point is to the right of clip rectangle
                        y = y0 + (y1 - y0) * (extents.Right - x0) / (x1 - x0);
                        x = extents.Right;
                    }
                    else if ((outcodeOut & LEFT) != 0)
                    {   // point is to the left of clip rectangle
                        y = y0 + (y1 - y0) * (extents.Left - x0) / (x1 - x0);
                        x = extents.Left;
                    }
                    else
                    {
                        x = double.NaN;
                        y = double.NaN;
                    }
                    // Now we move outside point to intersection point to clip
                    // and get ready for next pass.
                    if (outcodeOut == outcode0)
                    {
                        x0 = x;
                        y0 = y;
                        outcode0 = CohenSutherland.ComputeOutCode(extents, x0, y0);
                    }
                    else
                    {
                        x1 = x;
                        y1 = y;
                        outcode1 = CohenSutherland.ComputeOutCode(extents, x1, y1);
                    }
                }
            }
            // return the clipped line
            return accept ? new List<Point>(){
                new Point(x0,y0),
                new Point(x1, y1),
            } : null;
        }

        private static Extents CalucuteExtents(Point[] clipPoly)
        {
            double left = clipPoly[0].X;
            double top = clipPoly[0].Y;
            double right = clipPoly[0].X;
            double bottom = clipPoly[0].Y;
            //
            for (int i = 1; i < clipPoly.Length; i++)
            {
                if (left > clipPoly[i].X)
                    left = clipPoly[i].X;
                if (right < clipPoly[i].X)
                    right = clipPoly[i].X;
                if (top < clipPoly[i].Y)
                    top = clipPoly[i].Y;
                if (bottom > clipPoly[i].Y)
                    bottom = clipPoly[i].Y;
            }
            return new Extents()
            {
                Left = left,
                Top = top,
                Right = right,
                Bottom = bottom
            };
        }

        public static List<Point> GetIntersectedPolyline(Point[] subjectPolyline, Point[] clipPoly)
        {
            Extents extents = CalucuteExtents(clipPoly);

            List<Point> clipLines = new List<Point>();
            for (int i = 0; i < subjectPolyline.Length; i++)
            {
                Point p0 = subjectPolyline[i];
                Point p1 = subjectPolyline[i + 1];
                List<Point> cliped = CohenSutherlandLineClip(p0, p1, extents);
            }
            return clipLines;
        }

    }
}
