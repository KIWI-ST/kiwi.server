using Engine.GIS.GOperation.Arithmetic;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GOperation
{
    /// <summary>
    /// 金字塔输出
    /// </summary>
    public class OutputPyramid
    {
        VectorPyramid _vectorPyramid;

        public OutputPyramid(VectorPyramid vectorPyramid)
        {
            _vectorPyramid = vectorPyramid;
        }
        /// <summary>
        /// 切割矢量并输出成bitmap
        /// </summary>
        /// <param name="featureCollection"></param>
        /// <param name="outputDir"></param>
        public void Output(FeatureCollection featureCollection, string outputDir)
        {
            var _tileDictionary = _vectorPyramid.TileDictionary;
            int _tileSize = _vectorPyramid.Projection.TileSize;
            //
            foreach (int zoom in _tileDictionary.Keys)
            {
                var tileCollection = _tileDictionary[zoom];
                foreach (var tile in tileCollection)
                {
                    try
                    {
                        //
                        Bitmap bmp = new Bitmap(_tileSize, _tileSize);
                        Graphics g = Graphics.FromImage(bmp);
                        Pen pen = new Pen(Color.Black, 3);
                        //
                        for (int i = 0; i < featureCollection.Count; i++)
                        {
                            IFeature f = featureCollection[i];
                            //点
                            if (f.Geometry.OgcGeometryType == OgcGeometryType.Point)
                            {
                                Coordinate point = f.Geometry.Coordinate;
                                if (tile.Bound.PointInPolygon(point))
                                {
                                    //2.2.1 计算点的像素坐标
                                    Coordinate pixel = _vectorPyramid.Projection.LatlngToPoint(point, zoom);
                                    //
                                    double deltaX = pixel.X / _tileSize - tile.X;
                                    double deltaY = pixel.Y / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    g.DrawLine(pen, x, x, x, y);
                                }
                                continue;
                            }
                            //线
                            else if (f.Geometry.OgcGeometryType == OgcGeometryType.LineString)
                            {
                                //2.1瓦片裁剪道路
                                List<Coordinate> clipLine = CohenSutherland.GetIntersectedPolyline(f.Geometry.Coordinates, tile.Bound);
                                if (clipLine.Count == 0) continue;
                                int x0 = -1000, y0 = -1000;
                                //2.2 绘制clipLine
                                foreach (Coordinate point in clipLine)
                                {
                                    //2.2.1 计算点的像素坐标
                                    Coordinate pixel = _vectorPyramid.Projection.LatlngToPoint(point, zoom);
                                    //
                                    double deltaX = pixel.X / _tileSize - tile.X;
                                    double deltaY = pixel.Y / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    if (x0 == -1000 && y0 == -1000)
                                    {
                                        x0 = x;
                                        y0 = y;
                                        continue;
                                    }
                                    else
                                    {
                                        g.DrawLine(pen, x0, y0, x, y);
                                        x0 = x;
                                        y0 = y;
                                    }
                                }
                            }
                            //面
                            else if (f.Geometry.OgcGeometryType == OgcGeometryType.Polygon)
                            {
                                List<Coordinate> clipPolygon = SutherlandHodgman.GetIntersectedPolygon(f.Geometry.Coordinates, tile.Bound);
                                if (clipPolygon.Count < 3) continue;
                                int x0 = -1000, y0 = -1000;
                                //2.2 绘制clipLine
                                foreach (Coordinate point in clipPolygon)
                                {
                                    //2.2.1 计算点的像素坐标
                                    Coordinate pixel = _vectorPyramid.Projection.LatlngToPoint(point, zoom);
                                    //
                                    double deltaX = pixel.X / _tileSize - tile.X;
                                    double deltaY = pixel.Y / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    if (x0 == -1000 && y0 == -1000)
                                    {
                                        x0 = x;
                                        y0 = y;
                                        continue;
                                    }
                                    else
                                    {
                                        g.DrawLine(pen, x0, y0, x, y);
                                        x0 = x;
                                        y0 = y;
                                    }
                                }
                            }
                        }
                        //2.3 保存bmp到指定路径
                        if (!System.IO.Directory.Exists(outputDir + @"\" + zoom))
                            System.IO.Directory.CreateDirectory(outputDir + @"\" + zoom);
                        //根据geometry id存储，获取不到geometry的id，所以只能自定内部序号
                        bmp.Save(outputDir + @"\" + zoom + @"\" + tile.X + "_" + tile.Y + "_" + tile.Z + ".jpg");
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

    }
}
