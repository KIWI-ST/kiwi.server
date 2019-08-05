using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Engine.Brain.Utils;
using Engine.GIS.GLayer.GRasterLayer;

namespace Host.UI.SettingForm
{
    public partial class CNNForm : Form
    {
        public CNNForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        public string NetName { get; private set; }
        /// <summary>
        /// savemodel
        /// </summary>
        public string SaveModelFilename { get; private set;  }
        /// <summary>
        /// 样本文件地址
        /// </summary>
        public string SampleFilename { get; private set; }
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
        /// raster layer name
        /// </summary>
        public string RasterLayerName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, GRasterLayer> _rasterDic;
        /// <summary>
        /// set raster dic
        /// </summary>
        public Dictionary<string, GRasterLayer> RasterDic
        {
            set
            {
                _rasterDic = value;
                Initial(_rasterDic);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rasterDic"></param>
        public void Initial(Dictionary<string, GRasterLayer> rasterDic)
        {
            //convNet types
            List<string> convTypes = NP.SupportNeualNetwork.ConvSupportCollection;
            if (convTypes != null&&convTypes.Count>0)
            {
                ConvType_comboBox.Items.Clear();
                convTypes.ForEach(modelType => {
                    ConvType_comboBox.Items.Add(modelType);
                });
                //set 0 as default
                ConvType_comboBox.SelectedIndex = 0;
                //net name
                NetName = ConvType_comboBox.Items[0].ToString();
            }
            //check devices name
            List<string> devices = NP.CNTKHelper.DeviceCollection;
            device_comboBox.Items.Clear();
            if (devices != null&&devices.Count>0)
            {
                devices.ForEach(device => {
                    device_comboBox.Items.Add(device);
                });
                //select index 0 as default
                device_comboBox.SelectedIndex = 0;
                //device name
                DeviceName = device_comboBox.Items[0].ToString();
            }
            //source image select 
            RasterLayer_comboBox.Items.Clear();
            if(rasterDic!=null && rasterDic.Keys.Count > 0)
            {
                rasterDic.Keys.ToList().ForEach(raster => {
                    RasterLayer_comboBox.Items.Add(raster);
                });
                RasterLayer_comboBox.SelectedIndex = 0;
                RasterLayerName = RasterLayer_comboBox.Items[0].ToString();
            }
        }

        private void Device_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            DeviceName = key;
        }
        private void ConvType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            NetName = key;
        }

        private void RasterLayer_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            RasterLayerName = key;
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
            Epochs = Convert.ToInt32(Epochs_numericUpDown.Value);
            SaveModelFilename = string.Format(@"{0}\tmp\{1}_{2}_{3}_{4}_{5}.model", Directory.GetCurrentDirectory(), DateTime.Now.ToFileTimeUtc(),ImageWidth, ImageHeight, ImageDepth, Epochs);
            Close();
        }

        private void Open_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "样本文件|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                open_sample_textBox.Text = opg.FileName;
                SampleFilename = opg.FileName;
                string[] parameters = Path.GetFileNameWithoutExtension(SampleFilename).Split('_');
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
