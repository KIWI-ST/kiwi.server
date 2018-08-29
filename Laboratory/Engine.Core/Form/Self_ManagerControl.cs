using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Engine.Core.Function;

namespace Engine.Core.Form
{
    public partial class Self_ManagerControl : UserControl
    {
        public Self_ManagerControl()
        {
            //窗体构建
            InitializeComponent();
            //组件初始化
            Inilization();
        }

        private void Inilization()
        {
            //
            this._flag = true;
            this._path = System.IO.Directory.GetCurrentDirectory();
            this._addHandler = new AddListViewItemEventHandler(AddUI);
            this._updateHandler = new UpdateListViewItemEventHandler(UpdateUI);
            this._removeHandler = new RemoveListViewItemEventHandler(RemoveUI);
        }

        /// <summary>
        /// 路径
        /// </summary>
        private string _path;
        /// <summary>
        /// 是否继续更新的标识
        /// </summary>
        private bool _flag;
        /// <summary>
        /// commandtype
        /// </summary>
        List<string> commandTypeList = new List<string>();
        /// <summary>
        /// 加载后的列表
        /// </summary>
        List<string> loadedPluginList = new List<string>();
        /// <summary>
        /// 待启动列表
        /// </summary>
        List<string> startPluginList = new List<string>();
        /// <summary>
        /// 停止启动列表
        /// </summary>
        List<string> stopPluginList = new List<string>();

        #region 委托

        //新增记录
        private delegate void AddListViewItemEventHandler(ListViewItem value, UpdateUIEnum updateEnum);
        //修改指定索引处的item
        private delegate void UpdateListViewItemEventHandler(ListViewItem value, int indexer);
        //删除指定位置的item
        private delegate void RemoveListViewItemEventHandler(ListViewItem value);
        //清空界面
        private delegate void ClearListViewItemEventHandler();

        //添加item
        AddListViewItemEventHandler _addHandler;
        //更新item
        UpdateListViewItemEventHandler _updateHandler;
        //删除指定item
        RemoveListViewItemEventHandler _removeHandler;

        /// <summary>
        /// 委托至invoke调用方式，新增ListViewItem
        /// </summary>
        private void AddUI(ListViewItem value, UpdateUIEnum updateEnum)
        {
            switch (updateEnum)
            {
                //加入类型
                case UpdateUIEnum.workState:
                    this.workstate_listview_state.Items.Add(value);
                    break;
                //活动dll状态
                case UpdateUIEnum.workType:
                    this.workstate_listview_type.Items.Add(value);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 更新ListViewItem
        /// </summary>
        private void UpdateUI(ListViewItem value, int indexer)
        {
            //赋值即可
            workstate_listview_state.Items[indexer] = value;
        }

        /// <summary>
        /// 删除指定位置的item
        /// </summary>
        private void RemoveUI(ListViewItem value)
        {
            workstate_listview_state.Items.Remove(value);
        }
        #endregion

        /// <summary>
        /// 重建索引
        /// </summary>
        private void ReBuildIndexing()
        {
        }

        /// <summary>
        /// 更新bootstrap状态
        /// </summary>
        public void UpdateState()
        {
            try
            {
                //如果_flag指示当前可以更新界面，则更新，否则跳过
                if (_flag)
                {
                    // commandType
                    foreach (var element in CmdPlgin.CommandType)
                    {
                        if (commandTypeList.Contains(element))
                            continue;
                        else
                        {
                            ListViewItem value = new ListViewItem();
                            value.SubItems.Add(element);
                            value.SubItems.Add(_path);
                            value.SubItems.Add("未知");
                            this.Invoke(_addHandler, value, UpdateUIEnum.workType);
                        }
                        commandTypeList.Add(element);
                    }

                    for (int count = 0; count < CmdPlgin.Count; count++)
                    {
                        var element = CmdPlgin.Plugins[count];
                        if (loadedPluginList.Contains(element.PluginName))
                            continue;
                        ListViewItem value = new ListViewItem();
                        //名称
                        value.SubItems.Add(element.Plugin.AssemInfoManager.CmdName);
                        //是否启动
                        value.SubItems.Add(element.Plugin.EventManager.HasStarted.ToString());
                        //所在dll
                        value.SubItems.Add(element.Plugin.AssemInfoManager.DLLName);
                        //Dll运行版本
                        value.SubItems.Add(element.Plugin.AssemInfoManager.AssemVersion.ToString());
                        //类运行GUID（版本）
                        value.SubItems.Add(element.Plugin.AssemInfoManager.CmdGuid.ToString());
                        //OutPut
                        value.SubItems.Add("记录数：" + element.Plugin.OutPut.OutPutElements.Count.ToString());
                        //
                        this.Invoke(_addHandler, value, UpdateUIEnum.workState);
                        //
                        loadedPluginList.Add(element.PluginName);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                _flag = true;
            }
        }

        //选中事件
        private void workstate_listview_state_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }


    }

    public enum UpdateUIEnum
    {
        //加入dll类型
        workType = 0,
        //活动dll状态
        workState = 1
    }

    //使用索引提高更新效率

    /// <summary>
    /// 索引对比更新
    /// </summary>
    public class IndexingUpdateClass
    {
        //对外属性
        private Guid _guid;
        //所在listview的索引
        private int _indexer;
        //用于删除
        private ListViewItem _sourceValue;

        //初始化
        private void Initialization()
        {
        }

        //
        public IndexingUpdateClass(Guid guid, int indexer, ListViewItem sourceValue)
        {
            //变量初始化
            Initialization();
            this._guid = guid;
            this._indexer = indexer;
            this._sourceValue = sourceValue;
        }

        //更新完毕后还原
        private void Complete()
        {
            Initialization();
        }

    }

}
