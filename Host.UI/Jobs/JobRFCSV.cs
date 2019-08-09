using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Engine.Brain.Method;
using Engine.Brain.Method.Discriminate;

namespace Host.UI.Jobs
{
    class JobRFCSV:IJob
    {
        public string Name => "RFCSVClassificationTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime CreateTime { get; private set; } = DateTime.Now;
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
        Thread _t;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeCount"></param>
        /// <param name="fullFilename"></param>
        /// <param name="rasterLayer"></param>
        public JobRFCSV(int treeCount, string smapleFullFilename, string waitFullFilename, string saveFullFilename)
        {
            _t = new Thread(() =>
            {
                IDiscriminate rf = new RandomForest(treeCount);
                //training
                Summary = "随机森林训练中";
                using (StreamReader sr = new StreamReader(smapleFullFilename))
                {
                    List<List<float>> inputList = new List<List<float>>();
                    List<int> outputList = new List<int>();
                    string text = sr.ReadLine().Replace("\t", ",").Replace("N/A", "0");
                    do
                    {
                        text = text.Replace("N/A", "0");
                        string[] rawdatas = text.Split(',');
                        outputList.Add(Convert.ToInt32(rawdatas.Last()));
                        List<float> inputItem = new List<float>();
                        for (int i = 0; i < rawdatas.Length - 1; i++)
                            inputItem.Add(float.Parse(rawdatas[i]));
                        inputList.Add(inputItem);
                        text = sr.ReadLine();
                    } while (text != null);
                    float[][] inputs = new float[inputList.Count][];
                    int[] outputs = outputList.ToArray();
                    for (int i = 0; i < inputList.Count; i++)
                        inputs[i] = inputList[i].ToArray();
                    rf.Train(inputs, outputs);
                }
                //image classify
                Summary = "分类应用中";
                //
                using(StreamReader sr = new StreamReader(waitFullFilename))
                {
                    using (StreamWriter sw = new StreamWriter(saveFullFilename))
                    {
                        Process = 0.0;
                        string text = sr.ReadLine().Replace("\t", ",").Replace("N/A", "0");
                        while (text != null){
                            text = text.Replace("N/A", "0");
                            string[] rawdatas = text.Split(',');
                            List<float> inputItem = new List<float>();
                            for (int i = 0; i < rawdatas.Length; i++)
                                inputItem.Add(float.Parse(rawdatas[i]));
                            float[]  input = inputItem.ToArray();
                            int classtype = rf.Predict(input);
                            sw.WriteLine(classtype);
                            text = sr.ReadLine();
                            Process++;
                        } 
                    }
                }
                //rf complete
                Summary = "RF训练和分类完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, "");
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
        /// 
        /// </summary>
        /// <param name="paramaters"></param>
        public void Start()
        {
            CreateTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
