using Engine.Image.Analysis;
using OSGeo.GDAL;
using System;
using System.Drawing;

namespace Engine.Image.Eneity.GBand
{
    public class GdalBandFloat32 : IGdalBand
    {
        int _bandIndex;

        float[] _rawData;

        byte[,] _byteData;

        int _xCount, _yCount;

        double _min, _max, _mean, _stdDev;

        string _name;

        public string BandName { get { return _name; } }

        public int BandIndex { get { return _bandIndex; } }

        public int Width { get { return _xCount; } }

        public int Height { get { return _yCount; } }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="bandIndex"></param>
        /// <param name="xCount"></param>
        /// <param name="yCount"></param>
        /// <param name="pBand"></param>
        public void SetData(int bandIndex, int xCount, int yCount, Band pBand)
        {
            _bandIndex = bandIndex;
            _byteData = null;
            _xCount = xCount;
            _yCount = yCount;
            _rawData = new float[_xCount * _yCount];
            //1.统计
            pBand.SetNoDataValue(0);
            pBand.GetStatistics(1, 1, out _min, out _max, out _mean, out _stdDev);
            //2.读取band原始数据
            pBand.ReadRaster(0, 0, _xCount, _yCount, _rawData, _xCount, _yCount, 0, 0);
        }
        /// <summary>
        /// byte数据流
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
                        _byteData[count % _xCount, count / _yCount] = 0;
                    else if (value >= _max)
                        _byteData[count % _xCount, count / _yCount] = 255;
                    else
                    {
                        double temp = (((value - _min) / scale) * 255);
                        _byteData[count % _xCount, count / _yCount] = Convert.ToByte(temp);
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

        public Bitmap GetBitmap()
        {
            if (_rawData == null)
                return null;
            Bitmap bitmap = BitmapAndByte.ToGrayBitmap(GetByteData(), _xCount, _yCount);
            return bitmap;
        }

        public byte[] GetByteBuffer()
        {
            byte[] rawByteData = new byte[_xCount * _yCount];
            for (int count = 0; count < _rawData.Length; count++)
                rawByteData[count] = _byteData[count % _xCount, count / _xCount];
            return rawByteData;
        }

    }
}
