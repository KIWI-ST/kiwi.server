using System;

namespace Engine.Word.Entity
{
    /// <summary>
    /// halfman编码结构
    /// </summary>
    public class HalfmanNode
    {
        /// <summary>
        /// 结点文字
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 节点权重
        /// </summary>
        public long Weight { get; set; }
        /// <summary>
        /// 子结点-左
        /// </summary>
        HalfmanNode Left { get; set; }
        /// <summary>
        /// 子结点-右
        /// </summary>
        HalfmanNode Right { get; set; }
    }

    /// <summary>
    /// 链表数据结构
    /// </summary>
    public class ChainHalfmanNode
    {
        /// <summary>
        /// 
        /// </summary>
        public HalfmanNode RootNode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ChainHalfmanNode NextNode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Vocabulary : HalfmanNode, IComparable<Vocabulary>
    {

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(Vocabulary o)
        {
            return (int)(o.Weight - Weight);
        }

    }
}
