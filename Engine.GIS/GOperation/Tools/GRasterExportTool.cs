using OSGeo.GDAL;
using System;
using System.Collections.Generic;

namespace Engine.GIS.GOperation.Tools
{
    /// <summary>
    /// @example
    /// new GRasterExportTool().Prepare().CombineBand().Export();
    /// </summary>
    public class GRasterExportTool: IRasterExportTool, IDisposable
    {
        int _band = 0;

        public GRasterExportTool(){ }

        Dictionary<int, double[]> _bandDict;

        public GRasterExportTool Prepare()
        {
            _band = 0;
            _bandDict = new Dictionary<int, double[]>();
            return this;
        }

        public GRasterExportTool CombineBand(double[] outputBuffer)
        {
            _band++;
            _bandDict[_band] = outputBuffer;
            return this;
        }

        public void Export(double[] nGeoTrans, int width,int height,string fullFilename, DataType dateType = DataType.GDT_CFloat32)
        {
            Driver drv = Gdal.GetDriverByName("GTiff");
            string[] options = new string[] { "BLOCKXSIZE=" + width, "BLOCKYSIZE=" + height };
            Dataset ds = drv.Create(fullFilename, width, height, _band, DataType.GDT_CFloat32, options);
            if (nGeoTrans != null)
                ds.SetGeoTransform(nGeoTrans);
            foreach (var key in _bandDict.Keys)
            {
                Band ba = ds.GetRasterBand(key);
                ba.WriteRaster(0, 0, width, height, _bandDict[key], width, height, 0, 0);
            }
            ds.FlushCache();
        }

        public void Dispose()
        {
            _bandDict.Clear();
        }
    }
}
