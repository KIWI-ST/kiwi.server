using Engine.GIS.GEntity;
using OSGeo.GDAL;
using System;
using System.Drawing;

namespace Engine.GIS.GLayer.GRasterLayer.GBand
{
    public class GFloat32Band:GBitmap, IGBand
    {
        //band 序号
        int _bandIndex;
        //byte数值
        float[] _rawData;
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
