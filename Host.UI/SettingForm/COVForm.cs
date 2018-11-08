using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class COVForm : Form
    {
        public COVForm()
        {
            InitializeComponent();
        }

        Dictionary<string, GRasterLayer> _rasterDic;

        public Dictionary<string, GRasterLayer> RasterDic
        {
            set
            {
                _rasterDic = value;
                Initial(_rasterDic);
            }
        }

        public string Target1Key { get; private set; }

        public string Target2Key { get; private set; }

        public void Initial(Dictionary<string, GRasterLayer> rasterDic)
        {
            target1_comboBox.Items.Clear();
            target2_comboBox.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                target1_comboBox.Items.Add(p);
                target2_comboBox.Items.Add(p);
            });
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void target1_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            Target1Key = key;
        }

        private void target2_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            Target2Key = key;
        }
    }
}
