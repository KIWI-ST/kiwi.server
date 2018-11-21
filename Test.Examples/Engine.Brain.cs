using System;
using Engine.Brain.AI.RL;
using Engine.Brain.AI.RL.Env;
using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Examples
{
    [TestClass]
    public class BrainTEST
    {
        /// <summary>
        /// feature layer
        /// </summary>
        string featureFullFilename = System.IO.Directory.GetCurrentDirectory() + @"\Datasets\Band18.tif";
        /// <summary>
        /// train layer
        /// </summary>
        string trainFullFilename = System.IO.Directory.GetCurrentDirectory() + @"\Datasets\Train.tif";
        /// <summary>
        /// test layer
        /// </summary>
        string testFullFilename = System.IO.Directory.GetCurrentDirectory() + @"\Datasets\Test.tif";

        [TestMethod]
        public void ClassificationByDQN()
        {
            double _loss = 1.0;
            //
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
            //apply dqn to apply fetureLayer
            //pick value
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureLayer);
            //
            double[] state = pRasterLayerCursorTool.PickNormalValue(50, 50);
            double[] action = dqn.ChooseAction(state).action;
            int landCoverType = dqn.ActionToRawValue(NP.Argmax(action));
            //do something as you need. i.e. draw landCoverType to bitmap at position ( i , j )
            //the classification results are not stable because of the training epochs are too few.
            Assert.IsTrue(landCoverType>=0);
        }

        [TestMethod]
        public void ClassificationByCNN()
        {

        }


    }
}
