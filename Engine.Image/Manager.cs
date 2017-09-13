using Engine.Image.Analysis;
using Engine.Image.Eneity.GLayer;
using Engine.Image.Entity;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Engine.Image
{
    /// <summary>
    /// 管理类，用于处理图片相关操作
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// layer集合
        /// </summary>
        List<IGdalLayer> _layerCollection;
        /// <summary>
        /// layer边界信息
        /// </summary>
        GPoint _tLPoint, _bRPoint;

        public Manager()
        {
            Gdal.AllRegister();
            _layerCollection = new List<IGdalLayer>();
        }

        /// <summary>
        /// 添加文件进入库
        /// </summary>
        public IGdalLayer AddImage(string imagePath)
        {
            IGdalLayer layer = new GdalRasterLayer();
            layer.ReadFromFile(imagePath);
            _layerCollection.Add(layer);
            return layer;
        }
        /// <summary>
        /// 获取第index个buffer的
        /// </summary>
        /// <param name="index"></param>
        public Bitmap GetBitmap(int index)
        {
            IGdalLayer gdalLayer = _layerCollection[index];
            return null;
        }
        /// <summary>
        /// 获取 Gdalayer
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IGdalLayer GetGdalLayer(int index)
        {
            return _layerCollection[index];
        }
        /// <summary>
        /// 读取配置，获得边界信息
        /// </summary>
        /// <param name="xmlPath"></param>
        public void ReadBound(string xmlPath)
        {
            if (File.Exists(xmlPath))
            {
                var _doc = XDocument.Load(xmlPath);
                var geoExt = (from geo in _doc.Descendants("GeoBndBox")
                              select new geoEle
                              {
                                  westBL = geo.Element("westBL").Value,
                                  eastBL = geo.Element("eastBL").Value,
                                  northBL = geo.Element("northBL").Value,
                                  southBL = geo.Element("southBL").Value
                              }).Single();
                //
                IProjection smProj = new SphericalMercatorProjection();
                _tLPoint = smProj.Porject(Convert.ToDouble(geoExt.northBL), Convert.ToDouble(geoExt.westBL));
                _bRPoint = smProj.Porject(Convert.ToDouble(geoExt.southBL), Convert.ToDouble(geoExt.eastBL));
            }
            else
            {


            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        public double scale(int zoomLevel)
        {
            var _tilePixelUnit = 256;
            return _tilePixelUnit * Math.Pow(2, zoomLevel);
        }

        public void CutMap(Bitmap bitmap)
        {
            for (int i = 15; i < 20; i++)
            {
                var _tlPixel = Transformation.T3857.Transform(_tLPoint, scale(i));
                var _brPixel = Transformation.T3857.Transform(_bRPoint, scale(i));
                int width = Convert.ToInt32(_brPixel.X - _tlPixel.X + 1);
                int height = Convert.ToInt32(_brPixel.Y - _tlPixel.Y + 1);
                width = width < 1 ? 1 : width;
                height = height < 1 ? 1 : height;
                //
                Bitmap b = new Bitmap(width, height);
                Graphics gb = Graphics.FromImage(b);
                gb.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gb.DrawImage(bitmap, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                //g.Dispose();
                //计算当前层-当前bitmap的真实位置
                //1.起点位置
                int x = Convert.ToInt32(Math.Floor(_tlPixel.X / 256));
                int y = Convert.ToInt32(Math.Floor(_tlPixel.Y / 256));
                int offsetX = Convert.ToInt32(x * 256 - _tlPixel.X);
                int offsetY = Convert.ToInt32(y * 256 - _tlPixel.Y);
                //
                int col = Convert.ToInt32(width / 256) < 1 ? 1 : Convert.ToInt32(width / 256) + 1;
                int row = Convert.ToInt32(height / 256) < 1 ? 1 : Convert.ToInt32(height / 256) + 1;
                //
                string mapDataPath = Directory.GetCurrentDirectory();
                for (int ii = x; ii <= x + row; ii++)
                {
                    for (int jj = y; jj <= y + col; jj++)
                    {
                        if (!Directory.Exists(mapDataPath + @"\mapabc\" + i + @"\"))
                            Directory.CreateDirectory(mapDataPath + @"\mapabc\" + i + @"\");
                        Bitmap saveBitmap = new Bitmap(256, 256);
                        Graphics gc = Graphics.FromImage(saveBitmap);
                        gc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        gc.Clear(Color.Transparent);
                        gc.DrawImage(b, new Rectangle(0, 0, 256, 256), new System.Drawing.Rectangle(offsetX + (ii - x) * 256, offsetY + (jj - y) * 256, 256, 256), System.Drawing.GraphicsUnit.Pixel);
                        if (File.Exists(mapDataPath + @"\mapabc\" + i + @"\" + ii + "_" + jj + ".png"))
                            File.Delete(mapDataPath + @"\mapabc\" + i + @"\" + ii + "_" + jj + ".png");
                        saveBitmap.MakeTransparent(Color.White);
                        saveBitmap.Save(mapDataPath + @"\mapabc\" + i + @"\" + ii + "_" + jj + ".png");
                        gc.Dispose();
                    }
                }
                gb.Dispose();
            }
        }
    }
}
