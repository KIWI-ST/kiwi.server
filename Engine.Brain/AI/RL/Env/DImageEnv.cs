using Engine.Brain.Entity;
using Engine.Brain.Extend;
using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Brain.AI.RL
{

    /// <summary>
    ///  图片分类学习环境
    /// </summary>
    public class DImageEnv : IDEnv
    {
        private GRasterLayer _featureRasterLayer, _labelRasterLayer;

        Dictionary<int, List<Point>> _memory = new Dictionary<int, List<Point>>();

        int _current_x, _current_y, _current_classindex;

        int _c_x = 0, _c_y = 0, _c_classIndex = -9999;

        /// <summary>
        /// 指定观察的图像，和样本所在的层位置
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="sampleIndex"></param>
        public DImageEnv(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer)
        {
            _featureRasterLayer = featureRasterLayer;
            _labelRasterLayer = labelRasterLayer;
            FeatureNum = featureRasterLayer.BandCount;
            ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max - _labelRasterLayer.BandCollection[0].Min);
            Prepare();
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
        /// 顺序学习环境样本
        /// </summary>
        /// <returns></returns>
        private (int x, int y, int classIndex) SequentialAccessEnv()
        {
            int _x, _y, _value;
            do
            {
                (_x, _y, _value) = _labelRasterLayer.BandCollection[0].Next();
            } while (_value == 0);
            return (_x, _y, _value - 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float[] Reset()
        {
            return Step(-1).state;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Prepare()
        {
            int x, y, classIndex;
            do
            {
                (x, y, classIndex) = SequentialAccessEnv();
                if (_memory.ContainsKey(classIndex))
                    _memory[classIndex].Add(new Point(x, y));
                else
                    _memory.Add(classIndex, new List<Point>() { new Point(x, y) });
            } while (classIndex != -2);
            //remove empty value
            _memory.Remove(-2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private (int x, int y, int classIndex) RandomAccessMemory()
        {
            int classIndex = NP.Random(ActionNum);
            Point p = _memory[classIndex].RandomTake();
            return (p.X, p.Y, classIndex);
        }

        public (List<float[]> states, int[] labels) RandomEval(int batchSize = 64)
        {
            List<float[]> states = new List<float[]>();
            int[] labels = new int[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                var (x, y, classIndex) = RandomAccessMemory();
                float[] raw = _featureRasterLayer.GetPixelFloat(x, y).ToArray();
                float[] normal = NP.Normalize(raw, 255f);
                states.Add(normal);
                labels[i] = classIndex;
            }
            return (states, labels);
        }

        public int RandomAction()
        {
            return NP.Random(ActionNum);
        }

        public (float[] state, float reward) Step(int action)
        {
            if (action == -1)
            {
                (_c_x, _c_y, _c_classIndex) = (_current_x, _current_y, _current_classindex);
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                float[] raw = _featureRasterLayer.GetPixelFloat(_c_x, _c_y).ToArray();
                float[] normal = NP.Normalize(raw, 255f);
                return (normal, 0f);
            }
            else
            {
                float reward = action == _current_classindex ? 1.0f : -1.0f;
                (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
                float[] raw = _featureRasterLayer.GetPixelFloat(_current_x, _current_y).ToArray();
                float[] normal = NP.Normalize(raw, 255f);
                return (normal, reward);
            }
        }

    }
}
