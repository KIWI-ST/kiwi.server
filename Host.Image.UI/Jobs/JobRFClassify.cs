using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Brain.AI.ML;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using OfficeOpenXml;
using OxyPlot;

namespace Host.Image.UI.Jobs
{
    public class JobRFClassify : IJob
    {

        double _process = 0.0;

        string _summary = "";

        public string Name => "Random Forest Job Task";

        public string Summary => _summary;

        public double Process => _process;

        public DateTime StartTime => throw new NotImplementedException();

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public event OnTaskCompleteHandler OnTaskComplete;

        public void Start(params object[] paramaters)
        {
            Thread t = new Thread(() =>
            {
                int treeCount = Convert.ToInt32(paramaters[0]);
                string fullFilename = paramaters[1] as string;
                GRasterLayer rasterLayer = paramaters[2] as GRasterLayer;
                //
                RF rf = new RF(treeCount);
                //read samples
                using (var excel = new ExcelPackage(new FileInfo(fullFilename)))
                {
                    var ws = excel.Workbook.Worksheets[1];
                    int rows = 3647;
                    int cols = ws.Cells.Columns;
                    double[][] inputs = new double[rows-2][];
                    int[] outputs = new int[rows-2];
                    for(int r = 2; r <rows; r++)
                    {
                        outputs[r-2] = Convert.ToInt32(ws.Cells[r, 3].Value);
                        inputs[r-2] = new double[9];
                        for (int c = 14; c <= 22; c++)
                            inputs[r-2][c - 14] = Convert.ToDouble(ws.Cells[r, c].Value);
                    }
                    //rf learning
                    rf.Train(inputs, outputs);
                }
                //image classify
                //bind raster layer
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(rasterLayer);
                //GDI graph
                Bitmap classificationBitmap = new Bitmap(rasterLayer.XSize, rasterLayer.YSize);
                Graphics g = Graphics.FromImage(classificationBitmap);
                //
                int seed = 0;
                int totalPixels = rasterLayer.XSize * rasterLayer.YSize;
                _process = 0.0;
                //应用dqn对图像分类
                for (int i = 0; i < rasterLayer.XSize; i++)
                    for (int j = 0; j < rasterLayer.YSize; j++)
                    {
                        //get normalized input raw value
                        double[] raw = pRasterLayerCursorTool.PickRawValue(i, j);
                        double[][] inputs = new double[1][];
                        inputs[0] = raw;
                        //}{debug
                        int[] ouputs = rf.Predict(inputs);
                        int gray = ouputs[0];
                        //convert action to raw byte value
                        Color c = Color.FromArgb(gray, gray, gray);
                        Pen p = new Pen(c);
                        SolidBrush brush = new SolidBrush(c);
                        g.FillRectangle(brush, new Rectangle(i, j, 1, 1));
                        //report progress
                        _process = (double)seed++ / totalPixels;
                    }
                //保存结果至tmp
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                classificationBitmap.Save(fullFileName);
                //complete work
                OnTaskComplete?.Invoke(Name,fullFileName);
            });
            t.IsBackground = true;
            t.Start();
        }
    }
}
