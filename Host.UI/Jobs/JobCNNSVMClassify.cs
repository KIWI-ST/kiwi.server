using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Method;
using Engine.Brain.Method.Convolution;
using Engine.Brain.Method.Discriminate;
using Engine.Brain.Utils;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.UI.Jobs
{
    public class JobCNNSVMClassify : IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name => "CNNSVMClassificationTask";

        public string Summary { get; private set; } = "";

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;
        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobCNNSVMClassify(GRasterLayer rasterLayer, int epochs, int model, int width, int height, int channel, string sampleFilename,string deviceName)
        {
            _t = new Thread(() =>
            {
                List<List<float>> inputList = new List<List<float>>();
                List<int> outputList = new List<int>();
                List<int> outputKey = new List<int>();
                using (StreamReader sr = new StreamReader(sampleFilename))
                {
                    string text = sr.ReadLine().Replace("\t", ",");
                    do
                    {
                        string[] rawdatas = text.Split(',');
                        int key = Convert.ToInt32(rawdatas.Last());
                        outputList.Add(key);
                        if (!outputKey.Contains(key)) outputKey.Add(key);
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
                int outputClassNum = outputKey.Count;
                int[] keysArray = outputKey.ToArray();
                int batchSize = 19;
                IConvNet cnn = new FullyChannelNet9(width, height, channel, outputClassNum, deviceName);
                //train model
                for (int i = 0; i < epochs; i++)
                {
                    float[][] cnnInputs = new float[batchSize][];
                    float[][] cnnLabels = new float[batchSize][];
                    for (int k = 0; k < batchSize; k++)
                    {
                        int index = NP.Random(smapleSize);
                        cnnInputs[k] = inputList[index].ToArray();
                        cnnLabels[k] = NP.ToOneHot(Array.IndexOf(keysArray, outputList[index]), outputClassNum);
                    }
                    double loss = cnn.Train(cnnInputs, cnnLabels);
                    Process = (double)i / epochs;
                    Summary = string.Format("loss:{0}", loss);
                }
                //training svm
                Summary = "SVM训练中";
                //1. convert to characteristic network
                cnn.ConvertToExtractNetwork();
                float[][] svmInputs = new float[inputList.Count][];
                int[] svmOutputs = new int[inputList.Count];
                //2.recalcute smaples
                for (int i = 0; i < inputList.Count; i++)
                {
                    svmInputs[i] = cnn.Predict(inputList[i].ToArray());
                }
                for (int i = 0; i < inputList.Count; i++)
                {
                    svmOutputs[i] = outputKey.IndexOf(outputList[i]);
                }
                int inputDiminsion = svmInputs[0].Length;
                int outputDiminsion = outputKey.Count;
                IDiscriminate svm = new L2SVM();
                svm.Train(svmInputs, svmOutputs);
                //3.applay classification
                Summary = "CNNSVM分类应用中";
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(rasterLayer);
                //GDI graph
                Bitmap classificationBitmap = new Bitmap(rasterLayer.XSize, rasterLayer.YSize);
                Graphics g = Graphics.FromImage(classificationBitmap);
                //
                int seed = 0;
                int totalPixels = rasterLayer.XSize * rasterLayer.YSize;
                Process = 0.0;
                //应用dqn对图像分类
                for (int i = 0; i < rasterLayer.XSize; i++)
                    for (int j = 0; j < rasterLayer.YSize; j++)
                    {
                        //get normalized input raw value
                        float[] raw = pRasterLayerCursorTool.PickNormalValue(i, j);
                        //}{debug
                        int gray = svm.Predict(raw);
                        //convert action to raw byte value
                        Color c = Color.FromArgb(gray, gray, gray);
                        Pen p = new Pen(c);
                        SolidBrush brush = new SolidBrush(c);
                        g.FillRectangle(brush, new Rectangle(i, j, 1, 1));
                        //report progress
                        Process = (double)seed++ / totalPixels;
                    }
                //保存结果至tmp
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                classificationBitmap.Save(fullFileName);
                //rf complete
                Summary = "CNNSVM训练分类完成";
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
