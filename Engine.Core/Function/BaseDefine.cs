using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * author: yellow
 * date:2013/6/3
 * func:define sington base types
 */
namespace Engine.Core.Function
{
    //对象容器
    public class Container<T>
    {
        private int _index = 0;
        private T[] _array;
        private Hashtable _hastable = new Hashtable();
        public Container(int length)
        {
            _array = new T[length];
        }
        /// <summary>
        /// 返回-1表示对象初始化失败
        /// </summary>
        public int Count
        {
            get
            {
                return _hastable.Count;
            }
        }
        public T this[int i]
        {
            get
            {
                return _array[i];
            }
            set
            {
                _array[i] = value;
            }

        }
        public T this[string name]
        {
            get
            {
                return (T)_hastable[name];
            }
        }
        /// <summary>
        /// 添加到索引
        /// </summary>
        public void Add(T obj, string name)
        {
            if (!_hastable.Contains(name) && _index < _array.Length)
            {
                _hastable.Add(name, obj);
                _array[_index] = obj;
                _index++;
            }
        }
        /// <summary>
        /// 判断是否存在此插件
        /// </summary>
        /// <param name="name">插件名</param>
        public bool HasPlugin(string name)
        {
            return _hastable.Contains(name);
        }
    }

    /// <summary>
    /// 定义插件模型，包含插件的类型，名称，包含内容等
    /// </summary>
    public class PlginModel
    {
        #region 属性
        public PlginModel(ICommand plugin, ICommandUI pluginUI,ICommandWorkSpace pluginWks,string pluginName)
        {
            this._pluginWks = pluginWks;
            this._plugin = plugin;
            this._pluginUI = pluginUI;
            this._pluginName = pluginName;
        }

        private string _pluginName;

        private ICommandWorkSpace _pluginWks;
        private ICommand _plugin;
        private ICommandUI _pluginUI;

        public ICommand Plugin
        {
            set { _plugin = value; }
            get { return _plugin; }
        }
        public ICommandUI PluginUI
        {
            set { _pluginUI = value; }
            get { return _pluginUI; }
        }
        public ICommandWorkSpace PluginWorkSpace
        {
            set { _pluginWks = value; }
            get { return _pluginWks; }
        }
        public string PluginName
        {
            get { return _pluginName; }
        }
        #endregion
    }

    /// <summary>
    /// 插件缓存器，所有插件有关的操作缓存都存放在这里
    /// </summary>
    public static class CmdPlgin
    {
        public static void Inilization(int num,string ifcmd,string ifcui,string ifcwks)
        {
            _plugins = new Container<PlginModel>(num);
            _commandType = new List<string> {ifcmd,ifcui,ifcwks};
        }
        public static List<string> CommandType
        {
            get { return _commandType; }
        }
        public static Container<PlginModel> Plugins
        {
            get { return _plugins; }
        }
        /// <summary>
        /// 插件数目
        /// </summary>
        public static int Count
        {
            get { return _plugins.Count; }
        }
        private static List<string> _commandType;
        //最多能容纳N款插件
        private static Container<PlginModel> _plugins;
        //寻找插件
        public static PlginModel FindPlugin(string plgName)
        {
            if (_plugins.HasPlugin(plgName))
                return _plugins[plgName];
            else
                return null;
        }
        public static bool HasPlugin(string plgName)
        {
            return _plugins.HasPlugin(plgName);
        }
        //添加插件
        public static void Add(PlginModel plugin)
        {
            if (!_plugins.HasPlugin(plugin.PluginName))
                _plugins.Add(plugin, plugin.PluginName);
        }
        public static void StopPlugin(string plgName)
        {
            FindPlugin(plgName).Plugin.EventManager.Stop(new object(), new EventManage.EventArgsStop());
        }
        public static void StartPlugin(string plgName)
        {
            FindPlugin(plgName).Plugin.EventManager.Start(new object(), new EventManage.EventArgsStart());
        }
        //启动所有插件
        public static void StartAll()
        {
            for (int count = 0; count < _plugins.Count; count++)
                _plugins[count].Plugin.EventManager.Start(new object(), new EventManage.EventArgsStart());
        }
        //停止所有插件
        public static void StopAll()
        {
            for (int count = 0; count < _plugins.Count; count++)
                _plugins[count].Plugin.EventManager.Stop(new object(), new EventManage.EventArgsStop());
        }
    }

}
