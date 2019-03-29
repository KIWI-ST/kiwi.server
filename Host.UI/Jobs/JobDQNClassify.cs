using System;
using System.Drawing;
using System.IO;
using System.Threading;
using Engine.Brain.AI.RL;
using Engine.Brain.AI.RL.Env;
using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using OxyPlot;

namespace Host.UI.Jobs
{
    /// <summary>
    /// image classification based on dqn
    /// </summary>
    public class JobDQNClassify : IJob
    {
        /// <summary>
        /// background thread
        /// </summary>
        Thread _t;
        /// <summary>
        /// 
        /// </summary>
        DQN _dqn;
        /// <summary>
        /// 
        /// </summary>
        double _gamma = 0.0;
        /// <summary>
        /// task name
        /// </summary>
        public string Name => "DqnClassificationTask";
        /// <summary>
        /// run process
        /// </summary>
        public double Process { get; private set; } = 0.0;
        /// <summary>
        /// polt models
        /// </summary>
        public PlotModel[] PlotModels => new PlotModel[] { _dqn.RewardModel, _dqn.LossPlotModel, _dqn.AccuracyModel };
        /// <summary>
        /// task start time
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; private set; }
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
        ImageClassifyEnv _env;

        /// <summary>
        /// DQN classify task
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="labelRasterLayer"></param>
        /// <param name="epochs"></param>
        public JobDQNClassify(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer, int epochs = 3000, int sampleSizeLimit = 200, bool lerpPick = true)
        {
            _t = new Thread(() =>
            {
                _env = new ImageClassifyEnv(featureRasterLayer, labelRasterLayer, sampleSizeLimit, lerpPick);
                _dqn = new DQN(_env);
                _dqn.SetParameters(epochs: epochs, gamma: _gamma);
                _dqn.OnLearningLossEventHandler += _dqn_OnLearningLossEventHandler;
                //training
                Summary = "模型训练中";
                _dqn.Learn();
                //classification
                Summary = "分类应用中";
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(featureRasterLayer);
                Bitmap classificationBitmap = new Bitmap(featureRasterLayer.XSize, featureRasterLayer.YSize);
                Graphics g = Graphics.FromImage(classificationBitmap);
                int seed = 0;
                int totalPixels = featureRasterLayer.XSize * featureRasterLayer.YSize;
                for (int i = 0; i < featureRasterLayer.XSize; i++)
                    for (int j = 0; j < featureRasterLayer.YSize; j++)
                    {
                        //get normalized input raw value
                        double[] normal = pRasterLayerCursorTool.PickNormalValue(i, j);
                        var (action, q) = _dqn.ChooseAction(normal);
                        //convert action to raw byte value
                        int gray = _dqn.ActionToRawValue(NP.Argmax(action));
                        Color c = Color.FromArgb(gray, gray, gray);
                        Pen p = new Pen(c);
                        SolidBrush brush = new SolidBrush(c);
                        g.FillRectangle(brush, new Rectangle(i, j, 1, 1));
                        //report progress
                        Process = (double)(seed++) / totalPixels;
                    }
                //save result
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                classificationBitmap.Save(fullFileName);
                //complete
                Summary = "DQN训练分类完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, fullFileName);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loss"></param>
        /// <param name="totalReward"></param>
        /// <param name="accuracy"></param>
        /// <param name="progress"></param>
        /// <param name="epochesTime"></param>
        private void _dqn_OnLearningLossEventHandler(double loss, double totalReward, double accuracy, double progress, string epochesTime)
        {
            Process = progress;
        }

        /// <summary>
        /// example samples
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename)
        {
            _env.Export(fullFilename);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
