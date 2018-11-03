using OxyPlot;
using System;

namespace Host.Image.UI.Jobs
{
    /// <summary>
    /// task complete event handler
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="outputs"></param>
    public delegate void OnTaskCompleteHandler(string taskName, params object[] outputs);
    /// <summary>
    /// Job tasks interface
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// 
        /// </summary>
        event OnTaskCompleteHandler OnTaskComplete;
        /// <summary>
        /// job name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// reslut summary
        /// </summary>
        string Summary { get; }
        /// <summary>
        /// progress
        /// </summary>
        double Process { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime StartTime { get; }
        /// <summary>
        /// start thread 
        /// </summary>
        void Start(params object[] paramaters);
        /// <summary>
        /// plot models
        /// </summary>
        PlotModel[] PlotModels { get; }
    }

}
