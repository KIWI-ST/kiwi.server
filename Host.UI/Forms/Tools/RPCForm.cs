using Host.UI.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class RPCForm : Form
    {
        public RPCForm()
        {
            InitializeComponent();
            ImageList imageList = new ImageList();
            imageList.Images.Add(Resources.globe);
            raw_bin_listView.SmallImageList = imageList;
        }

        public Dictionary<string, double> RPCParamaters = new Dictionary<string, double>();

        private List<List<double>> _abcd = new List<List<double>>();

        public double[] A { get { return _abcd[0].ToArray(); } }

        public double[] B { get { return _abcd[1].ToArray(); } }

        public double[] C { get { return _abcd[2].ToArray(); } }

        public double[] D { get { return _abcd[3].ToArray(); } }

        public List<string> RawBinRasterFullFilenames { get; private set; } = new List<string>();

        private void rpc_file_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "rpc file|*.rpc";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                rpc_file_textBox.Text = opg.FileName;
                using (StreamReader sr = new StreamReader(opg.FileName))
                {
                    string text = sr.ReadToEnd().Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                    Regex reg = new Regex(@"\(([^)]*)\)");
                    MatchCollection abcd = reg.Matches(text);
                    List<Match> result = abcd.Cast<Match>().ToList();
                    //get a b c d parameters
                    for (int i = 0; i < result.Count; i++)
                    {
                        _abcd.Add(new List<double>());
                        string[] tmp = result[i].Value.Replace("(", "").Replace(")", "").Replace(";", "").Split(',');
                        for (int j = 0; j < tmp.Length; j++)
                            _abcd[i].Add(Convert.ToDouble(tmp[j]));
                    }
                    //
                    string[] patterns = new string[] { "errBias", "errRand", "lineOffset", "sampOffset", "latOffset", "longOffset", "heightOffset", "lineScale", "sampScale", "latScale", "longScale", "heightScale" };
                    //errBias\s\=\s[1-9]\d*\.?\d*\;
                    for(int i=0;i<patterns.Length;i++)
                    {
                        string pText = patterns[i] + @"\s+\=\s+[0-9]\d*\.?\d*\;";
                        Match match = new Regex(pText).Match(text);
                        string value = match.Value.Replace(";", "");
                        string[] tmp = value.Split('=');
                        RPCParamaters[tmp[0].Trim()] = Convert.ToDouble(tmp[1]);
                    }
                }
            }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            //if (TargetLayerKey == null || rpc_file_textBox.Text.Length == 0)
            //    MessageBox.Show("必要参数未选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //else
                Close();
        }

        /// <summary>
        /// 添加待rpc校正的bin文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_raw_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog
            {
                Multiselect = true,
                RestoreDirectory = true,
                Filter = "图像文件|*.bin"
            };

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                Array.ForEach(openfiledialog.FileNames, filename => {
                    if (!RawBinRasterFullFilenames.Contains(filename))
                    {
                        RawBinRasterFullFilenames.Add(filename);
                        raw_bin_listView.Items.Add(filename, 0);
                    }
                });
            }
        }

    }
}
