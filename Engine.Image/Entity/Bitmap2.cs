using Engine.Image.Eneity.GBand;
using Engine.Image.Eneity.GLayer;
using System.Drawing;

namespace Engine.Image
{
    /// <summary>
    /// 带名称的BitMap,处理难应对的索引关系
    /// </summary>
    public class Bitmap2
    {
        string _name, _dec;

        Bitmap _bitmap;

        IGdalLayer _gdalLayer;

        IGdalBand _gdalBand;

        public Bitmap2(Bitmap bmp = null, string name = "", string dec = "", IGdalLayer gdalLayer = null, IGdalBand gdalBand = null)
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
    }
}
