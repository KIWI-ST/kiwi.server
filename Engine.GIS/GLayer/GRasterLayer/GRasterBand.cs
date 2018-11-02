using Engine.GIS.GEntity;
using OSGeo.GDAL;
using System;
using System.Drawing;

namespace Engine.GIS.GLayer.GRasterLayer
{
    /// <summary>
    /// raw data type is double
    /// </summary>
    public class GRasterBand : GBitmap
    {
        /// <summary>
        /// raw data 
        /// </summary>
        double[] _rawData;

        /// <summary>
        ///  normalized data
        /// </summary>
        double[,] _normalData;

        #region 属性

        private readonly int _width;
        private readonly int _height;
        private readonly double _min;
        private readonly double _max;
        private readonly double _mean;
        private readonly double _stdDev;

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
        public int Width { get { return _width; } }
        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get { return _height; } }
        /// <summary>
        /// normalized data
        /// </summary>
        public double[,] NormalData { get => _normalData; }
        /// <summary>
        /// raw data
        /// </summary>
        public double[] RawData { get => _rawData; }
        #endregion

        #region 应用拉伸
        /// <summary>
        /// apply image stretch
        /// </summary>
        private void Normalization()
        {
            _normalData = new double[_width, _height];
            double scale = _max - _min;
            for (int count = 0; count < _rawData.Length; count++)
                _normalData[count % _width, count / _width] = _rawData[count] == 0 ? 0 : (_rawData[count] - _min) / scale;
        }
        /// <summary>
        /// clearn the error data
        /// </summary>
        private void CleaningError()
        {
            for (int i = 0; i < _width * _height; i++)
                if (_rawData[i] > _max || _rawData[i] < _min)
                    _rawData[i] = 0;
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
            _width = pBand.XSize;
            //height
            _height = pBand.YSize;
            //统计
            pBand.SetNoDataValue(0);
            //approx_ok ：true 表示粗略统计，false表示严格统计
            //bForce：表示扫描图统计生成xml
            pBand.GetStatistics(0, 1, out _min, out _max, out _mean, out _stdDev);
            //读取rawdata
            _rawData = new double[_width * _height];
            pBand.ReadRaster(0, 0, _width, _height, _rawData, _width, _height, 0, 0);
            //remove error data
            CleaningError();
            //stretch pixel data
            Normalization();
        }
        /// <summary>
        /// byte数据流
        /// </summary>
        /// <returns>stretched byte buffer</returns>
        public byte[,] GetByteBuffer()
        {
            byte[,] _stretchedByteData = new byte[_width, _height];
            for (int count = 0; count < _rawData.Length; count++)
                _stretchedByteData[count % _width, count / _width] = Convert.ToByte(_normalData[count % _width, count / _width] * 255);
            return _stretchedByteData;
        }
        /// <summary>
        /// 获取未拉伸的原始bytebuffer
        /// </summary>
        /// <returns></returns>
        public double[] GetRawBuffer()
        {
            return _rawData;
        }
        /// <summary>
        /// gray scale image
        /// </summary>
        public Bitmap GrayscaleImage
        {
            get { return ToGrayBitmap(_normalData, _width, _height); }
        }

    }
}
