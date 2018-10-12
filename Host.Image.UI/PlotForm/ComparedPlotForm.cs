using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Host.Image.UI.PlotForm
{
    public partial class ComparedPlotForm : Form
    {
        public ComparedPlotForm()
        {
            InitializeComponent();
        }

        PlotModel _plotModel = new PlotModel { LegendBackground = OxyColor.FromAColor(200, OxyColors.White), LegendBorder = OxyColors.Transparent };

        List<DataPoint> _points = new List<DataPoint>();

        public void InilializationModel(string title, double x_min = 0, double x_max = 5000, double y_min = 0.85, double y_max = 1)
        {
            //
            _plotModel.Axes.Add(new LinearAxis()
            {
                Title = "Training Epochs",
                Position = AxisPosition.Bottom,
                Minimum = x_min,
                Maximum = x_max * 1.1,
                FontSize = 26
            });
            //
            _plotModel.Axes.Add(new LinearAxis()
            {
                Title = "Accuracy",
                Position = AxisPosition.Left,
                Minimum = y_min,
                Maximum = y_max,
                FontSize = 26
            });
            //
            _plotModel.Title = title;
            //legend
            _plotModel.LegendPlacement = LegendPlacement.Inside;
            _plotModel.LegendPosition = LegendPosition.RightTop;
            _plotModel.LegendFontSize = 26;
        }

        public void AddLineSeries(List<DataPoint> points,string title)
        {
            LineSeries line = new LineSeries();
            line.Title = title;
            points.ForEach(p => line.Points.Add(p));
            _plotModel.Series.Add(line);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                double x = Convert.ToDouble(position_textbox.Text);
                double y = Convert.ToDouble(value_textbox.Text);
                DataPoint point = new DataPoint(x, y);
                _points.Add(point);
                //
                listBox1.Items.Add(string.Format("{0},{1}",x,y));
            }
            catch
            {
                MessageBox.Show("添加点错误，请检查输入值");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_points.Count == 0)
                return;
            listBox1.Items.Clear();
            AddLineSeries(_points, series_name_textbox.Text);
            _points = new List<DataPoint>();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            InilializationModel(plot_name_textbox.Text,Convert.ToDouble(x_min.Value), Convert.ToDouble(x_max.Value), Convert.ToDouble(y_min.Text), Convert.ToDouble(y_max.Text));
            listBox1.Items.Clear();
            //
            EmptyPlotForm form = new EmptyPlotForm();
            form.SetModel(_plotModel);
            form.Show();
            form.UpdateDarw();
        }
    }
}
