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

        List<IJob> _jobs;

        ListViewItem _selected_item;

        Timer _t = new Timer();

        public List<IJob> Jobs
        {
            set {
                _jobs = value;
                LoadTaskInforList(value);
            }
        }

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
                case "Sample_Export_ToolStripMenuItem":
                    int index = _selected_item.Index;
                    IJob job = _jobs[index];
                    SaveFileDialog sfg = new SaveFileDialog();
                    sfg.AddExtension = true;
                    sfg.DefaultExt = ".txt";
                    if (sfg.ShowDialog() == DialogResult.OK)
                    {
                        string fullFilename = sfg.FileName;
                        job.Export(fullFilename);
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadTaskInforList(List<IJob> jobList)
        {
            Task_listView.Items.Clear();
            for (int i = 0; i < jobList.Count; i++)
            {
                IJob job = jobList[i];
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems[0].Text = job.Name;
                lvi.SubItems.Add(string.Format("process:{0:P}", job.Process));
                lvi.SubItems.Add(job.Summary);
                Task_listView.Items.Add(lvi);
            }
            //
            _t.Tick += T_Tick;
            _t.Start();
        }
        /// <summary>
        /// update statues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T_Tick(object sender, EventArgs e)
        {
            //update process and remark
            //only update task list views
            for (int i = 0; i < Task_listView.Items.Count; i++)
            {
                IJob job = _jobs[i];
                if (job.Complete) continue;
                ListViewItem lvi = Task_listView.Items[i];
                lvi.SubItems[0].Text = job.Name;
                lvi.SubItems[1].Text = string.Format("process:{0:P}", job.Process);
                lvi.SubItems[2].Text = job.Summary;
            }
        }
        /// <summary>
        /// dispose resoruces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            _t.Tick -= T_Tick;
            _t.Dispose();
        }
    }
}
