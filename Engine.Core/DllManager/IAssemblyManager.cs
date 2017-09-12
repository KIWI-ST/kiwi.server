using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Engine.Core.Function;

/*
 *  创建：黄奎
 *  时间：2013/6/3
 *  功能：管理plugin
 */

namespace Engine.Core.DllManager
{
    /// <summary>
    /// Assembly管理，实现插件读取存放
    /// </summary>
    public interface IAssemblyManager
    {
        /// <summary>
        /// 已经加载的插件名
        /// </summary>
        List<string> LoadedList { get; }
        /// <summary>
        /// 输出打印
        /// </summary>
        List<IOutPut> OutPut { get; }
        /// <summary>
        /// 搜索目录下可用的插件
        /// </summary>
        void SearchPlugin();
        /// <summary>
        /// 启动插件
        /// </summary>
        /// <param name="plgName">插件名</param>
        void StartPlugin(string plgName);
        /// <summary>
        /// 停止插件
        /// </summary>
        /// <param name="plgName">插件名</param>
        void StopPlugin(string plgName);
        /// <summary>
        /// 停止全部插件
        /// </summary>
        void StopAll();
        /// <summary>
        /// 启动全部插件
        /// </summary>
        void StartAll();
    }

    /// <summary>
    /// DLL加载
    /// </summary>
    public class AssemblyManagerClass : IAssemblyManager
    {
        public List<string> LoadedList { get { return _loadedList; } }
        public List<IOutPut> OutPut { get { return _outPut; } }

        private List<IOutPut> _outPut = new List<IOutPut>();
        private List<string> _loadedList = new List<string>();
        private string _startWith;
        private string _plugPath;
        private string _ifcmd = "ICommand";    //ICommand 
        private string _ifcui = "ICommandUI";      //ICommandUI
        private string _ifcwks = "ICommandWorkSpace";   //ICommandWorkSpace
        //插件装载器
        /// <summary>
        /// PlugingManager插件加载
        /// </summary>
        /// <param name="plugspath">插件所在目录必须是运行目录中的文件夹</param>
        /// <param name="startsWith">加载指定插件（插件包含的名称）</param>
        public AssemblyManagerClass(string plugspath="", string startsWith=".dll", string ifcmd = "ICommand", string ifcui = "ICommandUI", string ifcwks = "ICommandWorkSpace")
        {
            //变量初始化
            this._startWith = startsWith;
            this._plugPath = plugspath;
            this._ifcmd = ifcmd;
            this._ifcui = ifcui;
            this._ifcwks = ifcwks;
            //插件容器初始化
            CmdPlgin.Inilization(20,_ifcmd,_ifcui,_ifcwks);
        }
        /// <summary>
        /// 搜索插件
        /// </summary>
        public void SearchPlugin()
        {
            //遍历启动目前下所有dll
            string[] files = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + @"\\" + _plugPath);
            //
            foreach (string file in files)
            {
                //确定结尾
                if (file.ToUpper().EndsWith(_startWith.ToUpper()) && !LoadedList.Contains(file))
                {
                    //已加载的列表中不存在file，则新增
                    LoadedList.Add(file);
                    try
                    {
                        Assembly assembly = null;
                        //将插件拷贝到内存缓冲
                        byte[] addinStream = null;
                        using (System.IO.FileStream input = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                        {
                            BinaryReader reader = new BinaryReader(input);
                            addinStream = reader.ReadBytes((int)input.Length);
                        }
                        //加载内存中的Dll
                        assembly = Assembly.Load(addinStream);
                        Type[] types = assembly.GetTypes();

                        //生成传入参数
                        object[] args = new object[1];
                        args[0] = assembly;

                        foreach (Type element in types)
                        {
                            try
                            {
                                string plgName = element.FullName;
                                ICommand plugin = null;
                                ICommandUI pluginUI = null;
                                ICommandWorkSpace pluginWks=null;
                                //command初始化时执行new操作，其他情况则只执行转换操作
                                if (element.GetInterface(_ifcmd) != null)
                                {
                                    plugin = assembly.CreateInstance(element.FullName, true, System.Reflection.BindingFlags.Default, null,
                                        //传入参数
                                    args,
                                        //
                                    null, null) as ICommand;
                                }
                                //commandUI
                                if (element.GetInterface(_ifcui) != null)
                                {
                                    pluginUI = plugin as ICommandUI;
                                }
                                //
                                if (element.GetInterface(_ifcwks) != null)
                                {
                                    pluginWks = plugin as ICommandWorkSpace;

                                }
                                if (plugin == null & pluginUI == null & pluginWks == null)
                                    continue;
                                //查找是否存在plugin
                                if (CmdPlgin.HasPlugin(plgName))
                                {
                                    CmdPlgin.FindPlugin(plgName).Plugin = CmdPlgin.FindPlugin(plgName).Plugin == null ? plugin : CmdPlgin.FindPlugin(plgName).Plugin;
                                    CmdPlgin.FindPlugin(plgName).PluginUI = CmdPlgin.FindPlugin(plgName).PluginUI == null ? pluginUI : CmdPlgin.FindPlugin(plgName).PluginUI;
                                    CmdPlgin.FindPlugin(plgName).PluginWorkSpace = CmdPlgin.FindPlugin(plgName).PluginWorkSpace == null ? pluginWks : CmdPlgin.FindPlugin(plgName).PluginWorkSpace;
                                }
                                else
                                {
                                    PlginModel newPlugin = new PlginModel(plugin, pluginUI, pluginWks, plgName);
                                    CmdPlgin.Add(newPlugin);
                                }
                                //加载UI
                                if (pluginUI != null)
                                {
                                    Function.ExtSington.AttributeControl.AddTable(pluginUI.UserContrl);
                                    pluginUI.IsAdded = true;
                                }
                                //加载插件
                                if (plugin != null)
                                    _outPut.Add(plugin.OutPut);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        /// <summary>
        /// 启动单个插件
        /// </summary>
        public void StartPlugin(string plgName)
        {
            CmdPlgin.StartPlugin(plgName);
        }
        /// <summary>
        /// 停止单个插件
        /// </summary>
        public void StopPlugin(string plgName)
        {
            CmdPlgin.StopPlugin(plgName);
        }
        /// <summary>
        /// 停止所有插件
        /// </summary>
        public void StopAll()
        {
            CmdPlgin.StopAll();
        }
        /// <summary>
        /// 启动所有插件
        /// </summary>
        public void StartAll()
        {
            CmdPlgin.StartAll();
        }
    }

}
                    
                      



