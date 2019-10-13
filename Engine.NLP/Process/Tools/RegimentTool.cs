using System;
using System.Collections.Generic;
using System.Reflection;
using Baidu.Aip.Nlp;
using Engine.NLP.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Engine.NLP.Process.Tools
{

    /// <summary>
    /// 
    /// </summary>
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
                //处理items的（结果）
                if (item.Name == "items")
                {
                    //analysis
                    List<LexerItem> items =  ProcessBaiduAILexer(item.Value);
                }
            }
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lexers"></param>
        private List<LexerItem> ProcessBaiduAILexer(JToken lexers)
        {
            //返回词法分析结果属性
            string[] tokens = new string[] {
            "byte_length",
            "byte_offset",
            "formal",
            "item",
            "ne",
            "pos",
            "uri",
            "loc_details",
            "basic_words",
            };
            //反馈items
            List<LexerItem> items = new List<LexerItem>();
            //序列化构造对象
            foreach(JToken token in lexers)
            {
                string lexerString = JsonConvert.SerializeObject(token);
                LexerItem item = JsonConvert.DeserializeObject<LexerItem>(lexerString);
                items.Add(item);
            }
            //返回结果
            return items;
        }

    }
}

//  foreach (string name in tokens)
//  {
//          string value = NLPHelper.JTokenSelectNode(token, name);
//          PropertyInfo property = item.GetType().GetProperty(name);
//          var s = JsonConvert.DeserializeObject(value);
//          item.GetType().GetProperty(name).SetValue(item, value);
//  }