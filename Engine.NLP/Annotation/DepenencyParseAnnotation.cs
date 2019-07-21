using System;
using System.Collections.Generic;
using Engine.NLP.Utils;

namespace Engine.NLP.Annotation
{
    /// <summary>
    /// 分析处理句子集，构建情景
    /// 分析构建以下内容：
    /// 1. tokens
    /// 2. split words
    /// 3. 
    /// </summary>
    public class ScenarioAnnotation : IAnnotation
    {

        #region Annotation ClassName

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class entityTypeAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.EntityTypeAnnotation().getClass();

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
        /// 识别一些数字型的实体，例如日期，货币等
        /// </summary>
        readonly static java.lang.Class normalizedNamedEntityTagAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

        /// <summary>
        /// https://nlp.stanford.edu/software/sutime.shtml
        /// </summary>
        readonly static java.lang.Class timexAnnotationClass = new edu.stanford.nlp.time.TimeAnnotations.TimexAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class timeExpressionAnnotationClass = new edu.stanford.nlp.time.TimeExpression.Annotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class docDateAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.DocDateAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class treeAnnotationClass = new edu.stanford.nlp.trees.TreeCoreAnnotations.TreeAnnotation().getClass();

        /// <summary>
        /// dependency
        /// </summary>
        readonly static java.lang.Class basicDependenciesAnnotationClass = new edu.stanford.nlp.semgraph.SemanticGraphCoreAnnotations.BasicDependenciesAnnotation().getClass();

        /// <summary>
        /// 句子annotator 入口
        /// </summary>
        readonly static java.lang.Class mentionsAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.MentionsAnnotation().getClass();

        /// <summary>
        /// https://nlp.stanford.edu/nlp/javadoc/javanlp/edu/stanford/nlp/ling/CoreAnnotations.CanonicalEntityMentionIndexAnnotation.html
        /// </summary>
        readonly static java.lang.Class canonicalEntityMentionIndexAnnotation = new edu.stanford.nlp.ling.CoreAnnotations.CanonicalEntityMentionIndexAnnotation().getClass();

        #endregion

        /// <summary>
        /// indicate the ability of annotator
        /// </summary>
        java.util.Properties _props;

        /// <summary>
        /// cache sentences
        /// </summary>
        java.util.AbstractList _sentences;

        /// <summary>
        /// 记录时间-情景句子集
        /// </summary>
        Dictionary<DateTime, edu.stanford.nlp.util.CoreMap> timeStampSentences = new Dictionary<DateTime, edu.stanford.nlp.util.CoreMap>();

        /// <summary>
        ///  Annotation with SUTime
        /// </summary>
        public ScenarioAnnotation()
        {
            _props = new java.util.Properties();
            //refrenece https://stanfordnlp.github.io/CoreNLP/annotators.html
            _props.setProperty("annotators",
                //tokenize https://stanfordnlp.github.io/CoreNLP/tokenize.html
                "tokenize, " +
                //ssplit https://stanfordnlp.github.io/CoreNLP/ssplit.html
                "ssplit, " +
                //part of speech https://stanfordnlp.github.io/CoreNLP/pos.html
                "pos, " +
                //lemma https://stanfordnlp.github.io/CoreNLP/lemma.html
                "lemma, " +
                //named entity recongnition https://stanfordnlp.github.io/CoreNLP/ner.html
                "ner, " +
                //parse https://stanfordnlp.github.io/CoreNLP/parse.html
                "parse");
        }

        /// <summary>
        /// reference:
        /// https://stanfordnlp.github.io/CoreNLP/api.html
        /// </summary>
        /// <param name="rawText"></param>
        public void Process(string rawText)
        {
            edu.stanford.nlp.pipeline.StanfordCoreNLPClient pipeline = new edu.stanford.nlp.pipeline.StanfordCoreNLPClient(_props,  NLPConfiguration.CoreNLPAddress, Convert.ToInt32(NLPConfiguration.CoreNLPPort));
            edu.stanford.nlp.pipeline.Annotation document = new edu.stanford.nlp.pipeline.Annotation(rawText);
            //run all Annotators on this text
            pipeline.annotate(document);
            //cache sentences
            _sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            //var timeAll = document.get(timexAnnotationClass);
            if (_sentences == null) return;
            //分析时间顺序，得到 时间-情景句子集
            foreach (edu.stanford.nlp.util.CoreMap sentence in _sentences)
            {
                //句子内容
                string text = (string)sentence.get(textAnnotationClass);
                //https://github.com/stanfordnlp/CoreNLP/blob/c709c037aebb3ea3eb1e1591849e5a963b1d938f/src/edu/stanford/nlp/pipeline/GenderAnnotator.java#L42
                //edu.stanford.nlp.util, edu.stanford.nlp.coref.data.Mention
                //var mentions = sentence.get(mentionsAnnotationClass) as java.util.AbstractList
                //1.get tree sturcture
                edu.stanford.nlp.trees.Tree tree = sentence.get(treeAnnotationClass) as edu.stanford.nlp.trees.Tree;
                //2.build semantic graph
                ElementExtractByDependencyPrase(sentence);
            }
        }

        java.util.AbstractList _tokens;

        /// <summary>
        /// analysis the dependency in sentenc, and then extract information
        /// https://nlp.stanford.edu/software/dependencies_manual.pdf
        /// </summary>
        private void ElementExtractByDependencyPrase(edu.stanford.nlp.util.CoreMap sentence)
        {
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(basicDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            //逐token分析名字和其修饰
            java.util.AbstractList mentions = sentence.get(mentionsAnnotationClass) as java.util.AbstractList;
            _tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
            //
            foreach(edu.stanford.nlp.util.CoreMap mention in mentions)
            {
                string text =(string)mention.get(textAnnotationClass);
                string ner= (string)mention.get(namedEntityTagAnnotationClass);
                string noramlValue = (string)mention.get(normalizedNamedEntityTagAnnotationClass);
                //处理不同类型的ner，得到修饰依赖
                if (ner == "DATE")
                {
                    //情景时间
                }
                else if (ner == "ORGNIZATION")
                {
                    //备注信息
                }
                else if (ner == "NUMBER")
                {
                    //寻找修饰单位
                    FindAmod(dependencies, mention);
                }
                else if(ner == "CITY")
                {
                    //地点
                }
                else if(ner == "GPE")
                {

                }
            }
            //foreach (edu.stanford.nlp.ling.CoreLabel token in tokens)
            //{
            //    string word = (string)token.get(textAnnotationClass);
            //    string pos = (string)token.get(partOfSpeechAnnotationClass);
            //    string ner = (string)token.get(namedEntityTagAnnotationClass);
            //    string type = (string)token.get(entityTypeAnnotationClass);
            //    string value = (string)token.get(normalizedNamedEntityTagAnnotationClass);
            //    //根据ner判断命名实体内容，根据dependencies, 找到相关修饰关系
            //}
        }

        /// <summary>
        /// find token by index
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        private edu.stanford.nlp.ling.CoreLabel FindTokenByIdx(string idx)
        {
            foreach (edu.stanford.nlp.ling.CoreLabel token in _tokens)
                if (idx == token.index().ToString())
                    return token;
            return null;
        }

        /// <summary>
        /// 搜索与targetWord相关的修饰
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="targetWord">目标名词</param>
        private void FindAmod(edu.stanford.nlp.semgraph.SemanticGraph dependencies, edu.stanford.nlp.util.CoreMap mention)
        {
            //获取mention里的token，得到关键词并寻找修饰关系
            java.util.AbstractList tokens = mention.get(tokensAnnotationClass) as java.util.AbstractList;
            string cemIdx = (string)mention.get(canonicalEntityMentionIndexAnnotation);
            //根据cemidx寻找修饰关系，记录返回
            edu.stanford.nlp.ling.CoreLabel cemToken =FindTokenByIdx(cemIdx);

            //
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            //while (itr.hasNext())
            //{
            //    edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
            //}
        }


    }
}
