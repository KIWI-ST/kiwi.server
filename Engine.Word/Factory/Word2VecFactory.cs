namespace Engine.Word.Factory
{
    class Word2VecFactory
    {
        //file path
        public string TrainFullFilename { get; set; }
        public string OutputFullFilename { get; set; }
        public string SaveVocabularyFullFilename { get; set; }
        public string ReadVocabularyFullFilename { get; set; }

        //
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Word2VecModel Create()
        {
            return null;
        }

    }
}
