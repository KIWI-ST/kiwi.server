using Baidu.Aip.Nlp;
using Engine.NLP.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Engine.NLP.Process.Tools
{

    /// <summary>
    /// 
    /// </summary>
    public class LexerItem
    {
        /// <summary>
        /// 字节级length（使用GBK编码）
        /// </summary>
        public int byte_length { get; set; }
        /// <summary>
        /// 在text中的字节级offset（使用GBK编码）
        /// </summary>
        public int byte_offset { get; set; }
        /// <summary>
        /// 词汇的标准化表达，主要针对时间、数字单位，没有归一化表达的，此项为空串
        /// </summary>
        public string formal { get; set; }
        /// <summary>
        /// 词汇的字符串
        /// </summary>
        public string item { get; set; }
        /// <summary>
        /// 命名实体类型，命名实体识别算法使用。词性标注算法中，此项为空串
        /// </summary>
        public string ne { get; set; }
        /// <summary>
        /// 词性，词性标注算法使用。命名实体识别算法中，此项为空串
        /// </summary>
        public string pos { get; set; }
        /// <summary>
        /// 链指到知识库的URI，只对命名实体有效。对于非命名实体和链接不到知识库的命名实体，此项为空串
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// 地址成分，非必需，仅对地址型命名实体有效，没有地址成分的，此项为空数组。
        /// </summary>
        public List<string> loc_details { get; set; }
        /// <summary>
        /// 基本词成分
        /// </summary>
        public List<string> basic_words { get; set; }
    }

    /// <summary>
    /// 重组合原始预料，根据时间 (datetime.v2）规则重组预料
    /// </summary>
    public class RegimentTool
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
                //处理items的（结果）https://ai.baidu.com/docs#/NLP-Csharp-SDK/e31833ea
                if (item.Name == "items")
                {
                    List<LexerItem> items =  ProcessBaiduAILexer(item.Value);
                    //根据items中的TIME，重组对象
                    List<LexerItem> timexItems = items.Where(e => e.ne == "TIME").ToList();
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

//string[] tokens = new string[] {
//            "byte_length",
//            "byte_offset",
//            "formal",
//            "item",
//            "ne",
//            "pos",
//            "uri",
//            "loc_details",
//            "basic_words",
//            };

//  foreach (string name in tokens)
//  {
//          string value = NLPHelper.JTokenSelectNode(token, name);
//          PropertyInfo property = item.GetType().GetProperty(name);
//          var s = JsonConvert.DeserializeObject(value);
//          item.GetType().GetProperty(name).SetValue(item, value);
//  }