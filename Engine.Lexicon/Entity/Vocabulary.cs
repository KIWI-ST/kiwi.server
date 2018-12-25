using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Lexicon.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Vocabulary : IComparable<Vocabulary>
    {
        /// <summary>
        /// 
        /// </summary>
        Vocabulary _leftChild;

        /// <summary>
        /// 
        /// </summary>
        Vocabulary _rightChild;

        /// <summary>
        /// 父结点
        /// </summary>
        public Vocabulary Parent { get; private set; }

        /// <summary>
        /// 子结点-左
        /// </summary>
        public Vocabulary LeftChild
        {
            get { return _leftChild; }
            set
            {
                _leftChild = value;
                _leftChild.Parent = this;
            }
        }

        /// <summary>
        /// 子结点-右
        /// </summary>
        public Vocabulary RightChild
        {
            get { return _rightChild; }
            set
            {
                _rightChild = value;
                _rightChild.Parent = this;
            }
        }

        /// <summary>
        /// 结点文字
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 节点权重
        /// </summary>
        public long Weight { get; set; }

        /// <summary>
        /// 最长单词40个char组成
        /// </summary>
        public char[] Code { get; set; }

        /// <summary>
        /// 单词长度
        /// </summary>
        public int CodeLen { get; set; }

        /// <summary>
        /// 单词位置,即编码
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

    /// <summary>
    /// 链表数据结构
    /// </summary>
    public class VocabularyChain
    {
        /// <summary>
        /// 根词汇
        /// </summary>
        public Vocabulary RootVocabulary { get; set; }
        /// <summary>
        /// 链式词汇
        /// </summary>
        public VocabularyChain NextVocabulary { get; set; }
    }
}
