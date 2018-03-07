using Engine.GIS.GeoType;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Engine.GIS.GOperation.Arithmetic
{
    /// <summary>
    /// This represents a line segment
    /// 一条边线
    /// </summary>
    public class Edge
    {
        public Edge(Coordinate from, Coordinate to)
        {
            From = from;
            To = to;
        }
        public readonly Coordinate From;
        public readonly Coordinate To;
    }
    /// <summary>
    /// SutherlandHodgman 裁剪算法
    /// https://rosettacode.org/wiki/Sutherland-Hodgman_polygon_clipping#C.23
    /// </summary>
    public static class SutherlandHodgman
    {

        public static List<Coordinate> GetIntersectedPolygon(Coordinate[] subjectPoly, GBound bound)
        {
            Coordinate[] clipPoly = bound.ToClipPolygon();
            if (subjectPoly.Length < 3)
                throw new ArgumentException(string.Format("The polygons passed in must have at least 3 points: subject={0}", subjectPoly.Length.ToString()));
            List<Coordinate> outputList = subjectPoly.ToList();
            //	Make sure it's clockwise
            if (!IsClockwise(subjectPoly))
                outputList.Reverse();
            //	Walk around the clip polygon clockwise
            foreach (Edge clipEdge in IterateEdgesClockwise(clipPoly))
            {
                List<Coordinate> inputList = outputList.ToList();		//	clone it
                outputList.Clear();

                if (inputList.Count == 0)
                {
                    //	Sometimes when the polygons don't intersect, this list goes to zero.  Jump out to avoid an index out of range exception
                    break;
                }

                Coordinate S = inputList[inputList.Count - 1];

                foreach (Coordinate E in inputList)
                {
                    if (IsInside(clipEdge, E))
                    {
                        if (!IsInside(clipEdge, S))
                        {
                            Coordinate point = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                            outputList.Add(point);
                        }

                        outputList.Add(E);
                    }
                    else if (IsInside(clipEdge, S))
                    {
                        Coordinate point = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                        outputList.Add(point);
                    }

                    S = E;
                }
            }
            //	Exit Function
            return outputList;
        }

        private static IEnumerable<Edge> IterateEdgesClockwise(Coordinate[] polygon)
        {
            if (IsClockwise(polygon))
            {
                #region Already clockwise

                for (int cntr = 0; cntr < polygon.Length - 1; cntr++)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr + 1]);
                }

                yield return new Edge(polygon[polygon.Length - 1], polygon[0]);

                #endregion
            }
            else
            {
                #region Reverse

                for (int cntr = polygon.Length - 1; cntr > 0; cntr--)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr - 1]);
                }

                yield return new Edge(polygon[0], polygon[polygon.Length - 1]);

                #endregion
            }
        }

        private static Coordinate GetIntersect(Coordinate line1From, Coordinate line1To, Coordinate line2From, Coordinate line2To)
        {

            Point pLine1To = new Point(line1To.X, line1To.Y),
                pLine1From = new Point(line1From.X, line1From.Y),
                pLine2To = new Point(line2To.X, line2To.Y),
                pLine2From = new Point(line2From.X, line2From.Y);

            Vector direction1 = pLine1To - pLine1From;
            Vector direction2 = pLine2To - pLine2From;

            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            if (IsNearZero(dotPerp))
                return null;

            Vector c = pLine2From - pLine1From;
            double t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;
            Point p= pLine1From + (t * direction1);
            return new Coordinate(p.X, p.Y, 0);
        }

        private static bool IsInside(Edge edge, Coordinate test)
        {
            bool? isLeft = IsLeftOf(edge, test);
            if (isLeft == null)
            {
                //	Colinear points should be considered inside
                return true;
            }

            return !isLeft.Value;
        }

        private static bool IsClockwise(Coordinate[] polygon)
        {
            for (int cntr = 2; cntr < polygon.Length; cntr++)
            {
                bool? isLeft = IsLeftOf(new Edge(polygon[0], polygon[1]), polygon[cntr]);
                if (isLeft != null)		//	some of the points may be colinear.  That's ok as long as the overall is a polygon
                {
                    return !isLeft.Value;
                }
            }

            throw new ArgumentException("All the points in the polygon are colinear");
        }

        private static bool? IsLeftOf(Edge edge, Coordinate test)
        {
            double t1x = edge.To.X - edge.From.X,
                    t1y = edge.To.Y - edge.From.Y,
                    t2x = test.X - edge.To.X,
                    t2y = test.Y - edge.To.Y;
            //	dot product of perpendicular?
            double x = (t1x * t2y) - (t1y * t2x);
            //
            if (x < 0)
                return false;
            else if (x > 0)
                return true;
            else  //	Colinear points;
                return null;
        }

        private static bool IsNearZero(double testValue)
        {
            return Math.Abs(testValue) <= .000000001d;
        }

    }

}
