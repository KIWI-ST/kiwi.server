using System;
using System.Collections.Generic;
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
    public class TimeMarkupAnnotation:IAnnotation
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
        /// 
        /// </summary>
        readonly static java.lang.Class timeExpressionClass = new edu.stanford.nlp.time.TimeExpression.Annotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class treeCoreAnnotationClass = new edu.stanford.nlp.trees.TreeCoreAnnotations.TreeAnnotation().getClass();
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        java.util.Properties props;

        /// <summary>
        /// 
        /// </summary>
        public TimeMarkupAnnotation()
        {
            props = new java.util.Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, ner, parse");
            props.setProperty("ner.useSUTime", "1");
            props.setProperty("sutime.markTimeRanges", "1");
            props.setProperty("sutime.includeRange", "1");
        }

        public void Process(string rawText)
        {
            edu.stanford.nlp.pipeline.StanfordCoreNLPClient pipeline = new edu.stanford.nlp.pipeline.StanfordCoreNLPClient(props, "http://localhost", 9000);
            edu.stanford.nlp.pipeline.Annotation document = new edu.stanford.nlp.pipeline.Annotation(rawText);
            //run all Annotators on this text
            pipeline.annotate(document);
            java.util.AbstractList sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            if (sentences != null)
            {
                foreach(edu.stanford.nlp.util.CoreMap sentence in sentences)
                {
                    java.util.AbstractList tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                    foreach (edu.stanford.nlp.ling.CoreLabel token in tokens)
                    {
                        string word = (string)token.get(textAnnotationClass);
                        string pos = (string)token.get(partOfSpeechAnnotationClass);
                        string ner = (string)token.get(namedEntityTagAnnotationClass);
                        string value = (string)token.get(normalizedNamedEntityTagAnnotationClass);
                    }
                }
            }
            //
        }


    }
}
