using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.GIS.GOperation.Tools
{
    public class GRasterBandStatisticTool : IRasterBandStatisticTool,IDisposable
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

        GRasterBand _pBand;

        public void Visit(GRasterBand pBand)
        {
            _pBand = pBand;
            _width = pBand.Width;
            _height = pBand.Height;
            _normalData = pBand.NormalData;
            _rawData = pBand.RawData;
        }

        public void Dispose()
        {
            _pBand = null;
        }

        public double[,] StatisticalRawQueryTable
        {
            get
            {
                double[,] queryTable = new double[_width, _height];
                for (int position = 0; position < _width * _height; position++)
                    queryTable[position % _width, position / _width] = _rawData[position];
                return queryTable;
            }
        }

        public Dictionary<int, List<Point>> StaisticalRawGraph
        {
            get {
                Dictionary<int, List<Point>> memory = new Dictionary<int, List<Point>>();
                IRasterBandCursorTool pBandCursorTool = new GRasterBandCursorTool();
                pBandCursorTool.Visit(_pBand);
                foreach (var (x, y, value) in pBandCursorTool.ValidatedRawCollection)
                {
                    int classIndex = Convert.ToInt16(value);
                    if (memory.ContainsKey(classIndex))
                        memory[classIndex].Add(new Point(x, y));
                    else
                        memory.Add(classIndex, new List<Point>() { new Point(x, y) });
                }
                //sort memory
                memory = memory.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                //return statical result
                return memory;
            }
        }

    }
}
