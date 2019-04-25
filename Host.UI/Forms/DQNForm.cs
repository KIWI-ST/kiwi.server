using Engine.Brain.Utils;
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

        public string RasterLayerName { get; private set; }

        public int Epochs { get; private set; } = 3000;

        public string SampleFilename { get; private set; }

        public string NetName{get; private set;}

        public string DeviceName { get; private set; }

        public int ImageWidth { get; private set; } = 0;

        public int ImageHeight { get; private set; } = 0;

        public int ImageDepth { get; private set; } = 0;

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
            //soruce raster image keys
            if (rasterDic != null && rasterDic.Keys.Count > 0)
            {
                state_comboBox.Items.Clear();
                rasterDic.Keys.ToList().ForEach(raster => {
                    state_comboBox.Items.Add(raster);
                });
                state_comboBox.SelectedIndex = 0;
                RasterLayerName = state_comboBox.Items[0].ToString();
            }
            //add support net keys
            SupportNet_comboBox.Items.Clear();
            List<string> supportNetTypes = NP.Model.ReinforceSupportCollection;
            if (supportNetTypes != null && supportNetTypes.Count > 0)
            {
                SupportNet_comboBox.Items.Clear();
                supportNetTypes.ForEach(modelType => {
                    SupportNet_comboBox.Items.Add(modelType);
                });
                //set 0 as default
                SupportNet_comboBox.SelectedIndex = 0;
                //net name
                NetName = SupportNet_comboBox.Items[0].ToString();
            }
            //check devices name
            List<string> devices = NP.CNTK.DeviceCollection;
            Device_comboBox.Items.Clear();
            if (devices != null && devices.Count > 0)
            {
                devices.ForEach(device => {
                    Device_comboBox.Items.Add(device);
                });
                //select index 0 as default
                Device_comboBox.SelectedIndex = 0;
                //device name
                DeviceName = Device_comboBox.Items[0].ToString();
            }
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
                    ImageDepth = Convert.ToInt32(parameters[parameters.Length - 1]);
                    //width
                    ImageWidth = Convert.ToInt32(parameters[parameters.Length - 2]);
                    //height
                    ImageHeight = Convert.ToInt32(parameters[parameters.Length - 3]);
                }
                catch
                {
                    MessageBox.Show("样本文件不符合规范，请使用MiniBatch制作样本", "样本命名错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Device_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            DeviceName = key;
        }

        private void State_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            RasterLayerName = key;
        }

        private void SupportNet_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            NetName = key;
        }
    }
}
