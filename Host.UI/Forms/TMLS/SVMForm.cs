using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Engine.GIS.GLayer.GRasterLayer;

namespace Host.UI.SettingForm
{
    public partial class SVMFrom : Form
    {
        public SVMFrom()
        {
            InitializeComponent();
        }

        public int Model { get; private set; }

        public string FeatureKey { get; private set; }

        public string SampleFullFilename { get; private set; }

        public string WaitFullFilename { get; private set; }

        public string SaveFullFilename { get; private set; }

        public Dictionary<string, GRasterLayer> RasterDic
        {
            set
            {
                Initial(value);
            }
        }

        public void Initial(Dictionary<string, GRasterLayer> rasterDic)
        {
            featurelayer_comboBox.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                featurelayer_comboBox.Items.Add(p);
            });
        }

        private void open_sample_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "样本文件|*.txt|CSV文件|*.csv";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                sample_file_textBox.Text = opg.FileName;
                SampleFullFilename = opg.FileName;
            }
        }

        private void OK_Image_button_Click(object sender, EventArgs e)
        {
            Model = 1;
            Close();
        }

        private void featurelayer_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            FeatureKey = key;
        }

        private void open_wait_file_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "待分类文件|*.csv";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                wait_file_textBox.Text = opg.FileName;
                WaitFullFilename = opg.FileName;
            }
        }

        private void open_save_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.FileName = DateTime.Now.ToFileTimeUtc().ToString();
            sfg.AddExtension = true;
            sfg.DefaultExt = ".txt";
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                save_file_textBox.Text = sfg.FileName;
                SaveFullFilename = sfg.FileName;
            }
        }

        private void ok_other_button_Click(object sender, EventArgs e)
        {
            Model = 2;
            Close();
        }
    }
}
