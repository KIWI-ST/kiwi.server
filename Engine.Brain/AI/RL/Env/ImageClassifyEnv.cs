using Accord.Math;
using Engine.Brain.Entity;
using Engine.Brain.Extend;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.Brain.AI.RL.Env
{
    /// <summary>
    ///  the environment of image classification
    /// </summary>
    public class ImageClassifyEnv : IEnv
    {
        Dictionary<int, List<Point>> _memory = new Dictionary<int, List<Point>>();

        int[] _randomSeedKeys;

        private IRasterLayerCursorTool _pGRasterLayerCursorTool = new GRasterLayerCursorTool();

        private GRasterLayer _featureRasterLayer, _labelRasterLayer;
        /// <summary>
        /// x,y position
        /// </summary>
        int _current_x, _current_y;
        /// <summary>
        /// use one-hot vector represent image class(anno)
        /// </summary>
        double[] _current_classindex;
        /// <summary>
        /// 指定观察的图像，和样本所在的层位置
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="sampleIndex"></param>
        public ImageClassifyEnv(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer)
        {
            //input feature raster layer
            _featureRasterLayer = featureRasterLayer;
            //groundtruth raster layer
            _labelRasterLayer = labelRasterLayer;
            //num of categories 
            //标注层要求：
            //1.分类按照顺序，从1开始，逐步+1
            //2.背景值设置为0
            //ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max - _labelRasterLayer.BandCollection[0].Min);
            ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max - 0);
            //statical graph
            Prepare();
        }
        /// <summary>
        /// number of actions
        /// </summary>
        public int ActionNum { get; }
        /// <summary>
        /// number of features
        /// </summary>
        public int[] FeatureNum { get { return new int[] { _featureRasterLayer.BandCount }; } }
        /// <summary>
        /// 
        /// </summary>
        public int[] RandomSeedKeys { get { return _randomSeedKeys; } }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSingleAction { get { return true; } }
        /// <summary>
        /// 处理之后的样本集
        /// </summary>
        public Dictionary<int, List<Point>> Memory { get { return _memory; } }
        /// <summary>
        /// 分析标注道路区域
        /// </summary>
        public void Prepare()
        {
            //
            IBandStasticTool pBandStasticTool = new GBandStasticTool();
            pBandStasticTool.Visit(_labelRasterLayer.BandCollection[0]);
            //
            _pGRasterLayerCursorTool.Visit(_featureRasterLayer);
            //
            _memory = pBandStasticTool.StaisticalRawGraph;
            _randomSeedKeys = _memory.Keys.ToArray();
            //
            (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] Reset()
        {
            return Step(null).state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private (int x, int y, double[] classIndex) RandomAccessMemory()
        {
            //use actionNumber represent real types
            int rawValueIndex = NP.Random(_randomSeedKeys);
            Point p = _memory[rawValueIndex].RandomTake();
            //current one-hot action
            double[] classIndex = NP.ToOneHot(Array.IndexOf(_randomSeedKeys, rawValueIndex), ActionNum);
            return (p.X, p.Y, classIndex);
        }
        /// <summary>
        /// random测试集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<double[]> states, double[][] labels) RandomEval(int batchSize = 64)
        {
            List<double[]> states = new List<double[]>();
            double[][] labels = new double[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                var (x, y, classIndex) = RandomAccessMemory();
                double[] normal = _pGRasterLayerCursorTool.PickNormalValue(x, y);
                states.Add(normal);
                labels[i] = classIndex;
            }
            return (states, labels);
        }
        /// <summary>
        /// random数据集
        /// </summary>
        public double[] RandomAction()
        {
            int action = NP.Random(ActionNum);
            return NP.ToOneHot(action, ActionNum);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">use null to reset environment,else use one-hot vector</param>
        /// <returns></returns>
        public (double[] state, double reward) Step(double[] action)
        {
            if (action == null)
            {
                var (_c_x, _c_y, _c_classIndex) = (_current_x, _current_y, _current_classindex);
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                double[] raw = _pGRasterLayerCursorTool.PickNormalValue(_c_x, _c_y);
                return (raw, 0.0);
            }
            else
            {
                double reward = NP.Argmax(action) == NP.Argmax(_current_classindex) ? 1.0 : -1.0;
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                double[] raw = _pGRasterLayerCursorTool.PickNormalValue(_current_x, _current_y);
                return (raw, reward);
            }
        }

    }
}