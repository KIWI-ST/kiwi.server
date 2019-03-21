using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using Engine.Brain.Model.DL;
using java.util;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Host.UI.Jobs
{
    public class JobParsingText : IJob
    {
        /// <summary>
        /// background thread
        /// </summary>
        Thread _t;
        /// <summary>
        /// task name
        /// </summary>
        public string Name => "ParsingTextTask";
        /// <summary>
        /// run process
        /// </summary>
        public double Process { get; private set; } = 0.0;
        /// <summary>
        /// polt models
        /// </summary>
        public PlotModel[] PlotModels => throw new NotImplementedException();
        /// <summary>
        /// task start time
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;

        /// <summary>
        /// DQN classify task
        /// </summary>
        /// <param name="featureRasterLayer"></param>
        /// <param name="labelRasterLayer"></param>
        /// <param name="epochs"></param>
        public JobParsingText(string textFullFilename, string modelFullFilename, string lexiconFullFilename)
        {
            //lstm
            _t = new Thread(() => {
                OnStateChanged?.Invoke(Name, string.Format("{0} - {1}", DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(), "句法分析任务开始"));
                string text = "";
                using (StreamReader sr = new StreamReader(textFullFilename)) text = sr.ReadToEnd();
                OnStateChanged?.Invoke(Name, string.Format("{0} - {1}", DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(), text));
                var props = new java.util.Properties();
                props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, coref, sentiment, relation");
                props.setProperty("ner.useSUTime", "false");
                //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
                //props.setProperty("sutime.binders", "tokenize, ssplit, pos, lemma, ner, parse, coref");
                var document = new Annotation(text);
                OnStateChanged?.Invoke(Name, string.Format("{0} - {1}", DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(), "初始化NLP环境完成"));
                var pipeline = new StanfordCoreNLP(props);
                pipeline.annotate(document);
                var sentences = document.get(new CoreAnnotations.SentencesAnnotation().getClass()) as ArrayList;
                List<string> ners = new List<string>();
                //lstm
                //var lstmNetwork = LSTMNetwork.Load(modelFullFilename);
                //var lexicon = Engine.Lexicon.Entity.Lexicon.FromExistLexiconFile(lexiconFullFilename, Engine.Lexicon.Entity.EncodeScheme.Onehot);
                //foreach (CoreMap sentence in sentences.toArray())
                //{
                //    OnStateChanged?.Invoke(Name, string.Format("{0} - {1}", DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(), sentence.ToString()));
                //    // traversing the words in the current sentence
                //    var tokens = sentence.get(new CoreAnnotations.TokensAnnotation().getClass()) as ArrayList;
                //    List<string> words = new List<string>();
                //    foreach (CoreLabel token in tokens.toArray())
                //    {
                //        // this is the text of the token
                //        var word = token.get(new CoreAnnotations.TextAnnotation().getClass()) as string;
                //        // this is the POS tag of the token
                //        var pos = token.get(new CoreAnnotations.PartOfSpeechAnnotation().getClass()) as string;
                //        // this is the this is the NER label of the token
                //        var ne = token.get(new CoreAnnotations.NamedEntityTagAnnotation().getClass()) as string;
                //        //orig by ner
                //        if(ne!=null) OnStateChanged?.Invoke(Name, string.Format("{0} - {1} - {2} - {3}", DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(), word, pos, ne));
                //        //add to word collection
                //        if(lexicon.Exist(word)) words.Add(word);
                //    }
                //    //
                //    while (words.Count < 24)
                //        words.AddRange(words);
                //    var sentence2 =  lstmNetwork.WriteText(words.ToArray(), lexicon);
                //    OnStateChanged?.Invoke(Name, string.Format("{0} - {1}", DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(), sentence2.ToString()));
                //}
            });
        }
        /// <summary>
        /// example samples
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename){ }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
