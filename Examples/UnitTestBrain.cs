using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine.Brain.Extend;
using Engine.Brain.Method;
using Engine.Brain.Method.Convolution;
using Engine.Brain.Method.DeepQNet;
using Engine.Brain.Method.DeepQNet.Env;
using Engine.Brain.Method.DeepQNet.Net;
using Engine.Brain.Method.Discriminate;
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

        /// <summary>
        /// feature layer
        /// </summary>
        string saveModelFilename = Directory.GetCurrentDirectory() + @"\Datasets\model.bin";

        [TestMethod]
        public void ClassificationByDQNWithRasterEnv()
        {
            double _loss = 1.0;
            GRasterLayer featureLayer = new GRasterLayer(featureFilename);
            GRasterLayer labelLayer = new GRasterLayer(trainFilename);
            //create environment for agent exploring
            IEnv env = new ImageClassifyEnv(featureLayer, labelLayer);
            int actionsNumber = env.ActionNum;
            int featuresNumber = env.FeatureNum.Product();
            int[] actionKeys = env.RandomSeedKeys;
            ISupportNet actor = new DNetDNN(env.FeatureNum, actionsNumber);
            ISupportNet critic = new DNetDNN(env.FeatureNum, actionsNumber);
            //create dqn alogrithm
            IDeepQNet dqn = new DQN(actor, critic, actionsNumber, featuresNumber, actionKeys);
            //in order to do this quickly, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            dqn.PrepareLearn(env, 20, 0.0f);
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
            float[] state = pRasterLayerCursorTool.PickNormalValue(50, 50);
            int landCoverType = dqn.Predict(state);
            //do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            //the classification results are not stable because of the training epochs are too few.
            Assert.IsTrue(landCoverType >= 0);
        }

        [TestMethod]
        public void ClassificationByDQNWithSampleEnv()
        {
            //read sample and create environment
            IEnv env;
            string deviceName = NP.CNTKHelper.DeviceCollection[0];
            //read samples
            List<List<float>> inputList = new List<List<float>>();
            List<int> outputList = new List<int>();
            List<int> keys = new List<int>();
            using (StreamReader sr = new StreamReader(envFilename9x9x18))
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
                //convert to double[][] and double[]
                int count = inputList.Count;
                float[][] x = new float[count][];
                int[] y = outputList.ToArray();
                for (int i = 0; i < count; i++)
                    x[i] = inputList[i].ToArray();
                //create environment for agent exploring
                env = new SamplesEnv(x, y);
            }
            int actionsNumber = env.ActionNum;
            int featuresNumber = env.FeatureNum.Product();
            int[] actionKeys = env.RandomSeedKeys;
            double dloss = 1;
            //use DNet2(CNN) for dqn training
            ISupportNet actor = new DNetCNN(deviceName, 9, 9, 18, keys.Count);
            ISupportNet critic = new DNetCNN(deviceName, 9, 9, 18, keys.Count);
            //create dqn alogrithm
            IDeepQNet dqn = new DQN(actor, critic, actionsNumber, featuresNumber, actionKeys);
            //in order to test fast, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            dqn.PrepareLearn(env, 6, 0.0f);
            //register event to get information while training
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                dloss = loss;
            };
            //start dqn alogrithm learning
            dqn.Learn();
            Assert.IsTrue(dloss < 1.0);
            //apply 
            GRasterLayer featureLayer = new GRasterLayer(featureFilename);
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureLayer);
            float[] state = pRasterLayerCursorTool.PickRagneNormalValue(50, 50, 9, 9);
            int cover1 = dqn.Predict(state);
            //save 
            NP.SupportModel.SaveModel(dqn, saveModelFilename);
            IDeepQNet loaded = NP.SupportModel.Load(saveModelFilename, deviceName) as IDeepQNet;
            int cover2 =  loaded.Predict(state);
            Assert.Equals(cover1, cover2);
        }

        [TestMethod]
        public void ClassificationByCNN()
        {
            List<int> keys = new List<int>();
            using (StreamReader sr = new StreamReader(envFilename3x3x18))
            {
                List<List<float>> inputList = new List<List<float>>();
                List<int> outputList = new List<int>();
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
                //convert to double[][] and double[]
                int count = inputList.Count;
                float[][] x = new float[count][];
                float[][] y = new float[count][];
                for (int i = 0; i < count; i++)
                {
                    x[i] = inputList[i].ToArray();
                    y[i] = NP.ToOneHot(outputList[i], keys.Count);
                }
                double _loss = 1.0;
                int epochs = 100, batchSize = 19;
                //use fullychannelnet 
                IConvNet cnn = new FullyChannelNet9(3, 3, 18, keys.Count, NP.CNTKHelper.DeviceCollection[0]);
                //training epochs
                for (int i = 0; i < epochs; i++)
                {
                    NP.Shuffle(x, y);
                    float[][] inputs = x.Take(batchSize).ToArray();
                    float[][] labels = y.Take(batchSize).ToArray();
                    _loss = cnn.Train(inputs, labels);
                }
                //training result
                Assert.IsTrue(_loss < 1.0);
            }
        }

        [TestMethod]
        public void EmbeddingNet()
        {
            var deviceName = NP.CNTKHelper.DeviceCollection[0];
            GloVeNet net = new GloVeNet(deviceName, gloVeFilename);
            //net.UseGloVeWordEmebdding(imdbDir, gloveFullFilename);

            var woman = net.Predict("boy");
            var man = net.Predict("girl");
            var madam = net.Predict("brother");
            var sir = net.Predict("sister");

            //var s1 = NP.Sub(woman, man);
            //var s2 = NP.Sub(madam, sir);
            //var s3 = NP.Cosine(s1, s2);
        }

        [TestMethod]
        public void ClassificationByDNN()
        {
            List<float[]> inputList = new List<float[]>();
            List<float> labelList = new List<float>();
            //
            string featureFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\dnnSamples.csv";
            using (StreamReader sr = new StreamReader(featureFullFilename))
            {
                string text = sr.ReadLine();
                while (text != null)
                {
                    string[] texts = text.Split(',');
                    float lable = float.Parse(texts.Last());
                    float[] input = new float[texts.Length - 1];
                    for (int i = 0; i < texts.Length - 1; i++)
                        input[i] = float.Parse(texts[i]);
                    inputList.Add(input);
                    labelList.Add(lable);
                    //read new line
                    text = sr.ReadLine();
                }
            }
            int count = inputList.Count;
            float[][] inputs = new float[count][];
            float[][] labels = new float[count][];
            for (int i = 0; i < count; i++)
            {
                inputs[i] = inputList[i];
                labels[i] = new float[1] { labelList[i] };
            }
            INeuralNet net = new DNetDNN(new int[] { 8, 1, 1 }, 8);
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
            string devicesName = NP.CNTKHelper.DeviceCollection[0];
            //IConvNet cnn = new FullyChannelNet9(11, 11, 18, 10, devicesName);
            string filename = @"C:\Users\AXMAND\Desktop\test\2.bin";
            //
            //NP.SupportModel.SaveModel(cnn, filename);

            IMachineLarning s = NP.SupportModel.Load(filename, devicesName);

            IConvNet net = s as IConvNet;

            //IConvNet cnn2 = NP.CNTKHelper.LoadModel(modelFilename, devicesName);
        }

        [TestMethod]
        public void ClassificationByRF()
        {
            double loss = 1.0;
            //Randforest Method
            RandomForest rf = new RandomForest(30);
            using (StreamReader sr = new StreamReader(samplesFilename))
            {
                List<List<float>> inputList = new List<List<float>>();
                List<int> outputList = new List<int>();
                string text = sr.ReadLine();
                do
                {
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
                List<List<float>> inputList = new List<List<float>>();
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
                    List<float> inputItem = new List<float>();
                    for (int i = 0; i < rawdatas.Length - 1; i++)
                        inputItem.Add(float.Parse(rawdatas[i]));
                    inputList.Add(inputItem);
                    text = sr.ReadLine();
                } while (text != null);
                float[][] inputs = new float[inputList.Count][];
                int[] outputs = outputList.ToArray();
                //convert to training samples
                for (int i = 0; i < inputList.Count; i++)
                {
                    inputs[i] = inputList[i].ToArray();
                    //make sure the type value range form 0 to +
                    outputs[i] = keys.IndexOf(outputList[i]);
                }
                //svm
                IDiscriminate l2svm = new L2SVM();
                loss = l2svm.Train(inputs, outputs);
            }
            Assert.IsTrue(loss < 1.0);
        }

    }
}
