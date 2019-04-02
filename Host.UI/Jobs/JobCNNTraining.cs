using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;
using Engine.Brain.Utils;

namespace Host.UI.Jobs
{
    /// <summary>
    /// this job train cnn model only
    /// </summary>
    public class JobCNNTraining : IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name => "CnnTrainingTask";

        public string Summary { get; private set; } = "";

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobCNNTraining(string netName,string sampleFilename, string saveModelFilename, int epochs, int width, int height, int channel, string deviceName)
        {
            _t = new Thread(() =>
            {
                //input list
                List<List<double>> inputList = new List<List<double>>();
                List<int> outputList = new List<int>();
                List<int> keys = new List<int>();
                using (StreamReader sr = new StreamReader(sampleFilename))
                {
                    string text = sr.ReadLine().Replace("\t", ",");
                    do
                    {
                        string[] rawdatas = text.Split(',');
                        int key = Convert.ToInt32(rawdatas.Last());
                        outputList.Add(key);
                        if (!keys.Contains(key))
                            keys.Add(key);
                        List<double> inputItem = new List<double>();
                        for (int i = 0; i < rawdatas.Length - 1; i++)
                            inputItem.Add(Convert.ToDouble(rawdatas[i]));
                        inputList.Add(inputItem);
                        text = sr.ReadLine();
                    } while (text != null);
                }
                //create cnn model
                Summary = "模型训练中";
                int smapleSize = outputList.Count;
                int classNum = keys.Count;
                int[] keysArray = keys.ToArray();
                int batchSize = 31;
                //LeNet CNN 
                IDConvNet cnn = new FullyChannelNet(width, height, channel, classNum, deviceName);
                //train model
                for (int i = 0; i < epochs; i++)
                {
                    double[][] inputs = new double[batchSize][];
                    double[][] labels = new double[batchSize][];
                    for (int k = 0; k < batchSize; k++)
                    {
                        int index = NP.Random(smapleSize);
                        inputs[k] = inputList[index].ToArray();
                        labels[k] = NP.ToOneHot(Array.IndexOf(keysArray, outputList[index]), classNum);
                    }
                    double loss = cnn.Train(inputs, labels);
                    Process = (double)i / epochs;
                    Summary = string.Format("loss:{0}", loss);
                }
                //
                OnTaskComplete?.Invoke(Name, "train complete, model saved in");
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
        /// start task
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
