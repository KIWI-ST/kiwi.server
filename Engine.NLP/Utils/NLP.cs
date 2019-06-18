using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.NLP.Utils
{
    /// <summary>
    /// helper 类，用于：
    /// 1. 句子分析
    /// 2. 词性分析
    /// 3. 依赖分析
    /// </summary>
    public class NLPHelper
    {
        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class textAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.TextAnnotation().getClass();

        /// <summary>
        /// 句子annotator 入口
        /// </summary>
        readonly static java.lang.Class mentionsAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.MentionsAnnotation().getClass();

        /// <summary>
        /// https://nlp.stanford.edu/software/sutime.shtml
        /// </summary>
        readonly static java.lang.Class timexAnnotationClass = new edu.stanford.nlp.time.TimeAnnotations.TimexAnnotation().getClass();

        /// <summary>
        /// only prcess sentence to return text
        /// </summary>
        private static string ProcessSentenceText(edu.stanford.nlp.util.CoreMap sentence)
        {
            return sentence.get(textAnnotationClass) as string;
        }

        /// <summary>
        /// 处理sentence，得到mention, 内部用法
        /// </summary>
        public static java.util.AbstractList ProcessSentenceMention(edu.stanford.nlp.util.CoreMap sentence)
        {
            return sentence.get(mentionsAnnotationClass) as java.util.AbstractList;
        }

        /// <summary>
        /// process timex
        /// </summary>
        public static List<edu.stanford.nlp.time.Timex> ProcessTimex(edu.stanford.nlp.util.CoreMap sentence)
        {
            List<edu.stanford.nlp.time.Timex> times = new List<edu.stanford.nlp.time.Timex>();
            java.util.AbstractList mentions = ProcessSentenceMention(sentence);
            foreach (edu.stanford.nlp.util.CoreMap entity in mentions)
            {
                edu.stanford.nlp.time.Timex time = entity.get(timexAnnotationClass) as edu.stanford.nlp.time.Timex;
                if (time != null) times.Add(time);
            }
            return times == null ? times : null;
        }

    }
}
