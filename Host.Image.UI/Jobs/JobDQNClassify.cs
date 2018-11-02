using Engine.Brain.AI.RL;
using Engine.Brain.AI.RL.Env;
using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using OxyPlot;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Host.Image.UI.Jobs
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
        DateTime _startTime;
        /// <summary>
        /// 
        /// </summary>
        double _process = 0.0;
        /// <summary>
        /// 
        /// </summary>
        IEnv _env;
        /// <summary>
        /// 
        /// </summary>
        DQN _dqn;
        /// <summary>
        /// 
        /// </summary>
        string _summary;
        /// <summary>
        /// 
        /// </summary>
        double _gamma = 0.0;
        /// <summary>
        /// task name
        /// </summary>
        public string Name => "DQN Classification Task";
        /// <summary>
        /// run process
        /// </summary>
        public double Process => _process;
        /// <summary>
        /// polt models
        /// </summary>
        public PlotModel[] PlotModels => new PlotModel[] { _dqn.RewardModel, _dqn.LossPlotModel , _dqn.AccuracyModel };
        /// <summary>
        /// task start time
        /// </summary>
        public DateTime StartTime => _startTime;
        /// <summary>
        /// 
        /// </summary>
        public string Summary => _summary;
        /// <summary>
        /// 
        /// </summary>
        public event OnTaskCompleteHandler OnTaskComplete;
        /// <summary>
        /// DQN classify task
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="labelRasterLayer"></param>
        /// <param name="epochs"></param>
        public JobDQNClassify(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer, int epochs = 3000)
        {
            _t = new Thread(() =>{
                _env = new ImageClassifyEnv(featureRasterLayer, labelRasterLayer);
                _dqn = new DQN(_env);
                _dqn.SetParameters(epochs: epochs, gamma: _gamma);
                _dqn.OnLearningLossEventHandler += _dqn_OnLearningLossEventHandler;
                _dqn.Learn();
                _dqn_ImageClassification(featureRasterLayer);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        private void _dqn_ImageClassification(GRasterLayer featureRasterLayer)
        {
            //bind raster layer
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
                    var (action, q) = _dqn.ChooseAction(normal);
                    //convert action to raw byte value
                    int gray = _dqn.ActionToRawValue(NP.Argmax(action));
                    //后台绘制，报告进度
                    Color c = Color.FromArgb(gray, gray, gray);
                    Pen p = new Pen(c);
                    SolidBrush brush = new SolidBrush(c);
                    g.FillRectangle(brush, new Rectangle(i, j, 1, 1));
                    //report progress
                    seed++;
                    if ((seed * 10) % totalPixels == 0)
                    {
                        double _process = (double)(seed) / totalPixels;
                        _summary = string.Format("应用模型，当前时间{0},分类进度{1:P}", DateTime.Now.ToLongTimeString(), _process);
                    }
                }
            //保存结果至tmp
            string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
            classificationBitmap.Save(fullFileName);
            OnTaskComplete?.Invoke(Name,fullFileName);
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
            _process = progress;
            _summary  = string.Format("开始时间{0}，学习进度{1:P}", _startTime.ToLongDateString()+_startTime.ToLongTimeString(), _process);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start(params string[] paramaters)
        {
            _startTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }

    }
}
