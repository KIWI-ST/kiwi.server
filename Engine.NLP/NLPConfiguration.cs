using System.Configuration;
using System.IO;
using System.Reflection;

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
        private static string setupString = "-mx4g -cp * edu.stanford.nlp.pipeline.StanfordCoreNLPServer -port 9000 -timeout 99999";
        /// <summary>
        /// default golVe embedding string
        /// </summary>
        private static string gloVeEmbeddingString = Directory.GetCurrentDirectory() + @"\glove-embedding\glove.6B.100d.txt";
        ///// <summary>
        /// 
        /// </summary>
        private static readonly string configFilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NLPConfiguration.config";
        /// <summary>
        /// 
        /// </summary>
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(configFilename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void UpdateConfigKeyValue(string key, string value)
        {
            var element = config.AppSettings.Settings[key];
            if (element != null) config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        private static string GetConfigValueByKey(string key, string defaultString)
        {
            var element = config.AppSettings.Settings[key];
            return element == null ? defaultString : element.Value;
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 情景致灾因子
        /// </summary>
        public static string FactorScenarioString
        {
            get
            {
                return GetConfigValueByKey("FactorScenarioString", "");
            }
            set
            {
                UpdateConfigKeyValue("FactorScenarioString", value);
            }
        }
        /// <summary>
        /// 情景抗灾体
        /// </summary>
        public static string InduceScenarioString
        {
            get
            {
                return GetConfigValueByKey("InduceScenarioString", "");
            }
            set
            {
                UpdateConfigKeyValue("InduceScenarioString", value);
            }
        }
        /// <summary>
        /// 情景承灾体
        /// </summary>
        public static string AffectScenarioString {
            get
            {
                return GetConfigValueByKey("AffectScenarioString", "");
            }
            set
            {
                UpdateConfigKeyValue("AffectScenarioString", value);
            }
        }
        /// <summary>
        /// 情景承灾体
        /// </summary>
        public static string RescueScenarioString
        {
            get
            {
                return GetConfigValueByKey("RescueScenarioString", "");
            }
            set
            {
                UpdateConfigKeyValue("RescueScenarioString", value);
            }
        }
    }
}
