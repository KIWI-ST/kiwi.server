using System.Configuration;
using System.IO;

namespace Engine.NLP
{
    public class NLPConfiguration
    {
        /// <summary>
        /// default CoreNLP dir
        /// </summary>
        private static string corenlpDir = Directory.GetCurrentDirectory() + @"\stanford-corenlp-full\";
        /// <summary>
        /// default start command string
        /// </summary>
        private static string setupString = "-mx4g -cp * edu.stanford.nlp.pipeline.StanfordCoreNLPServer -port 9000 -timeout 15000";
        /// <summary>
        /// default golVe embedding string
        /// </summary>
        private static string gloVeEmbeddingString = Directory.GetCurrentDirectory() + @"\glove-embedding\glove.6B.100d.txt";

        private static readonly string configFilename = System.Windows.Forms.Application.ExecutablePath;

        private static Configuration config = ConfigurationManager.OpenExeConfiguration(configFilename);

        private static void UpdateConfigKeyValue(string key, string value)
        {
            var element = config.AppSettings.Settings[key];
            if (element != null) config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save();
        }

        private static string GetConfigValueByKey(string key, string defaultString)
        {
            var element = config.AppSettings.Settings[key];
            return element == null ? defaultString : element.Value;
        }

        public static string CoreNLPCommandString
        {
            get
            {
                return GetConfigValueByKey("CoreNLPCommandString", setupString);
            }
            set
            {
                UpdateConfigKeyValue("CoreNLPCommandString", value);
            }
        }

        public static string CoreNLPDirString
        {
            get
            {
                return GetConfigValueByKey("CoreNLPDirString", corenlpDir);
            }
            set
            {
                UpdateConfigKeyValue("CoreNLPDirString", value);
            }
        }

        public static string GloVeEmbeddingString
        {
            get
            {
                return GetConfigValueByKey("GloVeEmbeddingString", gloVeEmbeddingString);
            }
            set
            {
                UpdateConfigKeyValue("GloVeEmbeddingString", value);
            }
        }

    }
}
