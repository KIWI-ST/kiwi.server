using Engine.Brain.Entity;
using Engine.Brain.Extend;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Brain.AI.RL
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loss">loss value</param>
    /// <param name="totalReward">rewards</param>
    /// <param name="accuracy">train accuracy</param>
    /// <param name="epochesTime"></param>
    public delegate void UpdateLearningLossHandler(double loss, double totalReward, double accuracy, double progress, string epochesTime);
    /// <summary>
    /// memory
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// state at t
        /// </summary>
        public double[] ST { get; set; }
        /// <summary>
        /// state at t+1
        /// </summary>
        public double[] S_NEXT { get; set; }
        /// <summary>
        /// action at t
        /// </summary>
        public double[] AT { get; set; }
        /// <summary>
        /// q value at t
        /// </summary>
        public double QT { get; set; }
        /// <summary>
        /// reward at t
        /// </summary>
        public double RT { get; set; }
    }
    /// <summary>
    /// 用于影像分类的dqn学习机
    /// action固定为label图层的类别数
    /// </summary>
    public class DQN
    {
        /// <summary>
        /// 
        /// </summary>
        //Dictionary<string, int> _usedSamples = new Dictionary<string, int>();
        /// <summary>
        /// 
        /// </summary>
        //Dictionary<string, int> _usedInfos = new Dictionary<string, int>();
        /// <summary>
        /// 回调区
        /// </summary>
        public event UpdateLearningLossHandler OnLearningLossEventHandler;
        /// <summary>
        /// 记忆区
        /// </summary>
        private List<Memory> _memoryList = new List<Memory>();
        /// <summary>
        /// 决策网络
        /// </summary>
        private IDNet _actorNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private IDNet _criticNet;
        /// <summary>
        /// 观察环境
        /// </summary>
        private IEnv _env;
        //
        readonly int _memoryCapacity = 512;
        //拷贝net参数
        readonly int _everycopy = 128;
        //学习轮次
        int _epoches = 3000;
        //一次学习样本数
        readonly int _batchSize = 31;
        //一轮学习次数
        readonly int _forward = 256;
        //q值积累权重
        readonly double _alpah = 0.6;
        //q值印象权重
        double _gamma = 0.0;
        //输入feature长度
        readonly int _featuresNumber;
        //输入action长度
        readonly int _actionsNumber;
        //
        LineSeries _lossLine = new LineSeries();
        //
        LineSeries _accuracyLine = new LineSeries();
        //
        LineSeries _rewardLine = new LineSeries();
        //
        public PlotModel LossPlotModel { get; } = new PlotModel();
        //
        public PlotModel AccuracyModel { get; } = new PlotModel();
        //
        public PlotModel RewardModel { get; } = new PlotModel();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public DQN(IEnv env)
        {
            //
            _env = env;
            //
            _actionsNumber = _env.ActionNum;
            //
            _featuresNumber = _env.FeatureNum.Product();
            //决策
            _actorNet = new DNet(_env.FeatureNum, _actionsNumber);
            //训练
            _criticNet = new DNet(_env.FeatureNum, _actionsNumber);
        }
        /// <summary>
        /// 初始化plotModel
        /// </summary>
        private void InitPoltModel()
        {
            //缩放比例
            const double scale = 1.04;
            //loss line
            //LossPlotModel.LegendArea.Add(new LineAnnotation { Slope = 0.1, Intercept = 1, Text = "LineAnnotation", ToolTip = "This is a tool tip for the LineAnnotation" });
            LossPlotModel.Series.Add(_lossLine);
            LossPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches * scale
            });
            LossPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                Title = "Loss",
            });
            //LossPlotModel.Title = "Loss";
            //accuracy line
            AccuracyModel.Series.Add(_accuracyLine);
            AccuracyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches * scale
            });
            AccuracyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                Title = "Accuracy",
            });
            //AccuracyModel.Title = "Accuracy";
            //reward line
            RewardModel.Series.Add(_rewardLine);
            RewardModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches * scale,
                FontSize = 26,
                Title = "Training Epochs"
            });
            RewardModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -_forward,
                Maximum = _forward,
                FontSize = 26,
                Title = "Reward",
            });
            //RewardModel.Title = "Reward";
        }
        /// <summary>
        /// 设置运行参数
        /// </summary>
        /// <param name="epochs"></param>
        public void SetParameters(int epochs = 3000, double gamma = 0.0)
        {
            //训练轮次
            _epoches = epochs;
            //设置gamma, 表示对连续状态的计算
            _gamma = gamma;
            //构造plot绘制图
            InitPoltModel();
        }
        /// <summary>
        /// convert action to raw byte value
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int ActionToRawValue(int action)
        {
            return _env.RandomSeedKeys[action];
        }
        /// <summary>
        /// 控制记忆容量
        /// </summary>
        public void Remember(double[] state, double[] action, double q, double reward, double[] stateNext)
        {
            //容量上限限制
            _memoryList.DequeRemove(_memoryCapacity);
            //预学习N步，记录在memory里
            _memoryList.Add(new Memory()
            {
                ST = state,
                S_NEXT = stateNext,
                AT = action,
                QT = q,
                RT = reward
            });
        }
        /// <summary>
        /// 输出每一个 state 对应的 action 值
        /// </summary>
        /// <returns></returns>
        public (double[] action, double q) ChooseAction(double[] state)
        {
            double[] input = new double[_featuresNumber + _actionsNumber];
            Dictionary<double[], double> predicts = new Dictionary<double[], double>();
            //1.create dict to simulate action,based on singleAction == true
            if (_env.SingleAction)
                for (int i = 0; i < _actionsNumber; i++)
                    predicts.Add(NP.ToOneHot(i, _actionsNumber), -1.0);
            //2.env.singleAction == false
            else
                for (int i = 1; i < Math.Pow(2, _actionsNumber); i++)
                {
                    char[] strOnehot = Convert.ToString(i, 2).PadLeft(_actionsNumber, '0').ToCharArray();
                    double[] doubleOnehot = new double[_actionsNumber];
                    for (int index = 0; index < _actionsNumber; index++)
                        doubleOnehot[_actionsNumber - index - 1] = Convert.ToDouble(strOnehot[index].ToString());
                    predicts.Add(doubleOnehot, -1.0);
                }
            List<double[]> keyCollection = predicts.Keys.ToList();
            //2.choose action
            for (int i = 0; i < keyCollection.Count; i++)
            {
                double[] key = keyCollection[i];
                int offset = 0;
                Array.ConstrainedCopy(state, 0, input, offset, _featuresNumber);
                offset += _featuresNumber;
                Array.ConstrainedCopy(key, 0, input, offset, _actionsNumber);
                offset += _actionsNumber;
                double[] preditOutput = _actorNet.Predict(input);
                predicts[key] = preditOutput[0];
            }
            //3.sort dictionary
            var target = predicts.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, o => o.Value).First();
            //3. calcute action and qvalue
            return (target.Key, target.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private double[] MakeInput(double[] state)
        {
            double[] input = new double[_featuresNumber + _actionsNumber];
            int offset = 0;
            Array.ConstrainedCopy(state, 0, input, offset, _featuresNumber);
            offset += _featuresNumber;
            Array.ConstrainedCopy(NP.ToOneHot(0, _actionsNumber), 0, input, offset, _actionsNumber);
            offset += _actionsNumber;
            return input;
        }
        /// <summary>
        /// 随机抽取样本
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        private List<Memory> CreateRawDataBatch(int batchSize)
        {
            List<Memory> list = new List<Memory>();
            for (int i = 0; i < batchSize; i++)
            {
                Memory memory = _memoryList.RandomTake();
                //if(memory.RT == 1.0)
                //{
                //    string key = string.Join(",", memory.ST);
                //    _usedSamples[key] = 1;
                //}
                //else
                //{
                //    string key = string.Join(",", memory.ST) + string.Join(",", memory.AT);
                //    _usedInfos[key] = 1;
                //}
                list.Add(memory);
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input_features_tensor"></param>
        /// <param name="input_qvalue_tensor"></param>
        /// <param name="batchSize"></param>
        private (double[][] inputs, double[][] outputs) MakeDNNBatch(List<Memory> list)
        {
            //batchSize个样本
            int batchSize = list.Count;
            //feature input
            double[][] input_features = new double[batchSize][];
            //qvalue input
            double[][] input_qValue = new double[batchSize][];
            //let q value equals 0  
            double q = 0f;
            //
            for (int i = 0; i < batchSize; i++)
            {
                //写入当前sample
                double[] array = input_features[i] = new double[_featuresNumber + _actionsNumber];
                //写入偏移位
                int offset = 0;
                //input features assign
                Array.ConstrainedCopy(list[i].ST, 0, array, offset, _featuresNumber);
                offset += _featuresNumber;
                //input actions assign
                Array.ConstrainedCopy(list[i].AT, 0, array, offset, _actionsNumber);
                offset += _actionsNumber;
                //calcute q_next
                q = _gamma != 0 ? ChooseAction(list[i].S_NEXT).q : q;
                //input qvalue assign
                input_qValue[i] = new double[1] { (1 - _alpah) * list[i].QT + _alpah * (list[i].RT + _gamma * q) };
            }
            return (input_features, input_qValue);
        }
        /// <summary>
        /// 探索算法
        /// </summary>
        /// <param name="step"></param>
        /// <param name="ep_min"></param>
        /// <param name="ep_max"></param>
        /// <param name="ep_decay"></param>
        /// <param name="eps_total"></param>
        /// <returns></returns>
        public double EpsilonCalcute(int step, double ep_min = 0.0001, double ep_max = 1, double ep_decay = 0.0001, int eps_total = 2000)
        {
            return Math.Max(ep_min, ep_max - (ep_max - ep_min) * step / eps_total);
        }
        /// <summary>
        /// 获取当前actor下的action和reward
        /// </summary>
        /// <param name="step"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public (double[] action, double q) EpsilonGreedy(int step, double[] state)
        {
            int totalEpochs = Convert.ToInt32(_epoches * 0.9);
            var epsion = EpsilonCalcute(step,eps_total: totalEpochs);
            if (NP.Random() < epsion)
                return (_env.RandomAction(), 0);
            else
            {
                var (action, q) = ChooseAction(state);
                return (action, q);
            }
        }
        /// <summary>
        /// 经验回放
        /// </summary>
        /// <returns></returns>
        public (double loss, TimeSpan span) Replay()
        {
            DateTime now = DateTime.Now;
            //batch of memory
            List<Memory> rawBatchList = CreateRawDataBatch(_batchSize);
            var (inputs, outputs) = MakeDNNBatch(rawBatchList);
            //loss计算
            double loss = _criticNet.Train(inputs, outputs);
            return (loss, DateTime.Now - now);
        }
        /// <summary>
        /// 计算分类精度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private double Accuracy()
        {
            //eval data batchSize
            const int evalSize = 100;
            var (states, rawLabels) = _env.RandomEval(evalSize);
            double[][] predicts = new double[evalSize][];
            for (int i = 0; i < evalSize; i++)
                predicts[i] = ChooseAction(states[i]).action;
            //calcute accuracy
            var accuracy = NP.CalcuteAccuracy(predicts, rawLabels);
            return accuracy;
        }
        /// <summary>
        /// }{debug 
        /// 初始化记忆库时，需要给一定的优质记忆，否则记忆库里全是错误记忆，当action可选范围很大时，无法拟合
        /// </summary>
        /// <param name="rememberSize"></param>
        public void PreRemember(int rememberSize)
        {
            double[] state = _env.Reset();
            for (int i = 0; i < rememberSize; i++)
            {
                //增加随机探索记忆
                double[] action = _env.RandomAction();
                var (nextState, reward) = _env.Step(action);
                Remember(state, action, 0, reward, nextState);
                state = nextState;
            }
        }
        /// <summary>
        /// 批次训练
        /// </summary>
        /// <param name="batchSize"></param>
        public void Learn()
        {
            //dqn训练
            PreRemember(_memoryCapacity);
            for (int e = 1; e <= _epoches; e++)
            {
                //if(e==500|| e == 1000|| e == 1500|| e == 2000|| e == 2500|| e == 3000|| e == 3500)
                //{
                //    var s = _usedSamples;
                //    var s1 = _usedInfos;
                //}
                //reset environment every epoches
                double[] state = _env.Reset();
                DateTime now = DateTime.Now;
                double loss = 0, accuracy = 0, totalRewards = 0;
                for (int step = 0; step <= _forward; step++)
                {
                    TimeSpan span;
                    //choose action by epsilon_greedy
                    var (action, q) = EpsilonGreedy(e, state);
                    //play
                    var (nextState, reward) = _env.Step(action);
                    //store state and reward
                    Remember(state, action, q, reward, nextState);
                    //train
                    (loss, span) = Replay();
                    //
                    state = nextState;
                    totalRewards += reward;
                    //copy criticNet paramters to actorNet
                    if (step % _everycopy == 0)
                        _actorNet.Accept(_criticNet);
                }
                //calcute accuracy
                accuracy = Accuracy();
                //report learning progress
                OnLearningLossEventHandler?.Invoke(loss, totalRewards, accuracy, (float)e / _epoches, (DateTime.Now - now).TotalSeconds.ToString());
                //loss
                _lossLine.Points.Add(new DataPoint(e, loss));
                //accuracy
                _accuracyLine.Points.Add(new DataPoint(e, accuracy));
                //reward
                _rewardLine.Points.Add(new DataPoint(e, totalRewards));
            }
        }

    }
}