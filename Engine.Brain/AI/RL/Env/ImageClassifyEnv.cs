using Engine.Brain.Entity;
using Engine.Brain.Extend;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    ///  the environment of image classification
    /// </summary>
    public class ImageClassifyEnv : IEnv
    {
        Dictionary<int, List<Point>> _memory = new Dictionary<int, List<Point>>();

        int[] _randomSeedKeys;

        private GRasterLayer _featureRasterLayer, _labelRasterLayer;

        int _current_x, _current_y, _current_classindex;

        int _c_x = 0, _c_y = 0, _c_classIndex = -9999;

        /// <summary>
        /// 指定观察的图像，和样本所在的层位置
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="sampleIndex"></param>
        public ImageClassifyEnv(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer)
        {
            _featureRasterLayer = featureRasterLayer;
            _labelRasterLayer = labelRasterLayer;
            FeatureNum = featureRasterLayer.BandCount;
            //num of categories 
            //标注层要求：
            //1.分类按照顺序，从1开始，逐步+1
            //2.背景值设置为0
            //ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max - _labelRasterLayer.BandCollection[0].Min);
            ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max - 0);
            //statical graph
            Prepare();
            //
            (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
        }
        /// <summary>
        /// number of actions
        /// </summary>
        public int ActionNum { get; }
        /// <summary>
        /// number of features
        /// </summary>
        public int FeatureNum { get; }
        /// <summary>
        /// 
        /// </summary>
        public int[] RandomSeedKeys { get { return _randomSeedKeys; } }
        /// <summary>
        /// 处理之后的样本集
        /// </summary>
        public Dictionary<int, List<Point>> Memory { get { return _memory; } }
        /// <summary>
        /// 分析标注道路区域
        /// </summary>
        public void Prepare()
        {
            IBandStasticTool pBandStasticTool = new GBandStasticTool();
            pBandStasticTool.Visit(_labelRasterLayer.BandCollection[0]);
            _memory = pBandStasticTool.StaisticalRawGraph;
            _randomSeedKeys = _memory.Keys.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] Reset()
        {
            return Step(-1).state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private (int x, int y, int classIndex) RandomAccessMemory()
        {
            int classIndex = NP.Random(_randomSeedKeys);
            Point p = Memory[classIndex].RandomTake();
            return (p.X, p.Y, classIndex);
        }
        /// <summary>
        /// random测试集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<double[]> states, int[] labels) RandomEval(int batchSize = 64)
        {
            List<double[]> states = new List<double[]>();
            int[] labels = new int[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                var (x, y, classIndex) = RandomAccessMemory();
                double[] raw = _featureRasterLayer.GetNormalValue(x, y).ToArray();
                double[] normal = NP.Normalize(raw, 255f);
                states.Add(normal);
                labels[i] = classIndex;
            }
            return (states, labels);
        }
        /// <summary>
        /// random数据集
        /// </summary>
        /// <returns></returns>
        public int RandomAction()
        {
            return NP.Random(ActionNum);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public (double[] state, double reward) Step(int action)
        {
            if (action == -1)
            {
                (_c_x, _c_y, _c_classIndex) = (_current_x, _current_y, _current_classindex);
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                double[] raw = _featureRasterLayer.GetNormalValue(_c_x, _c_y).ToArray();
                return (raw, 0.0);
            }
            else
            {
                double reward = action == _current_classindex ? 1.0 : -1.0;
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                double[] raw = _featureRasterLayer.GetNormalValue(_current_x, _current_y).ToArray();
                return (raw, reward);
            }
        }

    }
}