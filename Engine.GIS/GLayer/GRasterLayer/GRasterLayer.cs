using Engine.GIS.GEntity;
using Engine.GIS.GLayer.GRasterLayer.GBand;
using Engine.GIS.GOperation.Arithmetic;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace Engine.GIS.GLayer.GRasterLayer
{
    public class GRasterLayer
    {
        /// <summary>
        /// 构建raster层
        /// </summary>
        /// <param name="rasterFilename"></param>
        public GRasterLayer(string rasterFilename)
        {
            //注册gdal库
            Gdal.AllRegister();
            //图层名
            Name = Path.GetFileNameWithoutExtension(rasterFilename);
            //只读方式读取图层
            PDataSet = Gdal.Open(rasterFilename, Access.GA_ReadOnly);
            //波段数目
            BandCount = PDataSet.RasterCount;
            //读取band
            BandCollection = new List<IGBand>();
            for (int i = 1; i <= BandCount; i++)
            {
                Band pBand = PDataSet.GetRasterBand(i);
                PDataType = pBand.DataType;
                if (PDataType == DataType.GDT_Float32 || PDataType == DataType.GDT_Byte)
                    BandCollection.Add(new GFloat32Band(pBand));
            }
        }

        #region 属性字段

        /// <summary>
        /// height
        /// </summary>
        public int YSize { get => PDataSet.RasterYSize; }
        /// <summary>
        /// width
        /// </summary>
        public int XSize { get => PDataSet.RasterXSize; }
        /// <summary>
        /// RasterLayer 图层名
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// GDAL Dataset
        /// </summary>
        public Dataset PDataSet { get; }
        /// <summary>
        /// 波段层数
        /// </summary>
        public int BandCount { get; }
        /// <summary>
        /// 图层类型
        /// </summary>
        public DataType PDataType { get; }
        /// <summary>
        /// 波段集合
        /// </summary>
        public List<IGBand> BandCollection { get; }
        /// <summary>
        /// 获取rasterLayer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<double> GetPixelDouble(int x, int y)
        {
            List<double> pixels = new List<double>();
            for (int i = 0; i < BandCount; i++)
                pixels.Add(BandCollection[i].GetByteData()[x, y]);
            return pixels;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<float> GetPixeFloat(int x, int y)
        {
            List<float> pixels = new List<float>();
            for (int i = 0; i < BandCount; i++)
                pixels.Add(BandCollection[i].GetByteData()[x, y]);
            return pixels;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<float> GetPixelFloatWidthConv(int x, int y, int[] mask)
        {
            List<float> pixels = new List<float>();
            for (int i = 0; i < BandCount; i++)
            {
                Bitmap bandBmp = BandCollection[i].GetBitmap();
                byte v = GConvolution.Run(bandBmp, x, y, mask);
                pixels.Add(v);
            }
            return pixels;
        }

        #endregion

    }
}
