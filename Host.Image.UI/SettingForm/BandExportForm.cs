using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class BandExportForm : Form
    {
        public BandExportForm()
        {
            InitializeComponent();
        }

        public bool HasChecked
        {
            get
            {
                return this.textBox1.Text != null && this.textBox1.Text.Length > 0;
            }
        }

        public GRasterLayer RasterLayer { get; set; }

        public int Index { get; set; }

        Dictionary<string, GRasterLayer> _rasterDic;

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
        }

        public void Save()
        {
            int index = Index;
            double[] regionGeoTransform = SelectedRegionLayerKey != null ? _rasterDic[SelectedRegionLayerKey].GeoTransform : null;
            RasterLayer.SaveBand(index, textBox1.Text, regionGeoTransform);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        public string SelectedRegionLayerKey { get; set; }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedRegionLayerKey = key;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.AddExtension = true;
            sfg.DefaultExt = ".tif";
            if (sfg.ShowDialog() == DialogResult.OK)
                textBox1.Text = sfg.FileName;
        }
    }
}
