using Engine.GIS.GEntity;
using OSGeo.GDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GIS.GLayer.GRasterLayer.GBand
{
    public class PointElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double V { get; set; }
    }

    /// <summary>
    /// raw data type is double
    /// </summary>
    public class GRasterBand : GBitmap, IEnumerable
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
                _normalData[count % _width, count / _width] = (_rawData[count] - _min) / scale;
        }

        #endregion

        #region 迭代

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < 10; i++)
                yield return (1, 11, 1);
            //return new GDoubleBandEnumertor(_rawData,_width,_height);
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
            //
            pBand.GetStatistics(1, 1, out _min, out _max, out _mean, out _stdDev);
            //读取rawdata
            _rawData = new double[_width * _height];
            pBand.ReadRaster(0, 0, _width, Height, _rawData, _height, Height, 0, 0);
            //stretch pixel data
            Normalization();
        }
        /// <summary>
        /// get raw double value at position (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double GetRawValue(int x,int y)
        {
            int position = y * Width + x;
            return _rawData[position];
        }
        /// <summary>
        /// get normalized value at position (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double GetNormalValue(int x, int y)
        {
            return _normalData[x, y];
        }
        /// <summary>
        /// 获取 Row x Col 个像素的矩形区域像素值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rowcol">represent matrix rows and col,the matrix is row*col,i.e  9 represent 3x3 </param>
        public double[] GetNormalValueByMask(int x, int y, int row = 3, int col = 3)
        {
            int offset = row / 2;
            List<double> pixels = new List<double>();
            for (int i = -offset; i < row - offset; i++)
                for (int j = -offset; j < col - offset; j++)
                {
                    int pi = x + i;
                    int pj = y + j;
                    pi = pi <= 0 || pi >= Width ? x : pi;
                    pj = pj <= 0 || pj >= Height ? y : pj;
                    pixels.Add(_normalData[pi, pj]);
                }
            return pixels.ToArray();
        }
        /// <summary>
        /// byte数据流
        /// </summary>
        /// <returns>stretched byte buffer</returns>
        public byte[,] GetByteBuffer()
        {
            byte[,] _stretchedByteData = new byte[_width,_height];
            for (int count = 0; count < _rawData.Length; count++)
                _stretchedByteData[count % Width, count / Width] = Convert.ToByte(_normalData[count % Width, count / Width]*255);
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
