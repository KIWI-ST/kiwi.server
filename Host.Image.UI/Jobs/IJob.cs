using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Image.UI.Jobs
{
    public delegate void OnTaskCompleteHandler(string result);

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
        void Start(params string[] paramaters);
        /// <summary>
        /// plot models
        /// </summary>
        PlotModel[] PlotModels { get; }
    }

}
