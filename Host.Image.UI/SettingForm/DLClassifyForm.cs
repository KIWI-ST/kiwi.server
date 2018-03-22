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
    public partial class DLClassifyForm : Form
    {
        public DLClassifyForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "深度学习模型文件|*.pb";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = opg.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "超像素中心|*.json";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = opg.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "超像素边界|*.json";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = opg.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string PBName{ get { return textBox1.Text; } }

        public string CenterName { get { return textBox2.Text; } }

        public string LabelName { get { return textBox3.Text; } }

        public bool UseSLIC { get { return checkBox1.Checked; } }

    }
}
