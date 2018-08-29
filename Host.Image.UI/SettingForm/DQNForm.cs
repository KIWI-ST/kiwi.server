using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Host.Image.UI.SettingForm
{
    public partial class DQNForm : Form
    {

        public DQNForm()
        {
            InitializeComponent();
            Epoches = (int)numericUpDown1.Value;
        }

        Dictionary<string, GRasterLayer> _rasterDic;

        public string SelectedFeatureRasterLayer { get; set; }

        public string SelectedLabelRasterLayer { get; set; }

        public int Epoches { get; set; }

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
            comboBox1.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                comboBox1.Items.Add(p);
            });
            comboBox2.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                comboBox2.Items.Add(p);
            });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedFeatureRasterLayer = key;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedLabelRasterLayer = key;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Epoches = (int)(sender as NumericUpDown).Value;
        }
    }
}
