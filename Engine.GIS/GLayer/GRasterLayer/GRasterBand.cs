using Engine.GIS.GEntity;
using OSGeo.GDAL;
using System;
using System.Drawing;

namespace Engine.GIS.GLayer.GRasterLayer
{
    /// <summary>
    /// raw data type is double
    /// </summary>
    public class GRasterBand : GBitmap, IDisposable
    {

        #region 属性

        /// <summary>
        /// 统计属性
        /// </summary>
        private readonly double _min, _max, _mean, _stdDev;

        /// <summary>
        /// 标准差
        /// </summary>
        public double StdDev { get { return _stdDev; } }

        /// <summary>
        /// 全图均值
        /// </summary>
        public double Mean { get { return _mean; } }

        /// <summary>
        /// 波段最小值
        /// </summary>
        public double Min { get { return _min; } }

        /// <summary>
        /// 波段最大值
        /// </summary>
        public double Max { get { return _max; } }

        /// <summary>
        /// 波段索引
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 波段图层名
        /// </summary>
        public string BandName { get; set; }

        /// <summary>
        /// 波段序号
        /// </summary>
        public int BandIndex { get { return Index; } }

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// normalized data
        /// </summary>
        public double[,] NormalData { get; private set; }

        /// <summary>
        /// raw data
        /// </summary>
        public double[] RawData { get; private set; }
        #endregion

        #region 应用拉伸
        private void ApplyPrecentClipStretch(int[] pBandHistogram,int histLength, double percentMin, double percentMax)
        {
            int countMin = 0, countMax = 0;
            int dbMin = 0, dbMax =0;
            //找出 percentMin的像素的灰度值
            int percentMinCount = Convert.ToInt32(percentMin * Width * Height), percentMaxCount = Convert.ToInt32(percentMax * Width * Height);
            for (int i = 0; i < histLength; i++)
            {
                countMin += pBandHistogram[i];
                if (countMin > percentMinCount)
                {
                    dbMin = i;
                    break;
                }
            }
            //找出percnetMax的像素的灰度值
            for (int k = 0; k < histLength; k++)
            {
                countMax += pBandHistogram[k];
                if (countMax > percentMaxCount)
                {
                    dbMax = k;
                    break;
                }
            }
            NormalData = new double[Width, Height];
            //calcute scale
            for (int count = 0; count < RawData.Length; count++)
            {
                if (RawData[count] <= dbMin)
                    NormalData[count % Width, count / Width] = 0;
                else if(RawData[count]>=dbMax)
                    NormalData[count % Width, count / Width] = 1;
                else
                    NormalData[count % Width, count / Width]= (RawData[count] - dbMin) / (dbMax - dbMin);
            }
        }
        /// <summary>
        /// only normalization
        /// </summary>
        private void Normalization()
        {
            NormalData = new double[Width, Height];
            double scale = _max - _min;
            for (int count = 0; count < RawData.Length; count++)
                NormalData[count % Width, count / Width] = RawData[count] == 0 ? 0 : (RawData[count] - _min) / scale;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBand"></param>
        /// <param name="percentMin"></param>
        /// <param name="percentMax"></param>
        private void PercentClipStretch(Band pBand,double percentMin=0.02, double percentMax=0.98)
        {
            double pdfMin, pdfMax, pdfMean, pdfStdDev;
            pBand.ComputeStatistics(true, out pdfMin, out pdfMax, out pdfMean, out pdfStdDev, null, null);
            switch (pBand.DataType)
            {
                case DataType.GDT_Byte:
                    {
                        int nBuckets = 256;
                        int[] pBandHistogram = new int[nBuckets];
                        pBand.GetHistogram(-0.5, 255.5, nBuckets, pBandHistogram, 0, 0, null, null);
                        ApplyPrecentClipStretch(pBandHistogram, nBuckets, percentMin, percentMax);
                    }
                    break;
                case DataType.GDT_UInt16:
                    break;
                case DataType.GDT_Float32:
                    {
                        int nBuckets = (int)(pdfMax - pdfMin+0.5);
                        int[] pBandHistogram = new int[nBuckets];
                        pBand.GetHistogram(-pdfMin-0.5, pdfMax+0.5, nBuckets, pBandHistogram, 0, 0, null, null);
                        ApplyPrecentClipStretch(pBandHistogram, nBuckets, percentMin, percentMax);
                    }
                    break;
            }
        }
        /// <summary>
        /// clearn the error data
        /// </summary>
        private void CleaningError()
        {
            for (int i = 0; i < Width * Height; i++)
                if (RawData[i] > _max || RawData[i] < _min)
                    RawData[i] = 0;
        }

        #endregion

        /// <summary>
        /// 包装GDALBand
        /// </summary>
        /// <param name="pBand"></param>
        public GRasterBand(Band pBand)
        {
            //band 序号
            Index = pBand.GetBand();
            //width
            Width = pBand.XSize;
            //height
            Height = pBand.YSize;
            //统计
            pBand.SetNoDataValue(0);
            //approx_ok ：true 表示粗略统计，false表示严格统计
            //bForce：表示扫描图统计生成xml
            pBand.GetStatistics(0, 1, out _min, out _max, out _mean, out _stdDev);
            //读取rawdata
            RawData = new double[Width * Height];
            pBand.ReadRaster(0, 0, Width, Height, RawData, Width, Height, 0, 0);
            //remove error data
            CleaningError();
            //stretch pixel data
            if (_max - _min > 256)
                PercentClipStretch(pBand);
            else
                Normalization();
        }
        /// <summary>
        /// byte数据流
        /// </summary>
        /// <returns>stretched byte buffer</returns>
        public byte[,] GetByteBuffer()
        {
            byte[,] _stretchedByteData = new byte[Width, Height];
            for (int count = 0; count < RawData.Length; count++)
                _stretchedByteData[count % Width, count / Width] = Convert.ToByte(NormalData[count % Width, count / Width] * 255);
            return _stretchedByteData;
        }
        /// <summary>
        /// 获取未拉伸的原始bytebuffer
        /// </summary>
        /// <returns></returns>
        public double[] GetRawBuffer()
        {
            return RawData;
        }
        /// <summary>
        /// get normal data buffer
        /// </summary>
        /// <returns></returns>
        public double[] NormalDataBuffer()
        {
            double[] normalDataBuffer = new double[RawData.Length];
            for (int count = 0; count < RawData.Length; count++)
                normalDataBuffer[count] = NormalData[count % Width, count / Width];
            return normalDataBuffer;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            RawData = null;
            NormalData = null;
        }

        /// <summary>
        /// gray scale image
        /// </summary>
        public Bitmap GrayscaleImage
        {
            get { return ToGrayBitmap(NormalData, Width, Height); }
        }

    }
}
