using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engine.Word.Entity
{
    /// <summary>
    /// 词库
    /// </summary>
    public class Lexicon
    {
        /// <summary>
        /// 单词字母字符串长度上限
        /// </summary>
        int _max_code_length = 40;

        /// <summary>
        /// 设置最低词频，对最低词频的此在sort后予以剔除
        /// sort操作在
        /// 1. reduce lexicon后发生
        /// 2. 初次学习raw数据后，统计完全部词库后发生
        /// </summary>
        int _min_frequent = 3;

        /// <summary>
        /// vocabulary size of lexicon
        /// </summary>
        int _voca_size = 0;

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
        int _voca_max_size = 1000;

        /// <summary>
        /// 
        /// </summary>
        int[] _voca_hash_array;

        /// <summary>
        /// vocabulary 
        /// </summary>
        Vocabulary[] _voca_array;

        public Lexicon()
        {
            //初始化值为-1的hash数组
            _voca_hash_array = Enumerable.Repeat(-1, _voca_hash_size).ToArray();
            //初始化上限为 max size 的词汇数组
            _voca_array = Enumerable.Repeat(new Vocabulary(), _voca_hash_size).ToArray();
        }

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
            _voca_array[_voca_size].Word = word;
            _voca_array[_voca_size].Frequent = 0;
            _voca_size++;
            //如果词汇超过预设上限，需要重新构造词汇数组
            if (_voca_size + 2 > _voca_max_size)
            {
                _voca_max_size += 1000;
                Array.Resize(ref _voca_array, _voca_max_size);
            }
            //计算hash并返回索引
            uint hash = TranslateWordHash(word);
            while (_voca_hash_array[hash] != -1)
                hash = (hash + 1) % _voca_hash_size;
            _voca_hash_array[hash] = _voca_size - 1;
            return _voca_size - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        int SearchVocabulary(string word)
        {
            uint hash = TranslateWordHash(word);
            while(true)
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
            Array.Sort(_voca_hash_array, 1, _voca_size - 1);
            _voca_hash_array = Enumerable.Repeat(-1, _voca_hash_size).ToArray();
            int size = _voca_size;
            _train_word_count = 0;
            //对预词库此进行处理，如果词频小于预设最小词频，剔除词
            for(int a = 0; a < size; a++)
            {
                if (_voca_array[a].Frequent < _min_frequent && (a != 0))
                {
                    _voca_size--;
                    _voca_array[a].Word = null;
                }
                else
                {
                    //重新计算hash
                    uint hash = TranslateWordHash(_voca_array[a].Word);
                    while (_voca_hash_array[hash] != -1)
                        hash = (hash + 1) % _voca_hash_size;
                    _voca_hash_array[hash] = a;
                    _train_word_count += _voca_array[a].Frequent;
                }
            }
            Array.Resize(ref _voca_array, _voca_size + 1);
            //
            for(int a=0;a<_voca_size;a++)
            {
                _voca_array[a].Code = new char[_max_code_length];
                _voca_array[a].Point = new int[_max_code_length];
            }
        }

        #region 构建方法

        /// <summary>
        /// 从已分析存储的辞典载入数据
        /// </summary>
        public static Lexicon FromExistLexiconFile(string existLexiconFile)
        {
            using(StreamReader sr = new StreamReader(existLexiconFile))
            {

            }



            return null;
        }
        /// <summary>
        /// 从原始文本文件中分析词句
        /// </summary>
        /// <param name="vocabularyFile"></param>
        /// <returns></returns>
        public static Lexicon FromVocabularyFile(string vocabularyFile)
        {
            Lexicon lexicon = new Lexicon();
            Regex regex = new Regex("\\s");
            lexicon.AddVocabulary("</s>");
            //读取文本构建词库
            using (StreamReader sr = new StreamReader(vocabularyFile))
            {
                string line;
                while(string.IsNullOrEmpty((line = sr.ReadLine()))){
                    if (sr.EndOfStream) break;
                    string[] words = regex.Split(line);
                    Array.ForEach(words, word => {
                        if (!string.IsNullOrWhiteSpace(word)) {
                            lexicon._train_word_count++;
                            int i = lexicon.SearchVocabulary(word);
                            if (i == -1)
                                lexicon._voca_array[lexicon.AddVocabulary(word)].Frequent = 1;
                            else
                                lexicon._voca_array[i].Frequent++;
                            if (lexicon._voca_size > _voca_hash_size * 0.7)
                                lexicon.ReduceVocabulary();
                        }
                    });
                }
                //1. 剔除达不到最低频次要求的词
                //2. 排序
                lexicon.SortVocabulary();
            }
            return lexicon;
        }

        #endregion




    }
}
