using Engine.Word.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Test.Examples
{
    [TestClass]
    public class WORDTEST
    {
        /// <summary>
        /// 原始待统计数据源
        /// </summary>
        string vocabularyFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\word\vocabulary.txt";
        
        /// <summary>
        /// 词典数据
        /// </summary>
        string lexiconFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\word\lexicon.txt";

        [TestMethod]
        public void ReadVocabularyLibrary()
        {
            //1. form raw vocabulary file
            Lexicon lexicon1 = Lexicon.FromVocabularyFile(vocabularyFullFilename);
            //2. from lexicon file
            //Lexicon lexicon2 = Lexicon.FromExistLexiconFile(lexiconFullFilename);
        }

    }
}
