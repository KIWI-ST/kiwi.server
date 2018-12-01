using Engine.Word.Entity;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Engine.Word.Factory
{
    /// <summary>
    /// an implemention of Google Word2Vec algorithm
    /// reference:
    /// https://github.com/eabdullin/Word2Vec.Net/blob/master/Word2Vec.Net/Word2Vec.cs
    /// </summary>
    public class Word2VecModel
    {
        #region constants
        const int MAXCODELENGTH = 40;
        const int MAXSENTENCELENGHT = 1000;
        /// <summary>
        /// exp tablesize
        /// </summary>
        const int EXPTABLESIZE = 1000;
        const int MAXEXP = 6;
        const int VOCABHASHSIZE = 300000000;
        /// <summary>
        /// unigram tablesize
        /// </summary>
        const int UNIGRAMTABLESIZE = 100000000;
        const int VOCABMAXSIZE = 1000;
        #endregion

        #region external parameters
        public string OutputFullFilename { get; set; }
        public string TrainVocabularyFullFilename { get; set; }
        public string SaveVocabularyFullFilename { get; set; }
        public string ReadVocabularyFullFilename { get; set; }
        public int Binary { get; set; } = 0;
        public int Cbow { get; set; } = 1;
        public int DebugMode { get; set; } = 2;
        public int MinCount { get; set; } = 5;
        public int NumberOfThreads { get; set; } = 8;
        public int Size { get; set; } = 100;
        public long Iteration { get; set; } = 5;
        public long Classes { get; set; } = 0;
        public float Alpha { get; set; } = 0.025f;
        public float Sample { get; set; } = 0.001f;
        public int Hs { get; set; } = 0;
        public int Negative { get; set; } = 5;
        public int WindowSize { get; set; } = 5;
        #endregion

        #region internal parameters
        //
        int _minReduce = 1;
        //
        Vocabulary[] _vocabularys;
        int[] _vocabularyHash;
        //
        int _vocabSize;
        //
        int _layer1Size;
        long _trainWords;
        long _wordCountActual;
        long _fileSize;
        float _startingAlpha;
        //
        float[] _syn0;
        float[] _syn1;
        float[] _syn1Neg;
        float[] _expTable;
        //
        int[] _table;
        #endregion

        Stopwatch _stopwatch;

        #region consturct

        public Word2VecModel(string trainFullFilename, string outputFullFilename, string saveVocabularyFullFilename, string readVocabularyFullFilename)
        {
            //
            TrainVocabularyFullFilename = trainFullFilename;
            OutputFullFilename = outputFullFilename;
            SaveVocabularyFullFilename = saveVocabularyFullFilename;
            ReadVocabularyFullFilename = readVocabularyFullFilename;
            //
            _vocabularys = new Vocabulary[VOCABMAXSIZE];
            _vocabularyHash = new int[VOCABHASHSIZE];
            //
            Initialization();
        }

        private void Initialization()
        {
            //build exp table
            _expTable = new float[EXPTABLESIZE + 1];
            //fill table value
            for (int i=0;i<EXPTABLESIZE;i++)
            {
                // Precompute the exp() table
                _expTable[i] = (float)Math.Exp((i / EXPTABLESIZE * 2 - 1) * MAXEXP);
                // Precompute f(x) = x / (x + 1)
                _expTable[i] = _expTable[i] / (_expTable[i] + 1); 
            }
        }

        #endregion

    

        private int AddWordToVocabularys(string word)
        {
            return 1;
            //_vocabSize
        }

        #region Net

        private void InitNet()
        {
            long a, b;
            ulong nextRandom = 1;
            _syn0 = new float[_vocabSize * Size];
            if (Hs > 0)
            {
                _syn1 = new float[_vocabSize * _layer1Size];
                for (a = 0; a < _vocabSize; a++)
                    for (b = 0; b < _layer1Size; b++)
                        _syn1[a * _layer1Size + b] = 0;
            }
            if (Negative > 0)
            {
                _syn1Neg = new float[_vocabSize * _layer1Size];
                for (a = 0; a < _vocabSize; a++)
                    for (b = 0; b < _layer1Size; b++)
                        _syn1Neg[a * _layer1Size + b] = 0;
            }
            for (a = 0; a < _vocabSize; a++)
            {
                for (b = 0; b < _layer1Size; b++)
                {
                    nextRandom = nextRandom * 25214903917 + 11;
                    _syn0[a * _layer1Size + b] = ((nextRandom & 0xFFFF) / (float)65536 - (float)0.5) / _layer1Size;
                }
            }
            //


        }

        #endregion

        #region training



        /// <summary>
        /// read raw file and statistic
        /// </summary>
        public void StatisticVocabulary(string rawVocabularyFullFilename)
        {
            //regex patter string
            Regex regex = new Regex("\\s");
            //fill hash value as -1;
            for (int i = 0; i < VOCABHASHSIZE; i++)
                _vocabularyHash[i] = -1;
            //seed size
            _vocabSize = 0;
            //
            string line;
            //
            using (StreamReader sr = new StreamReader(rawVocabularyFullFilename))
            {
                while((line = sr.ReadLine()) != null)
                {
                    string[] vals = regex.Split(line);
                    if(vals.Length == 2)
                    {
                        int index = AddWordToVocabularys(vals[0]);
                        _vocabularys[index].Weight = int.Parse(vals[1]);
                    }
                }
                //
    

            }
        }

        private void SaveVocabulary(string SaveVocabularyFullFilename)
        {
            using (var stream = new FileStream(SaveVocabularyFullFilename, FileMode.OpenOrCreate))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    for (var i = 0; i < _vocabSize; i++)
                        streamWriter.WriteLine("{0} {1}", _vocabularys[i].Word, _vocabularys[i].Weight);
                }
            }
        }
        /// <summary>
        /// directly extract vaocabulary from exist file
        /// </summary>
        public void ExtractVocabulary(string statisticalVocabularyFullFilename)
        {

        }
        /// <summary>
        /// 程序入口
        /// </summary>
        public void Train()
        {
            //1. 构建词汇表，得到 得到_vocabularys 和 _vocabSize
            if (!string.IsNullOrEmpty(ReadVocabularyFullFilename))
                ExtractVocabulary(ReadVocabularyFullFilename);
            else
                StatisticVocabulary(TrainVocabularyFullFilename);
            //save vocabulary
            if (!string.IsNullOrEmpty(SaveVocabularyFullFilename))
                SaveVocabulary(SaveVocabularyFullFilename);
            if (string.IsNullOrEmpty(OutputFullFilename)) return;

            //2. 得到_vocabularys 和 _vocabSize 后，构建网络学习相关性


        }


        #endregion

    }
}
