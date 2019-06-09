using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Engine.Brain.Model;
using Engine.Brain.Utils;

namespace Engine.NLP.Forms
{
    /// <summary>
    /// 情景要素分类
    /// </summary>
    enum ScenarioElementType
    {
        /// <summary>
        /// 致灾因子
        /// </summary>
        Hazard = 1,

        /// <summary>
        /// 承灾体
        /// </summary>
        Exposure = 2,

        /// <summary>
        /// 承灾体
        /// </summary>
        HumanBehavior = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class NLPExpertiseForm : Form
    {

        public NLPExpertiseForm()
        {
            InitializeComponent();
            InitialConfigValue();
        }

        #region Intilization

        /// <summary>
        ///灾害系统要素
        /// </summary>
        private ScenarioElementType scenarioType = ScenarioElementType.Hazard;

        /// <summary>
        /// 词嵌入模型（GloVe）
        /// </summary>
        public IDEmbeddingNet GloveNet { get; set; }

        /// <summary>
        /// 孕灾环境
        /// </summary>
        List<string> exposure = new List<string>();

        /// <summary>
        /// 致灾因子
        /// </summary>
        List<string> hazard = new List<string>();
        /// <summary>
        /// 承灾体
        /// </summary>
        List<string> humanBehavior = new List<string>();

        /// <summary>
        /// 救援力量
        /// </summary>
        List<string> rescues = new List<string>();

        /// <summary>
        /// load config data
        /// </summary>
        void InitialConfigValue()
        {
            //cascading effect analysis
            hazard = NLPConfiguration.FactorScenarioString.Split(';').ToList();
            exposure = NLPConfiguration.InduceScenarioString.Split(';').ToList();
            humanBehavior = NLPConfiguration.AffectScenarioString.Split(';').ToList();
            //ui
            UpdateScenarioListView(Hazard_listView, exposure);
            UpdateScenarioListView(Exposure_listView, hazard);
            UpdateScenarioListView(Affect_listView, humanBehavior);
        }

        /// <summary>
        /// 更新进度委托
        /// </summary>
        /// <param name="process"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private delegate void UpdateProcessTipHandler(double process, double[][] a = null, double[][] b = null, double[][] c = null);
        
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
                scott_plot_form.Render();
                scott_plot_form.ShowDialog();
            }
            else//Visual_button.Text = string.Format("{0:P}", process);
                Visual_button.Text = "正在处理";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="words"></param>
        private void UpdateScenarioListView(ListView listView, List<string> words)
        {
            listView.Items.Clear();
            words.ForEach(word =>
            {
                listView.Items.Add(word);
            });
        }

        #endregion

        #region UI response

        private void Scenario_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    scenarioType = ScenarioElementType.Hazard;
                    break;
                case 1:
                    scenarioType = ScenarioElementType.Exposure;
                    break;
                case 2:
                    scenarioType = ScenarioElementType.HumanBehavior;
                    break;
                default:
                    scenarioType = ScenarioElementType.Hazard;
                    break;
            }
        }

        private void Add_button_Click(object sender, EventArgs e)
        {
            string word = Word_textBox.Text.Trim();
            if (word == null || word.Length == 0) return;
            switch (scenarioType)
            {
                case ScenarioElementType.Hazard:
                    {
                        if (!hazard.Contains(word))
                            hazard.Add(word);
                        UpdateScenarioListView(Exposure_listView, hazard);
                    }
                    break;
                case ScenarioElementType.Exposure:
                    {
                        if (!exposure.Contains(word))
                            exposure.Add(word);
                        UpdateScenarioListView(Hazard_listView, exposure);
                    }
                    break;
                case ScenarioElementType.HumanBehavior:
                    {
                        if (!humanBehavior.Contains(word))
                            humanBehavior.Add(word);
                        UpdateScenarioListView(Affect_listView, humanBehavior);
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
                case ScenarioElementType.Hazard:
                    {
                        if (hazard.Contains(word))
                            hazard.Remove(word);
                        UpdateScenarioListView(Exposure_listView, hazard);
                    }
                    break;
                case ScenarioElementType.Exposure:
                    {
                        if (exposure.Contains(word))
                            exposure.Remove(word);
                        UpdateScenarioListView(Hazard_listView, exposure);
                    }
                    break;
                case ScenarioElementType.HumanBehavior:
                    {
                        if (humanBehavior.Contains(word))
                            humanBehavior.Remove(word);
                        UpdateScenarioListView(Affect_listView, humanBehavior);
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
                //0.通知预处理开始
                Invoke(new UpdateProcessTipHandler(UpdateProcessTip), 0.0);
                int totalNum = hazard.Count + exposure.Count + humanBehavior.Count;
                //1. 构建词W集合
                double[][] words = new double[totalNum][];
                //2.定义词颜色
                for (int i = 0; i < hazard.Count; i++)
                    words[i] = GloveNet.Predict(hazard[i]);
                for (int i = 0; i < exposure.Count; i++)
                    words[hazard.Count + i] = GloveNet.Predict(exposure[i]);
                for (int i = 0; i < humanBehavior.Count; i++)
                    words[hazard.Count + exposure.Count + i] = GloveNet.Predict(humanBehavior[i]);
                //3.t-SNE算法降维
                var vWords = NP.TSNE2(words);
                //4.可视化
                Invoke(new UpdateProcessTipHandler(UpdateProcessTip), 1.0,
                    vWords.Take(hazard.Count).ToArray(),
                    vWords.Skip(hazard.Count).Take(exposure.Count).ToArray(),
                    vWords.Skip(hazard.Count + exposure.Count).Take(humanBehavior.Count).ToArray());
            });
            t.IsBackground = true;
            t.Start();
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectItems = (sender as ListView).SelectedItems;
            Word_textBox.Text = selectItems.Count > 0 ? selectItems[0].Text : "";
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            NLPConfiguration.AffectScenarioString = string.Join(";", humanBehavior.ToArray());
            NLPConfiguration.InduceScenarioString = string.Join(";", exposure.ToArray());
            NLPConfiguration.FactorScenarioString = string.Join(";", hazard.ToArray());
            NLPConfiguration.RescueScenarioString = string.Join(";", rescues.ToArray());
            MessageBox.Show("专家知识保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
