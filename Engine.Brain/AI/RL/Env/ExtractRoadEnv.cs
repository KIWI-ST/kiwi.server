using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
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
        /// query Table
        /// </summary>
        double[,] _queryTable;
        /// <summary>
        /// input features and output
        /// </summary>
        private GRasterLayer _featureRasterLayer, _labelRasterLayer;
        /// <summary>
        /// explore parameters
        /// </summary>
        int _seed_classIndex, _seed_x, _seed_y, _seed_action;
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
        /// limit explor x
        /// </summary>
        private readonly int _limit_x;
        /// <summary>
        /// limit explor y
        /// </summary>
        private readonly int _limit_y;
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
            // 探索方式，沿顺时针 
            // -----------------------------------------------------
            // *    0  |  1  |  2
            // * -----------------------
            // *    7  |  8  |  3
            // * -----------------------
            // *    6  |  5  |  4
            //represent eight direction actions
            ActionNum = 9;
            //pixel matrix of M x M ， and use one value to indicate clockwise(whether 1 is clock wise,0 is not)
            FeatureNum = _maskx * _masky;
            //limit of x
            _limit_x = _labelRasterLayer.XSize;
            //limit of y
            _limit_y = _labelRasterLayer.YSize;
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
            //1. reset seed
            _seed_classIndex = NP.Random(_randomSeedKeys);
            //2 get seed point list
            List<Point> seed_points = _memory[_seed_classIndex];
            //3. get seed point
            Point p = seed_points[NP.Random(seed_points.Count)];
            (_seed_x,_seed_y) = (p.X,p.Y);
            //4. retrun state
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
            //convert memory to queryable structure
            _queryTable = pStaticTool.StatisticalRawQueryTable;
            //random seeds
            _randomSeedKeys = _memory.Keys.ToArray();
        }
        /// <summary>
        /// }{  debug 探索方向固定
        /// -----------------------------------------------------
        /// *    0  |  1  |  2
        /// * -----------------------
        /// *    7  |  8  |  3
        /// * -----------------------
        /// *    6  |  5  |  4
        /// </summary>
        /// <returns></returns>
        private (int x, int y, int classIndex) SequentialAccessMemory()
        {
            //快速搜索x++方向点
            //组成onehot
            List<Point> points = new List<Point>() {
                new Point(_seed_x-1,_seed_y-1), //(-1,-1)
                new Point(_seed_x,_seed_y-1),   //(0,-1)
                new Point(_seed_x+1,_seed_y-1),//(1,-1)
                new Point(_seed_x+1,_seed_y),   //(1,0)
                new Point(_seed_x+1,_seed_y+1),//(1,1)
                new Point(_seed_x,_seed_y+1),//(0,1)
                new Point(_seed_x-1,_seed_y+1),//(-1,1)
                new Point(_seed_x-1,_seed_y),//(-1,0)
            };
            //create target point
            Point target = new Point(_seed_x, _seed_y);
            //while explore to the end
            if (_seed_x == _limit_x - 1||_seed_y==_limit_y-1||_seed_x==0||_seed_y==0)
                return (_seed_x, _seed_y, 8);
            //search next point
            for(int action = 0; action < ActionNum; action++)
            {
                Point p = points[action];
                //if reach to the end, use original point
                if (p.X >= _limit_x || p.X < 0 || p.Y >= _limit_y || p.Y < 0) {
                    _seed_action = 8;
                };
                if (_queryTable[p.X, p.Y] == _c_classIndex)
                {
                    //get seed action 
                    _seed_action = action;
                    //set target point
                    target = p;
                    //set seed x
                    _seed_x = target.X;
                    //set seed y
                    _seed_y = target.Y;
                    //
                    break;
                }
            }
            //
            return (_seed_x,_seed_y, _seed_classIndex);
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
                labels[i] = _seed_action;
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
        /// action direction :
        /// -----------------------------------------------------
        /// *    0  |  1  |  2
        /// * -----------------------
        /// *    7  |  X  |  3
        /// * -----------------------
        /// *    6  |  5  |  4
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public (double[] state, double reward) Step(int action)
        {
            if (action == -1)
            {
                (_c_x, _c_y, _c_classIndex) = (_seed_x,_seed_y, _seed_classIndex);
                double[] raw = _pBandCursorTool.PickNormalValueByMask(_c_x, _c_y,_maskx,_masky);
                return (raw, 0);
            }
            else
            {
                //p+1 与 p的关系，得到方向，通过方向得到Reward
                double reward = action == _seed_action ? 1.0 : -1.0;
                (_c_x, _c_y, _c_classIndex) = SequentialAccessMemory();
                double[] raw = _pBandCursorTool.PickNormalValueByMask(_c_x, _c_y,_maskx,_masky);
                return (raw, reward);
            }
        }
    }

}
