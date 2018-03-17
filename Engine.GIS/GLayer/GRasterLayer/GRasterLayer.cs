using Engine.GIS.GEntity;
using Engine.GIS.GLayer.GRasterLayer.GBand;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Engine.GIS.GLayer.GRasterLayer
{
    public class GRasterLayer
    {
        /// <summary>
        /// 构建raster层
        /// </summary>
        /// <param name="rasterFilename"></param>
        public GRasterLayer(string rasterFilename)
        {
            //注册gdal库
            Gdal.AllRegister();
            //图层名
            _name = Path.GetFileNameWithoutExtension(rasterFilename);
            //只读方式读取图层
            _pDataSet = Gdal.Open(rasterFilename, Access.GA_ReadOnly);
            //波段数目
            _bandCount = _pDataSet.RasterCount;
            //读取band
            _bandCollection = new List<IGBand>();
            for(int i = 1; i <= _bandCount; i++)
            {
                Band pBand = _pDataSet.GetRasterBand(i);
                _pDataType = pBand.DataType;
                if(_pDataType == DataType.GDT_Float32 || _pDataType == DataType.GDT_Byte)
                    _bandCollection.Add(new GFloat32Band(pBand));
            }
        }
        #region 属性字段
        //波段格式
        DataType _pDataType;
        //波段集合
        List<IGBand> _bandCollection;
        //图层名，和文件名一致
        string _name;
        //波段层数
        int _bandCount;
        // GDAL Dataset
        Dataset _pDataSet;
        /// <summary>
        /// height
        /// </summary>
        public int YSize { get => _pDataSet.RasterYSize; }
        /// <summary>
        /// width
        /// </summary>
        public int XSize { get => _pDataSet.RasterXSize; }
        /// <summary>
        /// RasterLayer 图层名
        /// </summary>
        public string Name { get => _name; }
        /// <summary>
        /// GDAL Dataset
        /// </summary>
        public Dataset PDataSet { get => _pDataSet;}
        /// <summary>
        /// 波段层数
        /// </summary>
        public int BandCount { get => _bandCount;}
        /// <summary>
        /// 图层类型
        /// </summary>
        public DataType PDataType { get => _pDataType;}
        /// <summary>
        /// 波段集合
        /// </summary>
        public List<IGBand> BandCollection { get => _bandCollection;}
        /// <summary>
        /// 获取rasterLayer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<byte> GetPixel(int x, int y)
        {
            List<byte> pixels = new List<byte>();
            for(int i = 0; i < BandCount; i++)
                pixels.Add(BandCollection[i].GetByteData()[x, y]);
            return pixels;
        }

        #endregion

    }

}
