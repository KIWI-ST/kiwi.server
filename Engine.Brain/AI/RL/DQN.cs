using Engine.Brain.Entity;
using Engine.Brain.Extend;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;

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
    /// 记录器
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// st
        /// </summary>
        public double[] ST { get; set; }
        /// <summary>
        /// st+1
        /// </summary>
        public double[] ST_1 { get; set; }
        /// <summary>
        /// rt
        /// </summary>
        public double RT { get; set; }
        /// <summary>
        /// at
        /// </summary>
        public double[] AT { get; set; }
        /// <summary>
        /// qt
        /// </summary>
        public double QT { get; set; }
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
        private IDEnv _env;
        
        //预训练
        readonly int _preTrainNum = 256;
        //memory容量
        readonly int _memoryCapacity = 512;
        //拷贝net参数
        readonly int _everycopy = 128;
        //学习轮次
        readonly int _epoches = 5000;
        //一次学习样本数
        readonly int _batchSize = 29;
        //一轮学习次数
        readonly int _forward = 256;
        //q值积累权重
        readonly double _alpah = 0.5;
        //q值印象权重
        readonly double _gamma = 0;
        //输入feature长度
        readonly int _featuresNumber;
        //输入action长度
        readonly int _actionsNumber;

        //训练loss曲线
        LineSeries _lossLine = new LineSeries();
        //训练精度曲线
        LineSeries _accuracyLine = new LineSeries();
        //dqn reward曲线
        LineSeries _rewardLine = new LineSeries();
        //loss绘制框
        public PlotModel LossPlotModel { get; } = new PlotModel();
        //精度可视化框
        public PlotModel AccuracyModel { get; } = new PlotModel();
        //reward可视化框
        public PlotModel RewardModel { get; } = new PlotModel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public DQN(IDEnv env)
        {
            //environment
            _env = env;
            //
            _actionsNumber = _env.ActionNum;
            //
            _featuresNumber = _env.FeatureNum;
            //决策
            _actorNet = new DNet(_featuresNumber,_actionsNumber);
            //训练
            _criticNet = new DNet(_featuresNumber,_actionsNumber);
            //loss line
            LossPlotModel.Series.Add(_lossLine);
            LossPlotModel.Axes.Add(new LinearAxis(){
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = _epoches
            });
            LossPlotModel.Axes.Add(new LinearAxis(){
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1
            });
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
                Minimum = -1*_forward,
                Maximum = _forward
            });
        }
        /// <summary>
        /// 控制记忆容量
        /// </summary>
        public void Remember(double[] state, double[] action, double q, double reward, double[] netxState)
        {
            //按堆栈顺序删除memory记录数据
            _memoryList.DequeRemove(_memoryCapacity);
            //预学习N步，记录在memory里
            _memoryList.Add(new Memory()
            {
                ST = state,
                ST_1 = netxState,
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
            //默认使用actorNet
            double[] input = new double[_featuresNumber + _actionsNumber];
            double[] predicts = new double[_actionsNumber];
            for (int i = 0; i < _actionsNumber; i++)
            {
                int offset = 0;
                Array.ConstrainedCopy(state, 0, input, offset, _featuresNumber);
                offset += _featuresNumber;
                Array.ConstrainedCopy(NP.ToOneHot(i, _actionsNumber), 0, input, offset, _actionsNumber);
                offset += _actionsNumber;
                double[] predict = _actorNet.Predict(input);
                predicts[i] = predict[0];
            }
            return (NP.Argmax(predicts), NP.Max(predicts));
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
            int feature_num = _featuresNumber + _actionsNumber;
            //feature input
            double[][] inputs = new double[batchSize][];
            //qvalue input
            double[][] outputs = new double[batchSize][];
            //q value
            double q = 0.0;
            int seed = 0;
            //
            list.ForEach(p => {
                int offset = 0;
                inputs[seed] = new double[_featuresNumber+_actionsNumber];
                Array.ConstrainedCopy(p.ST, 0, inputs[seed], offset, _featuresNumber);
                offset += _featuresNumber;
                Array.ConstrainedCopy(NP.ToOneHot(seed, _actionsNumber), 0, inputs[seed], offset, _actionsNumber);
                outputs[seed] = new double[] { (1 - _alpah) * p.QT + _alpah * (p.RT + _gamma * q) };
                seed++;
            });
            //
            return (inputs, outputs);
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
        public double EpsilonCalcute(int step, double ep_min = 0.01, double ep_max = 1.0, double ep_decay = 0.0001, int eps_total = 2018)
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
            double epsion = EpsilonCalcute(step);
            if ( NP.Random() < epsion)
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
            const int batchSize = 64;
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
        public void Learn()
        {
            PreRemember(_preTrainNum);
            double[] state = _env.Reset();
            for (int e = 0; e <= _epoches; e++)
            {
                DateTime now = DateTime.Now;
                double loss = 0, accuracy = 0, totalRewards = 0;
                for (int step = 0; step <= _forward; step++)
                {
                    TimeSpan span;
                    //choose action by epsilon_greedy
                    var (action, q) = EpsilonGreedy(step, state);
                    //play
                    var (nextState, rt) = _env.Step(action);
                    //store state and reward
                    Remember(state, NP.ToOneHot(action, _env.ActionNum), q, rt, nextState);
                    //train
                    (loss, span) = Replay();
                    //
                    state = nextState;
                    totalRewards += rt;
                    //
                    if (step % _everycopy == 0)
                        _actorNet.Accept(_criticNet);
                }
                accuracy = Accuracy();
                OnLearningLossEventHandler?.Invoke(loss, totalRewards, accuracy, (float)e / _epoches, (DateTime.Now - now).TotalSeconds.ToString());
                _lossLine.Points.Add(new DataPoint(e, loss));
                _accuracyLine.Points.Add(new DataPoint(e, accuracy));
                _rewardLine.Points.Add(new DataPoint(e, totalRewards));
            }
        }

    }
}
