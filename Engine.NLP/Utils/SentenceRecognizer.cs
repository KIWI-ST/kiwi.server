using System.Text.RegularExpressions;

namespace Engine.NLP.Utils
{
    public class SentenceRecognizer
    {
        /// <summary>
        /// reference:
        /// https://stanfordnlp.github.io/CoreNLP/ssplit.html
        /// </summary>
        static string _boundaryTokenRegex = "[.。]|[!?！？]+";

        /// <summary>
        /// ssplit (refercen stanford nlp)
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns></returns>
        public static string[] Split(string rawText)
        {
            return Regex.Split(rawText, _boundaryTokenRegex);
        }
    }
}
