using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Word.Entity
{
    /// <summary>
    /// 构建halman树
    /// 
    /// </summary>
    public class HalfmanTree<T> where T:new()
    {
        /// <summary>
        /// 待构建halfman树的数据集合
        /// </summary>
       public T[] RawNodeCollection { get; set; }

        public HalfmanTree()
        {
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class VocabularyHalfmanTree : HalfmanTree<Vocabulary>
    {
        /// <summary>
        /// max length represent every code
        /// </summary>
        int MaxCodeLength;
        /// <summary>
        /// the vocabulary that acutally used
        /// </summary>
        int VocabularySize;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCodeLength"></param>
        public VocabularyHalfmanTree(int maxCodeLength,int vocabularySize)
        {
            MaxCodeLength = maxCodeLength;
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="voc"></param>
        public void Concat(Vocabulary voc)
        {

        }
    }

}

//    long pos1;
//    long pos2;
//    var code = new char[MaxCodeLength];
//    var point = new long[MaxCodeLength];
//    var count = new long[_vocabSize * 2 + 1];
//    var binary = new long[_vocabSize * 2 + 1];
//    var parentNode = new int[_vocabSize * 2 + 1];

//            for (var a = 0; a<_vocabSize; a++) count[a] = _vocab[a].Cn;
//            for (var a = _vocabSize; a<_vocabSize* 2; a++) count[a] = (long)1e15;
//            pos1 = _vocabSize - 1;
//            pos2 = _vocabSize;
//            // Following algorithm constructs the Huffman tree by adding one node at a time
//            for (var a = 0; a<_vocabSize - 1; a++)
//            {
//                // First, find two smallest nodes 'min1, min2'
//                long min1i;
//                if (pos1 >= 0)
//                {
//                    if (count[pos1] < count[pos2])
//                    {
//                        min1i = pos1;
//                        pos1--;
//                    }
//                    else
//                    {
//                        min1i = pos2;
//                        pos2++;
//                    }
//                }
//                else
//                {
//                    min1i = pos2;
//                    pos2++;
//                }
//                long min2i;
//                if (pos1 >= 0)
//                {
//                    if (count[pos1] < count[pos2])
//                    {
//                        min2i = pos1;
//                        pos1--;
//                    }
//                    else
//                    {
//                        min2i = pos2;
//                        pos2++;
//                    }
//                }
//                else
//                {
//                    min2i = pos2;
//                    pos2++;
//                }
//                count[_vocabSize + a] = count[min1i] + count[min2i];
//                parentNode[min1i] = _vocabSize + a;
//                parentNode[min2i] = _vocabSize + a;
//                binary[min2i] = 1;
//            }
//            // Now assign binary code to each vocabulary word
//            for (long a = 0; a<_vocabSize; a++)
//            {
//                var b = a;
//long i = 0;
//                while (true)
//                {
//                    code[i] = (char) binary[b];
//point[i] = b;
//                    i++;
//                    b = parentNode[b];
//                    if (b == _vocabSize* 2 - 2) break;
//                }
//                _vocab[a].CodeLen = (int) i;
//_vocab[a].Point[0] = _vocabSize - 2;
//                for (b = 0; b<i; b++)
//                {
//                    _vocab[a].Code[i - b - 1] = code[b];
//                    _vocab[a].Point[i - b] = (int) (point[b] - _vocabSize);
//                }
//            }