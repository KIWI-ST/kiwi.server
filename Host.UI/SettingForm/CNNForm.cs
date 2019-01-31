using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class CNNForm : Form
    {
        public CNNForm()
        {
            InitializeComponent();
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
        /// 获取图层数据方式， 0 表示单像素逐波段获取
        /// </summary>
        public int Model { get; set; }
        /// <summary>
        /// 输入图像宽
        /// </summary>
        public int ImageWidth { get; set; }
        /// <summary>
        /// 输入图像高
        /// </summary>
        public int ImageHeight { get; set; }
        /// <summary>
        /// 模型训练轮次
        /// </summary>
        public int Epochs { get; set; }
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
            //init feature picked methods combox
            List<string> featurePickedMethods = new List<string>() {
                "基于像素的多波段输入"
            };
            FeaturePicked_comboBox.Items.Clear();
            featurePickedMethods.ForEach(p => {
                FeaturePicked_comboBox.Items.Add(p);
            });
            FeaturePicked_comboBox.SelectedIndex = 0;
            Model = 0;
            //combox
            source_image_comboBox.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                source_image_comboBox.Items.Add(p);
            });
            //setting value
            Epochs = (int)Epochs_numericUpDown.Value;
            ImageWidth = (int)Width_numericUpDown.Value;
            ImageHeight = (int)Height_numericUpDown.Value;
        }

        private void Source_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedFeatureRasterLayer = key;
        }

        private void Width_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ImageWidth = (int)(sender as NumericUpDown).Value;
        }

        private void Height_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ImageHeight = (int)(sender as NumericUpDown).Value;
        }

        private void Epochs_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Epochs = (int)(sender as NumericUpDown).Value;
        }

        private void FeaturePicked_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Model = FeaturePicked_comboBox.SelectedIndex;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            if (SelectedFeatureRasterLayer == null)
            {
                MessageBox.Show("请先选定待分类的影像");
            }
            else
            {
                Close();
            }
        }

        private void open_sample_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "样本文件|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                open_sample_textBox.Text = opg.FileName;
                FullFilename = opg.FileName;
            }
        }
    }
}
