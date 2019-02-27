using Engine.Brain.Entity;
using Engine.Brain.Extend;
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
    ///  2.random take classIndex and point
    ///  3.next random point
    ///  after running epoche once, reset enviroment, back to step 1
    /// </summary>
    public class ExtractRoadEnv : IEnv
    {
        List<double[]> _existActions = new List<double[]>();
        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, List<Point>> _memory = new Dictionary<int, List<Point>>();
        /// <summary>
        /// 
        /// </summary>
        IRasterLayerCursorTool _pRasterLayerCursorTool = new GRasterLayerCursorTool();
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
        int _current_x, _current_y;
        /// <summary>
        /// one-hot action
        /// </summary>
        double[] _current_action;
        /// <summary>
        /// mask width
        /// </summary>
        const int _maskx = 7;
        /// <summary>
        /// mask height
        /// </summary>
        const int _masky = 7;
        /// <summary>
        /// use channel
        /// </summary>
        int _channel = 1;
        /// <summary>
        /// limit explor x
        /// </summary>
        private readonly int _limit_x;
        /// <summary>
        /// limit explor y
        /// </summary>
        private readonly int _limit_y;
        /// <summary>
        /// indicate the action can be combine
        /// </summary>
        public bool SingleAction { get { return false; } }
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
            // *    7  |  p  |  3
            // * -----------------------
            // *    6  |  5  |  4
            //represent eight direction actions
            ActionNum = 8;
            //
            _channel = _featureRasterLayer.BandCount;
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
        /// pixel matrix of M x M ， and use one value to indicate clockwise(whether 1 is clock wise,0 is not)
        /// number of features
        /// </summary>
        public int[] FeatureNum { get { return new int[] { _maskx, _masky, _channel }; } }
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
            // retrun state
            return Step(null).state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename, int row=1, int col=1)
        {

        }
        /// <summary>
        /// 分析标注道路区域
        /// </summary>
        public void Prepare()
        {
            //set cursor tool to featureRasterLayer
            _pRasterLayerCursorTool.Visit(_featureRasterLayer);
            //statical label raw graph
            IRasterBandStatisticTool pLabelBandStaticTool = new GRasterBandStatisticTool();
            pLabelBandStaticTool.Visit(_labelRasterLayer.BandCollection[0]);
            //set visitor band
            _memory = pLabelBandStaticTool.StaisticalRawGraph;
            //convert memory to queryable structure
            _queryTable = pLabelBandStaticTool.StatisticalRawQueryTable;
            //random seeds
            _randomSeedKeys = _memory.Keys.ToArray();
            //initial x, y, action
            (_current_x, _current_y, _current_action) = RandomAccessMemory();
        }
        /// <summary>
        /// -----------------------------------------------------
        /// *    0  |  1  |  2
        /// * -----------------------
        /// *    7  |  8  |  3
        /// * -----------------------
        /// *    6  |  5  |  4
        /// </summary>
        /// <returns></returns>
        private (int x, int y, double[] actions) RandomAccessMemory()
        {
            //
            int rawValueIndex = NP.Random(_randomSeedKeys);
            Point pt = _memory[rawValueIndex].RandomTake();
            //
            double[] actions = new double[ActionNum];
            //快速搜索x++方向点
            List<Point> points = new List<Point>() {
                new Point(pt.X-1,pt.Y-1), //(-1,-1)
                new Point(pt.X,pt.Y-1),   //(0,-1)
                new Point(pt.X+1,pt.Y-1),//(1,-1)
                new Point(pt.X+1,pt.Y),   //(1,0)
                new Point(pt.X+1,pt.Y+1),//(1,1)
                new Point(pt.X,pt.Y+1),//(0,1)
                new Point(pt.X-1,pt.Y+1),//(-1,1)
                new Point(pt.X-1,pt.Y),//(-1,0)
            };
            //search next point
            for (int pointIndex = 0; pointIndex < ActionNum; pointIndex++)
            {
                Point p = points[pointIndex];
                //if reach to the end, use original point
                if (p.X >= _limit_x || p.X < 0 || p.Y >= _limit_y || p.Y < 0)
                    continue;
                //store right action(one-hot)
                if (_queryTable[p.X, p.Y] == rawValueIndex)
                    actions.CombineOneHot(NP.ToOneHot(pointIndex, ActionNum));
            }
            //
            if (!_existActions.Exists(p => NP.Equal(p, actions)))
                _existActions.Add(actions);
            //
            return (pt.X, pt.Y, actions);
        }
        /// <summary>
        /// random测试集
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<double[]> states, double[][] labels) RandomEval(int batchSize = 64)
        {
            //store states and actions
            List<double[]> states = new List<double[]>();
            double[][] labels = new double[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                var (x, y, actions) = RandomAccessMemory();
                double[] raw = _pRasterLayerCursorTool.PickRagneNormalValue(x, y, _maskx, _masky);
                states.Add(raw);
                labels[i] = actions;
            }
            //return states and labels
            return (states, labels);
        }
        /// <summary>
        /// random数据集
        /// </summary>
        /// <returns></returns>
        public double[] RandomAction()
        {
            return NP.StochasticOnehot(ActionNum);
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
        public (double[] state, double reward) Step(double[] action)
        {
            if (action == null)
            {
                var (_c_x, _c_y, _c_action) = (_current_x, _current_y, _current_action);
                (_current_x, _current_y, _current_action) = RandomAccessMemory();
                double[] raw = _pRasterLayerCursorTool.PickRagneNormalValue(_c_x, _c_y, _maskx, _masky);
                return (raw, 0);
            }
            else
            {
                double reward = NP.Equal(action,_current_action) ? 1.0 : -1.0;
                (_current_x, _current_y, _current_action) = RandomAccessMemory();
                double[] raw = _pRasterLayerCursorTool.PickRagneNormalValue(_current_x, _current_y, _maskx, _masky);
                return (raw, reward);
            }
        }
    }

}
