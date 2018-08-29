using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core.EventManage
{
    public class EventManager
    {
        /// <summary>
        /// 启动标识符，默认为false
        /// </summary>
        private bool _hasStarted=false;
        /// <summary>
        /// 此方法是否启动
        /// </summary>
        public bool HasStarted
        {
            get { return _hasStarted; }
        }
        /// <summary>
        /// 日志更新事件
        /// </summary>
        public delegate void UpdateLogEventHandler(object sender, EventArgsLog e);
        /// <summary>
        /// 启动事件
        /// </summary>
        public delegate void StartEventHanlder(object sender,EventArgsStart e);
        /// <summary>
        /// 停止事件
        /// </summary>
        public delegate void StopEventHanlder(object sender,EventArgsStop e);
        /// <summary>
        /// 启动事件
        /// </summary>
        public event StartEventHanlder OnStart;
        /// <summary>
        /// 停止事件
        /// </summary>
        public event StopEventHanlder OnStop;
        /// <summary>
        /// 更新运行日志
        /// </summary>
        public event UpdateLogEventHandler OnUpdateLog;
        /// <summary>
        /// 激发日志更新
        /// </summary>
        public void UpdateLog(object sender,EventArgsLog e)
        {
            OnUpdateLog(sender,e);
        }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start(object sender,EventArgsStart e)
        {
            if (!_hasStarted)
            {
                OnStart(sender, e);
                _hasStarted = true;
            } 
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop(object sender, EventArgsStop e)
        {
            if (_hasStarted)
            {
                OnStop(sender, e);
                _hasStarted = false;
            }
        }
        //
    }
}
