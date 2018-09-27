using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GLayer.GRasterLayer.GBand;

namespace Engine.GIS.GOperation.Tools
{
    public class GRasterLayerCursorTool : IRasterLayerCursorTool
    {

        GRasterLayer _pLayer;

        IBandCursorTool _pBandCursorTool = new GBandCursorTool();

        public double[] PickNormalValue(int x, int y)
        {
            double[] normalValueArray = new double[_pLayer.BandCollection.Count];
            for(int i = 0; i < _pLayer.BandCollection.Count; i++)
            {
                GRasterBand pBand = _pLayer.BandCollection[i];
                _pBandCursorTool.Visit(pBand);
                normalValueArray[i] = _pBandCursorTool.PickNormalValue(x, y);
            }
            return normalValueArray;
        }

        public double[] PickRagneNormalValue(int x, int y, int row = 5, int col = 5)
        {
            throw new NotImplementedException();
        }

        public void Visit(GRasterLayer pLayer)
        {
            _pLayer = pLayer;
        }

    }
}
