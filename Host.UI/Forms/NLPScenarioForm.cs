using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.semgraph;
using edu.stanford.nlp.time;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using Engine.Brain.Model;
using Engine.Brain.Utils;
using Engine.NLP;
using static edu.stanford.nlp.semgraph.SemanticGraphCoreAnnotations;

namespace Host.UI.Forms
{
    public partial class NLPScenarioForm : Form
    {
        public NLPScenarioForm()
        {
            InitializeComponent();
        }
        #region Properties Class Name
        readonly static java.lang.Class sentencesAnnotationClass = new CoreAnnotations.SentencesAnnotation().getClass();
        readonly static java.lang.Class tokensAnnotationClass = new CoreAnnotations.TokensAnnotation().getClass();
        readonly static java.lang.Class textAnnotationClass = new CoreAnnotations.TextAnnotation().getClass();
        readonly static java.lang.Class partOfSpeechAnnotationClass = new CoreAnnotations.PartOfSpeechAnnotation().getClass();
        readonly static java.lang.Class namedEntityTagAnnotationClass = new CoreAnnotations.NamedEntityTagAnnotation().getClass();
        readonly static java.lang.Class normalizedNamedEntityTagAnnotationClass = new CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();
        readonly static java.lang.Class timeExpressionClass = new TimeExpression.Annotation().getClass();
        #endregion
        /// <summary>
        /// 
        /// </summary>
        string rawFullText = "";
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, List<CoreMap>> timeMLDict;
        /// <summary>
        /// 
        /// </summary>
        public IDEmbeddingNet GloveNet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            switch (button.Name)
            {
                //中心相似度计算
                case "Similarity_toolStripButton":
                    {
                        List<string> factors = NLPConfiguration.FactorScenarioString.Split(';').ToList();
                        List<string> antis = NLPConfiguration.AntiScenarioString.Split(';').ToList();
                        List<string> affects = NLPConfiguration.AffectScenarioString.Split(';').ToList();
                        //
                        Corpus_listBox.Items.Clear();
                        //
                        foreach(var word in CorpusWordList)
                        {
                            //string line = word;
                            double[] vWrod = GloveNet.Predict(word);
                            string factorText = "";
                            foreach (var factor in factors)
                            {
                                double[] vFactor = GloveNet.Predict(factor);
                                double cosine = NP.Cosine(vWrod, vFactor);
                                factorText += string.Format("{0}-{1},", factor, cosine);
                                //factorText += string.Format("{0},", cosine);
                            }
                            string antiText = "";
                            foreach (var factor in antis)
                            {
                                double[] vFactor = GloveNet.Predict(factor);
                                double cosine = NP.Cosine(vWrod, vFactor);
                                antiText += string.Format("{0}-{1},", factor, cosine);
                                //antiText += string.Format("{0},", cosine);
                            }
                            string affectText = "";
                            foreach (var factor in affects)
                            {
                                double[] vFactor = GloveNet.Predict(factor);
                                double cosine = NP.Cosine(vWrod, vFactor);
                                affectText += string.Format("{0}-{1},", factor, cosine);
                                //affectText += string.Format("{0},",cosine);
                            }
                            string line = string.Format("单词:{0}, 致灾因子相似度:{1},抗灾体相似度:{2},承灾体相似度:{3}", word, factorText, antiText, affectText);
                            UpdateListBox(line, false);
                        }
                    }
                    break;
                //预览文本词分布
                case "Preview_toolStripButton":
                    {
                        List<string> factors = NLPConfiguration.FactorScenarioString.Split(';').ToList();
                        List<string> antis = NLPConfiguration.AntiScenarioString.Split(';').ToList();
                        List<string> affects = NLPConfiguration.AffectScenarioString.Split(';').ToList();
                        Thread t = new Thread(() => {
                            int totalNum = factors.Count + antis.Count + affects.Count+CorpusWordList.Count;
                            //1. 构建词W集合
                            double[][] words = new double[totalNum][];
                            //2.定义词颜色
                            for (int i = 0; i < factors.Count; i++)
                                words[i] = GloveNet.Predict(factors[i]);
                            for (int i = 0; i < antis.Count; i++)
                                words[factors.Count + i] = GloveNet.Predict(antis[i]);
                            for (int i = 0; i < affects.Count; i++)
                                words[factors.Count + antis.Count + i] = GloveNet.Predict(affects[i]);
                            for(int i=0;i< CorpusWordList.Count;i++)
                                words[factors.Count + antis.Count + affects.Count + i] = GloveNet.Predict(CorpusWordList[i]);
                            //3.t-SNE算法降维
                            var vWords = NP.TSNE2(words);
                            //4.可视化
                            Invoke(new UpdatePreviewHandler(UpdatePreview),
                                vWords.Take(factors.Count).ToArray(),
                                vWords.Skip(factors.Count).Take(antis.Count).ToArray(),
                                vWords.Skip(factors.Count + antis.Count).Take(affects.Count).ToArray(),
                                vWords.Skip(factors.Count + antis.Count + affects.Count).Take(CorpusWordList.Count).ToArray());
                        });
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                case "Open_toolStripButton":
                    {
                        OpenFileDialog opg = new OpenFileDialog();
                        opg.Filter = "语料文本|*.txt";
                        if (opg.ShowDialog() == DialogResult.OK)
                        {
                            Corpus_listBox.Items.Clear();
                            rawFullText = "";
                            using (StreamReader sr = new StreamReader(opg.FileName, Encoding.UTF8))
                            {
                                string text = sr.ReadLine();
                                while (text != null)
                                {
                                    Corpus_listBox.Items.Add(text);
                                    rawFullText += text;
                                    text = sr.ReadLine();
                                }
                            }
                        }
                    }
                    break;
                //split by timeML
                case "Split_toolStripButton":
                    {
                        Split_toolStripButton.Enabled = false;
                        Thread t = new Thread(() =>
                        {
                            timeMLDict = SplitByTimeMarkupLanguage(rawFullText);
                        });
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                //print result
                case "Print_Scenario_Text_toolStripButton":
                    {
                        Corpus_listBox.Items.Clear();
                        foreach (var element in timeMLDict)
                        {
                            string timeText = element.Key;
                            UpdateListBox("---------------------------------------", false);
                            UpdateListBox("scenario: " + timeText, false);
                            foreach (CoreMap sentence in element.Value)
                            {
                                var tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                                //Tree tree = sentence.get(new TreeCoreAnnotations.TreeAnnotation().getClass()) as Tree;
                                //SemanticGraph deps = sentence.get(new BasicDependenciesAnnotation().getClass()) as SemanticGraph;
                                OutputDependenciesInSentence(sentence);
                                OutputTreeInSentence(sentence);
                                UpdateListBox("", true);
                                foreach (CoreLabel token in tokens)
                                {
                                    string word = (string)token.get(textAnnotationClass);
                                    string pos = (string)token.get(partOfSpeechAnnotationClass);
                                    string ner = (string)token.get(namedEntityTagAnnotationClass);
                                    string value = (string)token.get(normalizedNamedEntityTagAnnotationClass);
                                    UpdateListBox(string.Format("{0}--{1}--{2}--{3}", word, pos, ner, value), false);
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        delegate void UpdatePreviewHandler(double[][] a, double[][] b, double[][] c, double[][] d);
        /// <summary>
        /// 
        /// </summary>
        public void UpdatePreview(double[][] a, double[][] b, double[][] c, double[][] d)
        {
            ScottPlotForm scott_plot_form = new ScottPlotForm();
            //first draw backgrounds words
            scott_plot_form.AddData(d, d.Length, Color.Gray);
            scott_plot_form.AddData(a, a.Length, Color.Red);
            scott_plot_form.AddData(b, b.Length, Color.Blue);
            scott_plot_form.AddData(c, c.Length, Color.Green);
            scott_plot_form.Render();
            scott_plot_form.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="useTime"></param>
        delegate void UpdateListBoxHandler(string item, bool useTime = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="useTime"></param>
        private void UpdateListBox(string item, bool useTime = false)
        {
            string text = useTime ? string.Format("Time:{0},{1}", DateTime.Now.ToLongTimeString(), item) : item;
            Corpus_listBox.Items.Add(text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private Tree GetTreeStructInSentence(CacheMap sentence)
        {
            var tree = sentence.get(new TreeCoreAnnotations.TreeAnnotation().getClass()) as Tree;
            return tree;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private void OutputDependenciesInSentence(CoreMap sentence)
        {
            SemanticGraph deps = sentence.get(new BasicDependenciesAnnotation().getClass()) as SemanticGraph;
            foreach (var edge in deps.edgeListSorted().toArray())
            {
                SemanticGraphEdge graphEdge = edge as SemanticGraphEdge;
                var gov = graphEdge.getGovernor();
                var dep = graphEdge.getDependent();
                var relation = graphEdge.getRelation();
                var govWord = gov.word();
                var depWord = dep.word();
                //if (relation.toString() == "nsubj")
                //{
                string text = string.Format("relation: {0}, gov word: {1}, dep word: {2}", relation, govWord, depWord);
                if (InvokeRequired)
                    Invoke(new UpdateListBoxHandler(UpdateListBox), text, false);
                else
                    UpdateListBox(text, false);

                //}
            }
            //return deps.typedDependencies();
        }
        //
        private void OutputTreeInSentence(CoreMap sentence)
        {
            Tree tree = sentence.get(new TreeCoreAnnotations.TreeAnnotation().getClass()) as Tree;
            //foreach(LabeledScoredTreeNode leaf in tree.getLeaves().toArray())
            //{
            //    tree.toArray();
            //}
            //foreach(var element in tree.toArray())
            //{
            //    string text = string.Format("{0}", element);
            //    if (InvokeRequired)
            //        Invoke(new UpdateListBoxHandler(UpdateListBox), text, false);
            //    else
            //        UpdateListBox(text, false);
            //}
        }

        List<string> CorpusWordList = new List<string>();
        /// <summary>
        /// 基于timeML重组句子
        /// </summary>
        /// <param name="rawText"></param>
        private Dictionary<string, List<CoreMap>> SplitByTimeMarkupLanguage(string rawText)
        {
            //create props
            var props = new java.util.Properties();
            //tokenize, ssplit, pos, lemma, ner, parse, coref, depparse, natlog, openie
            //tokenize, ssplit, pos, lemma, ner, parse, dcoref 
            props.setProperty("annotators", "tokenize, ssplit, pos, ner, parse");
            props.setProperty("ner.useSUTime", "true");
            StanfordCoreNLPClient pipeline = new StanfordCoreNLPClient(props, "http://localhost", 9000);
            //pipeline.addAnnotator(new edu.stanford.nlp.parser.lexparser.LexicalizedParser().get);
            Annotation document = new Annotation(rawText);
            pipeline.annotate(document);
            //get sentance
            var sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            //time-sentences
            Dictionary<string, List<CoreMap>> dict = new Dictionary<string, List<CoreMap>>();
            //analysis by time(date)
            foreach (CoreMap sentence in sentences)
            {
                var tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                if (InvokeRequired)
                    Invoke(new UpdateListBoxHandler(UpdateListBox), ("------------------------------------------------------------------------------------------"), true);
                //Tree tree = sentence.get(new TreeCoreAnnotations.TreeAnnotation().getClass()) as Tree;
                //SemanticGraph deps = sentence.get(new BasicDependenciesAnnotation().getClass()) as SemanticGraph;
                //标记此句子是否有时间标注
                bool sig = false;
                foreach (CoreLabel token in tokens)
                {
                    string word = (string)token.get(textAnnotationClass);
                    //string pos = (string)token.get(partOfSpeechAnnotationClass);
                    string ner = (string)token.get(namedEntityTagAnnotationClass);
                    string value = (string)token.get(normalizedNamedEntityTagAnnotationClass);
                    //re constructed
                    if (ner == "DATE" && value.Length > 0)
                    {
                        if (!dict.ContainsKey(value))
                            dict.Add(value, new List<CoreMap>());
                        if (!dict[value].Contains(sentence))
                            dict[value].Add(sentence);
                        sig = true;
                    }
                    //1.基于NLP服务端预处理timeML标注语料(split)
                    //2.基于NLP服务端拆分词(pos, nn, ner)
                    //3.组织sentences
                    //4.基于 words embedding ，对情景三要素聚类（t-SNE降维可视化并聚类）
                    //    //词
                    //    
                    //    //词性
                    //    
                    //    //名词词性
                    //    
                    //    //属性值
                    //    
                    //    //CoreAnnotations
                    //    string mention = (string)token.get(new EnglishGrammaticalStructure().typedDependencies()) +"-";
                    //    //recollection sentance
                    //    string outputText = string.Format("{0}\t[pos={1};\tner={2};\tvalue={3};\tmention={4}]", word, pos, ner, value, mention);
                    //    if (InvokeRequired) Invoke(new UpdateListBoxHandler(UpdateListBox), outputText, false);
                    //    //recollect sentance
                    //    //if((string)number.len)
                    //    //if (ner == "DATE" && value.Length > 0)
                    //add to wardlist
                    CorpusWordList.Add(word);
                }
                if (!sig && dict.Values.Last() != null)
                    dict.Values.Last().Add(sentence);
            }
            //return dict
            return dict;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (item.Name)
            {
                case "Exprot_ToolStripMenuItem":
                    {
                        SaveFileDialog sfg = new SaveFileDialog(); 
                        if(sfg.ShowDialog() == DialogResult.OK)
                        {
                            string text = "";
                            foreach( var line in Corpus_listBox.SelectedItems)
                                text += line + "\r\n";
                            using (StreamWriter sw = new StreamWriter(sfg.FileName, true, Encoding.UTF8))
                                sw.Write(text);
                            //show tip
                            MessageBox.Show("导出完成");
                        }
                    }
                    break;
            }
        }
    }
}
