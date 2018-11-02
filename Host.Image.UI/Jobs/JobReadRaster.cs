using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.GIS.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using OxyPlot;

namespace Host.Image.UI.Jobs
{
    public class JobReadRaster : IJob
    {
        TreeNode _rootNode;
        Dictionary<string, Bitmap2> _rootImageDic;

        public JobReadRaster(TreeNode rootNode,Dictionary<string,Bitmap2> rootImageDic)
        {
            _rootNode = rootNode;
            _rootImageDic = rootImageDic;
        }

        public string Name => "Read Raster Image Task";

        public string Summary => throw new NotImplementedException();

        public double Process => throw new NotImplementedException();

        public DateTime StartTime => throw new NotImplementedException();

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public event OnTaskCompleteHandler OnTaskComplete;

        public void Start(params string[] paramaters)
        {
            Thread t = new Thread(()=> {
                string fullFilename = paramaters[0];
                string name = Path.GetFileNameWithoutExtension(fullFilename);
                GRasterLayer rasterLayer = new GRasterLayer(fullFilename);
                for (int i = 0; i < rasterLayer.BandCount; i++)
                {
                    GRasterBand band = rasterLayer.BandCollection[i];
                    band.BandName = name + "_波段_" + i;
                    Bitmap2 bmp2 = new Bitmap2(bmp: band.GrayscaleImage, name: band.BandName, gdalBand: band, gdalLayer: rasterLayer);
                    _rootImageDic.Add(band.BandName, bmp2);
                    TreeNode childrenNode = new TreeNode(band.BandName);
                    


                }
                //
            });

        }

    }
}
