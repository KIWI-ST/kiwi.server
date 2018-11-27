using System;

namespace Engine.Word.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Vocabulary : IComparable<Vocabulary>
    {
        /// <summary>
        /// 当前单词在语料库中统计出现的频次
        /// </summary>
        public long Frequent { get; set; }
        /// <summary>
        /// 单词
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 最长单词40个char组成
        /// </summary>
        public char[] Code { get; set; }
        /// <summary>
        /// 单词长度
        /// </summary>
        public int CodeLen { get; set; }
        /// <summary>
        /// 单词位置
        /// </summary>
        public int[] Point { get; set; }

        public int CompareTo(Vocabulary o)
        {
            return (int)(o.Frequent - Frequent);
        }

    }
}
