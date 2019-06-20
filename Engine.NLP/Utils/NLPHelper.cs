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
        public static string ProcessSentenceText(edu.stanford.nlp.util.CoreMap sentence)
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
        public static List<DateTime> ProcessTimex(edu.stanford.nlp.util.CoreMap sentence)
        {
            List<DateTime> times = new List<DateTime>();
            java.util.AbstractList mentions = ProcessSentenceMention(sentence);
            foreach (edu.stanford.nlp.util.CoreMap entity in mentions)
            {
                edu.stanford.nlp.time.Timex timex = entity.get(timexAnnotationClass) as edu.stanford.nlp.time.Timex;
                if (timex != null)
                {
                    DateTime time = ParseToDate(timex.altVal());
                    if (!times.Contains(time)&&time!=DateTime.MinValue) times.Add(time);
                }
            }
            return times;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>if error, return minVlaue of DateTime</returns>
        public static DateTime ParseToDate(string value)
        {
            string[] segments = value.Split(' ');
            for(int i = 0; i < segments.Length; i++)
            {
                string segment = segments[i];
                DateTime outTime;
                if (DateTime.TryParse(segment, out outTime))
                    return outTime;
            }
            return DateTime.MinValue;
        }

    }
}
