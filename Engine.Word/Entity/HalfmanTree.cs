using System.Collections.Generic;
using System.Linq;

namespace Engine.Word.Entity
{

    /// <summary>
    /// 构建halman树
    /// 
    /// </summary>
    public class HalfmanTree<T> where T : new()
    {
        /// <summary>
        /// 待构建halfman树的数据集合
        /// </summary>
        protected T[] RawNodeCollection { get; set; }

        public HalfmanTree()
        {
        }

    }

    /// <summary>
    /// vocabulary halfman tree
    /// reference:
    /// https://blog.csdn.net/fisherwan/article/details/23123041
    /// </summary>
    /// 
    public class VocabularyHalfmanTree : HalfmanTree<Vocabulary>
    {

        Lexicon _lexicon;

        int MaxCodeLength;

        int VocaSize;

        Vocabulary[] VocaArray;

        Vocabulary RootVocabulary;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lexicon"></param>
        public VocabularyHalfmanTree(Lexicon lexicon)
        {
            _lexicon = lexicon;
            //
            MaxCodeLength = lexicon.MAX_CODE_LENGTH;
            VocaSize = lexicon.VocaSize;
            VocaArray = lexicon.VocaArray;
            //
            RootVocabulary = InitializationHalfmanTree();
            InitializationHalfmanCode();
        }

        /// <summary>
        /// 
        /// </summary>
        void InitializationHalfmanCode()
        {
            foreach(var vocabulary in VocaArray)
                HalfmanCode(vocabulary);
        }

        /// <summary>
        /// 
        /// </summary>
        Vocabulary InitializationHalfmanTree()
        {
            int i;
            VocabularyChain l, p1, p2;
            Vocabulary  h, h1, h2;
            //创建seed结点
            l = new VocabularyChain();
            //
            for (i = 0; i < VocaSize; i++)
            {
                OrderWeight(l, VocaArray[i]);
                //hnew = new Vocabulary();
                //hnew.Weight = VocaArray[i].Weight;
                //OrderWeight(l, hnew);
            }
            //处理二叉树结点超过两层的情况
            while (l.NextVocabulary.NextVocabulary != null)
            {
                p1 = l.NextVocabulary;
                p2 = p1.NextVocabulary;
                l.NextVocabulary = p2.NextVocabulary;
                h1 = p1.RootVocabulary;
                h2 = p2.RootVocabulary;
                h = new Vocabulary();
                h.Weight = h1.Weight + h2.Weight;
                h.LeftChild = h1;
                h.RightChild = h2;
                OrderWeight(l, h);
            }
            //
            p1 = l.NextVocabulary;
            h = p1.RootVocabulary;
            return h;
        }

        /// <summary>
        /// 二叉树结点按权值从小到大的顺序挂在一颗树上
        /// </summary>
        /// <param name="l">树根结点或者树</param>
        /// <param name="ht">新增结点</param>
        void OrderWeight(VocabularyChain l, Vocabulary ht)
        {
            VocabularyChain ltmp, lnew, lp;
            lnew = new VocabularyChain();
            lnew.RootVocabulary = ht;
            //
            lp = l;
            ltmp = lp.NextVocabulary;
            //
            while (ltmp != null)
            {
                if (lnew.RootVocabulary.Weight > ltmp.RootVocabulary.Weight)
                {
                    ltmp = ltmp.NextVocabulary;
                    lp = lp.NextVocabulary;
                }
                else
                {
                    ltmp = null;
                }
            }
            lnew.NextVocabulary = lp.NextVocabulary;
            lp.NextVocabulary = lnew;
        }

        public void HalfmanCode(Vocabulary vocabulary)
        {
            List<int> code = new List<int>();
            Vocabulary templeVocabulary = vocabulary;
            //temple vocabulary
            Vocabulary parent = templeVocabulary.Parent;
            //假定二叉树的最大深度是 MaxCodeLength
            while (parent != null)
            {
                if (parent.LeftChild == templeVocabulary)
                    code.Add(0);
                else
                    code.Add(1);
                templeVocabulary = parent;
                parent = templeVocabulary.Parent;
            }
            code.Reverse();
            vocabulary.Point = code.ToArray();
        }

    }
}