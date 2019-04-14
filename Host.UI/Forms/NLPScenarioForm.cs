using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host.UI.Forms
{
    public partial class NLPScenarioForm : Form
    {
        public NLPScenarioForm()
        {
            InitializeComponent();
        }

        public void DoSomething()
        {
            //1.基于NLP服务端预处理timeML标注语料(split)
            //2.基于NLP服务端拆分词(pos, nn, ner)
            //3.组织sentences
            //4.基于 words embedding ，对情景三要素聚类（t-SNE降维可视化并聚类）
        }

    }
}
