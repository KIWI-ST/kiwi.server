using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/**
 * 黄奎 2012-6-26
 * 创建command信息包，实现command版本控制，功能确认和错误判断
 */
namespace Engine.Core
{
    /// <summary>
    /// 程序集信息包
    /// </summary>
    public interface IAssemInfoManager
    {
        /// <summary>
        /// 所在DLL名称
        /// </summary>
        string DLLName { get; }
        /// <summary>
        /// Command的guid编号
        /// </summary>
        Guid CmdGuid { get; }
        /// <summary>
        /// 程序集版本号
        /// </summary>
        Version AssemVersion { get; }
        /// <summary>
        /// Command的类名称（全名）
        /// </summary>
        string CmdName { get; }
    }

    public class AssemInfoManagerClass : IAssemInfoManager
    {
        #region 属性
        private Guid _guid;
        private string _cmdName;
        private Version _version;
        private string _dllName;

        public string DLLName
        {
            get { return _dllName; }
        }

        public Guid CmdGuid
        {
            get { return _guid; }
        }

        public Version AssemVersion
        {
            get { return _version; }
        }


        public string CmdName
        {
            get { return _cmdName; }
        }
        #endregion

        //构造
        public AssemInfoManagerClass(object obj,System.Reflection.Assembly assembly)
        {
            //重新生成唯一值
            this._guid = Guid.NewGuid();
            //暂时不定义
            //获取当前运行dll版本
            this._version = assembly.GetName().Version;
            //获取core版本
            //Command名
            this._cmdName = obj.ToString();
            if (_cmdName.Contains("Class"))
                this._dllName = _cmdName.Substring(0,_cmdName.IndexOf("Class"))+"dll";
            else
                this._dllName = _cmdName+".dll";
        }
    }

    /// <summary>
    /// 基础操作接口，其他接口可继承自此接口扩展功能
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 运行日志，记录Command的运行状态、错误等
        /// </summary>
        Core.IOutPut OutPut { get; }
        /// <summary>
        /// 事件管理器
        /// </summary>
        EventManage.EventManager EventManager { get; }
        /// <summary>
        /// 日志数据管理器
        /// </summary>
        IDataManager DataManager { get; }
        /// <summary>
        /// 程序集内部属性管理
        /// </summary>
        IAssemInfoManager AssemInfoManager { get; }
    }

    /// <summary>
    /// 未扩展command接口实现，基础类实现
    /// </summary>
    public partial class BaseCommand : ICommand
    {
        #region 属性
        private System.Timers.Timer _commandTimer = new System.Timers.Timer();
        //插件名
        private string _name;
        //数据管理（Log输出，数据来源等）
        private Core.IDataManager _dataManager;
        //事件管理
        private EventManage.EventManager _eventManager;
        //库运行管理
        private Core.IAssemInfoManager _assemInfoManager;
        //程序运行记录
        protected Core.IOutPut _outPut;
        //计时器自动运行此Action内容
        private List<Func<string>> _funcQueue;

        protected List<Func<string>> FuncQueue
        {
            get { return _funcQueue; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Core.IAssemInfoManager AssemInfoManager
        {
            get { return _assemInfoManager; }
        }
        public Core.IOutPut OutPut
        {
            get { return _outPut; }
        }
        public Core.IDataManager DataManager
        {
            get { return _dataManager; }
        }
        public EventManage.EventManager EventManager
        {
            get { return _eventManager; }
        }
        #endregion 
        
        //构造（base）
        public BaseCommand(System.Reflection.Assembly assembly)
        {
            this._assemInfoManager = new AssemInfoManagerClass(this,assembly);
            this._outPut = new Core.OutPutClass();
            this._funcQueue = new List<Func<string>>();
            this._eventManager = new EventManage.EventManager();
            this._dataManager = new Core.DataManagerClass("log.Log", "info.Info", "error.Error");
            this._commandTimer.Elapsed += _commandTimer_Elapsed;
            //30分钟执行一次
            this._commandTimer.Interval = 1800 * 1000;
            this._commandTimer.Start();
        }

        void _commandTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _funcQueue.ForEach(p => p.Invoke());
            }
            catch (Exception ex) {
                this._outPut.UpdateLog(new EventManage.EventArgsLog(LogType.Error, System.DateTime.Now.ToString() + " : " + ex.ToString()));
            }
        }

    }

    /// <summary>
    /// 基础接口，扩展Command功能，提供Command的UI功能
    /// </summary>
    public interface ICommandUI
    {
        /// <summary>
        /// 获取UI
        /// </summary>
        System.Windows.Forms.UserControl UserContrl { get; }
        /// <summary>
        /// 是否添加过的
        /// </summary>
        bool IsAdded { get; set;}
    }

    /// <summary>
    /// 提供前台工作界面的command
    /// </summary>
    public interface ICommandWorkSpace
    {
        /// <summary>
        /// 工作界面 workspace
        /// </summary>
        System.Windows.Forms.UserControl WorkSpaceContrl { get; }
        bool IsAdded { get; }
    }
}
