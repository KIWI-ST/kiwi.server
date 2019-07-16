using System;
using System.Windows.Forms;

namespace Host.UI.Forms
{
    public partial class DQNSceneForm : Form
    {
        public DQNSceneForm()
        {
            InitializeComponent();
        }

        public string SampleBatchesDirectoryName { get { return Sample_Batches_Directory_textBox.Text; } }

        public string ApplyDirectoryName { get { return Apply_Directory_textBox.Text; } }

        public int Epochs { get { return Convert.ToInt32(Epochs_numericUpDown.Value); } }

        public int SwitchEpochs { get { return Convert.ToInt32(Switch_Epoch_numericUpDown.Value); } }

        private void SampleBatches_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置样本根目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Sample_Batches_Directory_textBox.Text = dialog.SelectedPath;
            }
        }

        private void Apply_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置应用数据根目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Apply_Directory_textBox.Text = dialog.SelectedPath;
            }
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            if(SampleBatchesDirectoryName == null|| SampleBatchesDirectoryName.Length==0)
            {
                MessageBox.Show("请设置样本根目录");
                return;
            }
            if (ApplyDirectoryName == null || ApplyDirectoryName.Length == 0)
            {
                MessageBox.Show("请设置待处理数据根目录");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
