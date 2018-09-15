using Engine.GIS.GLayer.GRasterLayer.GBand;
using System.Collections.Generic;

namespace Engine.GIS.GOperation.Tools
{
    /// <summary>
    /// cursor for each band
    /// </summary>
    public class GBandCursorTool: IBandCursorTool
    {
        /// <summary>
        /// band width and hight
        /// </summary>
        int _width, _height;

        /// <summary>
        /// noramlized data
        /// </summary>
        double[,] _normalData;

        /// <summary>
        /// raw data
        /// </summary>
        double[] _rawData;

        /// <summary>
        /// visit band
        /// </summary>
        /// <param name="pBand"></param>
        public void Visit(GRasterBand pBand)
        {
            _width = pBand.Width;
            _height = pBand.Height;
            _normalData = pBand.NormalData;
            _rawData = pBand.RawData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(int,int,double)> ValidatedNormalCollection
        {
            get {
                for(int x=0;x<_width;x++)
                    for(int y = 0; y < _height; y++)
                        if(_normalData[x,y]!=0)
                            yield return (x, y, _normalData[x,y]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<(int, int, double)> ValidatedRawCollection
        {
            get
            {
                for(int i=0;i<_width*_height;i++)
                        if (_rawData[i] != 0)
                            yield return (i%_width, i/_width, _rawData[i]);
            }
        }
        /// <summary>
        /// pick raw value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double PickRawValue(int x, int y)
        {
            int position = y * _width + x;
            return _rawData[position];
        }
        /// <summary>
        /// pick normalized value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double PickNormalValue(int x,int y)
        {
            return _normalData[x, y];
        }
        /// <summary>
        /// 获取 Row x Col 个像素的矩形区域像素值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rowcol">represent matrix rows and col,the matrix is row*col,i.e  9 represent 3x3 </param>
        public double[] PickNormalValueByMask(int x,int y,int row=5,int col = 5)
        {
            int offset = row / 2;
            List<double> pixels = new List<double>();
            for (int i = -offset; i < row - offset; i++)
                for (int j = -offset; j < col - offset; j++)
                {
                    int pi = x + i;
                    int pj = y + j;
                    pi = pi <= 0 || pi >= _width ? x : pi;
                    pj = pj <= 0 || pj >= _height ? y : pj;
                    pixels.Add(_normalData[pi, pj]);
                }
            return pixels.ToArray();
        }
    }
}
