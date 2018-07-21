using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using System;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// 
    /// </summary>
    public class DImageEnv : IDEnv
    {
        private GRasterLayer _featureRasterLayer, _labelRasterLayer;

        int _current_x, _current_y, _current_classindex;

        int _c_x, _c_y, _c_classIndex;

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
            (_current_x,_current_y,_current_classindex) = Observe();
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

        private void RandomAccessEnv(out int x, out int y, out int classIndex)
        {
            //1.随机从label层获取一次观察结果
            x = new Random().Next(_labelRasterLayer.XSize - 1);
            y = new Random().Next(_labelRasterLayer.YSize - 1);
            classIndex = (int)_labelRasterLayer.BandCollection[0].GetRawPixel(x, y);
        }

        public float[] Reset()
        {
            return Step(-1).state;
        }

        private (int x, int y, int classIndex) Observe()
        {
            int x, y, classIndex;
            do
            {
                RandomAccessEnv(out x, out y, out classIndex);
            } while (classIndex == 0);
            return (x, y, classIndex);
        }

        public int RandomAction()
        {
            return new Random().Next(ActionNum);
        }

        public (float[] state, float reward) Step(int action)
        {
            if (action == -1)
            {
                (_c_x,_c_y,_c_classIndex) = (_current_x,_current_y,_current_classindex);
                (_current_x, _current_y, _current_classindex) = Observe();
                float[] raw = _featureRasterLayer.GetPixelFloat(_c_x, _c_y).ToArray();
                float[] normal = NP.Normalize(raw, 255f);
                return (normal, 0f);
            }
            else
            {
                float reward = action == _current_classindex ? 1.0f : -1.0f;
                (_current_x, _current_y, _current_classindex) = Observe();
                float[] raw = _featureRasterLayer.GetPixelFloat(_current_x, _current_y).ToArray();
                float[] normal = NP.Normalize(raw, 255f);
                return (normal, reward);
            }
        }

    }
}
