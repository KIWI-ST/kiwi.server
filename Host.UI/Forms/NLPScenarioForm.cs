using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        #region Properties Class Name
        readonly static java.lang.Class sentencesAnnotationClass =new CoreAnnotations.SentencesAnnotation().getClass();
        readonly static java.lang.Class tokensAnnotationClass =new CoreAnnotations.TokensAnnotation().getClass();
        readonly static java.lang.Class textAnnotationClass =new CoreAnnotations.TextAnnotation().getClass();
        readonly static java.lang.Class partOfSpeechAnnotationClass =new CoreAnnotations.PartOfSpeechAnnotation().getClass();
        readonly static java.lang.Class namedEntityTagAnnotationClass =new CoreAnnotations.NamedEntityTagAnnotation().getClass();
        readonly static java.lang.Class normalizedNamedEntityTagAnnotation =new CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();
        #endregion
        /// <summary>
        /// 
        /// </summary>
        string rawFullText = "";
        public void DoSomething()
        {
            //1.基于NLP服务端预处理timeML标注语料(split)
            //2.基于NLP服务端拆分词(pos, nn, ner)
            //3.组织sentences
            //4.基于 words embedding ，对情景三要素聚类（t-SNE降维可视化并聚类）
        }
        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            switch (button.Name)
            {
                case "Open_toolStripButton":
                    {
                        OpenFileDialog opg = new OpenFileDialog();
                        opg.Filter = "语料文本|*.txt";
                        if (opg.ShowDialog() == DialogResult.OK)
                        {
                            Corpus_listBox.Items.Clear();
                            rawFullText = "";
                            using(StreamReader sr = new StreamReader(opg.FileName, Encoding.UTF8))
                            {
                                string text = sr.ReadLine();
                                while(text!=null)
                                {
                                    Corpus_listBox.Items.Add(text);
                                    rawFullText += text;
                                    text = sr.ReadLine();
                                }
                            }
                        }
                    }
                    break;
                case "Split_toolStripButton":
                    {
                        SplitByTimeMarkupLanguage(rawFullText);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SplitByTimeMarkupLanguage(string rawText)
        {
            var props = new java.util.Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            StanfordCoreNLPClient pipeline = new StanfordCoreNLPClient(props, "http://localhost", 9000, 4);
            Annotation document = new Annotation(rawText);
            pipeline.annotate(document);
            var sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            foreach (CoreMap sentence in sentences)
            {
                var tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                Split_listBox.Items.Add("---------------");
                foreach (CoreLabel token in tokens)
                {
                    var word = token.get(textAnnotationClass);
                    var pos = token.get(partOfSpeechAnnotationClass);
                    var ner = token.get(namedEntityTagAnnotationClass);
                    Split_listBox.Items.Add(string.Format("{0}\t[pos={1};\tner={2}]", word, pos, ner));
                }
            }
        }

    }
}
