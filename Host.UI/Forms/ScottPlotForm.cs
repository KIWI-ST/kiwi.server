using System.Drawing;
using System.Windows.Forms;

namespace Host.UI.Forms
{
    public partial class ScottPlotForm : Form
    {
        public ScottPlotForm()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            Mainly_scottPlotUC.plt.data.Clear();
        }

        public void Title(string title)
        {
            Mainly_scottPlotUC.plt.settings.title = title;
        }

        public void Render()
        {
            Mainly_scottPlotUC.Render();
        }

        public void AddData(double[][] xy, int length, Color color)
        {
            for(int i=0; i <length; i++)
                Mainly_scottPlotUC.plt.data.AddPoint(xy[i][0], xy[i][1], 4, color);
            Mainly_scottPlotUC.plt.settings.AxisFit();
        }

    }
}
