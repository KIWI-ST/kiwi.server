using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Host.UI.Jobs
{
    public class IJobRPCRectify : IJob
    {
        public bool Complete { get; private set; } = false;

        public string Name => "RPCRasterRectifyTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public event OnTaskCompleteHandler OnTaskComplete;

        Thread _t;

        public IJobRPCRectify(double[] a, double[] b, double[] c, double[] d, Dictionary<string, double> paramaters, List<string> rawBinRasterFullFilenames)
        {
            _t = new Thread(() =>
            {
                using (IRasterRPCTool pRasterRPCTool = new GRasterRPCTool(a, b, c, d, paramaters))
                {
                    for (int i = 0; i < rawBinRasterFullFilenames.Count; i++)
                    {
                        string rasterFilename = rawBinRasterFullFilenames[i];
                        GRasterLayer rasterLayer = new GRasterLayer(rasterFilename);
                        Summary = string.Format("total:{1}/{2}, RPC rectify for {0} is in progress.... ", rasterLayer.Name, i + 1, rawBinRasterFullFilenames.Count);
                        pRasterRPCTool.Visit(rasterLayer);
                        pRasterRPCTool.DoRPCRectify();
                        Process = i / (double)rawBinRasterFullFilenames.Count;
                    }
                    OnTaskComplete?.Invoke(Name);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename)
        {
           
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }

    }
}
