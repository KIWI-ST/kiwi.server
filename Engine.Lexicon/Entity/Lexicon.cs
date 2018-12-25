using Engine.Lexicon.Extend;
using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engine.Lexicon.Entity
{
    /// <summary>
    /// 编码方式
    /// </summary>
    public enum EncodeScheme
    {
        /// <summary>
        /// halfman方式，不等长
        /// </summary>
        Halfman = 1,
        /// <summary>
        /// onehot，等长
        /// </summary>
        Onehot = 2
    }


    /// <summary>
    /// 词库
    /// </summary>
    public class Lexicon
    {
        /// <summary>
        /// 单词字母字符串长度上限
        /// </summary>
        public int MAX_CODE_LENGTH { get; private set; } = 40;

        /// <summary>
        /// 设置最低词频，对最低词频的此在sort后予以剔除
        /// sort操作在
        /// 1. reduce lexicon后发生
        /// 2. 初次学习raw数据后，统计完全部词库后发生
        /// </summary>
        int _min_frequent = 1;

        /// <summary>
        /// vocabulary size of lexicon
        /// </summary>
        public int VocaSize { get; private set; } = 0;

        /// <summary>
        /// 记录已经处理过的次总数（用于debug）
        /// </summary>
        long _train_word_count = 0;

        /// <summary>
        /// vocabulary hash size of lexicon
        /// </summary>
        const int _voca_hash_size = 30000000;

        /// <summary>
        /// 默认最大的词汇量
        /// </summary>
        readonly int _voca_max_size = 3000;

        /// <summary>
        /// 
        /// </summary>
        int[] _voca_hash_array;

        /// <summary>
        /// vocabulary 
        /// </summary>
        Vocabulary[] _voca_array;

        /// <summary>
        /// 
        /// </summary>
        public Vocabulary[] VocaArray { get { return _voca_array; } }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, int> DictIndex { get; private set; } = new Dictionary<string, int>();

        /// <summary>
        /// 使用结巴分词
        /// </summary>
        JiebaSegmenter _segmenter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segmenter"></param>
        public Lexicon(JiebaSegmenter segmenter)
        {
            //分词器
            _segmenter = segmenter;
            //初始化值为-1的hash数组
            _voca_hash_array = Enumerable.Repeat(-1, _voca_hash_size).ToArray();
            //初始化上限为 max size 的词汇数组
            _voca_array = new Vocabulary[_voca_max_size];
            for (int i = 0; i < _voca_max_size; i++) _voca_array[i] = new Vocabulary();
        }

        /// <summary>
        /// 拆分句子
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public string[] Sgement(string sentence)
        {
            return _segmenter.Cut(sentence).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private uint TranslateWordHash(string word)
        {
            int a;
            ulong hash = 0;
            for (a = 0; a < word.Length; a++) hash = hash * 257 + word[a];
            hash = hash % _voca_hash_size;
            return (uint)hash;
        }

        /// <summary>
        /// 返回新增的单词在字典的索引（自增量编号）
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        int AddVocabulary(string word)
        {
            _voca_array[VocaSize].Word = word;
            _voca_array[VocaSize].Weight = 0;
            VocaSize++;
            //计算hash并返回索引
            uint hash = TranslateWordHash(word);
            while (_voca_hash_array[hash] != -1)
                hash = (hash + 1) % _voca_hash_size;
            _voca_hash_array[hash] = VocaSize - 1;
            return VocaSize - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        int SearchVocabulary(string word)
        {
            uint hash = TranslateWordHash(word);
            while (true)
            {
                if (_voca_hash_array[hash] == -1) return -1;
                if (word.Equals(_voca_array[_voca_hash_array[hash]].Word)) return _voca_hash_array[hash];
                hash = (hash + 1) % _voca_hash_size;
            }
        }

        void ReduceVocabulary()
        {

        }

        void SortVocabulary()
        {
            //sort vocabulary array
            Array.Sort(_voca_array, 0, VocaSize - 1);
            //set hash array with default -1 
            _voca_hash_array = Enumerable.Repeat(-1, _voca_hash_size).ToArray();
            int size = VocaSize;
            _train_word_count = 0;
            //对预词库此进行处理，如果词频小于预设最小词频，剔除词
            for (int a = 0; a < size; a++)
            {
                if (_voca_array[a].Weight < _min_frequent && (a != 0))
                {
                    VocaSize--;
                    _voca_array[a].Word = null;
                }
                else
                {
                    //重新计算hash
                    uint hash = TranslateWordHash(_voca_array[a].Word);
                    while (_voca_hash_array[hash] != -1)
                        hash = (hash + 1) % _voca_hash_size;
                    _voca_hash_array[hash] = a;
                    _train_word_count += _voca_array[a].Weight;
                }
            }
            Array.Resize(ref _voca_array, VocaSize);
            //
            for (int a = 0; a < VocaSize; a++)
            {
                _voca_array[a].Code = new char[MAX_CODE_LENGTH];
                _voca_array[a].Point = new int[MAX_CODE_LENGTH];
            }
        }

        /// <summary>
        /// 保存统计后的字典文件
        /// </summary>
        /// <param name="lexiconFullFilename"></param>
        public void SaveLexiconFile(string lexiconFullFilename)
        {
            using (var stream = new FileStream(lexiconFullFilename, FileMode.OpenOrCreate))
            using (var streamWriter = new StreamWriter(stream))
                for (var i = 0; i < VocaSize; i++)
                    streamWriter.WriteLine("{0} {1}", _voca_array[i].Word, _voca_array[i].Weight);
        }

        /// <summary>
        /// 执行halfman编码
        /// </summary>
        public void UpdateHalfmanCode()
        {
            VocabularyHalfmanTree halfmanTree = new VocabularyHalfmanTree(this);
            halfmanTree.BuildOrUpdate();
        }

        /// <summary>
        /// 执行onehot编码
        /// </summary>
        public void UpdateOnehotCode()
        {
            OnehotEncode onehotEncode = new OnehotEncode(this);
            onehotEncode.BuildOrUpdate();
        }

        /// <summary>
        /// reference:
        /// https://blog.csdn.net/qq1483661204/article/details/78975847
        /// </summary>
        public void UpdateSubsample()
        {

        }

        #region 构建方法

        /// <summary>
        /// 从已分析存储的辞典载入数据
        /// </summary>
        public static Lexicon FromExistLexiconFile(string existLexiconFile)
        {
            JiebaSegmenter segmenter = new JiebaSegmenter();
            Lexicon lexicon = new Lexicon(segmenter);
            using (StreamReader sr = new StreamReader(existLexiconFile))
            {
                Regex regex = new Regex("\\s");
                string line;
                while (!string.IsNullOrEmpty((line = sr.ReadLine().ClearPunctuation())))
                {
                    string[] vals = regex.Split(line);
                    if (vals.Length == 2)
                    {
                        var a = lexicon.AddVocabulary(vals[0]);
                        lexicon._voca_array[a].Weight = int.Parse(vals[1]);
                    }
                }
                //
                lexicon.SortVocabulary();
            }
            lexicon.UpdateHalfmanCode();
            return lexicon;
        }

        /// <summary>
        /// 从原始文本文件中分析词句
        /// </summary>
        /// <param name="vocabularyFile"></param>
        /// <returns></returns>
        public static Lexicon FromVocabularyFile(string vocabularyFile, EncodeScheme encode = EncodeScheme.Halfman)
        {
            JiebaSegmenter segmenter = new JiebaSegmenter();
            Lexicon lexicon = new Lexicon(segmenter);
            //读取文本构建词库
            using (StreamReader sr = new StreamReader(vocabularyFile))
            {
                string line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().ClearPunctuation();
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                        continue;
                    //sgement
                    string[] words = lexicon.Sgement(line);
                    Array.ForEach(words, word =>
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            lexicon._train_word_count++;
                            int i = lexicon.SearchVocabulary(word);
                            if (i == -1)
                                lexicon._voca_array[lexicon.AddVocabulary(word)].Weight = 1;
                            else
                                lexicon._voca_array[i].Weight++;
                            if (lexicon.VocaSize > _voca_hash_size * 0.7)
                                lexicon.ReduceVocabulary();
                        }
                    });
                }
                //1. 剔除达不到最低频次要求的词
                //2. 排序
                lexicon.SortVocabulary();
            }
            //应用halfman编码
            if (encode == EncodeScheme.Halfman)
                lexicon.UpdateHalfmanCode();
            //应用one-hot编码
            else if (encode == EncodeScheme.Onehot)
                lexicon.UpdateOnehotCode();
            //
            return lexicon;
        }

        #endregion
    }
}
