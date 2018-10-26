using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Host.Image.UI.PlotForm
{

    public partial class ComparedPlotForm : Form
    {

        public ComparedPlotForm()
        {
            InitializeComponent();
        }

        List<double> _x, _y;

        List<LineSeries> _lines = new List<LineSeries>();

        public void InilializationModel(PlotModel plotModel, string title, double x_min = 0, double x_max = 5000, double y_min = 0.85, double y_max = 1)
        {
            //
            plotModel.Axes.Add(new LinearAxis()
            {
                Title = "Training Epochs",
                Position = AxisPosition.Bottom,
                Minimum = x_min,
                Maximum = x_max * 1.1,
                FontSize = 26
            });
            //
            plotModel.Axes.Add(new LinearAxis()
            {
                Title = "Accuracy",
                Position = AxisPosition.Left,
                Minimum = y_min,
                Maximum = y_max,
                FontSize = 26
            });
            //
            plotModel.Title = title;
            //legend
            plotModel.LegendPlacement = LegendPlacement.Inside;
            plotModel.LegendPosition = LegendPosition.LeftTop;
            plotModel.LegendFontSize = 26;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlotModel plotModel = new PlotModel { LegendBackground = OxyColor.FromAColor(200, OxyColors.White), LegendBorder = OxyColors.Transparent };
            InilializationModel(plotModel, title_textbox.Text, Convert.ToDouble(x_min.Value), Convert.ToDouble(x_max.Value), Convert.ToDouble(y_min.Text), Convert.ToDouble(y_max.Text));
            _lines.ForEach(p => {
                plotModel.Series.Add(p);
            });
            EmptyPlotForm form = new EmptyPlotForm();
            form.SetModel(plotModel);
            form.Show();
            form.UpdateDarw();
        }

        private void x_open_button_Click(object sender, EventArgs e)
        {
            //打开x轴数据
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "文本文件(*.txt)|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(opg.FileName))
                {
                    _x = new List<double>();
                    string text = sr.ReadToEnd();
                    Array.ForEach(text.Split(','), p =>
                    {
                        _x.Add(Convert.ToDouble(p));
                    });
                    x_textBox.Text = text;
                }
            }
        }

        private void addline_button_Click(object sender, EventArgs e)
        {
            if (_x == null || _y == null)
            {
                MessageBox.Show("缺少数据");
                return;
            }
            else if (_x.Count != _y.Count)
            {
                MessageBox.Show("x,y轴数据长度不一致");
                return;
            }
            //
            LineSeries line = new LineSeries();
            line.Title = lengend_textBox.Text;
            for (int i = 0; i < _x.Count; i++)
            {
                DataPoint p = new DataPoint(_x[i], _y[i]);
                line.Points.Add(p);
            }
            _lines.Add(line);
            // clear 
            Reset();
        }

        private void Reset()
        {
            _x = null;
            _y = null;
            lengend_textBox.Text = "";
            x_textBox.Text = "";
            y_textBox.Text = "";
        }

        private void y_open_button_Click(object sender, EventArgs e)
        {
            //打开y轴数据
            //打开x轴数据
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "文本文件(*.txt)|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(opg.FileName))
                {
                    _y = new List<double>();
                    string text = sr.ReadToEnd();
                    Array.ForEach(text.Split(','), p =>
                    {
                        _y.Add(Convert.ToDouble(p));
                    });
                    y_textBox.Text = text;
                }
            }
        }

    }
}
