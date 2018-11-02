using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host.Image.UI.SettingForm
{
    public partial class TaskMonitor : Form
    {
        public TaskMonitor()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        ListViewItem _selected_item;

        private void task_listView_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            _selected_item = listView.SelectedItems[0];
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (item.Name)
            {
                case "accuracy_ToolStripMenuItem":
                    MessageBox.Show(_selected_item.Text);
                    break;
                default:
                    break;
            }
        }
    }
}
