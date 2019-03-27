using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class RandomForestForm : Form
    {
        public RandomForestForm()
        {
            InitializeComponent();
        }

        public string FullFilename { get; private set; }

        public string FeatureKey { get; private set; }

        public int TreeCount { get { return Convert.ToInt32(tree_count_numericUpDown.Value); } }

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

        private void open_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "样本文件|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                sample_file_textBox.Text = opg.FileName;
                FullFilename = opg.FileName;
            }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void featurelayer_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            FeatureKey = key;
        }

    }
}
