using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Engine.Core.Form
{
    public partial class Public_AttributeControl : UserControl
    {
        public Public_AttributeControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 跨线程调用需要使用委托
        /// </summary>
        delegate void AddTableEventHandler(UserControl customControl);
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="customControl"></param>
        public void AddTable(UserControl customControl)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddTableEventHandler(AddTable), customControl);
                return;
            }
            this.tabContainer.TabPages.Add(customControl.Name);
            this.tabContainer.TabPages[this.tabContainer.TabPages.Count - 1].Controls.Add(customControl);
            customControl.Dock = DockStyle.Fill;
        }
    }
}
