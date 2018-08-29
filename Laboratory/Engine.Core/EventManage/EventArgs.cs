using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core.EventManage
{
    public class EventArgsLog : EventArgs
    {
        private LogType _logType;

        public LogType LogType
        {
            get { return _logType; }
            set { _logType = value; }
        }
        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        //
        public EventArgsLog(LogType logType,string content)
        {
            _logType = logType;
            _content = content;
        }
    }

    /// <summary>
    /// 启动事件参数
    /// </summary>
    public class EventArgsStart : EventArgs
    {
    }
    /// <summary>
    /// 停止事件参数
    /// </summary>
    public class EventArgsStop : EventArgs
    {
    }
    /// <summary>
    /// 重启
    /// </summary>
    public class EventArgsReStart : EventArgs
    {
    }


}
