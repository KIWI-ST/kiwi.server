using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

        List<string> factors = new List<string>();
        List<string> antis = new List<string>();
        List<string> affects = new List<string>();

        ScenarioElementType scenarioType = ScenarioElementType.Factor;

        private void UpdateScenarioListView(ListView listView,List<string> words)
        {
            listView.Items.Clear();
            words.ForEach(word => {
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

        }

        private void Visual_button_Click(object sender, EventArgs e)
        {

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
            MessageBox.Show("已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
