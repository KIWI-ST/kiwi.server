using System;

namespace Host.UI.Jobs
{
    /// <summary>
    /// task complete event handler
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="outputs"></param>
    public delegate void OnTaskCompleteHandler(string taskName, params object[] outputs);

    /// <summary>
    /// task state changed event handler
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="outputs"></param>
    public delegate void OnStateChangedHandler(string taskName, params object[] outputs);

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
        /// 
        /// </summary>
        event OnStateChangedHandler OnStateChanged;

        /// <summary>
        /// 
        /// </summary>
        void Export(string fullFilename);

        /// <summary>
        /// indicate job states
        /// </summary>
        bool Complete { get; }

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
        DateTime CreateTime { get; }

        /// <summary>
        /// start thread 
        /// </summary>
        void Start();
    }
}
