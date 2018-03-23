using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GLayer.GRasterLayer.GBand;
using System.Drawing;

namespace Engine.GIS.Entity
{
    public class Bitmap2
    {
        string _name, _dec;

        Bitmap _bitmap;

        GRasterLayer _gdalLayer;

        IGBand _gdalBand;

        public Bitmap2(Bitmap bmp = null, string name = "", string dec = "", GRasterLayer gdalLayer = null, IGBand gdalBand = null)
        {
            _bitmap = bmp;
            _name = name;
            _dec = dec;
            _gdalBand = gdalBand;
            _gdalLayer = gdalLayer;
        }
        /// <summary>
        /// bitmap原始数据
        /// </summary>
        public Bitmap BMP
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }
        /// <summary>
        /// 图片名
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// 图片描述
        /// </summary>
        public string Dec
        {
            get { return _dec; }
        }

        public GRasterLayer GdalLayer
        {
            get
            {
                return _gdalLayer;
            }
        }
    }
}
