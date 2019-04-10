using System.Windows.Forms;

namespace Host.UI.Forms
{
    public partial class ScottPlotForm : Form
    {
        public ScottPlotForm()
        {
            InitializeComponent();
        }

        public void LoadData(double[] x, double[] y, int length)
        {
            Mainly_scottPlotUC.plt.data.Clear();
            for(int i=0; i <length; i++)
                Mainly_scottPlotUC.plt.data.AddPoint(x[i], y[i]);
            Mainly_scottPlotUC.plt.settings.AxisFit();
            //Mainly_scottPlotUC.plt.data.AddPoint
            Mainly_scottPlotUC.plt.settings.title = "Words Embedding Visualization";
            Mainly_scottPlotUC.Render();
        }

    }
}
