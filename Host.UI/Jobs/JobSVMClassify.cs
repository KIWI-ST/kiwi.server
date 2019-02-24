﻿using Engine.Brain.Model.ML;
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
    public class JobSVMClassify : IJob
    {
        public bool Complete { get; private set; } = false;

        public string Name => "SVMClassificationTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public event OnTaskCompleteHandler OnTaskComplete;

        Thread _t;

        public JobSVMClassify(string fullFilename, GRasterLayer rasterLayer)
        {
            _t = new Thread(() => {
                Summary = "SVM训练中";
                L2SVM svm;
                List<int> outputKey = new List<int>();
                using (StreamReader sr = new StreamReader(fullFilename))
                {
                    List<List<double>> inputList = new List<List<double>>();
                    List<int> outputList = new List<int>();
                    string text = sr.ReadLine().Replace("\t", ",");
                    do
                    {
                        string[] rawdatas = text.Split(',');
                        //make sure the label classes from 0
                        int output = Convert.ToInt32(rawdatas.Last());
                        outputList.Add(output);
                        if (!outputKey.Contains(output)) outputKey.Add(output);
                        List<double> inputItem = new List<double>();
                        for (int i = 0; i < rawdatas.Length - 1; i++)
                            inputItem.Add(Convert.ToDouble(rawdatas[i]));
                        inputList.Add(inputItem);
                        text = sr.ReadLine();
                    } while (text != null);
                    double[][] inputs = new double[inputList.Count][];
                    int[] outputs = new int[inputList.Count];
                    for (int i = 0; i < inputList.Count; i++)
                    {
                        inputs[i] = inputList[i].ToArray();
                    }
                    for(int i = 0; i < inputList.Count; i++)
                    {
                         outputs[i] = outputKey.IndexOf(outputList[i]);
                    }
                    int inputDiminsion = inputs[0].Length;
                    int outputDiminsion = outputKey.Count;
                    svm = new L2SVM(inputDiminsion, outputDiminsion);
                    svm.Train(inputs, outputs);
                }
                Summary = "分类应用中";
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
                        inputs[0] = raw;
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
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }

    }
}