using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Core.Function;
using Engine.Core.DllManager;

namespace Engine.Core
{
    /// <summary>
    /// 程序入口
    /// </summary>
    public class StartFunciton
    {
        /// <summary>
        /// 启动队列
        /// </summary>
        public static Queue<string> StartCommandQueue;
        /// <summary>
        /// 停止队列
        /// </summary>
        public static Queue<string> StopCommandQueue;
        /// <summary>
        /// 界面元素
        /// </summary>
        public static AttributeControlUI AttributeControlUI
        {
            get { return _attributeControlUI; }
        }
        /// <summary>
        /// 插件消息打印
        /// </summary>
        public static List<IOutPut> OutPutList
        {
            get { return _assemblyManager.OutPut; }
        }
        /// <summary>
        /// 启动管理器
        /// </summary>
        public static IAssemblyManager AssemblyManager
        {
            get { return StartFunciton._assemblyManager; }
        }

        private static AttributeControlUI _attributeControlUI;
        private static System.Timers.Timer _coreTimer;
        private static bool _searchbool = false;
        private static bool _commandbool = false;
        private static IAssemblyManager _assemblyManager;
        private static UpdateUIEventHanlder _updateUI;

        private delegate void UpdateUIEventHanlder();

        private static void UpdateUI()
        {
            _attributeControlUI.SelfControl.UpdateState();
        }

        public static void Run()
        {
            Initialization();
        }

        /// <summary>
        /// 加载
        /// </summary>
        private static void Initialization()
        {
            //插件机制，队列初始化
            StartCommandQueue = new Queue<string>();
            StopCommandQueue = new Queue<string>();
            //插件管理器
            _assemblyManager = new AssemblyManagerClass();
            //
            _attributeControlUI = new AttributeControlUI();
            //
            _updateUI = new UpdateUIEventHanlder(UpdateUI);
            //
            _coreTimer = new System.Timers.Timer();
            _coreTimer.Interval = 3 * 1000;
            _coreTimer.Elapsed += new System.Timers.ElapsedEventHandler(_serchtimer_Elapsed);
            _coreTimer.Elapsed += new System.Timers.ElapsedEventHandler(_commandTimer_Elapsed);
            //管理器界面绘制
            _attributeControlUI.Attribute = new Form.Public_AttributeControl();
            _attributeControlUI.SelfControl = new Form.Self_ManagerControl();
            //启动计时器
            _coreTimer.Start();
        }

        /// <summary>
        /// 启动管理器
        /// </summary>
        static void _commandTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //0 表示未更改
            int plgstate = 0;
            //
            try
            {
                if (!_commandbool)
                {
                    _commandbool = true;
                    if (StartCommandQueue.Count > 0)
                    {
                        _assemblyManager.StartPlugin(StartCommandQueue.Dequeue());
                        plgstate++;
                    }
                    if (StopCommandQueue.Count > 0)
                    {
                        _assemblyManager.StopPlugin(StopCommandQueue.Dequeue());
                        plgstate++;
                    }
                    _commandbool = false;
                }
            }
            catch
            {

            }
            finally
            {
                //父窗体容器句柄创建成功则执行更新操作
                if (_attributeControlUI.SelfControl.ParentForm.IsHandleCreated)
                {
                    //更新管理器状态
                    if (_attributeControlUI.SelfControl.InvokeRequired)
                        _attributeControlUI.SelfControl.Invoke(_updateUI);
                    else
                        _updateUI();
                }
            }
        }

        /// <summary>
        /// 搜索管理器
        /// </summary>
        static void _serchtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_searchbool)
            {
                _searchbool = true;
                _assemblyManager.SearchPlugin();
                _attributeControlUI.Attribute = ExtSington.AttributeControl;
                _searchbool = false;
            }
        }

    }
}
