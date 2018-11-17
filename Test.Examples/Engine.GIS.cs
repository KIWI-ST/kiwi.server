using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Examples
{
    [TestClass]
    public class GISTEST
    {
        [TestMethod]
        public void LoadRasterLayer()
        {
            string fullFilename = System.IO.Directory.GetCurrentDirectory()+ @"\Datasets\A_Band18.tif";
            Engine.GIS.GLayer.GRasterLayer.GRasterLayer rasterLayer = new Engine.GIS.GLayer.GRasterLayer.GRasterLayer(fullFilename);
        }
    }
}
