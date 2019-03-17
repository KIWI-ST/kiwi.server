using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
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
        /// <summary>
        /// 
        /// </summary>
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
            _t = new Thread(() => {
                string text = "";
                using (StreamReader sr = new StreamReader(textFullFilename))
                {
                    text = sr.ReadToEnd();
                }
                //var jarRoot = @"Stanford\stanford-corenlp-3.9.1-models";
                var props = new java.util.Properties();
                //string props = @"StanfordCoreNLP-chinese.properties";
                //Directory.SetCurrentDirectory(jarRoot);
                props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, coref, sentiment, relation");
                props.setProperty("ner.useSUTime", "false");
                //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
                //props.setProperty("sutime.binders", "tokenize, ssplit, pos, lemma, ner, parse, coref");
                var document = new Annotation(text);
                var pipeline = new StanfordCoreNLP(props);
                pipeline.annotate(document);
                var sentences = document.get(new CoreAnnotations.SentencesAnnotation().getClass()) as ArrayList;
                List<string> ners = new List<string>();
                foreach (CoreMap sentence in sentences.toArray())
                {
                    // traversing the words in the current sentence
                    var tokens = sentence.get(new CoreAnnotations.TokensAnnotation().getClass()) as ArrayList;
                    foreach (CoreLabel token in tokens.toArray())
                    {
                        // this is the text of the token
                        var word = token.get(new CoreAnnotations.TextAnnotation().getClass()) as string;
                        //words.Add(word);
                        // this is the POS tag of the token
                        var pos = token.get(new CoreAnnotations.PartOfSpeechAnnotation().getClass()) as string;
                       // posTags.Add(pos);
                        // this is the this is the NER label of the token
                        var ne = token.get(new CoreAnnotations.NamedEntityTagAnnotation().getClass()) as string;
                        if(ne!=null) ners.Add(ne);
                    }
                }
                string ss = "";
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
