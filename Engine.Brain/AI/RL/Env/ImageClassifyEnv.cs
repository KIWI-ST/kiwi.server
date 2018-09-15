using Engine.Brain.Entity;
using Engine.Brain.Extend;
using Engine.GIS.GLayer.GRasterLayer;
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
        /// 处理之后的样本集
        /// </summary>
        public Dictionary<int, List<Point>> Memory { get; private set; } = new Dictionary<int, List<Point>>();
        /// <summary>
        /// 探索有值的像素
        /// </summary>
        /// <returns></returns>
        (int x, int y, int classIndex) SeuqnetialNext()
        {
            //ignore zero
            //int x, y, value;
            //do
            //{
            //    (x, y, value) = _labelRasterLayer.BandCollection[0].Next();
            //} while (value == 0);
            //return (x, y, value - 1);
            return (0, 0, 0);
        }
        /// <summary>
        /// 分析标注道路区域
        /// </summary>
        public void Prepare()
        {
            int x, y, pixelValue;
            do
            {
                (x, y, pixelValue) = SeuqnetialNext();
                if (Memory.ContainsKey(pixelValue))
                    Memory[pixelValue].Add(new Point(x, y));
                else
                    Memory.Add(pixelValue, new List<Point>() { new Point(x, y) });
            } while (pixelValue != -2);
            //remove empty value
            Memory.Remove(-2);
            //remove
            Memory = Memory.Where(p => { return Convert.ToDouble(p.Key) < _labelRasterLayer.BandCollection[0].Max && Convert.ToDouble(p.Key) >= 0; }).OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            //reset cursor to zero
           
        }
        /// <summary>
        /// 顺序学习环境样本
        /// </summary>
        /// <returns></returns>
        private (int x, int y, int classIndex) SequentialAccessEnv()
        {
            //int _x, _y, _value;
            //do
            //{
            //    (_x, _y, _value) = _labelRasterLayer.BandCollection[0].Next();
            //} while (_value == 0);//当值为0，即表示此像素为背景值，
            //return (_x, _y, _value-1);
            return (0, 0, 0);
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
            int classIndex = NP.Random(ActionNum);
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
                double[] normal = NP.Normalize(raw, 255f);
                return (normal, 0f);
            }
            else
            {
                float reward = action == _current_classindex ? 1.0f : -1.0f;
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                double[] raw = _featureRasterLayer.GetNormalValue(_current_x, _current_y).ToArray();
                double[] normal = NP.Normalize(raw, 255f);
                return (normal, reward);
            }
        }
    }
}