using System;
using System.Drawing;
using System.IO;
using System.Threading;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Arithmetic;
using Engine.GIS.GOperation.Tools;
using OxyPlot;

namespace Host.UI.Jobs
{
    public class JobCOVRaster : IJob
    {
        public bool Complete { get; private set; } = false;

        public string Name => "COVRasterTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public event OnTaskCompleteHandler OnTaskComplete;

        Thread _t;

        public JobCOVRaster(GRasterBand target1band, GRasterBand target2band)
        {
            _t = new Thread(() => {
                IBandCursorTool pRasterBandCursorTool1 = new GBandCursorTool();
                IBandCursorTool pRasterBandCursorTool2 = new GBandCursorTool();
                pRasterBandCursorTool1.Visit(target1band);
                pRasterBandCursorTool2.Visit(target2band);
                //
                int seed = 0;
                int totalPixels = target1band.Width * target1band.Height;
                Bitmap bitmap = new Bitmap(target1band.Width, target1band.Height);
                Graphics g = Graphics.FromImage(bitmap);
                //
                for (int i = 0; i < target1band.Width; i++)
                    for (int j = 0; j < target1band.Height; j++)
                    {
                        double[] raw1 = pRasterBandCursorTool1.PickRangeRawValue(i, j, 3, 3);
                        double[] raw2 = pRasterBandCursorTool2.PickRangeRawValue(i, j, 3, 3);
                        double cov = ConvarianceIndex.CalcuteConvarianceIndex(raw1, raw2);
                        //拉伸-1 - 1 
                        int gray = Convert.ToInt32((cov + 1.0) * 20);
                        Color c = Color.FromArgb(gray, gray, gray);
                        Pen p = new Pen(c);
                        SolidBrush brush = new SolidBrush(c);
                        g.FillRectangle(brush, new Rectangle(i, j, 1, 1));
                        Process = (double)(seed++) / totalPixels;
                    }
                //save result
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                bitmap.Save(fullFileName);
                //
                Summary = "计算完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, fullFileName);
            });
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
