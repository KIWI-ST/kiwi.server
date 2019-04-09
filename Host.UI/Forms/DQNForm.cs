using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class DQNForm : Form
    {

        public DQNForm()
        {
            InitializeComponent();
        }

        Dictionary<string, GRasterLayer> _rasterDic;

        public string SelectedFeatureRasterLayer { get; private set; }

        public int Epochs { get; private set; } = 3000;

        public string SampleFilename { get; private set; }

        public int Width { get; private set; } = 0;

        public int Height { get; private set; } = 0;

        public int Depth { get; private set; } = 0;

        public Dictionary<string, GRasterLayer> RasterDic
        {
            set
            {
                _rasterDic = value;
                Initial(_rasterDic);
            }
        }

        public void Initial(Dictionary<string, GRasterLayer> rasterDic)
        {
            //add raster keys
            state_comboBox.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                state_comboBox.Items.Add(p);
            });
        }

        private void State_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedFeatureRasterLayer = key;
        }

        private void Ok_button_Click(object sender, EventArgs e)
        {
            if (SampleFilename == null)
            {
                MessageBox.Show("未提供样本环境");
                return;
            }
            //训练轮次
            Epochs = (int)(epochs_numericUpDown as NumericUpDown).Value;
            //关闭设置窗体
            Close();
        }

        private void Open_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "样本文件|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                SampleFile_textBox.Text = opg.FileName;
                SampleFilename = opg.FileName;
                string[] parameters = Path.GetFileNameWithoutExtension(SampleFilename).Split('_');
                try
                {
                    //depth
                    Depth = Convert.ToInt32(parameters[parameters.Length - 1]);
                    //width
                    Width = Convert.ToInt32(parameters[parameters.Length - 2]);
                    //height
                    Height = Convert.ToInt32(parameters[parameters.Length - 3]);
                }
                catch
                {
                    MessageBox.Show("样本文件不符合规范，请使用MiniBatch制作样本", "样本命名错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
