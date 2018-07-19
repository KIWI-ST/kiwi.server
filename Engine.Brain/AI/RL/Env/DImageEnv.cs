using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// 
    /// </summary>
    public class DImageEnv
    {
        private GRasterLayer _featureRasterLayer, _labelRasterLayer;

        /// <summary>
        /// 指定观察的图像，和样本所在的层位置
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="sampleIndex"></param>
        public DImageEnv(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer)
        {
            _featureRasterLayer = featureRasterLayer;
            _labelRasterLayer = labelRasterLayer;
            //
            FeatureNum = featureRasterLayer.BandCount;
            ActionNum = Convert.ToInt32(_labelRasterLayer.BandCollection[0].Max);
        }
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
            //
            classIndex = (int)_labelRasterLayer.BandCollection[0].GetRawPixel(x, y);
        }

        public DRaw Step(int action)
        {
            int x, y, classIndex = 0;
            //直到观察到有结果的反馈
            do
            {
                RandomAccessEnv(out x, out y, out classIndex);
            } while (classIndex == 0);
            //加入memory
            float[] raw = _featureRasterLayer.GetPixelFloat(x, y).ToArray();
            float[] normal = NP.Normalize(raw, 255f);
            //加入随机值，用来优化action的选取，样本的制作
            float reward = action == classIndex ? 1.0f : -1.0f;
            return new DRaw()
            {
                State = normal,
                Action = NP.ToOneHot(action, ActionNum),
                Reward = reward,
            };
        }
    }
}
