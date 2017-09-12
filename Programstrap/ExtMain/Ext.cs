using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Programstrap.ExtMain
{
    class ExtMainCore
    {
        /// <summary>
        /// 初始化Core
        /// </summary>
        private void CoreInitialization()
        {
            //
            Engine.Core.Function.StartFunciton.Run();
            //
            Engine.Core.Function.StartFunciton.AttributeControlUI.SelfControl.Dock = DockStyle.Fill;
            _dllManagerForm = new Attr.DllManager();
            _dllManagerForm.Controls.Add(Engine.Core.Function.StartFunciton.AttributeControlUI.SelfControl);
            //
            Engine.Core.Function.StartFunciton.AttributeControlUI.Attribute.Dock = DockStyle.Fill;
            _settingForm = new Attr.Setting();
            _settingForm.Controls.Add(Engine.Core.Function.StartFunciton.AttributeControlUI.Attribute);
            //打印线程
            _backgroundworkOutPut = new BackgroundWorker();
            _backgroundworkOutPut.DoWork += new DoWorkEventHandler(_backgroundworkOutPut_DoWork);
            _backgroundworkOutPut.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundworkOutPut_RunWorkerCompleted);
            _backgroundworkOutPut.RunWorkerAsync();
        }

        void _backgroundworkOutPut_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _backgroundworkOutPut.RunWorkerAsync();
        }

        void _backgroundworkOutPut_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new UpdateOutPutEventHandler(UpdateOutPut), Engine.Core.Function.StartFunciton.OutPutList);
            else
                UpdateOutPut(Engine.Core.Function.StartFunciton.OutPutList);
        }

        private void UpdateOutPut(List<Engine.Core.IOutPut> outPutList)
        {
            foreach (var outputElements in outPutList)
            {
                foreach (var element in outputElements.OutPutElements)
                {
                    //分类打印日志
                    switch (element.LogType)
                    {
                        case Engine.Core.LogType.Info:
                            listBox_info.Items.Add(element.Content);
                            listBox_info.SetSelected(this.listBox_info.Items.Count - 1, true);
                            break;
                        case Engine.Core.LogType.Error:
                            listBox_error.Items.Add(element.Content);
                            listBox_error.SetSelected(this.listBox_error.Items.Count - 1, true);
                            break;
                        case Engine.Core.LogType.Log:
                            listBox_log.Items.Add(element.Content);
                            listBox_log.SetSelected(this.listBox_log.Items.Count - 1, true);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        delegate void UpdateOutPutEventHandler(List<Engine.Core.IOutPut> outOutList);

        delegate void UpdateSettingFormEventHandler(Attr.Setting settingForm);

        private void UpdateSettingForm(Attr.Setting settingForm)
        {
            settingForm.Controls.Clear();
            settingForm.Controls.Add(Engine.Core.Function.StartFunciton.AttributeControlUI.Attribute);
            Engine.Core.Function.StartFunciton.AttributeControlUI.Attribute.Dock = DockStyle.Fill;
            settingForm.ShowDialog();
        }

        #region 全局窗体容器
        //dll管理器容器
        Attr.DllManager _dllManagerForm = new Attr.DllManager();
        //设置容器
        Attr.Setting _settingForm = new Attr.Setting();
        #endregion

        /// <summary>
        /// 打印output后台执行线程
        /// </summary>
        BackgroundWorker _backgroundworkOutPut;

        /// <summary>
        /// 按键事件集合
        /// </summary>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "Dll管理器(&O)":
                    _dllManagerForm.ShowDialog();
                    break;
                case "设置":
                    if (this.InvokeRequired)
                        this.Invoke(new UpdateSettingFormEventHandler(UpdateSettingForm), _settingForm);
                    else
                        UpdateSettingForm(_settingForm);
                    break;
                case "启动":
                    Engine.Core.Function.StartFunciton.Bootstrap.StartCommand();
                    break;
                case "停止":

                    break;

                case "选择启动":

                    break;
                default:
                    break;
            }
        }

    }
}
