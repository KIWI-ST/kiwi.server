using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;

namespace Engine.NLP.Utils
{
    public class SentenceGroup
    {
        /// <summary>
        /// reference:
        /// https://stanfordnlp.github.io/CoreNLP/ssplit.html
        /// </summary>
        static string _boundaryTokenRegex = "[.。]|[!?！？]+";

        string _rawText;

        string[] _sentences;

        Dictionary<string, List<string>> _dict = new Dictionary<string, List<string>>();

        public SentenceGroup(string rawText)
        {
            _rawText = rawText;
            _sentences = Regex.Split(rawText, _boundaryTokenRegex);
        }

        public void RegroupByTimeline()
        {
            //清理dict信息
            _dict.Clear();
            List<ModelResult> timeline = DateTimeRecognizer.RecognizeDateTime(_rawText, Culture.English);
            // analysis sentences point, get start point and end point 
            ModelResult time = timeline.First();
            string timex = GetTimexString(time);
            int idx = time.Start;
            for (int i = 1; i < timeline.Count; i++)
            {
                time = timeline[i];
                var (start, end) = FixPosition(time.Start);
                //idx - start 区间是记录上一次timex的句子集
                List<string> sentences = GetStentencesByPositionRange(idx, start);
                _dict.Add(timex, sentences);
                //更新timex index的信息
                timex = GetTimexString(time);
                idx = end;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<string> GetStentencesByPositionRange(int start, int end)
        {
            int idx = 0;
            List<string> sentences = new List<string>();
            Array.ForEach(_sentences, (sentence) => {
                idx += sentence.Length;
                if (idx >= start && idx<=end)
                    sentences.Add(sentence);
            });
            return sentences;
        }

        /// <summary>
        /// 重新定位位置，返回所在句子的头和尾
        /// </summary>
        /// <param name="start">the position of timestring occurs in the context</param>
        /// <param name="end"></param>
        /// <returns></returns>
        private (int start, int end) FixPosition(int positionIndex)
        {
            int start = 0, end = 0;
            for(int i = 0; i < _sentences.Length; i++)
            {
                end += _sentences[i].Length;
                if (end > positionIndex)
                    break;
                start = end;
            }
            return (start, end);
        }

        private string GetTimexString(ModelResult timeResult)
        {
            string timeString = "";
            foreach(var res in timeResult.Resolution.Values)
            {
                List<Dictionary<string, string>> resDict = res as List<Dictionary<string, string>>;
                Dictionary<string, string> timeObject = resDict[0];
                if (timeObject["type"] == "daterange")
                    return timeObject["start"];
                else if (timeObject["type"] == "duration")
                    return timeObject["timex"] + timeObject["value"];
                else if (timeObject["type"] == "time")
                    return timeObject["timex"] + timeObject["value"];
                else if (timeObject["type"] == "date")
                    return timeObject["value"];
                else
                    return timeObject["timex"];
            }
            return timeString;
        }

    }
}
