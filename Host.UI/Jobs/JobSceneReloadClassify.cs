﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Extend;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;
using Engine.Brain.Model.RL;
using Engine.Brain.Model.RL.Env;
using Engine.Brain.Utils;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.UI.Jobs
{
    /// <summary>
    /// image classification based on dqn
    /// </summary>
    public class JobSceneReloadClassify : IJob
    {
        /// <summary>
        /// background thread
        /// </summary>
        Thread _t;

        /// <summary>
        /// task name
        /// </summary>
        public string Name => "DQNSceneReloadClassificationTask";

        /// <summary>
        /// run process
        /// </summary>
        public double Process { get; private set; } = 0.0;

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

        /// <summary>
        /// 
        /// </summary>
        public event OnStateChangedHandler OnStateChanged;


        /// <summary>
        /// classification tast by DQN
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="envSampleFilename"></param>
        /// <param name="totalEpochs"></param>
        public JobSceneReloadClassify(string trainDirectoryName, string applyDirectoryName, string dqnModelDirectoryName,int totalEpochs=12000, int switchEpoch = 55)
        {
            _t = new Thread(() =>
            {
                string deviceName = NP.CNTK.DeviceCollection[0];
                int row = 193, col = 193;
                //初始化训练
                Summary = "初始化样本中...";
                List<DirectoryInfo> samplesDirCollection = new DirectoryInfo(trainDirectoryName).GetDirectories().ToList();
                IEnv env = LoadSampleBatch(samplesDirCollection.RandomTake());
                DQN dqn = DQN.ReLoad(dqnModelDirectoryName, deviceName, env, epochs: totalEpochs, switchEpoch: switchEpoch);
                dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) => {
                    Process = progress;
                    Summary = string.Format("accuracy: {0:P}, loss:{1:0.000}, reward:{2}", accuracy, loss, totalReward);
                };
                dqn.OnSwitchEnvironmentHandler += ()=> {
                    Summary = "环境切换, 载入中...";
                    samplesDirCollection = new DirectoryInfo(trainDirectoryName).GetDirectories().ToList();
                    env = LoadSampleBatch(samplesDirCollection.RandomTake());
                    dqn.Env = env;
                };
                dqn.OnSaveCheckpointHandler += (int i) =>
                {
                    //保存模型
                    string dqnDirectoryName = dqn.PersistencNative();
                    if (i % 10 == 0)
                    {
                        string resultFilename = dqnDirectoryName + @"\result.txt";
                        ApplyModel(applyDirectoryName, resultFilename, row, col, dqn);
                    }
                };
                //开始训练
                dqn.Learn();
                Summary = "DQN场景分类完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, "Complete");
            });
        }

        private IEnv LoadSampleBatch(DirectoryInfo sampleDir)
        {
            List<double[]> samples = new List<double[]>();
            List<int> labels = new List<int>();
            foreach (FileInfo file in sampleDir.GetFiles())
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string text = sr.ReadLine();
                    do
                    {
                        var (sample, label) = ConvertToSample(text);
                        samples.Add(sample);
                        labels.Add(label);
                        text = sr.ReadLine();
                    } while (text != null);
                }
            }
            return new SamplesEnv(samples.ToArray(), labels.ToArray());
        }

        private (double[] sample, int label) ConvertToSample(string text)
        {
            string[] samplesText = text.Split(',');
            int label = Convert.ToInt32(samplesText.Last());
            double[] sample = new double[samplesText.Length - 1];
            for (int i = 0; i < samplesText.Length - 1; i++)
                sample[i] = Convert.ToDouble(samplesText[i]);
            return (sample, label);
        }

        private void ApplyModel(string applyDirectoryName, string resultFilename, int row, int col, DQN dqn)
        {
            DirectoryInfo applyRoot = new DirectoryInfo(applyDirectoryName);
            ClearXML(applyRoot);
            using (StreamWriter sw = new StreamWriter(resultFilename))
                foreach (FileInfo file in applyRoot.GetFiles())
                {
                    try
                    {
                        double[] sampleValue = PickSampleNormalValue(file.FullName, row, col);
                        var (action, q) = dqn.ChooseAction(sampleValue);
                        int classType = dqn.ActionToRawValue(NP.Argmax(action));
                        sw.WriteLine(string.Format("{0} {1}", file.Name, classType));
                    }
                    catch
                    {
                        continue;
                    }
     
                }
        }

        /// <summary>
        /// remove xml
        /// </summary>
        /// <param name="dir"></param>
        private void ClearXML(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles("*.xml"))
                file.Delete();
        }

        private double[] PickSampleNormalValue(string fullFilename, int row, int col)
        {
            GRasterLayer featureRasterLayer = new GRasterLayer(fullFilename);
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureRasterLayer);
            int centerX = featureRasterLayer.XSize / 2;
            int centerY = featureRasterLayer.YSize / 2;
            double[] sampleValue = pRasterLayerCursorTool.PickRagneNormalValue(centerX, centerY, row, col);
            return sampleValue;
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
            Summary = string.Format("accuracy: {0:P}, loss:{1:0.000}, reward:{2}", accuracy, loss, totalReward);
        }
        /// <summary>
        /// example samples
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename)
        {
            //_env.Export(fullFilename);
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
