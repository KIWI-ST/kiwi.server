using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using System.IO;

namespace Engine.Image
{
    /*
     * 黄奎  2012-7-8
     * 图像处理功能类
     * @modify }{hk 2015/11/21
     */
    public class ImageProcess
    {
        #region 属性
        Container<RasterBand> _ransBands;
        /// <summary>
        /// 栅格图像波段集合
        /// </summary>
        public Container<RasterBand> RastBands { get { return _ransBands; } }

        Engine.Image.BaseType.geoEle _geoBound;
        /// <summary>
        /// bound信息
        /// </summary>
        public Engine.Image.BaseType.geoEle GeoBound { get { return _geoBound; } }
        #endregion
        public ImageProcess(string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string xmlFile = directoryName + "\\" + fileName + ".xml";
            //读取配置文件
            ReadBound(xmlFile);
            //读取rast
            ReadRaster(filePath);
        }
        /// <summary>
        /// 读取配置，获得边界信息
        /// </summary>
        /// <param name="xmlPath"></param>
        public void ReadBound(string xmlPath)
        {
            if (System.IO.File.Exists(xmlPath))
            {
                var _doc = XDocument.Load(xmlPath);
                var geoExt = (from geo in _doc.Descendants("GeoBndBox")
                              select new Engine.Image.BaseType.geoEle
                              {
                                  westBL = geo.Element("westBL").Value,
                                  eastBL = geo.Element("eastBL").Value,
                                  northBL = geo.Element("northBL").Value,
                                  southBL = geo.Element("southBL").Value
                              }).Single();
                //
                Analysis.IProjection smProj = new Analysis.SphericalMercatorProjection();
                _tLPoint = smProj.Porject(Convert.ToDouble(geoExt.northBL), Convert.ToDouble(geoExt.westBL));
                _bRPoint = smProj.Porject(Convert.ToDouble(geoExt.southBL), Convert.ToDouble(geoExt.eastBL));
            }
            else
            {


            }
        }

        public void CutMap(System.Drawing.Bitmap bitmap) {
            for (int i = 15; i < 20; i++)
            {
                var _tlPixel = Analysis.Transformation.T3857.Transform(_tLPoint, scale(i));
                var _brPixel = Analysis.Transformation.T3857.Transform(_bRPoint, scale(i));
                int width = Convert.ToInt32(_brPixel.X - _tlPixel.X+1);
                int height = Convert.ToInt32(_brPixel.Y - _tlPixel.Y+1);
                width = width < 1 ? 1 : width;
                height = height < 1 ? 1 : height;
                //
                System.Drawing.Bitmap b=new System.Drawing.Bitmap(width,height);
                System.Drawing.Graphics gb = System.Drawing.Graphics.FromImage(b);
                gb.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gb.DrawImage(bitmap, new System.Drawing.Rectangle(0,0, width, height), new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.GraphicsUnit.Pixel);
                //g.Dispose();
                //计算当前层-当前bitmap的真实位置
                //1.起点位置
                int x = Convert.ToInt32(Math.Floor(_tlPixel.X / 256)); 
                int y = Convert.ToInt32(Math.Floor(_tlPixel.Y / 256));
                int offsetX =Convert.ToInt32(x * 256-_tlPixel.X );
                int offsetY = Convert.ToInt32(y * 256-_tlPixel.Y );
                //
                int col = Convert.ToInt32(width / 256)<1?1:Convert.ToInt32(width / 256)+1;
                int row = Convert.ToInt32(height / 256)<1?1:Convert.ToInt32(height / 256)+1;
                //
                string mapDataPath = System.IO.Directory.GetCurrentDirectory();
                for (int ii = x; ii <= x+row; ii++)
                {
                    for (int jj = y; jj <= y+col; jj++)
                    {
                        if (!System.IO.Directory.Exists(mapDataPath + @"\mapabc\" + i + @"\"))
                            System.IO.Directory.CreateDirectory(mapDataPath + @"\mapabc\" + i + @"\");
                        System.Drawing.Bitmap saveBitmap = new System.Drawing.Bitmap(256,256);
                        System.Drawing.Graphics gc = System.Drawing.Graphics.FromImage(saveBitmap);
                        gc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        gc.Clear(System.Drawing.Color.Transparent);
                        gc.DrawImage(b, new System.Drawing.Rectangle(0, 0, 256, 256), new System.Drawing.Rectangle(offsetX + (ii-x) * 256, offsetY + (jj-y) * 256, 256, 256), System.Drawing.GraphicsUnit.Pixel);
                        if (File.Exists(mapDataPath + @"\mapabc\" + i + @"\" + ii + "_" + jj + ".png"))
                            File.Delete(mapDataPath + @"\mapabc\" + i + @"\" + ii + "_" + jj + ".png");
                        saveBitmap.MakeTransparent(System.Drawing.Color.White);
                        saveBitmap.Save(mapDataPath + @"\mapabc\" + i + @"\" + ii+"_"+jj+".png");
                        gc.Dispose();
                    }
                }
                gb.Dispose();
            }
        }

        BaseType.Point _tLPoint;
        BaseType.Point _bRPoint;

        public double scale(int zoomLevel)
        {
            var _tilePixelUnit = 256;
            return _tilePixelUnit*Math.Pow(2, zoomLevel);
        }
        /// <summary>
        /// 读取raster图像（分波段）
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadRaster(string filePath)
        {
            OSGeo.GDAL.Gdal.AllRegister();
            OSGeo.GDAL.Dataset pDataSet = OSGeo.GDAL.Gdal.Open(filePath, OSGeo.GDAL.Access.GA_ReadOnly);
            if (pDataSet != null)
            {
                //波段只能从1开始
                for (int count = 1; count <= pDataSet.RasterCount; count++)
                {
                    //新建数据集
                    if (_ransBands == null)
                        _ransBands = new Container<RasterBand>(pDataSet.RasterCount);
                    byte[] temp = new byte[pDataSet.RasterXSize * pDataSet.RasterYSize];
                    //ReadRaster读取方式，纵向读取，LineSpace表示横向比例分割线，1：1应该是图像X方向长度
                    pDataSet.GetRasterBand(count).ReadRaster(0, 0, pDataSet.RasterXSize, pDataSet.RasterYSize, temp, pDataSet.RasterXSize, pDataSet.RasterYSize,1, pDataSet.RasterXSize);
                    RasterBand dataClass = new RasterBand(count.ToString()+"波段", temp, pDataSet.RasterXSize, pDataSet.RasterYSize);
                    _ransBands[count - 1] = dataClass;
                }
            }
        }
    }
}
