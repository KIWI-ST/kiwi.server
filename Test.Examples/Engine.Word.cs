using Engine.Word.Entity;
using java.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using java.io;
using edu.stanford.nlp.pipeline;
using System;
using edu.stanford.nlp.ling;


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
        public void TrainWithSkipGram()
        {
            // Text for processing
            var text = "王尼玛跑的很快.";
            // Annotation pipeline configuration
            //var props = new Properties();
            //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            //props.setProperty("sutime.binders", "0");
            //
            string props = "StanfordCoreNLP-chinese.properties";
            // We should change current directory, so StanfordCoreNLP could find all the model files automatically 
            var curDir = Environment.CurrentDirectory;
            var pipeline = new StanfordCoreNLP(props);
            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            var sentences = annotation.get(typeof(CoreAnnotations.SentencesAnnotation));

        }

    }
}
