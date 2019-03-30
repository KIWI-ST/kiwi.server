using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Engine.GIS.Entity;
using Engine.GIS.GLayer.GRasterLayer;

namespace Host.UI.Jobs
{
    public class JobReadRaster : IJob
    {

        public string Name => "ReadRasterTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime StartTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public bool Complete { get; private set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;
        /// <summary>
        /// 
        /// </summary>
        Thread _t;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        public JobReadRaster(string fullFilename)
        {
             _t = new Thread(() =>
            {
                Dictionary<string, Bitmap2> dict = new Dictionary<string, Bitmap2>();
                string name = Path.GetFileNameWithoutExtension(fullFilename);
                GRasterLayer rasterLayer = new GRasterLayer(fullFilename);
                //reading
                Summary = "数据读取中";
                for (int i = 0; i < rasterLayer.BandCount; i++)
                {
                    GRasterBand band = rasterLayer.BandCollection[i];
                    band.BandName = name + "_band_" + i;
                    Bitmap2 bmp2 = new Bitmap2(bmp: band.GrayscaleImage, name: band.BandName, gdalBand: band, gdalLayer: rasterLayer);
                    dict[band.BandName] = bmp2;
                    Process = (double)(i+1) / rasterLayer.BandCount;
                }
                //read complete
                Summary = "读取完毕";
                Complete = true;
                OnTaskComplete?.Invoke(Name, name, dict, rasterLayer);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramaters"></param>
        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
