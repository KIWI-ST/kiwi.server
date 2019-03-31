using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;
using Engine.Brain.Model.DL.GPU;
using Engine.Brain.Utils;
using Engine.GIS.GEntity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.UI.Jobs
{
    public class JobCNNClassify : IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name => "CnnClassificationTask";

        public string Summary { get; private set; } = "";

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobCNNClassify(GRasterLayer rasterLayer, int epochs, int model, int width, int height, int channel, string sampleFilename)
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
                int batchSize = 19;
                //LeNet CNN 
                IDNet cnn = null;
                cnn = new GLeNet5(new int[] { channel, width, height }, classNum);
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
                //classify
                Summary = "分类应用中";
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(rasterLayer);
                int seed = 0;
                int totalPixels = rasterLayer.XSize * rasterLayer.YSize;
                byte[] buffer = new byte[totalPixels];
                //应用dqn对图像分类
                for (int i = 0; i < rasterLayer.XSize; i++)
                    for (int j = 0; j < rasterLayer.YSize; j++)
                    {
                        //get normalized input raw value
                        double[] normal = pRasterLayerCursorTool.PickRagneNormalValue(i, j, width, height);
                        //}{debug
                        double[] action = cnn.Predict(normal);
                        //convert action to raw byte value
                        int gray = keys.ToArray()[NP.Argmax(action)];
                        buffer[j * rasterLayer.XSize + i] = Convert.ToByte(gray);
                        //report progress
                        Process = (double)(seed++) / totalPixels;
                    }
                //保存结果至tmp
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
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
