namespace Engine.Lexicon.Extend
{
    public static class StringExtend
    {
        /// <summary>
        /// 清洗文本
        /// 目前简单处理标点符号问题
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ClearPunctuation(this string text)
        {
            text = text.Replace(",", "")
                     .Replace("、", "")
                     .Replace("\t", "")
                     .Replace(" ", "")
                     .Replace("\tab", "")
                     .Replace("，", "")
                     .Replace("“", "")
                     .Replace("”", "")
                     .Replace(".", "")
                     .Replace("。", "")
                     .Replace("!", "")
                     .Replace("！", "")
                     .Replace("?", "")
                     .Replace("？", "")
                     .Replace(":", "")
                     .Replace("：", "")
                     .Replace(";", "")
                     .Replace("；", "")
                     .Replace("～", "")
                     .Replace("-", "")
                     .Replace("_", "")
                     .Replace("——", "")
                     .Replace("—", "")
                     .Replace("--", "")
                     .Replace("【", "")
                     .Replace("】", "")
                     .Replace("[", "")
                     .Replace("]", "")
                     .Replace("\\", "")
                     .Replace("(", "")
                     .Replace(")", "")
                     .Replace("（", "")
                     .Replace("）", "")
                     .Replace("#", "")
                     .Replace("$", "");
            return text;
        }
    }
}
