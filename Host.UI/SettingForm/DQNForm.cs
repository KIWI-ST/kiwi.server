using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
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

        public string TaskName { get; private set; }

        public string SelectedFeatureRasterLayer { get; private set; }

        public string SelectedLabelRasterLayer { get; private set; }

        public int Epochs { get; private set; }

        private string[] _task_names = new string[] {"Image Classification","Road Extraction"};

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
            //add task names
            task_name_comboBox.Items.Clear();
            Array.ForEach(_task_names, p => {
                task_name_comboBox.Items.Add(p);
            });
            //add raster keys
            state_comboBox.Items.Clear();
            feedback_comboBox.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                state_comboBox.Items.Add(p);
                feedback_comboBox.Items.Add(p);
            });
        }

        private void state_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedFeatureRasterLayer = key;
        }

        private void feedback_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            SelectedLabelRasterLayer = key;
        }

        private void task_name_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskName = (sender as ComboBox).SelectedItem as string;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            Epochs = (int)(epochs_numericUpDown as NumericUpDown).Value;
            Close();
        }
    }
}
