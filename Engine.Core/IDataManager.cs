using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    /// <summary>
    /// 日志，数据信息基础接口
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        log4net.ILog Log { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        log4net.ILog Info { get; set; }
        /// <summary>
        /// 错误
        /// </summary>
        log4net.ILog Error { get; set; }
       
    }
    public class DataManagerClass:IDataManager
    {
        #region
        log4net.ILog _log;
        public log4net.ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }
        log4net.ILog _info;
        public log4net.ILog Info
        {
            get { return _info; }
            set { _info = value; }
        }
        log4net.ILog _error;
        public log4net.ILog Error
        {
            get { return _error; }
            set { _error = value; }
        }
        #endregion
        public DataManagerClass(string logName=null, string infoName=null, string errorName=null)
        {
            if (logName == null)
                logName = "Engine.Core.Log";
            if (infoName == null)
                infoName = "Engine.Core.Info";
            if (errorName == null)
                errorName = "Engine.Core.Error";
            _log = log4net.LogManager.GetLogger(logName);
            _info = log4net.LogManager.GetLogger(infoName);
            _error = log4net.LogManager.GetLogger(errorName);
        }
    }
    /// <summary>
    /// 记录日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error=0,
        /// <summary>
        /// 日志
        /// </summary>
        Log=1,
        /// <summary>
        /// 更新
        /// </summary>
        Info=2
    }

    /// <summary>
    /// 输出打印单个对象
    /// </summary>
    public class OutPutElement
    {
        public string Content;
        public LogType LogType;
        public OutPutElement(string content,LogType logType)
        {
            Content = content;
            LogType = logType;
        }
    }
    /// <summary>
    /// 记录log，可提供打印
    /// </summary>
    public interface IOutPut : IDataManager
    {
        /// <summary>
        /// 是否存在更新
        /// </summary>
       bool ExistUpdate { get; }
        /// <summary>
        ///  日志更新触发事件
        /// </summary>
       void UpdateLog(EventManage.EventArgsLog e);
        /// <summary>
        ///  打印当前变更
        /// </summary>
        List<OutPutElement> OutPutElements { get; }
    }

    public class OutPutClass :DataManagerClass,IOutPut
    {
        private bool _existUpdate;
        public bool ExistUpdate
        {
            get { return _existUpdate; }
        }

        private List<OutPutElement> _outPutElements;
        //游标
        private int _cursor;
        public List<OutPutElement> OutPutElements
        {
            get
            {
                List<OutPutElement> waitOutPutElements = new List<OutPutElement>();
                for (int count = _cursor; count < _outPutElements.Count; count++)
                    waitOutPutElements.Add(_outPutElements[count]);
                
                _existUpdate = false;
                _cursor=_outPutElements.Count;
                return waitOutPutElements;
            }
        }

        public void UpdateLog(EventManage.EventArgsLog e)
        {
            _outPutElements.Add(new OutPutElement(e.Content, e.LogType));
            _existUpdate = true;
        }

        public OutPutClass():base()
        {
            _cursor = 0;
            _existUpdate = false;
            _outPutElements = new List<OutPutElement>();
        }

        public OutPutClass(string logName,string infoName,string errorName):base(logName,infoName,errorName)
        {
            _cursor = 0;
            _existUpdate = false;
            _outPutElements = new List<OutPutElement>();
        }
        public OutPutClass(IDataManager dataManager)
        {
            _cursor = 0;
            _existUpdate = false;
            _outPutElements = new List<OutPutElement>();
            //
            base.Info = dataManager.Info;
            base.Log = dataManager.Log;
            base.Error = dataManager.Error;
        }
        public void UpdateLog(LogType logType, string content)
        {
            switch (logType)
            {
                case LogType.Error:
                    break;
                case LogType.Info:
                    break;
                case LogType.Log:
                    break;
                default:
                    break;
            }
        }
    }
}
