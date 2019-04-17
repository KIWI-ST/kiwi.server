using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Engine.Brain.Model;
using Engine.Brain.Utils;
using Engine.NLP;

namespace Host.UI.Forms
{
    /// <summary>
    /// 情景要素分类
    /// </summary>
    enum ScenarioElementType
    {
        /// <summary>
        /// 致灾因子
        /// </summary>
        Factor = 1,
        /// <summary>
        /// 抗灾体
        /// </summary>
        Anti = 2,
        /// <summary>
        /// 承灾体
        /// </summary>
        Affect = 3
    }

    public partial class NLPExpertiseForm : Form
    {

        public NLPExpertiseForm()
        {
            InitializeComponent();
            InitialConfigValue();
        }

        void InitialConfigValue()
        {
            factors = NLPConfiguration.FactorScenarioString.Split(';').ToList();
            antis = NLPConfiguration.AntiScenarioString.Split(';').ToList();
            affects = NLPConfiguration.AffectScenarioString.Split(';').ToList();
            //
            UpdateScenarioListView(Factor_listView, factors);
            UpdateScenarioListView(Anti_listView, antis);
            UpdateScenarioListView(Affect_listView, affects);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        private void UpdateProcessTip(double process, double[][] a = null, double[][] b = null, double[][] c = null)
        {
            if (process == 1.0)
            {
                Visual_button.Text = "预览";
                Visual_button.Enabled = true;
                ScottPlotForm scott_plot_form = new ScottPlotForm();
                scott_plot_form.AddData(a, a.Length, Color.Red);
                scott_plot_form.AddData(b, b.Length, Color.Blue);
                scott_plot_form.AddData(c, c.Length, Color.Green);
                scott_plot_form.Title("Words Embedding Visualization");
                scott_plot_form.ShowDialog();
            }
            else
                Visual_button.Text = string.Format("{0:P}", process);
        }

        private delegate void UpdateProcessTipHandler(double process, double[][] a = null, double[][] b = null, double[][] c = null);

        public IDEmbeddingNet GloveNet { get; set; }

        List<string> factors = new List<string>();
        List<string> antis = new List<string>();
        List<string> affects = new List<string>();

        ScenarioElementType scenarioType = ScenarioElementType.Factor;

        private void UpdateScenarioListView(ListView listView, List<string> words)
        {
            listView.Items.Clear();
            words.ForEach(word =>
            {
                listView.Items.Add(word);
            });
        }

        private void Add_button_Click(object sender, EventArgs e)
        {
            string word = Word_textBox.Text.Trim();
            if (word == null || word.Length == 0) return;
            switch (scenarioType)
            {
                case ScenarioElementType.Factor:
                    {
                        if (!factors.Contains(word))
                            factors.Add(word);
                        UpdateScenarioListView(Factor_listView, factors);
                    }
                    break;
                case ScenarioElementType.Anti:
                    {
                        if (!antis.Contains(word))
                            antis.Add(word);
                        UpdateScenarioListView(Anti_listView, antis);
                    }
                    break;
                case ScenarioElementType.Affect:
                    {
                        if (!affects.Contains(word))
                            affects.Add(word);
                        UpdateScenarioListView(Affect_listView, affects);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Remove_button_Click(object sender, EventArgs e)
        {
            string word = Word_textBox.Text;
            if (word == null || word.Length == 0) return;
            switch (scenarioType)
            {
                case ScenarioElementType.Factor:
                    {
                        if (factors.Contains(word))
                            factors.Remove(word);
                        UpdateScenarioListView(Factor_listView, factors);
                    }
                    break;
                case ScenarioElementType.Anti:
                    {
                        if (antis.Contains(word))
                            antis.Remove(word);
                        UpdateScenarioListView(Anti_listView, antis);
                    }
                    break;
                case ScenarioElementType.Affect:
                    {
                        if (affects.Contains(word))
                            affects.Remove(word);
                        UpdateScenarioListView(Affect_listView, affects);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Visual_button_Click(object sender, EventArgs e)
        {
            if (GloveNet == null)
            {
                MessageBox.Show("预览前请先载入GloVe模型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Visual_button.Enabled = false;
            Thread t = new Thread(() =>
            {
                int totalNum = factors.Count + antis.Count + affects.Count;
                //1. 构建词W集合
                double[][] words = new double[totalNum][];
                //2.定义词颜色
                for (int i = 0; i < factors.Count; i++)
                    words[i] = GloveNet.Predict(factors[i]);
                for (int i = 0; i < antis.Count; i++)
                    words[factors.Count + i] = GloveNet.Predict(antis[i]);
                for (int i = 0; i < affects.Count; i++)
                    words[factors.Count + antis.Count + i] = GloveNet.Predict(affects[i]);
                //3.t-SNE算法降维
                var vWords = NP.TSNE2(words);
                //4.可视化
                Invoke(new UpdateProcessTipHandler(UpdateProcessTip), 1.0,
                    vWords.Take(factors.Count).ToArray(),
                    vWords.Skip(factors.Count).Take(antis.Count).ToArray(),
                    vWords.Skip(factors.Count + antis.Count).Take(affects.Count).ToArray());
            });
            t.IsBackground = true;
            t.Start();
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectItems = (sender as ListView).SelectedItems;
            Word_textBox.Text = selectItems.Count > 0 ? selectItems[0].Text : "";
        }

        private void Scenario_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    scenarioType = ScenarioElementType.Factor;
                    break;
                case 1:
                    scenarioType = ScenarioElementType.Anti;
                    break;
                case 2:
                    scenarioType = ScenarioElementType.Affect;
                    break;
                default:
                    scenarioType = ScenarioElementType.Factor;
                    break;
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            NLPConfiguration.AffectScenarioString = string.Join(";", affects.ToArray());
            NLPConfiguration.AntiScenarioString = string.Join(";", antis.ToArray());
            NLPConfiguration.FactorScenarioString = string.Join(";", factors.ToArray());
            MessageBox.Show("情景要素中心词已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
