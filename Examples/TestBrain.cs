using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine.Brain.AI.RL;
using Engine.Brain.AI.RL.Env;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;
using Engine.Brain.Model.ML;
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
        /// feature layer
        /// </summary>
        string featureFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\Band18.tif";
        /// <summary>
        /// train layerg
        /// </summary>
        string trainFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\Train.tif";
        /// <summary>
        /// test layer
        /// </summary>
        string testFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\Test.tif";
        /// <summary>
        /// RF samples
        /// </summary>
        string samplesFullfilename = Directory.GetCurrentDirectory() + @"\Datasets\Samples.txt";

        [TestMethod]
        public void ClassificationByDQN()
        {
            double _loss = 1.0;
            GRasterLayer featureLayer = new GRasterLayer(featureFullFilename);
            GRasterLayer labelLayer = new GRasterLayer(trainFullFilename);
            //create environment for agent exploring
            IEnv env = new ImageClassifyEnv(featureLayer, labelLayer);
            //create dqn alogrithm
            DQN dqn = new DQN(env);
            //in order to do this quickly, we set training epochs equals 10.
            //please do not use so few training steps in actual use.
            dqn.SetParameters(10, 0);
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
        public void TrainFullyChannelNet()
        {
            //set device
            var devicesName = NP.CNTK.DeviceCollection[0];
            //training epochs
            int epochs = 100;
            GRasterLayer featureLayer = new GRasterLayer(featureFullFilename);
            GRasterLayer labelLayer = new GRasterLayer(trainFullFilename);
            //create environment for agent exploring
            IEnv env = new ImageClassifyEnv(featureLayer, labelLayer);
            //assume 18dim equals 3x6 (image)
            FullyChannelNet cnn = new FullyChannelNet(11, 11, 18, env.ActionNum, devicesName);
            string lossText = "";
            //training
            for (int i = 0; i < epochs; i++)
            {
                int batchSize = 31;
                var (states, labels) = env.RandomEval(11, 11, batchSize);
                double[][] inputX = new double[batchSize][];
                for (int j = 0; j < batchSize; j++)
                    inputX[j] = states[j];
                var loss = cnn.Train(inputX, labels);
                lossText += loss + "\r\n";
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
        public void ClassificationByRF()
        {
            double _loss = 1.0;
            //Randforest Method
            RF rf = new RF(30);
            using (StreamReader sr = new StreamReader(samplesFullfilename))
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
