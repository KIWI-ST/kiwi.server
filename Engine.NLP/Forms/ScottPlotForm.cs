using System.Drawing;
using System.Windows.Forms;

namespace Engine.NLP.Forms
{
    public partial class ScottPlotForm : Form
    {
        public ScottPlotForm()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            //convert to v3.0.3
            Mainly_scottPlotUC.plt.Clear();
        }

        public void Title(string title)
        {
            Mainly_scottPlotUC.plt.Title(title);
            Mainly_scottPlotUC.plt.XLabel(title);
        }

        public void Render()
        {
            Mainly_scottPlotUC.plt.Title(" ");
            Mainly_scottPlotUC.plt.XLabel(" ");
            Mainly_scottPlotUC.plt.YLabel(" ");
            Mainly_scottPlotUC.plt.AxisAuto();
            Mainly_scottPlotUC.Render();
        }

        public void AddData(double[][] xy, int length, Color color)
        {

            //Mainly_scottPlotUC.plt.plots
            //
            //for (int i=0; i <length; i++)
            //    Mainly_scottPlotUC.plt.data.AddPoint(xy[i][0], xy[i][1], 4, color);
        }

        public void PrepareData(double[] x, double[] y)
        {
            Mainly_scottPlotUC.plt.PlotScatter(x,y);
            Mainly_scottPlotUC.plt.AxisAuto();
        }

    }
}
