using Host.UI.Jobs;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class TaskMonitor : Form
    {
        public TaskMonitor()
        {
            InitializeComponent();
        }

        public List<IJob> Jobs
        {
            set { LoadTaskInforList(value); }
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

        private void LoadTaskInforList(List<IJob> jobList)
        {
            for (int i = 0; i < jobList.Count; i++)
            {
                IJob job = jobList[i];
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(job.Name);
                lvi.SubItems.Add( string.Format("process:{0:P}", job.Process));
                task_listView.Items.Add(lvi);
            }
        }

    }
}
