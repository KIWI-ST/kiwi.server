using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.NLP.Analysis
{
    /// <summary>
    /// 分析构建以下内容：
    /// 1. tokens
    /// 2. split words
    /// 3. 
    /// </summary>
    public class TimeMarkupAnnotation : IAnnotation
    {

        #region Annotation ClassName

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class sentencesAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.SentencesAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class tokensAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.TokensAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class textAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.TextAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class partOfSpeechAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.PartOfSpeechAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class namedEntityTagAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.NamedEntityTagAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class normalizedNamedEntityTagAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

        /// <summary>
        /// https://nlp.stanford.edu/software/sutime.shtml
        /// </summary>
        readonly static java.lang.Class timexAnnotationClass = new edu.stanford.nlp.time.TimeAnnotations.TimexAnnotations().getClass();

        readonly static java.lang.Class timeExpressionAnnotationClass = new edu.stanford.nlp.time.TimeExpression.Annotation().getClass();

        readonly static java.lang.Class docDateAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.DocDateAnnotation().getClass();


        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class treeCoreAnnotationClass = new edu.stanford.nlp.trees.TreeCoreAnnotations.TreeAnnotation().getClass();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        java.util.Properties props;

        java.util.AbstractList sentences;

        /// <summary>
        /// TimeML Annotation
        /// </summary>
        public TimeMarkupAnnotation()
        {
            props = new java.util.Properties();
            //refrenece : https://stanfordnlp.github.io/CoreNLP/annotators.html
            props.setProperty("annotators",
                //tokenize : https://stanfordnlp.github.io/CoreNLP/tokenize.html
                "tokenize, " +
                "ssplit, " +
                "pos, " +
                "lemma, " +
                "ner, " +
                "parse");
        }

        /// <summary>
        /// reference:
        /// https://stanfordnlp.github.io/CoreNLP/api.html
        /// 
        /// 
        /// </summary>
        /// <param name="rawText"></param>
        public void Process(string rawText)
        {
            edu.stanford.nlp.pipeline.StanfordCoreNLPClient pipeline = new edu.stanford.nlp.pipeline.StanfordCoreNLPClient(props, "http://localhost", 9000);
            edu.stanford.nlp.pipeline.Annotation document = new edu.stanford.nlp.pipeline.Annotation(rawText);
            //run all Annotators on this text
            pipeline.annotate(document);
            sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            //var timeAll = document.get(timexAnnotationClass);
            if (sentences == null) return;
            //
            foreach (edu.stanford.nlp.util.CoreMap sentence in sentences)
            {

                var tree = sentence.get(treeCoreAnnotationClass);

                java.util.AbstractList tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                foreach (edu.stanford.nlp.ling.CoreLabel token in tokens)
                {
                    string word = (string)token.get(textAnnotationClass);
                    string pos = (string)token.get(partOfSpeechAnnotationClass);
                    string ner = (string)token.get(namedEntityTagAnnotationClass);
                    string value = (string)token.get(normalizedNamedEntityTagAnnotationClass);
                }
            }
            //
        }

        private void SUTime(edu.stanford.nlp.util.CoreMap sentence)
        {
            //edu.stanford.nlp.time.
            //sentence.get(timeExpressionClass);
        }



    }
}
