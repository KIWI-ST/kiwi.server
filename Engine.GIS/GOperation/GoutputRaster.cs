using Engine.GIS.GLayer.GRasterLayer;
using OSGeo.GDAL;

namespace Engine.GIS.GOperation
{
    /// <summary>
    /// 写入图层
    /// </summary>
    public class GoutputRaster
    {
        GLayer.GRasterLayer.GRasterLayer _rasterLayer;

        public GoutputRaster(GLayer.GRasterLayer.GRasterLayer rasterLayer)
        {
            _rasterLayer = rasterLayer;
        }

        public void Output(string outputDir)
        {
            Dataset readDs = _rasterLayer.PDataSet;
            Driver drv = Gdal.GetDriverByName("GTiff");
            string[] options = new string[] { "BLOCKXSIZE=" + readDs.RasterXSize, "BLOCKYSIZE=" + readDs.RasterYSize };
            Dataset writeDs = drv.Create(outputDir, readDs.RasterXSize, readDs.RasterYSize, 1, _rasterLayer.PDataType, options);
            for(int i=1;i<= _rasterLayer.BandCount; i++)
            {
                GRasterBand readBand = _rasterLayer.BandCollection[i - 1];
                Band writeBand = writeDs.GetRasterBand(i);
                writeBand.WriteRaster(0, 0, readDs.RasterXSize, readDs.RasterYSize, readBand.GetRawBuffer(), readDs.RasterXSize, readDs.RasterYSize, 0, 0);
            }
            writeDs.FlushCache();
        }

    }
}
