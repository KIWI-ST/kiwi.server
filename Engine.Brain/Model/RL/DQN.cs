using System;
using System.Collections.Generic;
using System.IO;
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
    /// 切换学习环境
    /// </summary>
    public delegate void SwitchEnvironmentHandler();

    /// <summary>
    /// 保存环境
    /// </summary>
    public delegate void SaveCheckpointHandler(int i);

    /// <summary>
    /// memory
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// state at t
        /// </summary>
        public float[] ST { get; set; }

        /// <summary>
        /// state at t+1
        /// </summary>
        public float[] S_NEXT { get; set; }

        /// <summary>
        /// action at t
        /// </summary>
        public float[] AT { get; set; }

        /// <summary>
        /// q value at t
        /// </summary>
        public float QT { get; set; }

        /// <summary>
        /// reward at t
        /// </summary>
        public float RT { get; set; }
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
        /// switch envrionment
        /// </summary>
        public event SwitchEnvironmentHandler OnSwitchEnvironmentHandler;

        /// <summary>
        /// 
        /// </summary>
        public event SaveCheckpointHandler OnSaveCheckpointHandler;

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
        public IEnv Env { get; set; }

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
        readonly float _alpha = 0.6f;

        //q值印象权重
        readonly float _gamma = 0.0f;

        //输入feature长度
        readonly int _featuresNumber;

        //输入action长度
        readonly int _actionsNumber;

        //切换环境步长
        readonly int _switchEpoch;

        #endregion

        #region 模型存储

        public string PersistencNative()
        {
            string timex = DateTime.Now.ToShortDateString().Replace('/', '-') + "#" + DateTime.Now.ToLongTimeString().Replace(':', '_');
            //1. crearte checkpoint
            string directoryname = Directory.GetCurrentDirectory() + @"\tmp\" + timex;
            if(!Directory.Exists(directoryname)) Directory.CreateDirectory(directoryname);
            //2. save paramaters
            using(StreamWriter sw = new StreamWriter(directoryname + @"\paramaters.log"))
            {
                sw.WriteLine(string.Format("{0}:{1}", "actionsNumber", _actionsNumber));
                sw.WriteLine(string.Format("{0}:{1}", "featuresNumber", _featuresNumber));
                sw.WriteLine(string.Format("{0}:{1}", "netType", _actorNet.GetType().Name));
            }
            //3. return checpoint filename;
            string criticFilename = directoryname + @"\critic.model";
            string actorFilename = directoryname + @"\actor.model";
            _criticNet.PersistencNative(criticFilename);
            _actorNet.PersistencNative(actorFilename);
            //_criticNet.PersistencNative
            return directoryname;
        }

        /// <summary>
        /// env的类型必须和之前的
        /// </summary>
        /// <param name="modelFilename"></param>
        /// <param name="env"></param>
        /// <param name="epochs"></param>
        /// <returns></returns>
        public static DQN ReLoad(string modelDirectoryname, string deviceName, IEnv env, int epochs = 3000, int switchEpoch = -1)
        {
            //0.读取参数配置
            Dictionary<string, string> paramaters = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(modelDirectoryname + @"\paramaters.log"))
            {
                string text = sr.ReadLine();
                do
                {
                    string[] key = text.Split(':');
                    paramaters[key[0]] = key[1];
                    text = sr.ReadLine();
                } while (text != null);
            }
            //是用Dnet构造
            if (paramaters["netType"] == typeof(DNet2).Name)
            {
                var critic = DNet2.Load(modelDirectoryname + @"\critic.model", deviceName);
                var actor = DNet2.Load(modelDirectoryname + @"\actor.model", deviceName);
                return new DQN(env, actor, critic, epochs:epochs, switchEpoch:switchEpoch);
            }
            return null;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public DQN(IEnv env = null, IDSupportDQN actor =null, IDSupportDQN critic = null, int epochs = 3000, float gamma = 0.0f, int switchEpoch = -1)
        {
            Env = env;
            _gamma = gamma;
            _epoches = epochs;
            //设置切换环境步长
            _switchEpoch = switchEpoch == -1 ? _epoches : switchEpoch;
            //input and output
            _actionsNumber = Env.ActionNum;
            _featuresNumber = Env.FeatureNum.Product();
            //actor-critic
            _actorNet = actor??new DNet(Env.FeatureNum, _actionsNumber);
            _criticNet = critic??new DNet(Env.FeatureNum, _actionsNumber);
        }

        /// <summary>
        /// convert action to raw byte value
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int ActionToRawValue(int action)
        {
            return Env.RandomSeedKeys[action];
        }

        /// <summary>
        /// 控制记忆容量
        /// </summary>
        public void Remember(float[] state, float[] action, float q, float reward, float[] stateNext)
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
        public (float[] action, float q) ChooseAction(float[] state)
        {
            float[] pred = _actorNet.Predict(state);
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
        private (float[][] inputs, float[][] outputs) MakeBatch(List<Memory> list)
        {
            //batchSize个样本
            int batchSize = list.Count;
            //feature input
            float[][] input_features = new float[batchSize][];
            //qvalue input
            float[][] input_qValue = new float[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                //input_qValue[i] = new double[_actionsNumber];
                //写入当前sample
                float[] array = input_features[i] = new float[_featuresNumber];
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
        public (float[] action, float q) EpsilonGreedy(int step, float[] state)
        {
            int totalEpochs = Convert.ToInt32(_epoches * 0.9);
            var epsion = EpsilonCalcute(step,eps_total: totalEpochs);
            if (NP.Random() < epsion)
                return (Env.RandomAction(), 0);
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
            var (states, rawLabels) = Env.RandomEval(evalSize);
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
            float[] state = Env.Reset();
            for (int i = 0; i < rememberSize; i++)
            {
                //增加随机探索记忆
                float[] action = Env.RandomAction();
                var (nextState, reward) = Env.Step(action);
                Remember(state, action, 0, reward, nextState);
                state = nextState;
            }
        }

        int _switchStep = 0;

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
                //如果达到需要切换环境的步长，则提出构建策略
                _switchStep++;
                if (_switchStep > _switchEpoch)
                {
                    OnSaveCheckpointHandler?.Invoke(e/_switchEpoch);
                    OnSwitchEnvironmentHandler?.Invoke();
                    _switchStep = 0;
                }
                float[] state = Env.Reset();
                DateTime now = DateTime.Now;
                double loss = 0, accuracy = 0, totalRewards = 0;
                for (int step = 0; step <= _forward; step++)
                {
                    TimeSpan span;
                    //choose action by epsilon_greedy
                    var (action, q) = EpsilonGreedy(e, state);
                    //play
                    var (nextState, reward) = Env.Step(action);
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