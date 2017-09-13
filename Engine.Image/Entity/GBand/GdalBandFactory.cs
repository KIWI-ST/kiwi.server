using OSGeo.GDAL;

namespace Engine.Image.Eneity.GBand
{
    /// <summary>
    /// 波段处理工厂
    /// </summary>
    public class GdalBandFactory
    {
        public static IGdalBand Create(DataType eDataType)
        {
            IGdalBand band;
            switch (eDataType)
            {
                case DataType.GDT_Float32:
                    band = new GdalBandFloat32();
                    break;
                case DataType.GDT_Byte:
                    band = new GdalBandFloat32();
                    break;
                default:
                    band = null;
                    break;
            }
            return band;
        }
    }
}
