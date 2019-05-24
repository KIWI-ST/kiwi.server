using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;
using Engine.Brain.Model.ML;
using Engine.Brain.Model.RL;
using Engine.Brain.Model.RL.Env;
using Engine.Brain.Utils;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Examples
{
    [TestClass]
    public class TestBrain
    {
        /// <summary>
        /// 
        /// </summary>
        string envFilename3x3x18 = Directory.GetCurrentDirectory() + @"\Datasets\3x3x18x100.txt";
        /// <summary>
        /// dqn environment full filename
        /// </summary>
        string envFilename9x9x18 = Directory.GetCurrentDirectory() + @"\Datasets\9x9x18x100.txt";
        /// <summary>
        /// feature layer
        /// </summary>
        string featureFilename = Directory.GetCurrentDirectory() + @"\Datasets\Band18.tif";
        /// <summary>
        /// train layerg
        /// </summary>
        string trainFilename = Directory.GetCurrentDirectory() + @"\Datasets\Train.tif";
        /// <summary>
        /// test layer
        /// </summary>
        string testFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\Test.tif";
        /// <summary>
        /// RF samples
        /// </summary>
        string samplesFilename = Directory.GetCurrentDirectory() + @"\Datasets\Samples.txt";
        /// <summary>
        /// glove model 
        /// </summary>
        string gloVeFilename = Directory.GetCurrentDirectory() + @"\Datasets\glove.840B.300d.txt";
        /// <summary>
        /// 
        /// </summary>
        string imdbDir = Directory.GetCurrentDirectory() + @"\Datasets\aclImdb\";


        [TestMethod]
        public void ClassificationByDQNWithRasterEnv()
        {
            double _loss = 1.0;
            GRasterLayer featureLayer = new GRasterLayer(featureFilename);
            GRasterLayer labelLayer = new GRasterLayer(trainFilename);
            //create environment for agent exploring
            IEnv env = new ImageClassifyEnv(featureLayer, labelLayer);
            //create dqn alogrithm
            DQN dqn = new DQN(env, epochs: 10);
            //in order to do this quickly, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            //register event to get information while training
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) => { _loss = loss; };
            //start dqn alogrithm learning
            dqn.Learn();
            //in general, loss is less than 1
            Assert.IsTrue(_loss < 1.0);
            //apply dqn to classify fetureLayer
            //pick value
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureLayer);
            //
            double[] state = pRasterLayerCursorTool.PickNormalValue(50, 50);
            double[] action = dqn.ChooseAction(state).action;
            int landCoverType = dqn.ActionToRawValue(NP.Argmax(action));
            //do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            //the classification results are not stable because of the training epochs are too few.
            Assert.IsTrue(landCoverType >= 0);
        }

        [TestMethod]
        public void ClassificationByDQNWithSampleEnv()
        {
            //read sample and create environment
            IEnv env;
            //read samples
            List<List<double>> inputList = new List<List<double>>();
            List<int> outputList = new List<int>();
            List<int> keys = new List<int>();
            using (StreamReader sr = new StreamReader(envFilename3x3x18))
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
                //convert to double[][] and double[]
                int count = inputList.Count;
                double[][] x = new double[count][];
                int[] y = outputList.ToArray();
                for (int i = 0; i < count; i++)
                    x[i] = inputList[i].ToArray();
                //create environment for agent exploring
                env = new SamplesEnv(x, y);
            }
            double dNetLoss = 999;
            //use DNet (DNN) for dqn training
            IDSupportDQN actor = new DNet(new int[] { 3, 3, 18 }, keys.Count);
            IDSupportDQN critic = new DNet(new int[] { 3, 3, 18 }, keys.Count);
            //create dqn alogrithm
            DQN dqn = new DQN(env, actor, critic, epochs: 10);
            //in order to test fast, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            //register event to get information while training
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                dNetLoss = loss;
            };
            //start dqn alogrithm learning
            dqn.Learn();
            //in general, loss is less than 1
            Assert.IsTrue(dNetLoss < 1.0);
            double dNet2Loss = 999;
            //use DNet2(CNN) for dqn training
            IDSupportDQN actor2 = new DNet2(NP.CNTK.DeviceCollection[0], 9, 9, 18, keys.Count);
            IDSupportDQN critic2 = new DNet2(NP.CNTK.DeviceCollection[0], 9, 9, 18, keys.Count);
            //create dqn alogrithm
            DQN dqn2 = new DQN(env, actor2, critic2, epochs: 10);
            //in order to test fast, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            //register event to get information while training
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                dNet2Loss = loss;
            };
            //start dqn alogrithm learning
            dqn.Learn();
            Assert.IsTrue(dNet2Loss < 1.0);
        }

        [TestMethod]
        public void ClassificationByCNN()
        {
            //read samples
            double[][] x;
            double[][] y;
            List<int> keys = new List<int>();
            using (StreamReader sr = new StreamReader(envFilename3x3x18))
            {
                List<List<double>> inputList = new List<List<double>>();
                List<int> outputList = new List<int>();
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
                //convert to double[][] and double[]
                int count = inputList.Count;
                x = new double[count][];
                y = new double[count][];
                for (int i = 0; i < count; i++)
                {
                    x[i] = inputList[i].ToArray();
                    y[i] = NP.ToOneHot(outputList[i], keys.Count);
                }
            }
            double _loss = 1.0;
            int epochs = 100, batchSize = 19;
            //use fullychannelnet 
            IDConvNet cnn = new FullyChannelNet9(3, 3, 18, keys.Count, NP.CNTK.DeviceCollection[0]);
            //training epochs
            for (int i = 0; i < epochs; i++)
            {
                NP.Shuffle(x, y);
                double[][] inputs = x.Take(batchSize).ToArray();
                double[][] labels = y.Take(batchSize).ToArray();
                _loss = cnn.Train(inputs, labels);
            }
            //training result
            Assert.IsTrue(_loss < 1.0);
        }

        [TestMethod]
        public void EmbeddingNet()
        {
            var deviceName = NP.CNTK.DeviceCollection[0];
            GloVeNet net = new GloVeNet(deviceName, gloVeFilename);
            //net.UseGloVeWordEmebdding(imdbDir, gloveFullFilename);

            var woman = net.Predict("boy");
            var man = net.Predict("girl");
            var madam = net.Predict("brother");
            var sir = net.Predict("sister");

            var s1 = NP.Sub(woman, man);
            var s2 = NP.Sub(madam, sir);

            var s3 = NP.Cosine(s1, s2);

        }

        [TestMethod]
        public void ClassificationByDNN()
        {
            List<double[]> inputList = new List<double[]>();
            List<double> labelList = new List<double>();
            //
            string featureFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\dnnSamples.csv";
            using (StreamReader sr = new StreamReader(featureFullFilename))
            {
                string text = sr.ReadLine();
                while (text != null)
                {
                    string[] texts = text.Split(',');
                    double lable = Convert.ToDouble(texts.Last());
                    double[] input = new double[texts.Length - 1];
                    for (int i = 0; i < texts.Length - 1; i++)
                        input[i] = Convert.ToDouble(texts[i]);
                    inputList.Add(input);
                    labelList.Add(lable);
                    //read new line
                    text = sr.ReadLine();
                }
            }
            int count = inputList.Count;
            double[][] inputs = new double[count][];
            double[][] labels = new double[count][];
            for (int i = 0; i < count; i++)
            {
                inputs[i] = inputList[i];
                labels[i] = new double[1] { labelList[i] };
            }
            IDNet net = new DNet(new int[] { 8, 1, 1 }, 8);
            string loss = "";
            for (int i = 0; i < 10000; i++)
            {
                NP.Shuffle(inputs, labels);
                loss += net.Train(inputs.Take(31).ToArray(), labels.Take(31).ToArray());
                loss += "\r\n";
            }
            string ssss = loss;
        }

        [TestMethod]
        public void SaveAndLoad()
        {
            var devicesName = NP.CNTK.DeviceCollection[0];
            IDConvNet cnn = new FullyChannelNet9(11, 11, 18, 10, devicesName);
            string modelFilename = cnn.PersistencNative();
            IDConvNet cnn2 = NP.CNTK.LoadModel(modelFilename, devicesName);
        }

        [TestMethod]
        public void ClassificationByRF()
        {
            double loss = 1.0;
            //Randforest Method
            RF rf = new RF(30);
            using (StreamReader sr = new StreamReader(samplesFilename))
            {
                List<List<double>> inputList = new List<List<double>>();
                List<int> outputList = new List<int>();
                string text = sr.ReadLine();
                do
                {
                    string[] rawdatas = text.Split(',');
                    outputList.Add(Convert.ToInt32(rawdatas.Last()));
                    List<double> inputItem = new List<double>();
                    for (int i = 0; i < rawdatas.Length - 1; i++)
                        inputItem.Add(Convert.ToDouble(rawdatas[i]));
                    inputList.Add(inputItem);
                    text = sr.ReadLine();
                } while (text != null);
                double[][] inputs = new double[inputList.Count][];
                int[] outputs = outputList.ToArray();
                for (int i = 0; i < inputList.Count; i++)
                    inputs[i] = inputList[i].ToArray();
                loss = rf.Train(inputs, outputs);
            }
            Assert.IsTrue(loss < 1.0);
        }

        [TestMethod]
        public void ClassificationBySVM()
        {
            double loss = 1.0;
            using (StreamReader sr = new StreamReader(samplesFilename))
            {
                List<List<double>> inputList = new List<List<double>>();
                List<int> outputList = new List<int>();
                List<int> keys = new List<int>();
                string text = sr.ReadLine();
                do
                {
                    string[] rawdatas = text.Split(',');
                    int key = Convert.ToInt32(rawdatas.Last());
                    if (!keys.Contains(key))
                        keys.Add(key);
                    outputList.Add(key);
                    List<double> inputItem = new List<double>();
                    for (int i = 0; i < rawdatas.Length - 1; i++)
                        inputItem.Add(Convert.ToDouble(rawdatas[i]));
                    inputList.Add(inputItem);
                    text = sr.ReadLine();
                } while (text != null);
                double[][] inputs = new double[inputList.Count][];
                int[] outputs = outputList.ToArray();
                //convert to training samples
                for (int i = 0; i < inputList.Count; i++)
                {
                    inputs[i] = inputList[i].ToArray();
                    //make sure the type value range form 0 to +
                    outputs[i] = keys.IndexOf(outputList[i]);
                }
                //svm
                L2SVM l2svm = new L2SVM(inputs[0].Length, keys.Count);
                loss = l2svm.Train(inputs, outputs);
            }
            Assert.IsTrue(loss < 1.0);
        }

    }
}
