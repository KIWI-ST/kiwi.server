using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Method;
using Engine.Brain.Method.Convolution;
using Engine.Brain.Utils;
using Engine.GIS.GEntity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.UI.Jobs
{
    /// <summary>
    /// this job train cnn model only
    /// </summary>
    public class JobCNNClassify : IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name => "CnnClassificationTask";

        public string Summary { get; private set; } = "";

        public DateTime CreateTime { get; private set; } = DateTime.Now;

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobCNNClassify(GRasterLayer rasterLayer,string netName,string sampleFilename, string saveModelFilename, int epochs, int width, int height, int channel, string deviceName)
        {
            _t = new Thread(() =>
            {
                //input list
                List<List<float>> inputList = new List<List<float>>();
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
                        List<float> inputItem = new List<float>();
                        for (int i = 0; i < rawdatas.Length - 1; i++)
                            inputItem.Add(float.Parse(rawdatas[i]));
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
                IConvNet cnn = new FullyChannelNet9(width, height, channel, classNum, deviceName);
                //train model
                for (int i = 0; i < epochs; i++)
                {
                    float[][] inputs = new float[batchSize][];
                    float[][] labels = new float[batchSize][];
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
                //save cnn model
                string modelFilename = cnn.PersistencNative();
                OnStateChanged?.Invoke(Name, string.Format("train complete, model saved in:{0}", modelFilename));
                //classify
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(rasterLayer);
                int seed = 0;
                int totalPixels = rasterLayer.XSize * rasterLayer.YSize;
                byte[] buffer = new byte[totalPixels];
                for(int j = 0; j < rasterLayer.YSize; j++)
                {
                    float[][] inputs = new float[rasterLayer.XSize][];
                    for(int i=0;i<rasterLayer.XSize;i++)
                        inputs[i] = pRasterLayerCursorTool.PickRagneNormalValue(i, j, width, height);
                    float[][] preds = cnn.Predicts(inputs);
                    for(int i=0;i< rasterLayer.XSize; i++)
                    {
                        float[] pred = preds[i];
                        int gray = keys.ToArray()[NP.Argmax(pred)];
                        buffer[j * rasterLayer.XSize + i] = Convert.ToByte(gray);
                        Process = (float)(seed++) / totalPixels;
                    }
                }
                //save result
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                Bitmap classificationBitmap = GBitmap.ToGrayBitmap(buffer, rasterLayer.XSize, rasterLayer.YSize);
                classificationBitmap.Save(fullFileName);
                //complete
                Summary = "CNN训练分类完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, fullFileName);
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
            CreateTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
