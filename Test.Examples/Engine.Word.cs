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
        readonly string rawTextFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\RawText.txt";

        /// <summary>
        /// 保存统计后的词频文件
        /// </summary>
        readonly string saveLexiconFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\lexicon.txt";

        [TestMethod]
        public void ReadVocabularyLibrary()
        {
            //form raw vocabulary file
            Lexicon lexicon = Lexicon.FromVocabularyFile(rawTextFullFilename,EncodeScheme.Onehot);
            lexicon.SaveLexiconFile(saveLexiconFullFilename);
            Assert.IsTrue(lexicon.VocaSize == 608);
        }

        [TestMethod]
        public void LoadLexiconFormLexiconFile()
        {
            //from exist lexicon file
            Lexicon lexicon = Lexicon.FromExistLexiconFile(saveLexiconFullFilename);
            Assert.IsTrue(lexicon.VocaSize == 608);
        }

        [TestMethod]
        public void LearnRawTextByLSTM()
        {

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
