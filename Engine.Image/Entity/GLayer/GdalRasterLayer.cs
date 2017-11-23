using Engine.Image.Eneity.GBand;
using OSGeo.GDAL;
using System;

namespace Engine.Image.Eneity.GLayer
{
    /// <summary>
    /// gdal 管理器
    /// </summary>
    public class GdalRasterLayer : IGdalLayer
    {

        Container<IGdalBand> _bands;

        Dataset _pDataSet;

        public Container<IGdalBand> BandCollection { get { return _bands; } }

        public int YSize => _pDataSet.RasterYSize;

        public int XSize => _pDataSet.RasterXSize;

        public void ReadFromFile(string filePath)
        {
            _pDataSet = Gdal.Open(filePath, Access.GA_ReadOnly);
            if (_pDataSet == null)
                throw new Exception("未找到指定文件");
            //raster的图层总数
            int bandCount = _pDataSet.RasterCount;
            _bands = new Container<IGdalBand>(bandCount);
            //
            for (int count = 1; count <= bandCount; count++)
            {
                Band pBand = _pDataSet.GetRasterBand(count);
                DataType eDataType = pBand.DataType;
                IGdalBand band = GdalBandFactory.Create(eDataType);
                if (band == null)
                    continue;
                band.SetData(count, _pDataSet.RasterXSize, _pDataSet.RasterYSize, pBand);
                //设置Band信息
                _bands[count - 1] = band;
            }
        }

        public GdalRasterLayer()
        {
            Gdal.AllRegister();
        }

        private byte[] GetBufferByte(int _xCount, int _yCount, byte[,] byteData)
        {
            byte[] rawByteData = new byte[_xCount * _yCount];
            for (int count = 0; count < rawByteData.Length; count++)
                rawByteData[count] = byteData[count % _xCount, count / _xCount];
            return rawByteData;
        }

        public void SaveToFile(string filePath, byte[] byteData)
        {
            Driver drv = Gdal.GetDriverByName("GTiff");
            string[] options = new string[] { "BLOCKXSIZE=" + _pDataSet.RasterXSize, "BLOCKYSIZE=" + _pDataSet.RasterYSize };
            Dataset ds = drv.Create(filePath, _pDataSet.RasterXSize, _pDataSet.RasterYSize, 1, DataType.GDT_Byte, options);
            Band ba = ds.GetRasterBand(1);
            // GetBufferByte(_pDataSet.RasterXSize, _pDataSet.RasterYSize,byteData)
            ba.WriteRaster(0, 0, _pDataSet.RasterXSize, _pDataSet.RasterYSize, byteData, _pDataSet.RasterXSize, _pDataSet.RasterYSize, 0, 0);
            ds.FlushCache();
            //Dataset ds = drv.Create(filePath, _pDataSet.RasterXSize, _pDataSet.RasterYSize, _bands.Count, DataType.GDT_Byte, options);
            //for (int i = 1; i <= _bands.Count; i++)
            //{
            //    Band ba = ds.GetRasterBand(i);
            //    ba.WriteRaster(0, 0, _pDataSet.RasterXSize, _pDataSet.RasterYSize, _bands[i-1].GetByteBuffer(byteData), _pDataSet.RasterXSize, _pDataSet.RasterYSize, 0, 0);
            //    ba.FlushCache();
            //}
            //ds.FlushCache();
        }
    }
}
