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

        int _seed = 0;

        int _current_x, _current_y, _current_classindex;

        int _c_x = 0, _c_y = 0, _c_classIndex = -1;

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
            ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max);
            DummyActions = NP.ToOneHot(1, ActionNum);
            Prepare();
            (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
        }
        public float[] DummyActions { get; }
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
        private (int x,int y,int classIndex) SequentialAccessEnv()
        {
            int _x, _y;
            int _value;
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
            Prepare();
            (_current_x, _current_y, _current_classindex) = RandomAccessMemory();
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
            //移除-2
            _memory.Remove(-2);
            //绘制统计结果
            //Bitmap bmp = new Bitmap(_labelRasterLayer.XSize, _labelRasterLayer.YSize);
            //Graphics g = Graphics.FromImage(bmp);
            //Brush[] burshes = new Brush[] { Brushes.Red, Brushes.Yellow, Brushes.Blue, Brushes.White, Brushes.Azure, Brushes.Beige, Brushes.Bisque, Brushes.Black, Brushes.BlanchedAlmond, Brushes.Blue, Brushes.BlueViolet, Brushes.Brown, Brushes.BurlyWood, Brushes.CadetBlue, Brushes.Chartreuse, Brushes.Chocolate, Brushes.Coral };
            //foreach (var element in _memory)
            //{
            //    Brush bursh = burshes[element.Key];
            //    foreach(var p in element.Value)
            //    {
            //        g.FillRectangle(bursh, p.X,p.Y, 1, 1);
            //    }
            //}
            //bmp.Save(@"D:\1.jpg");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private (int x, int y,int classIndex) RandomAccessMemory()
        {
            int classIndex = NP.Random(ActionNum);
            Point p = _memory[classIndex].RandomTake();
            return (p.X, p.Y, classIndex);
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
                _seed++;
                return (normal, reward);
            }
        }

    }
}
