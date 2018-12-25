using java.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using java.io;
using edu.stanford.nlp.pipeline;
using System;
using edu.stanford.nlp.ling;
using Engine.Lexicon.Entity;

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
            tree.BuildOrUpdate();
        }

        [TestMethod]
        public void StanfordCoreNLPForChinese()
        {
            // Text for processing
            var text = "王尼玛跑的很快.";
            // Annotation pipeline configuration
            string props = "StanfordCoreNLP-chinese.properties";
            // We should change current directory, so StanfordCoreNLP could find all the model files automatically 
            var pipeline = new StanfordCoreNLP(props);
            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);
            
            var sentences = annotation.get(typeof(CoreAnnotations.SentencesAnnotation));
        }

    }
}
