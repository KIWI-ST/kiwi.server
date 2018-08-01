using OxyPlot;
using OxyPlot.WindowsForms;
using System;
using System.Windows.Forms;

namespace Host.Image.UI.PlotForm
{
    public partial class DQNPlotFrom : Form
    {
        public DQNPlotFrom()
        {
            InitializeComponent();
        }

        public void SetModel(PlotModel model)
        {
            dqn_plotView.Model = model;
        }

        public void UpdateDarw()
        {
            dqn_plotView.Refresh();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                var pngExporter = new PngExporter { Background = OxyColors.White };
                pngExporter.ExportToFile(dqn_plotView.Model, sfg.FileName);
            }
        }
    }
}
