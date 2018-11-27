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
    /// </summary>
    public class VocabularyHalfmanTree : HalfmanTree<Vocabulary>
    {

        Lexicon _lexicon;

        int MaxCodeLength;

        int VocaSize;

        Vocabulary[] VocaArray;

        public VocabularyHalfmanTree(Lexicon lexicon)
        {
            _lexicon = lexicon;
            //
            MaxCodeLength = lexicon.MAX_CODE_LENGTH;
            VocaSize = lexicon.VocaSize;
            VocaArray = lexicon.VocaArray;
            BuildHalfmanTree();
        }

        void BuildHalfmanTree()
        {
            long pos1;
            long pos2;
            char[] code = new char[MaxCodeLength];
            long[] point = new long[MaxCodeLength];
            long[] count = new long[VocaSize * 2 + 1];
            long[] binary = new long[VocaSize * 2 + 1];
            int[] parentNode = new int[VocaSize * 2 + 1];
            for (int a = 0; a < VocaSize; a++)
                count[a] = VocaArray[a].Frequent;
            for (int a = 0; a < VocaSize * 2; a++)
                count[a] = (long)1e15;
            pos1 = VocaSize - 1;
            pos2 = VocaSize;
            //构建holfman树
            for(int a = 0; a < VocaSize - 1; a++)
            {
                long min1i;
                if (pos1 >= 0)
                {
                    if (count[pos1] < count[pos2])
                    {
                        min1i = pos1;
                        pos1--;
                    }
                    else
                    {
                        min1i = pos2;
                        pos2++;
                    }
                }
                else
                {
                    min1i = pos2;
                    pos2++;
                }
                long min2i;
                if (pos1 >= 0)
                {
                    if (count[pos1] < count[pos2])
                    {
                        min2i = pos1;
                        pos1--;
                    }
                    else
                    {
                        min2i = pos2;
                        pos2++;
                    }
                }
                else
                {
                    min2i = pos2;
                    pos2++;
                }
                count[VocaSize + a] = count[min1i] + count[min2i];
                parentNode[min1i] = VocaSize + a;
                parentNode[min2i] = VocaSize + a;
                binary[min2i] = 1;
            }
            //holfman 编码
            for (long a = 0; a < VocaSize; a++)
            {
                var b = a;
                long i = 0;
                while (true)
                {
                    code[i] = (char)binary[b];
                    point[i] = b;
                    i++;
                    b = parentNode[b];
                    if (b == VocaSize * 2 - 2) break;
                }
                VocaArray[a].CodeLen = (int)i;
                VocaArray[a].Point[0] = VocaSize - 2;
                for (b = 0; b < i; b++)
                {
                    VocaArray[a].Code[i - b - 1] = code[b];
                    VocaArray[a].Point[i - b] = (int)(point[b] - VocaSize);
                }
            }
            //
        }
    }
}