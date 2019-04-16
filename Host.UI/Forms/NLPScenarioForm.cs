using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.semgraph;
using edu.stanford.nlp.time;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                            SplitByTimeMarkupLanguage(rawFullText);
                        });
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                    //print result
                case "Print_Scenario_Text_toolStripButton":
                    {
                        Corpus_listBox.Items.Clear();
                        //foreach(var element in TimeMLDict)
                        //{
                        //    string timeText = element.Key;
                        //    UpdateListBox("------------------"+timeText+"------------------", false);
                        //    foreach(var sentence in element.Value)
                        //        UpdateListBox(sentence.ToString(), false);
                        //}
                    }
                    break;
                default:
                    break;
            }
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
        private void UpdateListBox(string item, bool useTime=false)
        {
            string text =useTime? string.Format("Time:{0},{1}", DateTime.Now.ToLongTimeString(), item):item;
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
        private java.util.Collection GetDependenciesInSentence(CoreMap sentence)
        {
            SemanticGraph deps = sentence.get(new BasicDependenciesAnnotation().getClass()) as SemanticGraph;
            foreach(var edge in deps.edgeListSorted().toArray())
            {
                SemanticGraphEdge graphEdge = edge as SemanticGraphEdge;
                var gov = graphEdge.getGovernor();
                var dep = graphEdge.getDependent();
                var relation = graphEdge.getRelation();
                var govWord = gov.word();
                var depWord = dep.word();
            }
            return deps.typedDependencies();
        }
        /// <summary>
        /// 基于timeML重组句子
        /// </summary>
        /// <param name="rawText"></param>
        private void SplitByTimeMarkupLanguage(string rawText)
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
            //analysis by time(date)
            foreach (CoreMap sentence in sentences)
            {
                var tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                if (InvokeRequired)
                    Invoke(new UpdateListBoxHandler(UpdateListBox), ("------------------------------------------------------------------------------------------"), true);
                Tree tree = sentence.get(new TreeCoreAnnotations.TreeAnnotation().getClass()) as Tree;
                SemanticGraph deps = sentence.get(new BasicDependenciesAnnotation().getClass()) as SemanticGraph;
                foreach (CoreLabel token in tokens)
                {
                    string word = (string)token.get(textAnnotationClass);
                    string pos = (string)token.get(partOfSpeechAnnotationClass);
                    string ner = (string)token.get(namedEntityTagAnnotationClass);
                    string value = (string)token.get(normalizedNamedEntityTagAnnotationClass);
                    //
                    if(ner == "DATE" && value.Length > 0) { }

                    //
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
                }
                //collect sentence
                //if (timeKey.Length > 0)
                //{
                //    if (!TimeMLDict.ContainsKey(timeKey))
                //        TimeMLDict[timeKey] = new List<CoreMap>();
                //    TimeMLDict[timeKey].Add(sentence);
                //}
                //else if (TimeMLDict.Count > 0)
                //   TimeMLDict.Last().Value.Add(sentence);
            }
        }
    }
}
