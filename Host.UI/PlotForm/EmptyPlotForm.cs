using OxyPlot;
using OxyPlot.WindowsForms;
using System;
using System.Windows.Forms;

namespace Host.UI.PlotForm
{
    public partial class EmptyPlotForm : Form
    {
        public EmptyPlotForm()
        {
            InitializeComponent();
            this.FormClosing += EmptyPlotForm_FormClosing;
        }

        private void EmptyPlotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dqn_plotView.Model.Series.Clear();
            dqn_plotView.Dispose();
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
            sfg.AddExtension = true;
            sfg.DefaultExt = ".png";
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                var pngExporter = new PngExporter { Background = OxyColors.White };
                pngExporter.Resolution = 300;
                pngExporter.ExportToFile(dqn_plotView.Model, sfg.FileName);
            }
        }
    }
}
