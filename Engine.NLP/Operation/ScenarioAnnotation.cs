using System;
using System.Collections.Generic;
using System.Linq;
using Engine.NLP.Entity;
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
    public class ScenarioAnnotation
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
        /// <param name="groupedText">已经过timeml重组后的文本资料</param>
        public Scenario Process(string groupedText)
        {
            if (groupedText == null || groupedText.Length == 0) return null;
            Scenario scenario = new Scenario();
            edu.stanford.nlp.pipeline.StanfordCoreNLPClient pipeline = new edu.stanford.nlp.pipeline.StanfordCoreNLPClient(_props, NLPConfiguration.CoreNLPAddress, Convert.ToInt32(NLPConfiguration.CoreNLPPort));
            edu.stanford.nlp.pipeline.Annotation document = new edu.stanford.nlp.pipeline.Annotation(groupedText);
            pipeline.annotate(document);
            java.util.AbstractList sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            //逐一分析句子结构，即 主语+谓语+xxxx
            foreach (edu.stanford.nlp.util.CoreMap sentence in sentences)
            {
                //}{debug 展示句子内容
                string text = (string)sentence.get(textAnnotationClass);
                //2.build semantic graph
                List<Pipline> plines = ElementExtractByDependencyPrase(sentence);
                //3.concat result
                //processResult = processResult.Concat(dict).ToDictionary(k => k.Key, v => v.Value);
                //https://github.com/stanfordnlp/CoreNLP/blob/c709c037aebb3ea3eb1e1591849e5a963b1d938f/src/edu/stanford/nlp/pipeline/GenderAnnotator.java#L42
                //edu.stanford.nlp.util, edu.stanford.nlp.coref.data.Mention
                //var mentions = sentence.get(mentionsAnnotationClass) as java.util.AbstractList
                //1.get tree sturcture
                //edu.stanford.nlp.trees.Tree tree = sentence.get(treeAnnotationClass) as edu.stanford.nlp.trees.Tree;
                scenario.MergePipline(plines);
            }
            //concat the result and return
            return scenario;
        }
        
        private string FindNounProperty(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.CoreLabel token)
        {
            string target = "";
            List<edu.stanford.nlp.trees.TypedDependency> deps = FindDeirctRefs(sentence, token);
            deps.ForEach(dep => {
                string relname = dep.reln().getShortName();
                //名词修饰关系
                if (relname == "compound:nn" || relname =="nmod:assmod")
                {
                    target += (string)dep.dep().backingLabel().get(textAnnotationClass);
                }
     
            });
            return target;
        }

        /// <summary>
        /// 找到target word全部的修饰关系，组成最后的词组
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private string FindNoun(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.IndexedWord word)
        {
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(basicDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.List children = dependencies.outgoingEdgeList(word);
            if (children.isEmpty()) return word.value();
            string property = "";
            java.util.Iterator itr = children.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.semgraph.SemanticGraphEdge edge = itr.next() as edu.stanford.nlp.semgraph.SemanticGraphEdge;
                edu.stanford.nlp.ling.IndexedWord target = edge.getTarget();
                string relname = edge.getRelation().getShortName();
                //需要递归得到修饰关系
                //包括：noun compound modifer，number modifer
                if (relname.Contains("compound:nn")|| relname.Contains("nummod"))
                    property += FindNoun(sentence, target)+word.value();
                //ass修饰即结束
                else if (relname.Contains("assmod"))
                    property += target.value();
                //数字单位，结束
                else if(relname.Contains("mark:clf"))
                    property += word.value()+target.value();
            }
            return property;
        }

        /// <summary>
        /// 分析谓语依赖，得到修饰关系（针对可量化的）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sentence"></param>
        private void AnalysisActionDepsInSentence(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.IndexedWord word, Pipunit punit)
        {
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(basicDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.List children =  dependencies.outgoingEdgeList(word);
            java.util.Iterator itr = children.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.semgraph.SemanticGraphEdge edge = itr.next() as edu.stanford.nlp.semgraph.SemanticGraphEdge;
                edu.stanford.nlp.ling.IndexedWord target = edge.getTarget();
                string relname = edge.getRelation().getShortName();
                //1. nmod, 继续下探一层，寻找修饰
                if(relname.Contains("nmod"))
                {
                    string property = FindNoun(sentence, target);
                    punit.AddDesc(property);
                }
                //2. 宾语结构，直接添加
                if(relname.Contains("dobj") || relname.Contains("iobj") || relname.Contains("pobj"))
                {
                    string property = target.value();
                    punit.AddDesc(property);
                }
            }
        }

        /// <summary>
        /// 分析主语相关依赖, 得到相关修饰关系（针对可量化的）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sentence"></param>
        private void AnalysisSubjectDepsInSentence(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.IndexedWord word, Pipunit punit)
        {
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(basicDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.List children = dependencies.outgoingEdgeList(word);
            java.util.Iterator itr = children.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.semgraph.SemanticGraphEdge edge = itr.next() as edu.stanford.nlp.semgraph.SemanticGraphEdge;
                edu.stanford.nlp.ling.IndexedWord target = edge.getTarget();
                string relname = edge.getRelation().getShortName();
                //1. nmod, 继续下探一层，寻找修饰
                if (relname.Contains("nmod")||relname.Contains("compound:nn"))
                {
                    string property = FindNoun(sentence, target);
                    punit.AddDesc(property);
                }
                //2.直接修饰
                //ass修饰即结束
                else if (relname.Contains("assmod"))
                {
                    string property = target.value();
                    punit.AddDesc(property);
                }
                //数字单位，结束
                else if (relname.Contains("mark:clf"))
                {
                    string property = word.value() + target.value();
                    punit.AddDesc(property);
                }
            }
        }

        /// <summary>
        /// analysis the dependency in sentenc, and then extract information
        /// https://nlp.stanford.edu/software/dependencies_manual.pdf
        /// </summary>
        private List<Pipline> ElementExtractByDependencyPrase(edu.stanford.nlp.util.CoreMap sentence)
        {
            //result properties
            List<Pipline> plines = new List<Pipline>();
            //1. 分析句子组成，即 主(NN)+谓(VV)
            List<edu.stanford.nlp.trees.TypedDependency> deps = FindDeptypeFromSentence(sentence, "nsubj");
            foreach(edu.stanford.nlp.trees.TypedDependency dep in deps)
            {
                //寻找修饰主语或者谓语的关系词
                edu.stanford.nlp.ling.IndexedWord sToken = dep.dep(), tToken = dep.gov();
                //1.当前情景片断主线已被解析出来，即 subject + action
                Pipunit sunit = new Pipunit((string)sToken.get(textAnnotationClass));
                Pipunit tunit = new Pipunit((string)tToken.get(textAnnotationClass));
                //2.构建流水线
                Pipline pline = new Pipline(sunit, tunit);
                //3. 分析主语相关修饰, 谓语相关修饰
                AnalysisSubjectDepsInSentence(sentence, sToken, sunit);
                AnalysisActionDepsInSentence(sentence, tToken, tunit);
                //4. 添加到 pipeline
                plines.Add(pline);
            }
            return plines;
            //逐token分析名字和其修饰
            //java.util.AbstractList mentions = sentence.get(mentionsAnnotationClass) as java.util.AbstractList;
            //_tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
            //
            //foreach (edu.stanford.nlp.util.CoreMap mention in mentions)
            //{
            //    string text = (string)mention.get(textAnnotationClass);
            //    string ner = (string)mention.get(namedEntityTagAnnotationClass);
            //    string noramlValue = (string)mention.get(normalizedNamedEntityTagAnnotationClass);
            //    //处理不同类型的ner，得到修饰依赖
            //    if (ner == "DATE")
            //    {
            //        //情景时间
            //    }
            //    else if (ner == "ORGNIZATION")
            //    {
            //        //备注信息
            //    }
            //    else if (ner == "NUMBER")
            //    {
            //        //寻找修饰单位
            //        var (targetEntity, targetAction, numericValue) = FindNumberDeps(dependencies, mention);
            //        //合并key-value, value 累加
            //        if (targetEntity != null) properties[targetEntity] = numericValue;
            //        if (targetAction != null) properties[targetAction] = numericValue;
            //    }
            //    else if (ner == "CITY")
            //    {
            //        //地点
            //    }
            //    else if (ner == "GPE")
            //    {

            //    }
            //}
        }

        /// <summary>
        /// find token by index
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        //private edu.stanford.nlp.ling.CoreLabel FindTokenByIdx(java.lang.Number idx)
        //{
        //    int target = idx.intValue();
        //    foreach (edu.stanford.nlp.ling.CoreLabel token in _tokens)
        //        if (target == token.index())
        //            return token;
        //    return null;
        //}

        /// <summary>
        /// 搜索与number相关的依赖：
        /// 1. 数字单位
        /// 2. target entity
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="targetWord">目标名词</param>
        private (string targetEntity, string targetAction, string numericValue) FindNumberDeps(edu.stanford.nlp.semgraph.SemanticGraph dependencies, edu.stanford.nlp.util.CoreMap mention)
        {
            string numericValue = null, targetEntity = null, targetAction = null;
            //方法1：根据cemidx寻找修饰关系，记录返回
            //java.lang.Number cemIdx = mention.get(canonicalEntityMentionIndexAnnotation) as java.lang.Number;
            //edu.stanford.nlp.ling.CoreLabel cemToken =FindTokenByIdx(cemIdx);
            //方法2：根据当前词idx, 寻找修饰关系, 获取mention里的token，得到关键词并寻找修饰关系
            string value = (string)mention.get(normalizedNamedEntityTagAnnotationClass);
            value = value ?? (string)mention.get(textAnnotationClass);
            java.util.AbstractList tokens = mention.get(tokensAnnotationClass) as java.util.AbstractList;
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            //处理entity里包含的tokens
            foreach (edu.stanford.nlp.ling.CoreLabel token in tokens)
            {
                //1 搜索与token相关的deps
                List<edu.stanford.nlp.trees.TypedDependency> deps = FindRefs(dependencies, token);
                //2 参考：Marie-Catherine de Marneffe, Christopher D. Manning ,Stanford typed dependencies manual, 2016
                //不限于：nn:noun compound modifier
                foreach (edu.stanford.nlp.trees.TypedDependency dep in deps)
                {
                    //2.1 查找数值修饰单位 (mark:clf)
                    if (dep.reln().getShortName() == "mark:clf")
                    {
                        string unitText = (string)dep.dep().get(textAnnotationClass);
                        numericValue = string.Format("{0} {1}", value, unitText);
                    }
                    //2.2 num修饰关系（名词修饰关系）
                    if (dep.reln().getShortName() == "nummod")
                    {
                        //2.2.1 得到被修饰词
                        edu.stanford.nlp.ling.CoreLabel nummodToken = dep.gov().backingLabel();
                        //2.2.2 找到 nsubj 修饰的主体subject名词(NN)
                        //2.2.3 compound修饰的主语
                        edu.stanford.nlp.ling.CoreLabel targetToken = FindTargetTokenByDependencytype(dependencies, nummodToken, "compound:nn");
                        if (targetToken != null)
                            targetEntity = targetToken != null ? (string)targetToken.get(textAnnotationClass) : null;
                        //2.2.4 acl修饰主语
                        if (targetToken == null)
                        {
                            targetToken = FindTargetTokenByDependencytype(dependencies, nummodToken, "acl");
                            if (targetToken != null) numericValue += (string)nummodToken.get(textAnnotationClass);
                            string poa = (string)targetToken.get(partOfSpeechAnnotationClass);
                            if (poa == "VV") targetAction = (string)targetToken.get(textAnnotationClass);
                        }
                    }
                    //2.3 num修饰关系（动词修饰关系）
                    if (dep.reln().getShortName() == "dep" && (string)dep.gov().backingLabel().get(partOfSpeechAnnotationClass) == "VV")
                    {
                        targetAction = (string)dep.gov().backingLabel().get(textAnnotationClass);
                    }
                    //2.4 num修饰名词
                    if (dep.reln().getShortName() == "dep" && (string)dep.gov().backingLabel().get(partOfSpeechAnnotationClass) == "NN")
                    {
                        numericValue = (string)dep.dep().backingLabel().get(textAnnotationClass) + (string)dep.gov().backingLabel().get(textAnnotationClass);
                        edu.stanford.nlp.ling.CoreLabel targetToken = FindTargetTokenByDependencytype(dependencies, dep.gov().backingLabel(), "nmod:prep");
                        string poa = (string)targetToken.get(partOfSpeechAnnotationClass);
                        if (poa == "VV") targetAction = (string)targetToken.get(textAnnotationClass);
                    }
                }
            }
            return (targetEntity, targetAction, numericValue);
        }

        /// <summary>
        /// FindTargetTokenByDependencytype(dep, token, "nsubj");
        /// </summary>
        /// <param name="depTypeString"></param>
        /// <returns></returns>
        private edu.stanford.nlp.ling.CoreLabel FindTargetTokenByDependencytype(edu.stanford.nlp.semgraph.SemanticGraph dependencies, edu.stanford.nlp.ling.CoreLabel token, string depTypeString)
        {
            List<edu.stanford.nlp.trees.TypedDependency> deps = FindRefs(dependencies, token);
            foreach (edu.stanford.nlp.trees.TypedDependency dep in deps)
                if (dep.reln().getShortName() == depTypeString)
                    return dep.gov().backingLabel() == token ? dep.dep().backingLabel() : dep.gov().backingLabel();
            return null;
        }

        /// <summary>
        /// 搜索与target token相关的dpendency关系集合
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private List<edu.stanford.nlp.trees.TypedDependency> FindRefs(edu.stanford.nlp.semgraph.SemanticGraph dependencies, edu.stanford.nlp.ling.CoreLabel token)
        {
            List<edu.stanford.nlp.trees.TypedDependency> tds = new List<edu.stanford.nlp.trees.TypedDependency>();
            string tokenValue = token.ToString();
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                string tdValue = td.toString();
                if (tdValue.IndexOf(tokenValue) != -1) tds.Add(td);
            }
            return tds;
        }

        /// <summary>
        /// 搜索token的直接关联关系
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private List<edu.stanford.nlp.trees.TypedDependency> FindDeirctRefs(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.CoreLabel token)
        {
            string tokenValue = token.ToString();
            List<edu.stanford.nlp.trees.TypedDependency> tds = new List<edu.stanford.nlp.trees.TypedDependency>();
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(basicDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                string tdValue = td.toString();
                if (tdValue.IndexOf(tokenValue) != -1) tds.Add(td);
            }
            return tds;
        }

        /// <summary>
        /// 因为句子并非要求语法上严格正确，所有经常出现错误的结果，但是结果不会变化
        /// </summary>
        /// <returns></returns>
        private List<edu.stanford.nlp.trees.TypedDependency> FindDeptypeFromSentence(edu.stanford.nlp.util.CoreMap sentence, string depTypeString)
        {
            List<edu.stanford.nlp.trees.TypedDependency> tds = new List<edu.stanford.nlp.trees.TypedDependency>();
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(basicDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                if(td.reln().getShortName()==depTypeString) tds.Add(td);
            }
            return tds;
        }

    }
}
