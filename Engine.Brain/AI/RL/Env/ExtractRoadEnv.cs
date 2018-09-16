using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.Brain.AI.RL.Env
{
    /// <summary>
    ///  the environment of Path extract
    ///  1.reset environment
    ///  2.random take classIndex and start point
    ///  3.next point
    ///  after run one epoche, reset enviroment, back to step 1
    /// </summary>
    public class ExtractRoadEnv : IEnv
    {
        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, List<Point>> _memory = new Dictionary<int, List<Point>>();
        /// <summary>
        /// 
        /// </summary>
        IBandCursorTool _pBandCursorTool = new GBandCursorTool();
        /// <summary>
        /// seed keys
        /// </summary>
        int[] _randomSeedKeys;
        /// <summary>
        /// defalut direction is ++, while explore the maximum of the points, the direction trun to --
        /// </summary>
        bool _direction = true;
        /// <summary>
        /// input features and output
        /// </summary>
        private GRasterLayer _featureRasterLayer, _labelRasterLayer;
        /// <summary>
        /// explore parameters
        /// </summary>
        int _seed_classIndex, _seed_pointIndex;
        /// <summary>
        /// current pearmeters
        /// </summary>
        int _c_x, _c_y, _c_classIndex;
        /// <summary>
        /// mask width
        /// </summary>
        const int _maskx = 5;
        /// <summary>
        /// mask height
        /// </summary>
        const int _masky = 5;
        /// <summary>
        /// 指定观察的图像，和样本所在的层位置
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="sampleIndex"></param>
        public ExtractRoadEnv(GRasterLayer featureRasterLayer, GRasterLayer labelRasterLayer)
        {
            //input
            _featureRasterLayer = featureRasterLayer;
            //output
            _labelRasterLayer = labelRasterLayer;
            //represent eight direction actions
            ActionNum = 8;
            //pixel matrix of M x M
            FeatureNum = _maskx * _masky;
            //read labellayer
            Prepare();
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
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] Reset()
        {
            //1.reset seed
            _seed_classIndex = NP.Random(_randomSeedKeys);
            //2.reset 
            _seed_pointIndex = NP.Random(Memory[_seed_classIndex].Count);
            //3.direction
            _direction = true;
            //3. retrun state
            return Step(-1).state;
        }
        /// <summary>
        /// 分析标注道路区域
        /// </summary>
        public void Prepare()
        {
            //set cursor tool to featureRasterLayer
            _pBandCursorTool.Visit(_featureRasterLayer.BandCollection[0]);
            //statical label raw graph
            IBandStasticTool pStaticTool = new GBandStasticTool();
            pStaticTool.Visit(_labelRasterLayer.BandCollection[0]);
            //set visitor band
            _memory = pStaticTool.StaisticalRawGraph;
            //random seeds
            _randomSeedKeys = _memory.Keys.ToArray();
        }
        /// <summary>
        /// 探索方式，沿着x轴++ 
        /// </summary>
        /// <returns></returns>
        private (int x, int y, int classIndex) SequentialAccessMemory()
        {
            int limits = Memory[_seed_classIndex].Count;
            //当正向探索，探索到线的终点，折返，并改变方向
            if (_direction && _seed_pointIndex == limits - 2)
                _direction = false;
            //当负向探索，探索到线的起点，折返，并改变方向
            if (!_direction && _seed_pointIndex == 1)
                _direction = true;
            _seed_pointIndex += _direction ? 1 : -1;
            Point p = Memory[_seed_classIndex][_seed_pointIndex];
            return (p.X, p.Y, _seed_classIndex);
        }
        /// <summary>
        /// random测试集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<double[]> states, int[] labels) RandomEval(int batchSize = 64)
        {
            //reset environment
            Reset();
            //store states and actions
            List<double[]> states = new List<double[]>();
            int[] labels = new int[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                var (x, y, classIndex) = SequentialAccessMemory();
                double[] raw = _pBandCursorTool.PickNormalValueByMask(x, y, _maskx, _masky);
                states.Add(raw);
                labels[i] = Reward();
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
        /// <returns></returns>
        private int Reward()
        {
            int current = _seed_pointIndex;
            int next = _direction ? _seed_pointIndex + 1 : _seed_pointIndex - 1;
            Point p = Memory[_seed_classIndex][current];
            Point pNext = Memory[_seed_classIndex][next];
            int direction = (pNext.X - p.X) * 10 + (pNext.Y - p.Y);
            switch (direction)
            {
                case -11:
                    return 0;
                case -1:
                    return 1;
                case 9:
                    return 2;
                case -10:
                    return 3;
                case 10:
                    return 4;
                case -9:
                    return 5;
                case 1:
                    return 6;
                case 11:
                    return 7;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// action direction :
        /// action value = 10*x+y
        /// *  -11 | -1 | 9
        /// * -----------------------
        /// *  -10 |  0 | 10
        /// * -----------------------
        /// *  -9   | 1  | 11
        /// -----------------------------------------------------
        /// *    0  |  1  |  2
        /// * -----------------------
        /// *    3  |  X  |  4
        /// * -----------------------
        /// *    5  |  6  |  7
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public (double[] state, double reward) Step(int action)
        {
            if (action == -1)
            {
                Point p = Memory[_seed_classIndex][_seed_pointIndex];
                (_c_x, _c_y, _c_classIndex) = (p.X, p.Y, _seed_classIndex);
                double[] raw = _pBandCursorTool.PickNormalValueByMask(_c_x, _c_y,_maskx,_masky);
                return (raw, 0);
            }
            else
            {
                //p+1 与 p的关系，得到方向，通过方向得到Reward
                double reward = action == Reward() ? 1.0 : -1.0;
                (_c_x, _c_y, _c_classIndex) = SequentialAccessMemory();
                double[] raw = _pBandCursorTool.PickNormalValueByMask(_c_x, _c_y,_maskx,_masky);
                return (raw, reward);
            }
        }
    }

}
