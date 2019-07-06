using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Brain.Extend;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;
using Engine.Brain.Model.RL;
using Engine.Brain.Model.RL.Env;
using Engine.Brain.Utils;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.APP
{
    class Program
    {
        static string workspace = @"D:\BaiduNetdiskDownload\scene";

        static string outputworkspace = @"D:\BaiduNetdiskDownload\scene\output";

        static string keyFilename = workspace + @"\ClsName2id.txt";

        static string trainDirectory = workspace + @"\train\";

        static string testDirectory = workspace + @"\test\";

        static string valDirectory = workspace + @"\val\";

        static Dictionary<string, int> _namekey = new Dictionary<string, int>();

        static int miniBatch = 50;

        static int row = 193, col = 193, bandcount = 3;

        /// <summary>
        /// remove xml
        /// </summary>
        /// <param name="dir"></param>
        private static void ClearXML(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles("*.xml"))
                file.Delete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        private static double[] PickSampleNormalValue(string fullFilename)
        {
            GRasterLayer featureRasterLayer = new GRasterLayer(fullFilename);
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureRasterLayer);
            int centerX = featureRasterLayer.XSize / 2;
            int centerY = featureRasterLayer.YSize / 2;
            double[] sampleValue = pRasterLayerCursorTool.PickRagneNormalValue(centerX, centerY, row, col);
            return sampleValue;
        }

        private static void CrateSampleBatch(string batchId)
        {
            //1.读取key
            using (StreamReader sr = new StreamReader(keyFilename))
            {
                string lineText = sr.ReadLine();
                do
                {
                    string[] lineKeys = lineText.Split(':');
                    _namekey[lineKeys[0]] = Convert.ToInt32(lineKeys[2]);
                    lineText = sr.ReadLine();
                } while (lineText != null);
            }
            //2.从训练集中制作 训练数据和测试数据
            DirectoryInfo root = new DirectoryInfo(trainDirectory);
            foreach (DirectoryInfo sampleDir in root.GetDirectories())
            {
                //清理xml
                ClearXML(sampleDir);
                //2.1样本标注
                //int key = _namekey[sampleDir.Name];
                string key = sampleDir.Name;
                //2.2随机获取样本
                List<FileInfo> files = sampleDir.GetFiles().ToList().RandomTakeBatch(miniBatch);
                //2.3 sample filename
                List<string> lines = new List<string>();
                //2.3获取值
                foreach (FileInfo file in files)
                {
                    try
                    {
                        //sampleing
                        double[] sampleValue = PickSampleNormalValue(file.FullName);
                        lines.Add(string.Join(",", sampleValue) + "," + key);
                    }
                    catch
                    {
                        continue;
                    }
                }
                //key
                string filename = string.Format("{0}_{1}_{2}_{3}", key, row, col, bandcount) + ".txt";
                if (!Directory.Exists(outputworkspace + @"\" + batchId))
                    Directory.CreateDirectory(outputworkspace + @"\" + batchId);
                //batchId
                using (StreamWriter sw = new StreamWriter(outputworkspace + @"\" + batchId + @"\" + filename))
                    foreach (string line in lines)
                        sw.WriteLine(line);
            }
        }

        static (double[] sample, int label) ConvertToSample(string text)
        {
            string[] samplesText = text.Split(',');
            int label = Convert.ToInt32(samplesText.Last());
            double[] sample = new double[samplesText.Length - 1];
            for (int i = 0; i < samplesText.Length - 1; i++)
                sample[i] = Convert.ToDouble(samplesText[i]);
            return (sample, label);
        }


        static void Train2()
        {
            //1.构建学习环境
            string sampleFiledir = @"D:\BaiduNetdiskDownload\scene\output\132067897763867953";
            DirectoryInfo sampleDir = new DirectoryInfo(sampleFiledir);
            List<double[]> samples = new List<double[]>();
            List<int> labels = new List<int>();
            List<int> keys = new List<int>();
            int process = 0;
            foreach (FileInfo file in sampleDir.GetFiles())
            {
                process++;
                Console.WriteLine(string.Format("{0} 载入样本中，第{1}/{2}个", DateTime.Now.ToLongTimeString(), process, 45));
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
            //2.训练
            IEnv env = new SamplesEnv(samples.ToArray(), labels.ToArray());
            //0-GPU ，1-CPU 
            IDSupportDQN actor = new DNet2(NP.CNTK.DeviceCollection[0], 193, 193, 3, 45);
            IDSupportDQN critic = new DNet2(NP.CNTK.DeviceCollection[0], 193, 193, 3, 45);
            dqn = new DQN(env, actor, critic, epochs: 1000);
            double dNet2Loss = 0;
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                dNet2Loss = loss;
                Console.WriteLine(string.Format("{0}:训练中，当前精度: {1:P}, 奖励: {2}, 进度: {3:P}, loss: {4}", DateTime.Now.ToLongTimeString(), accuracy, totalReward, progress, loss));
            };
            dqn.Learn();
        }

        static void Train()
        {
            //1.构建学习环境
            string sampleRootDir = @"D:\BaiduNetdiskDownload\scene\output";
            //2.训练
            IEnv env = new SamplesBatchEnv(sampleRootDir);
            //0-GPU ，1-CPU 
            IDSupportDQN actor = new DNet2(NP.CNTK.DeviceCollection[1], 193, 193, 3, 45);
            IDSupportDQN critic = new DNet2(NP.CNTK.DeviceCollection[1], 193, 193, 3, 45);
            dqn = new DQN(env, actor, critic, epochs: 1000);
            double dNet2Loss = 0;
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                dNet2Loss = loss;
                Console.WriteLine(string.Format("{0}:训练中，当前精度: {1:P}, 奖励: {2}, 进度: {3:P}, loss: {4}", DateTime.Now.ToLongTimeString(), accuracy, totalReward, progress, loss));
            };
            dqn.Learn();
        }

        static DQN dqn;

        static void Apply()
        {
            //3.应用模型
            DirectoryInfo testRoot = new DirectoryInfo(testDirectory);
            ClearXML(testRoot);
            using (StreamWriter sw = new StreamWriter(workspace + @"\result.txt"))
                foreach (FileInfo file in testRoot.GetFiles())
                {
                    double[] sampleValue = PickSampleNormalValue(file.FullName);
                    var (action, q) = dqn.ChooseAction(sampleValue);
                    int classType = dqn.ActionToRawValue(NP.Argmax(action));
                    sw.WriteLine(string.Format("{0} {1}", file.Name, classType));
                }
        }

        static void Main(string[] args)
        {
            //0.样本制作
            //for(int i = 0; i < 99; i++) CrateSampleBatch(DateTime.Now.ToFileTimeUtc().ToString());
            //1.train
            Train();
            //2.apply
            Apply();
        }

    }
}

