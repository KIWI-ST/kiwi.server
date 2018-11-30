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
        /// 测试halfman编码的词典数据
        /// </summary>
        string lexiconFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\word\lexicon.txt";

        /// <summary>
        /// 词典数据
        /// </summary>
        string saveLexiconFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\word\savelexicon.txt";

        [TestMethod]
        public void ReadVocabularyLibrary()
        {
            //1. form raw vocabulary file
            Lexicon lexicon1 = Lexicon.FromVocabularyFile(vocabularyFullFilename);
            lexicon1.SaveLexiconFile(saveLexiconFullFilename);
            //2. from lexicon file
            Lexicon lexicon2 = Lexicon.FromExistLexiconFile(lexiconFullFilename);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void VocabularyHalfmanTree()
        {
            Lexicon lexicon = Lexicon.FromExistLexiconFile(lexiconFullFilename);
            VocabularyHalfmanTree tree = new VocabularyHalfmanTree(lexicon);
        }

        [TestMethod]
        public void TrainWithSkipGram()
        {

        }

    }
}
