using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Engine.Brain.Utils;

namespace Host.UI.SettingForm
{
    public partial class CNNForm : Form
    {
        public CNNForm()
        {
            InitializeComponent();
            Initial();
        }
        /// <summary>
        /// feature Rastery, 待分类图
        /// </summary>
        public string SelectedFeatureRasterLayer { get; set; }
        /// <summary>
        /// 样本文件地址
        /// </summary>
        public string FullFilename { get; private set; }
        /// <summary>
        /// 输入图像宽
        /// </summary>
        public int ImageWidth { get; set; }
        /// <summary>
        /// 输入图像高
        /// </summary>
        public int ImageHeight { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int ImageDepth { get; set; }
        /// <summary>
        /// 模型训练轮次
        /// </summary>
        public int Epochs { get; set; }
        /// <summary>
        /// 获取计算设备名
        /// </summary>
        public string DeviceName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rasterDic"></param>
        public void Initial()
        {
            //convNet types
            List<string> convTypes = NP.CNN.ConvNetCollection;
            if (convTypes != null&&convTypes.Count>0)
            {
                ConvType_comboBox.Items.Clear();
                convTypes.ForEach(modelType => {
                    ConvType_comboBox.Items.Add(modelType);
                });
                ConvType_comboBox.SelectedIndex = 0;
            }
            //check devices name
            List<string> devices = NP.CNTK.DeviceCollection;
            device_comboBox.Items.Clear();
            if (devices != null&&devices.Count>0)
            {
                devices.ForEach(device => {
                    device_comboBox.Items.Add(device);
                });
                //select index 0 as default
                device_comboBox.SelectedIndex = 0;
            }
        }

        private void Source_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedFeatureRasterLayer = key;
        }

        private void Device_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            DeviceName = key;
        }

        private void Width_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ImageWidth = (int)(sender as NumericUpDown).Value;
        }

        private void Height_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ImageHeight = (int)(sender as NumericUpDown).Value;
        }

        private void DEPTH_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ImageDepth = (int)(sender as NumericUpDown).Value;
        }

        private void Epochs_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Epochs = (int)(sender as NumericUpDown).Value;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void open_sample_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "样本文件|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                open_sample_textBox.Text = opg.FileName;
                FullFilename = opg.FileName;
                string[] parameters = System.IO.Path.GetFileNameWithoutExtension(FullFilename).Split('_');
                try
                {
                    //depth
                    DEPTH_numericUpDown.Value = Convert.ToDecimal(parameters[parameters.Length - 1]);
                    ImageDepth = (int)DEPTH_numericUpDown.Value;
                    //width
                    Width_numericUpDown.Value = Convert.ToDecimal(parameters[parameters.Length - 2]);
                    ImageWidth = (int)Width_numericUpDown.Value;
                    //height
                    Height_numericUpDown.Value = Convert.ToDecimal(parameters[parameters.Length - 3]);
                    ImageHeight = (int)Height_numericUpDown.Value;
                }
                catch
                {
                    MessageBox.Show("样本文件不符合规范，请使用MiniBatch制作样本", "样本命名错误",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
