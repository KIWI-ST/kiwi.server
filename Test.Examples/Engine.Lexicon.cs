using System.IO;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using Engine.Lexicon.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        /// <summary>
        /// 保存lstm模型文件地址
        /// </summary>
        readonly string lstmSaveFullFilename = Directory.GetCurrentDirectory() + @"\Datasets\lstm.bin";

        [TestMethod]
        public void ReadVocabularyLibrary()
        {
            //form raw vocabulary file
            Lexicon lexicon = Lexicon.FromVocabularyFile(rawTextFullFilename, EncodeScheme.Onehot);
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
            //form raw vocabulary file
            //Lexicon lexicon = Lexicon.FromVocabularyFile(rawTextFullFilename, EncodeScheme.Onehot);
            //LSTMNetwork network = new LSTMNetwork(lexicon.VocaSize);
            //network.LearnFromRawText(rawTextFullFilename, lexicon);
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
