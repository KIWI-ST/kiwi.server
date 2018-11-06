using Engine.Brain.AI.DL;
using Engine.Brain.AI.RL.Env;
using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using OxyPlot;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Host.UI.Jobs
{
    public class JobCNNClassify : IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name => "CnnClassificationTask";

        public string Summary { get; private set; } = "";

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;

        Thread _t;

        public JobCNNClassify(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer, int epochs, int model, int width, int height, int channel)
        {
            _t = new Thread(() => {
                ImageClassifyEnv env = new ImageClassifyEnv(featureRasterLayer, labelRasterLayer);
                CNN cnn = new CNN(new int[] { channel, width, height }, env.ActionNum);
                //training
                Summary = "模型训练中";
                for (int i = 0; i < epochs; i++)
                {
                    int batchSize = cnn.BatchSize;
                    var (states, labels) = env.RandomEval(batchSize);
                    double[][] inputX = new double[batchSize][];
                    for (int j = 0; j < batchSize; j++)
                        inputX[j] = states[j];
                    double loss = cnn.Train(inputX, labels);
                    Process = (double)i / epochs;
                }
                //classify
                Summary = "分类应用中";
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(featureRasterLayer);
                //GDI graph
                Bitmap classificationBitmap = new Bitmap(featureRasterLayer.XSize, featureRasterLayer.YSize);
                Graphics g = Graphics.FromImage(classificationBitmap);
                //
                int seed = 0;
                int totalPixels = featureRasterLayer.XSize * featureRasterLayer.YSize;
                //应用dqn对图像分类
                for (int i = 0; i < featureRasterLayer.XSize; i++)
                    for (int j = 0; j < featureRasterLayer.YSize; j++)
                    {
                        //get normalized input raw value
                        double[] normal = pRasterLayerCursorTool.PickNormalValue(i, j);
                        //}{debug
                        double[] action = cnn.Predict(normal);
                        //convert action to raw byte value
                        int gray = env.RandomSeedKeys[NP.Argmax(action)];
                        //后台绘制，报告进度
                        Color c = Color.FromArgb(gray, gray, gray);
                        Pen p = new Pen(c);
                        SolidBrush brush = new SolidBrush(c);
                        g.FillRectangle(brush, new Rectangle(i, j, 1, 1));
                        //report progress
                        Process = (double)(seed++) / totalPixels;
                    }
                //保存结果至tmp
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                classificationBitmap.Save(fullFileName);
                //complete
                Summary = "CNN训练分类完成";
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
