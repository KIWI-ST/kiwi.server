using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Host.Image.UI.SettingForm
{
    public partial class CNNForm : Form
    {
        public CNNForm()
        {
            InitializeComponent();
        }

        Dictionary<string, GRasterLayer> _rasterDic;

        public string SelectedFeatureRasterLayer { get; set; }

        public string SelectedLabelRasterLayer { get; set; }

        public int Model { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public int Epochs { get; set; }

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
            //source and label combox 
            Source_comboBox.Items.Clear();
            Label_comboBox.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                Source_comboBox.Items.Add(p);
                Label_comboBox.Items.Add(p);
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

        private void Label_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedLabelRasterLayer = key;
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
            Close();
        }
    }
}
