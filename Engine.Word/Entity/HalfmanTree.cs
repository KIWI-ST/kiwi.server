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
            Initialization();
        }

        /// <summary>
        /// 
        /// </summary>
        void Initialization()
        {
            int i;
            ChainHalfmanNode l, p1, p2;
            HalfmanNode hnew, h, h1, h2;
            //创建seed结点
            l = new ChainHalfmanNode();
            //
            for(i=0;i<VocaSize;i++)
            {
                hnew = new HalfmanNode();
                hnew.Weight = VocaArray[i].Weight;
            }
        


        }

        /// <summary>
        /// 二叉树结点按权值从小到大的顺序挂在一颗树上
        /// </summary>
        /// <param name="l">树根结点或者树</param>
        /// <param name="ht">新增结点</param>
        void OrderWeight(ChainHalfmanNode l,HalfmanNode ht)
        {

        }


    }
}