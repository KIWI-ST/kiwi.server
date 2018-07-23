using Engine.GIS.GEntity;
using OSGeo.GDAL;
using System;
using System.Drawing;

namespace Engine.GIS.GLayer.GRasterLayer.GBand
{
    public class GFloat32Band : GBitmap, IGBand
    {
        //band 序号
        int _bandIndex;
        //byte数值
        float[] _rawData;
        byte[,] _rawByteData;
        //二维byte数值
        byte[,] _byteData;
        //宽，高
        int _xCount, _yCount;
        //用于统计变量
        double _min, _max, _mean, _stdDev;
        /// <summary>
        /// 包装GDALBand
        /// </summary>
        /// <param name="pBand"></param>
        public GFloat32Band(Band pBand)
        {
            //band 序号
            _bandIndex = pBand.GetBand();
            //width
            _xCount = pBand.XSize;
            //height
            _yCount = pBand.YSize;
            //统计
            pBand.SetNoDataValue(0);
            pBand.GetStatistics(1, 1, out _min, out _max, out _mean, out _stdDev);
            //读取rawdata
            _rawData = new float[_xCount * _yCount];
            pBand.ReadRaster(0, 0, _xCount, _yCount, _rawData, _xCount, _yCount, 0, 0);
        }

        int _cursor = 0;
        /// <summary>
        /// 游标方式读取图像值
        /// </summary>
        /// <returns>返回未拉伸的原始图像值</returns>
        public (int x, int y,int value) Next()
        {
            if (_cursor == _xCount * _yCount - 1)
                return (-1, -1, -1);
            int x = _cursor % _xCount;
            int y = _cursor / _xCount;
            int value = GetRawPixel(x, y);
            _cursor++;
            return (x, y, value);
        }
        /// <summary>
        /// 获取未拉伸的图像值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public byte GetRawPixel(int x, int y)
        {
            if (_rawByteData != null)
                return _rawByteData[x, y];
            _rawByteData = new byte[_xCount, _yCount];
            for (int count = 0; count < _rawData.Length; count++)
            {
                _rawByteData[count % _xCount, count / _xCount] = Convert.ToByte(_rawData[count]);
            }
            return _rawByteData[x, y];
        }

        public double StdDev { get { return _stdDev; } }

        public double Mean { get { return _mean; } }

        public double Min { get { return _min; } }

        public double Max { get { return _max; } }
        /// <summary>
        /// 波段图层名
        /// </summary>
        public string BandName { get; set; }
        /// <summary>
        /// 波段序号
        /// </summary>
        public int BandIndex { get { return _bandIndex; } }
        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get { return _xCount; } }
        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get { return _yCount; } }


        /// <summary>
        /// 图像byte二维数组
        /// </summary>
        public byte[,] GetByteData()
        {
            if (_byteData != null)
                return _byteData;
            _byteData = new byte[_xCount, _yCount];
            double scale = _max - _min;
            //判断是否需要拉伸
            if (_max < 128 || _max > 256)
            {
                for (int count = 0; count < _rawData.Length; count++)
                {
                    float value = _rawData[count];
                    if (value <= _min)
                        _byteData[count % _xCount, count / _xCount] = 0;
                    else if (value >= _max)
                        _byteData[count % _xCount, count / _xCount] = 255;
                    else
                    {
                        double temp = (((value - _min) / scale) * 255);
                        _byteData[count % _xCount, count / _xCount] = Convert.ToByte(temp);
                    }
                }
            }
            else
            {
                for (int count = 0; count < _rawData.Length; count++)
                {
                    _byteData[count % _xCount, count / _xCount] = Convert.ToByte(_rawData[count]);
                }
            }
            return _byteData;
        }
        /// <summary>
        /// byte数据流
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteBuffer()
        {
            byte[] rawByteData = new byte[_xCount * _yCount];
            byte[,] byteData = GetByteData();
            for (int count = 0; count < _rawData.Length; count++)
                rawByteData[count] = byteData[count % _xCount, count / _xCount];
            return rawByteData;
        }
        /// <summary>
        /// 获取单波段灰度图
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {
            if (_rawData == null)
                return null;
            Bitmap bitmap = ToGrayBitmap(GetByteData(), Width, Height);
            return bitmap;
        }

    }
}
