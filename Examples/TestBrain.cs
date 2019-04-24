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
        string envFilename2 = Directory.GetCurrentDirectory() + @"\Datasets\3x3x18x100.txt";
        /// <summary>
        /// dqn environment full filename
        /// </summary>
        string envFilename1 = Directory.GetCurrentDirectory() + @"\Datasets\9x9x18x100.txt";
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
        public void ClassificationByDQN()
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
        public void ClassificationByDQNWithDNet()
        {
            IEnv env;
            //read samples
            List<List<double>> inputList = new List<List<double>>();
            List<int> outputList = new List<int>();
            List<int> keys = new List<int>();
            using (StreamReader sr = new StreamReader(envFilename2))
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
            string msg = "";
            //create cnn structure for dqn training
            IDNet actor = new DNet(new int[] { 3, 3, 18 }, keys.Count);
            IDNet critic = new DNet(new int[] { 3, 3, 18 }, keys.Count);
            //create dqn alogrithm
            DQN dqn = new DQN(env, actor, critic, epochs: 1000);
            //in order to test fast, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            //register event to get information while training
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                msg += string.Format("loss: {0}, reward: {1}, accuracy: {2}. \r\n", loss, totalReward, accuracy);
            };
            //start dqn alogrithm learning
            dqn.Learn();

            string sss2 = "";
            //in general, loss is less than 1
            //Assert.IsTrue(_loss < 1.0);
            //apply dqn to classify fetureLayer
            //pick value
            //IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            //
            //double[] state = pRasterLayerCursorTool.PickNormalValue(50, 50);
            //double[] action = dqn.ChooseAction(state).action;
            //int landCoverType = dqn.ActionToRawValue(NP.Argmax(action));
            //do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            //the classification results are not stable because of the training epochs are too few.
            //Assert.IsTrue(landCoverType >= 0);
        }

        [TestMethod]
        public void ClassificationByDQNWithDNet2()
        {
            IEnv env;
            //read samples
            List<List<double>> inputList = new List<List<double>>();
            List<int> outputList = new List<int>();
            List<int> keys = new List<int>();
            using (StreamReader sr = new StreamReader(envFilename1))
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
            string msg = "";
            //create cnn structure for dqn training
            IDNet actor = new DNet2(NP.CNTK.DeviceCollection[0], 9, 9, 18, keys.Count);
            IDNet critic = new DNet2(NP.CNTK.DeviceCollection[0], 9, 9, 18, keys.Count);
            //create dqn alogrithm
            DQN dqn = new DQN(env, actor, critic, epochs: 10000);
            //in order to test fast, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            //register event to get information while training
            dqn.OnLearningLossEventHandler += (double loss, double totalReward, double accuracy, double progress, string epochesTime) =>
            {
                msg += string.Format("loss: {0}, reward: {1}, accuracy: {2}. \r\n", loss, totalReward, accuracy);
            };
            //start dqn alogrithm learning
            dqn.Learn();

            string sss2 = "";
            //in general, loss is less than 1
            //Assert.IsTrue(_loss < 1.0);
            //apply dqn to classify fetureLayer
            //pick value
            //IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            //
            //double[] state = pRasterLayerCursorTool.PickNormalValue(50, 50);
            //double[] action = dqn.ChooseAction(state).action;
            //int landCoverType = dqn.ActionToRawValue(NP.Argmax(action));
            //do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            //the classification results are not stable because of the training epochs are too few.
            //Assert.IsTrue(landCoverType >= 0);
        }

        [TestMethod]
        public void ClassificationByCNN()
        {
            //double _loss = 1.0;
            ////training epochs
            //int epochs = 1000;
            //GRasterLayer featureLayer = new GRasterLayer(featureFullFilename);
            //GRasterLayer labelLayer = new GRasterLayer(trainFullFilename);
            ////create environment for agent exploring
            //IEnv env = new ImageClassifyEnv(featureLayer, labelLayer);
            ////assume 18dim equals 3x6 (image)
            //IDConvNet cnn = new CNN(new int[] { 1, 3, 6 }, env.ActionNum);
            ////training
            //for (int i = 0; i < epochs; i++)
            //{
            //    int batchSize = 3;
            //    var (states, labels) = env.RandomEval(1, 1, batchSize);
            //    double[][] inputX = new double[batchSize][];
            //    for (int j = 0; j < batchSize; j++)
            //        inputX[j] = states[j];
            //    _loss = cnn.Train(inputX, labels);
            //}
            ////in general, loss is less than 5
            //Assert.IsTrue(_loss < 5.0);
            ////apply cnn to classify featureLayer
            //IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            //pRasterLayerCursorTool.Visit(featureLayer);
            ////get normalized input raw value
            //double[] normal = pRasterLayerCursorTool.PickNormalValue(50, 50);
            //double[] action = cnn.Predict(normal);
            //int landCoverType = env.RandomSeedKeys[NP.Argmax(action)];
            ////pred
            //cnn.ConvertToExtractNetwork();
            //double[] action2 = cnn.Predict(normal);
            ////do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            ////the classification results are not stable because of the training epochs are too few.
            //Assert.IsTrue(landCoverType >= 0);
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
        public void TrainModedByDNN()
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
        public void TrainFullyChannelNet()
        {
            //set device
            var devicesName = NP.CNTK.DeviceCollection[0];
            //training epochs
            int epochs = 3000;
            GRasterLayer featureLayer = new GRasterLayer(featureFilename);
            GRasterLayer labelLayer = new GRasterLayer(trainFilename);
            //create environment for agent exploring
            IEnv env = new ImageClassifyEnv(featureLayer, labelLayer);
            //assume 18dim equals 3x6 (image)
            IDConvNet cnn = new FullyChannelNet9(11, 11, 18, env.ActionNum, devicesName);
            string lossText = "";
            //training
            for (int i = 0; i < epochs; i++)
            {
                int batchSize = 29;
                var (states, labels) = env.RandomEval(batchSize);
                double[][] inputX = new double[batchSize][];
                for (int j = 0; j < batchSize; j++)
                    inputX[j] = states[j];
                var loss = cnn.Train(inputX, labels);
                lossText += loss + "\r\n";
            }
            string modelFilename = cnn.PersistencNative();
            //training2
            IDConvNet cnn2 = NP.CNTK.LoadModel(modelFilename, devicesName);
            for (int i = 0; i < epochs; i++)
            {
                var (states, labels) = env.RandomEval();
                var pred = cnn2.Predict(states[0]);
                var predText = NP.Argmax(pred);
                var labeText = NP.Argmax(labels[0]);

            }
            //in general, loss is less than 5
            //Assert.IsTrue(_loss < 5.0);
            //apply cnn to classify featureLayer
            //IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            //pRasterLayerCursorTool.Visit(featureLayer);
            //get normalized input raw value
            //double[] normal = pRasterLayerCursorTool.PickNormalValue(50, 50);
            //double[] action = cnn.Predict(normal);
            //int landCoverType = env.RandomSeedKeys[NP.Argmax(action)];
            //pred
            //cnn.ConvertToExtractNetwork();
            //double[] action2 = cnn.Predict(normal);
            //do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            //the classification results are not stable because of the training epochs are too few.
            //Assert.IsTrue(landCoverType >= 0);
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
            double _loss = 1.0;
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
                _loss = rf.Train(inputs, outputs);
            }
            Assert.IsTrue(_loss < 1.0);
        }
    }
}
