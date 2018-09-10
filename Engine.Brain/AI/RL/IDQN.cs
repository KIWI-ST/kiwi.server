using System;
using Engine.GIS.GLayer.GRasterLayer;
using OxyPlot;

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
    /// deep q learning 接口
    /// </summary>
    public interface IDQN
    {
        event UpdateLearningLossHandler OnLearningLossEventHandler;
        PlotModel AccuracyModel { get; }
        PlotModel LossPlotModel { get; }
        PlotModel RewardModel { get; }
        double CalcuteKappa(GRasterLayer classificationLayer);
        (int action, double q) ChooseAction(double[] state);
        double EpsilonCalcute(int step, double ep_min = 0.01, double ep_max = 1, double ep_decay = 0.0001, int eps_total = 2000);
        (int action, double q) EpsilonGreedy(int step, double[] state);
        void Learn();
        void PreRemember(int rememberSize);
        void Remember(double[] state, double[] action, double q, double reward, double[] stateNext);
        (double loss, TimeSpan span) Replay();
        void SetParameters(int epoches = 3000,double gamma = 0.0);
    }
}