using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Brain.Extend;
using Engine.Brain.Model.DL;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.RL
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
        /// reporter
        /// </summary>
        public event UpdateLearningLossHandler OnLearningLossEventHandler;

        /// <summary>
        /// memory
        /// </summary>
        private readonly List<Memory> _memoryList = new List<Memory>();

        /// <summary>
        /// actor model
        /// </summary>
        private readonly IDSupportDQN _actorNet;

        /// <summary>
        /// critic model
        /// </summary>
        private readonly IDSupportDQN _criticNet;

        #region Parameters
        //environment
        private readonly IEnv _env;
        //memory limit
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
        readonly double _alpha = 0.6;
        //q值印象权重
        readonly double _gamma = 0.0;
        //输入feature长度
        readonly int _featuresNumber;
        //输入action长度
        readonly int _actionsNumber;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public DQN(IEnv env, IDSupportDQN actor =null, IDSupportDQN critic = null, int epochs = 3000, double gamma = 0.0)
        {
            _env = env;
            _gamma = gamma;
            _epoches = epochs;
            //input and output
            _actionsNumber = _env.ActionNum;
            _featuresNumber = _env.FeatureNum.Product();
            //actor-critic
            _actorNet = actor??new DNet(_env.FeatureNum, _actionsNumber);
            _criticNet = critic??new DNet(_env.FeatureNum, _actionsNumber);
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
            double[] pred = _actorNet.Predict(state);
            return (pred, pred[NP.Argmax(pred)]);
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
        private (double[][] inputs, double[][] outputs) MakeBatch(List<Memory> list)
        {
            //batchSize个样本
            int batchSize = list.Count;
            //feature input
            double[][] input_features = new double[batchSize][];
            //qvalue input
            double[][] input_qValue = new double[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                //input_qValue[i] = new double[_actionsNumber];
                //写入当前sample
                double[] array = input_features[i] = new double[_featuresNumber];
                //input features assign
                Array.ConstrainedCopy(list[i].ST, 0, array, 0, _featuresNumber);
                //calcute q_next
                //double[] q = _gamma != 0 ? ChooseAction(list[i].S_NEXT).action : new double[_actionsNumber];
                input_qValue[i] = ChooseAction(list[i].ST).action;
                //input qvalue assign
                input_qValue[i][NP.Argmax(list[i].AT)] = (1 - _alpha) * list[i].QT + _alpha * (list[i].RT + _gamma * input_qValue[i][NP.Argmax(list[i].AT)]);
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
            var (inputs, outputs) = MakeBatch(rawBatchList);
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
            double[] predicts = new double[evalSize];
            double[] targets = new double[evalSize];
            for (int i = 0; i < evalSize; i++)
            {
                predicts[i] = NP.Argmax(ChooseAction(states[i]).action);
                targets[i] = NP.Argmax(rawLabels[i]);
            }
            //calcute accuracy
            var accuracy = NP.CalcuteAccuracy(predicts, targets);
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
            }
        }

    }
}