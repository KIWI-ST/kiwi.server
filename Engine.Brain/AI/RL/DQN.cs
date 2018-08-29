using Accord.Statistics.Analysis;
using Engine.Brain.Entity;
using Engine.Brain.Extend;
using Engine.GIS.GLayer.GRasterLayer;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Drawing;

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
    /// DQN学习机
    /// </summary>
    public class DQN
    {
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
        private DNet _actorNet;
        /// <summary>
        /// 目标网络
        /// </summary>
        private DNet _criticNet;
        /// <summary>
        /// 观察环境
        /// </summary>
        private IEnv _env;

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
        readonly double _alpah = 0.5;
        //q值印象权重
        readonly double _gamma = 0.0;
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
            _featuresNumber = _env.FeatureNum;
            //决策
            _actorNet = new DNet(_featuresNumber, _actionsNumber);
            //训练
            _criticNet = new DNet(_featuresNumber, _actionsNumber);
        }
        /// <summary>
        /// 初始化plotModel
        /// </summary>
        private void InitPoltModel()
        {
            //loss line
            LossPlotModel.Series.Add(_lossLine);
            LossPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches
            });
            LossPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1
            });
            LossPlotModel.Title = "Loss";
            //accuracy line
            AccuracyModel.Series.Add(_accuracyLine);
            AccuracyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches
            });
            AccuracyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1
            });
            AccuracyModel.Title = "Accuracy";
            //reward line
            RewardModel.Series.Add(_rewardLine);
            RewardModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches
            });
            RewardModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -_forward,
                Maximum = _forward
            });
            RewardModel.Title = "Reward";
        }
        /// <summary>
        /// 设置运行参数
        /// </summary>
        /// <param name="epoches"></param>
        public void SetParameters(int epoches = 3000)
        {
            _epoches = epoches;
            //构造plot绘制图
            InitPoltModel();
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
        public (int action, double q) ChooseAction(double[] state)
        {
            double[] input = new double[_featuresNumber + _actionsNumber];
            double[] output = new double[_actionsNumber];
            for (int i = 0; i < _actionsNumber; i++)
            {
                int offset = 0;
                Array.ConstrainedCopy(state, 0, input, offset, _featuresNumber);
                offset += _featuresNumber;
                Array.ConstrainedCopy(NP.ToOneHot(i, _actionsNumber), 0, input, offset, _actionsNumber);
                offset += _actionsNumber;
                double[] preditOutput = _actorNet.Predict(input);
                output[i] = preditOutput[0];
            }
            return (NP.Argmax(output), NP.Max(output));
        }

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
                list.Add(_memoryList.RandomTake());
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input_features_tensor"></param>
        /// <param name="input_qvalue_tensor"></param>
        /// <param name="batchSize"></param>
        private (double[][] inputs, double[][] outputs) MakeBatch(List<Memory> list)
        {
            //batchSize个样本
            int batchSize = list.Count;
            //feature input
            double[][] input_features = new double[batchSize][];
            //qvalue input
            double[][] input_qValue = new double[batchSize][];
            //let q value equals 0  
            float q = 0f;
            //
            for (int i = 0; i < batchSize; i++)
            {
                //写入当前sample
                double[] array = input_features[i] = new double[_featuresNumber+_actionsNumber];
                //写入偏移位
                int offset = 0;
                //input features assign
                Array.ConstrainedCopy(list[i].ST, 0, array, offset, _featuresNumber);
                offset += _featuresNumber;
                //input actions assign
                Array.ConstrainedCopy(list[i].AT, 0, array, offset, _actionsNumber);
                offset += _actionsNumber;
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
        public double EpsilonCalcute(int step, double ep_min = 0.01, double ep_max = 1, double ep_decay = 0.0001, int eps_total = 2000)
        {
            return Math.Max(ep_min, ep_max - (ep_max - ep_min) * step / eps_total);
        }
        /// <summary>
        /// 获取当前actor下的action和reward
        /// </summary>
        /// <param name="step"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public (int action, double q) EpsilonGreedy(int step, double[] state)
        {
            var epsion = EpsilonCalcute(step);
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
            var (inputs, outputs) = MakeBatch(rawBatchList);
            //loss计算
            double loss = _criticNet.Train(inputs, outputs);
            return (loss, DateTime.Now - now);
        }
        /// <summary>
        /// 计算分类精度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private float Accuracy()
        {
            const int batchSize = 128;
            var (states, rawLabels) = _env.RandomEval(batchSize);
            float[] actions = new float[batchSize];
            float[] labels = new float[batchSize];
            for (int i = 0; i < batchSize; i++)
            {
                actions[i] = ChooseAction(states[i]).action;
                labels[i] = rawLabels[i];
            }
            var accuracy = NP.CalcuteAccuracy(labels, actions);
            return accuracy;
        }
        /// <summary>
        /// 预存储记忆
        /// </summary>
        /// <param name="rememberSize"></param>
        public void PreRemember(int rememberSize)
        {
            double[] state = _env.Reset();
            for (int i = 0; i < rememberSize; i++)
            {
                int action = _env.RandomAction();
                var (nextState, reward) = _env.Step(action);
                Remember(state, NP.ToOneHot(action, _env.ActionNum), 0, reward, nextState);
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
            double[] state = _env.Step(-1).state;
            for (int e = 1; e <= _epoches; e++)
            {
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
                    Remember(state, NP.ToOneHot(action, _env.ActionNum), q, reward, nextState);
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
        /// <summary>
        /// 计算kappa系数
        /// </summary>
        /// <returns></returns>
        public double CalcuteKappa(GRasterLayer classificationLayer)
        {
            //creat m x m matrix
            //  int[,] matrix =
            //  {
            //      { 0,0,0 },
            //      { 0,0,0 },
            //      { 0,0,0 },
            //  };
            //
            int[,] matrix = new int[_actionsNumber, _actionsNumber];
            foreach (var key in _env.Memory.Keys)
            {
                List<Point> points = _env.Memory[key];
                //计算realKey类分类结果,存入混淆矩阵
                points.ForEach(p => {
                    int classificationType =  classificationLayer.BandCollection[0].GetRawPixel(p.X, p.Y) - 1;
                    matrix[key, classificationType]++;
                });
            }
            // Create a new multi-class Confusion Matrix
            var cm = new GeneralConfusionMatrix(matrix);
            //
            int totalNum = cm.NumberOfSamples;
            //p0
            double p0 = 0;
            for(int i = 0; i < _actionsNumber; i++)
                p0 += matrix[i, i];
            //pc
            double pc = 0;
            for (int i = 0; i < _actionsNumber; i++)
                pc += cm.ColumnTotals[i] * cm.RowTotals[i];
            pc = pc / totalNum;
            //
            double kappa = (p0 - pc) / (totalNum - pc);
            //
            return kappa;
        }

    }
}