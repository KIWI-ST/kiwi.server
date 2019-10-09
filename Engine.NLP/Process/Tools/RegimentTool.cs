using System.Collections.Generic;
using Baidu.Aip.Nlp;
using Engine.NLP.Utils;
using Newtonsoft.Json.Linq;

namespace Engine.NLP.Process.Tools
{

    public class LexerItem
    {
        public int byte_length { get; set; }
        public int byte_offset { get; set; }
        public string formal { get; set; }
        public string item { get; set; }
        public string ne { get; set; }
        public string pos { get; set; }
        public string uri { get; set; }
        public List<string> loc_details { get; set; }
        public List<string> basic_words { get; set; }
    }

    /// <summary>
    /// 词法分析返回对象
    /// </summary>
    public class Response
    {
        public int status { get; set; }
        public string version { get; set; }
        public List<LexerItem> results { get; set; }
    }

    /// <summary>
    /// 重组合原始预料，根据时间 (datetime.v2）规则重组预料
    /// </summary>
    public class RegimentTool : IRegimentTool
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Nlp _client;

        /// <summary>
        /// 
        /// </summary>
        public RegimentTool()
        {
            _client = new Nlp(NLPConfiguration.BaiduAIAPIKey, NLPConfiguration.BaiduAISecretKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawText"></param>
        public void RegimentTextByTimeline(string rawText)
        {
            //词法分析
            JObject result = _client.Lexer(rawText);
            //返回内容为results部分，包含 : log_id/text/items
            foreach (JProperty item in result.Children())
            {
                //https://ai.baidu.com/docs#/NLP-Csharp-SDK/e31833ea
                //处理items的问题
                if (item.Name == "items")
                {
                    JToken values = item.Value;
                    //analysis
                    ProcessLexer(values);
                }
            }
            //

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lexers"></param>
        private void ProcessLexer(JToken lexers)
        {
            foreach(JToken element in lexers)
            {

            }
        }

    }
}
