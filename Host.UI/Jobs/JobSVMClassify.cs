using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Method;
using Engine.Brain.Method.Discriminate;
using Engine.GIS.GEntity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.UI.Jobs
{
    public class JobSVMClassify : IJob
    {
        public bool Complete { get; private set; } = false;

        public string Name => "SVMClassificationTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime CreateTime { get; private set; } = DateTime.Now;

        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobSVMClassify(string fullFilename, GRasterLayer rasterLayer)
        {
            _t = new Thread(() =>
            {
                string[] parameters = Path.GetFileNameWithoutExtension(fullFilename).Split('_');
                int depth = Convert.ToInt32(parameters[parameters.Length - 1]);
                int width = Convert.ToInt32(parameters[parameters.Length - 2]);
                int height = Convert.ToInt32(parameters[parameters.Length - 3]);
                Summary = "SVM训练中";
                IDiscriminate svm;
                List<int> outputKey = new List<int>();
                using (StreamReader sr = new StreamReader(fullFilename))
                {
                    List<List<float>> inputList = new List<List<float>>();
                    List<int> outputList = new List<int>();
                    string text = sr.ReadLine().Replace("\t", ",");
                    do
                    {
                        string[] rawdatas = text.Split(',');
                        //make sure the label classes from 0
                        int output = Convert.ToInt32(rawdatas.Last());
                        outputList.Add(output);
                        if (!outputKey.Contains(output)) outputKey.Add(output);
                        List<float> inputItem = new List<float>();
                        for (int i = 0; i < rawdatas.Length - 1; i++)
                            inputItem.Add(float.Parse(rawdatas[i]));
                        inputList.Add(inputItem);
                        text = sr.ReadLine();
                    } while (text != null);
                    float[][] inputs = new float[inputList.Count][];
                    int[] outputs = new int[inputList.Count];
                    for (int i = 0; i < inputList.Count; i++)
                    {
                        inputs[i] = inputList[i].ToArray();
                    }
                    for (int i = 0; i < inputList.Count; i++)
                    {
                        outputs[i] = outputKey.IndexOf(outputList[i]);
                    }
                    int inputDiminsion = inputs[0].Length;
                    int outputDiminsion = outputKey.Count;
                    svm = new L2SVM();
                    svm.Train(inputs, outputs);
                }
                Summary = "分类应用中";
                IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
                pRasterLayerCursorTool.Visit(rasterLayer);
                //GDI graph
                int seed = 0;
                int totalPixels = rasterLayer.XSize * rasterLayer.YSize;
                byte[] buffer = new byte[totalPixels];
                Process = 0.0;
                //应用dqn对图像分类
                for (int i = 0; i < rasterLayer.XSize; i++)
                    for (int j = 0; j < rasterLayer.YSize; j++)
                    {
                        //get normalized input raw value
                        float[] raw = pRasterLayerCursorTool.PickRagneNormalValue(i, j, width, height);
                        int gray = svm.Predict(raw);
                        buffer[j*rasterLayer.XSize + i] = Convert.ToByte(gray);
                        //report progress
                        Process = (double)seed++ / totalPixels;
                    }
                //保存结果至tmp
                string fullFileName = Directory.GetCurrentDirectory() + @"\tmp\" + DateTime.Now.ToFileTimeUtc() + ".png";
                Bitmap classificationBitmap = GBitmap.ToGrayBitmap(buffer, rasterLayer.XSize, rasterLayer.YSize);
                classificationBitmap.Save(fullFileName);
                //rf complete
                Summary = "SVM训练分类完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, fullFileName);
            });
        }


        public void Export(string fullFilename)
        {

        }

        public void Start()
        {
            CreateTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }

    }
}
