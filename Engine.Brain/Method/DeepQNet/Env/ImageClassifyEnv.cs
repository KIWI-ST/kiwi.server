using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Accord.Math;
using Engine.Brain.Extend;
using Engine.Brain.Utils;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Engine.Brain.Method.DeepQNet.Env
{
    /// <summary>
    ///  the environment of image classification
    /// </summary>
    public class ImageClassifyEnv : IEnv
    {
        /// <summary>
        /// sample collection with labeled value index
        /// </summary>
        Dictionary<int, List<Point>> _memory { get; set; } = new Dictionary<int, List<Point>>();
        /// <summary>
        /// layer tool
        /// </summary>
        private IRasterLayerCursorTool _pGRasterLayerCursorTool = new GRasterLayerCursorTool();

        /// <summary>
        /// input layer and label layer
        /// </summary>
        private GRasterLayer _featureRasterLayer, _labelRasterLayer;

        /// <summary>
        /// limitation of every land cover type
        /// </summary>
        private readonly int _sampleSizeLimit;

        /// <summary>
        /// x,y position
        /// </summary>
        int _current_x, _current_y;

        /// <summary>
        /// use one-hot vector represent image class(anno)
        /// </summary>
        float[] _current_classindex;

        //lerp pick samples ,default is true
        private bool _lerpPick;

        /// <summary>
        /// 指定观察的图像，和样本所在的层位置
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="sampleIndex"></param>
        public ImageClassifyEnv(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer , int sampleSizeLimit = 200, bool lerpPick = true)
        {
            //defalut is 200
            _sampleSizeLimit = sampleSizeLimit;
            //input feature raster layer
            _featureRasterLayer = featureRasterLayer;
            //groundtruth raster layer
            _labelRasterLayer = labelRasterLayer;
            //lerp pick samples ,default is true
            _lerpPick = lerpPick;
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
        public int[] RandomSeedKeys { get; private set; }

        /// <summary>
        /// indicate the agent can can do only one-kind action at once, default is ture
        /// </summary>
        public bool SingleAction { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename, int row = 1, int col = 1)
        {
            if ((row == 1 && col == 1)||(row == 0 && col == 0))
            {
                using (StreamWriter sw = new StreamWriter(fullFilename))
                {
                    sw.NewLine = "\r\n";
                    //string str = "";
                    foreach (var element1 in _memory)
                        foreach (var element2 in element1.Value)
                            sw.WriteLine(string.Join(",", _pGRasterLayerCursorTool.PickNormalValue(element2.X, element2.Y)) + "," + element1.Key);
                            //str += string.Join(",", _pGRasterLayerCursorTool.PickNormalValue(element2.X, element2.Y)) + "," + element1.Key + "\r\n";
                    //sw.Write(str);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(fullFilename))
                {
                    sw.NewLine = "\r\n";
                    //string str = "";
                    foreach (var element1 in _memory)
                        foreach (var element2 in element1.Value)
                            sw.WriteLine(string.Join(",", _pGRasterLayerCursorTool.PickRagneNormalValue(element2.X, element2.Y, row, col)) + "," + element1.Key);
                            //str += string.Join(",", _pGRasterLayerCursorTool.PickRagneNormalValue(element2.X, element2.Y, row, col)) + "," + element1.Key + "\r\n";
                            //sw.Write(str);
                }
            }
        }

        /// <summary>
        /// 分析标注道路区域
        /// </summary>
        public void Prepare()
        {
            IRasterBandStatisticTool pBandStasticTool = new GRasterBandStatisticTool();
            pBandStasticTool.Visit(_labelRasterLayer.BandCollection[0]);
            _pGRasterLayerCursorTool.Visit(_featureRasterLayer);
            _memory = pBandStasticTool.StaisticalRawGraph;
            //limited the environment _memory size to cetrain number
            _memory = _memory.LimitedDictionaryCapcaity(_sampleSizeLimit, _lerpPick);
            //
            RandomSeedKeys = _memory.Keys.ToArray();
            //
            (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float[] Reset()
        {
            return Step(null).state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private (int x, int y, float[] classIndex) RandomAccessMemory()
        {
            //use actionNumber represent real types
            int rawValueIndex = NP.Random(RandomSeedKeys);
            Point p = _memory[rawValueIndex].RandomTake();
            //current one-hot action
            float[] classIndex = NP.ToOneHot(Array.IndexOf(RandomSeedKeys, rawValueIndex), ActionNum);
            return (p.X, p.Y, classIndex);
        }

        /// <summary>
        /// random测试集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<float[]> states, float[][] labels) RandomEval(int batchSize = 64)
        {
            List<float[]> states = new List<float[]>();
            float[][] labels = new float[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                var (x, y, classIndex) = RandomAccessMemory();
                float[] normal = _pGRasterLayerCursorTool.PickNormalValue(x, y);
                states.Add(normal);
                labels[i] = classIndex;
            }
            return (states, labels);
        }

        /// <summary>
        /// random数据集
        /// </summary>
        public float[] RandomAction()
        {
            int action = NP.Random(ActionNum);
            return NP.ToOneHot(action, ActionNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">use null to reset environment,else use one-hot vector</param>
        /// <returns></returns>
        public (float[] state, float reward) Step(float[] action)
        {
            if (action == null)
            {
                var (_c_x, _c_y, _c_classIndex) = (_current_x, _current_y, _current_classindex);
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                float[] raw = _pGRasterLayerCursorTool.PickNormalValue(_c_x, _c_y);
                return (raw, 0.0f);
            }
            else
            {
                float reward = NP.Argmax(action) == NP.Argmax(_current_classindex) ? 1.0f : -1.0f;
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                float[] raw = _pGRasterLayerCursorTool.PickNormalValue(_current_x, _current_y);
                return (raw, reward);
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public void Dispose()
        {
            _memory.Clear();
            _memory = null;
            _pGRasterLayerCursorTool = null;
        }

    }
}