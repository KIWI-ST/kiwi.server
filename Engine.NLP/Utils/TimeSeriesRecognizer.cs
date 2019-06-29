using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;

namespace Engine.NLP.Utils
{
    public class TimeSeriesRecognizer
    {
        /// <summary>
        /// 基于识别的时间实体重组句子集合
        /// </summary>
        public static void RegroupSentenceByTimeline(string rawText)
        {
            //1. recognize overall time
            List<ModelResult> timeline = DateTimeRecognizer.RecognizeDateTime(rawText, Culture.English);
            //2. split into sentences
            string[] sentences = SentenceRecognizer.Split(rawText);
            //3. regroup by time expression
            Array.ForEach(sentences, (sentence) => {
                DateTimeRecognizer.RecognizeDateTime(sentence, Culture.English);

            });
        }
    }
}
