using Engine.Brain.AI.DL;
using Engine.Brain.Entity;
using Engine.Brain.Model.ML;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace Host.UI.Jobs
{
    public class JobCNNSVMClassify : IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name => "CNNSVMClassificationTask";

        public string Summary { get; private set; } = "";

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;
        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobCNNSVMClassify(GRasterLayer rasterLayer, int epochs, int model, int width, int height, int channel, string sampleFilename)
        {
            _t = new Thread(() =>
            {
                List<List<double>> inputList = new List<List<double>>();
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
                int classNum = outputKey.Count;
                int[] keysArray = outputKey.ToArray();
                int batchSize = 19;
                CNN cnn = new CNN(new int[] { channel, width, height }, classNum);
                //train model
                for (int i = 0; i < epochs; i++)
                {
                    double[][] cnnInputs = new double[batchSize][];
                    double[][] cnnLabels = new double[batchSize][];
                    for (int k = 0; k < batchSize; k++)
                    {
                        int index = NP.Random(smapleSize);
                        cnnInputs[k] = inputList[index].ToArray();
                        cnnLabels[k] = NP.ToOneHot(Array.IndexOf(keysArray, outputList[index]), classNum);
                    }
                    double loss = cnn.Train(cnnInputs, cnnLabels);
                    Process = (double)i / epochs;
                    Summary = string.Format("loss:{0}", loss);
                }
                //training svm
                Summary = "SVM训练中";
                //1. convert to characteristic network
                cnn.ToCharacteristicNetwork();
                double[][] svmInputs = new double[inputList.Count][];
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
                L2SVM svm = new L2SVM(inputDiminsion, outputDiminsion);
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
                        double[] raw = pRasterLayerCursorTool.PickNormalValue(i, j);
                        double[][] inputs = new double[1][];
                        inputs[0] = cnn.Predict(raw);
                        //}{debug
                        int[] ouputs = svm.Predict(inputs);
                        //from 0
                        int gray = outputKey[ouputs[0]];
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
