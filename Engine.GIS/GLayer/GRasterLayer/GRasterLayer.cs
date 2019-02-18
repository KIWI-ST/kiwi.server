﻿using Engine.GIS.GOperation.Arithmetic;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Engine.GIS.GLayer.GRasterLayer
{
    public class GRasterLayer: IDisposable
    {
        /// <summary>
        /// 构建raster层
        /// </summary>
        /// <param name="rasterFilename"></param>
        public GRasterLayer(string rasterFilename)
        {
            //注册gdal库
            Gdal.AllRegister();
            //记录raster文件数据地址
            FullFilename = rasterFilename;
            //图层名
            Name = Path.GetFileNameWithoutExtension(rasterFilename);
            //只读方式读取图层
            PDataSet = Gdal.Open(rasterFilename, Access.GA_ReadOnly);
            //读取图像范围
            PDataSet.GetGeoTransform(GeoTransform);
            //波段数目
            BandCount = PDataSet.RasterCount;
            //读取band
            BandCollection = new List<GRasterBand>();
            //
            for (int i = 1; i <= BandCount; i++)
            {
                Band pBand = PDataSet.GetRasterBand(i);
                PDataType = pBand.DataType;
                BandCollection.Add(new GRasterBand(pBand));
            }
        }
        #region 属性字段

        /// <summary>
        /// raster file location
        /// </summary>
        public string FullFilename { get; set; }

        /// <summary>
        /// Raster data type
        /// </summary>
        public DataType PDataType { get; }
        /// <summary>
        /// 设置图像范围，上[3] 左[0] 
        /// </summary>
        public double[] GeoTransform { get; } = new double[6];
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
        /// 波段集合
        /// </summary>
        public List<GRasterBand> BandCollection { get; private set; }

        public void Dispose()
        {
            for (int i = 0; i < BandCount; i++)
                BandCollection[i].Dispose();
            //
            BandCollection = null;
            PDataSet.Dispose();
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
                Bitmap bandBmp = BandCollection[i].GrayscaleImage;
                byte v = GConvolution.Run(bandBmp, x, y, mask);
                pixels.Add(v);
            }
            return pixels;
        }
        /// <summary>
        /// 写入band
        /// </summary>
        /// <param name="bandIndex"></param>
        public void SaveBand(int bandIndex,string fullFileName,double[] geoTransform = null)
        {
            GRasterBand pband = BandCollection[bandIndex];
            Driver drv = Gdal.GetDriverByName("GTiff");
            string[] options = new string[] { "BLOCKXSIZE=" + PDataSet.RasterXSize, "BLOCKYSIZE=" + PDataSet.RasterYSize };
            Dataset ds = drv.Create(fullFileName, PDataSet.RasterXSize, PDataSet.RasterYSize, 1, DataType.GDT_Byte, options);
            Band ba = ds.GetRasterBand(1);
            //set geotransform by default value
            if (geoTransform != null)
                ds.SetGeoTransform(geoTransform);
            else
                ds.SetGeoTransform(GeoTransform);
            // GetBufferByte(_pDataSet.RasterXSize, _pDataSet.RasterYSize,byteData)
            ba.WriteRaster(0, 0, PDataSet.RasterXSize, PDataSet.RasterYSize, pband.GetRawBuffer(), PDataSet.RasterXSize, PDataSet.RasterYSize, 0, 0);
            ds.FlushCache();
        }

        /// <summary>
        /// 导出经过归一化处理后的图像
        /// </summary>
        public void ExprotNormalizedLayer(string fullFilename, double[] geoTransform = null)
        {
            Driver drv = Gdal.GetDriverByName("GTiff");
            string[] options = new string[] { "BLOCKXSIZE=" + PDataSet.RasterXSize, "BLOCKYSIZE=" + PDataSet.RasterYSize };
            Dataset ds = drv.Create(fullFilename, PDataSet.RasterXSize, PDataSet.RasterYSize, BandCount, DataType.GDT_Float32, options);
            if (geoTransform != null)
                ds.SetGeoTransform(geoTransform);
            for(int i = 1; i < BandCount; i ++)
            {
                GRasterBand pband = BandCollection[i-1];
                Band ba = ds.GetRasterBand(i);
                ba.WriteRaster(0, 0, PDataSet.RasterXSize, PDataSet.RasterYSize, pband.NormalDataBuffer(), PDataSet.RasterXSize, PDataSet.RasterYSize, 0, 0);
            }
            ds.FlushCache();
        }

        #endregion

    }
}