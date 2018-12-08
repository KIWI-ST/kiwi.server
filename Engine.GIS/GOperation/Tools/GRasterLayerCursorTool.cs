using Engine.GIS.GLayer.GRasterLayer;
using System;

namespace Engine.GIS.GOperation.Tools
{
    public class GRasterLayerCursorTool : IRasterLayerCursorTool,IDisposable
    {

        GRasterLayer _pLayer;

        public double[] PickRawValue(int x,int y)
        {
            IRasterBandCursorTool pBandCursorTool = new GRasterBandCursorTool();
            double[] normalValueArray = new double[_pLayer.BandCollection.Count];
            for (int i = 0; i < _pLayer.BandCollection.Count; i++)
            {
                GRasterBand pBand = _pLayer.BandCollection[i];
                pBandCursorTool.Visit(pBand);
                normalValueArray[i] = pBandCursorTool.PickRawValue(x, y);
            }
            return normalValueArray;
        }

        public double[] PickNormalValue(int x, int y)
        {
            IRasterBandCursorTool pBandCursorTool = new GRasterBandCursorTool();
            double[] normalValueArray = new double[_pLayer.BandCollection.Count];
            for (int i = 0; i < _pLayer.BandCollection.Count; i++)
            {
                GRasterBand pBand = _pLayer.BandCollection[i];
                pBandCursorTool.Visit(pBand);
                normalValueArray[i] = pBandCursorTool.PickNormalValue(x, y);
            }
            return normalValueArray;
        }

        public double[] PickRagneNormalValue(int x, int y, int row = 5, int col = 5)
        {
            IRasterBandCursorTool pBandCursorTool = new GRasterBandCursorTool();
            double[] rangeNormalValueArray = new double[row * col * _pLayer.BandCollection.Count];
            int offset = 0;
            for (int i = 0; i < _pLayer.BandCollection.Count; i++)
            {
                GRasterBand pBand = _pLayer.BandCollection[i];
                pBandCursorTool.Visit(pBand);
                double[] singleBandRangeNormalValue = pBandCursorTool.PickRangeNormalValue(x, y, row, col);
                Array.ConstrainedCopy(singleBandRangeNormalValue, 0, rangeNormalValueArray, offset, row * col);
                offset += row * col;
            }
            return rangeNormalValueArray;
        }

        public void Visit(GRasterLayer pLayer)
        {
            _pLayer = pLayer;
        }

        public void Dispose()
        {
            _pLayer = null;
        }

    }
}
