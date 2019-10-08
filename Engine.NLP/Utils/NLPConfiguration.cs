using System.Configuration;
using System.IO;
using System.Reflection;

namespace Engine.NLP.Utils
{
    /// <summary>
    /// confguration 
    /// </summary>
    public class NLPConfiguration
    {
        #region Config Function

        /// <summary>
        /// 
        /// </summary>
        private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(configFilename);

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

        #endregion

        #region Baidu.AI Settings 

        /// <summary>
        /// https://console.bce.baidu.com/ai/?_=1570501352525&fromai=1#/ai/nlp/app/list
        /// </summary>
        private static readonly string baiduAIAppId = "17451194";

        /// <summary>
        /// https://console.bce.baidu.com/ai/?_=1570501352525&fromai=1#/ai/nlp/app/list
        /// </summary>
        private static readonly string baiduAIAPIKey = "UGcTi6Y5mRWna8ddEXrbhuCt";

        /// <summary>
        /// https://console.bce.baidu.com/ai/?_=1570501352525&fromai=1#/ai/nlp/app/list
        /// </summary>
        private static readonly string baiduAISecretKey = "YlvUOTUN5fcTUM3041vpLMSC074GQG42 ";

        public static string BaiduAIAppId
        {
            get
            {
                return GetConfigValueByKey("BaiduAIAppId", baiduAIAppId);
            }
            set
            {
                UpdateConfigKeyValue("BaiduAIAppId", value);
            }
        }

        public static string BaiduAIAPIKey
        {
            get
            {
                return GetConfigValueByKey("BaiduAIAPIKey", baiduAIAPIKey);
            }
            set
            {
                UpdateConfigKeyValue("BaiduAIAPIKey", value);
            }
        }

        public static string BaiduAISecretKey
        {
            get
            {
                return GetConfigValueByKey("BaiduAISecretKey", baiduAISecretKey);
            }
            set
            {
                UpdateConfigKeyValue("BaiduAISecretKey", value);
            }
        }

        #endregion

        #region StanfordNLP Properties

        /// <summary>
        /// corenlp server address
        /// </summary>
        private static readonly string corenlpaddress = @"http://127.0.0.1";

        /// <summary>
        /// port of core nlp server
        /// </summary>
        private static readonly string corenlpport = "9000";

        /// <summary>
        ///  store configuration file
        /// </summary>
        private static readonly string configFilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NLPConfiguration.config";

        /// <summary>
        /// default golVe embedding string
        /// </summary>
        private static readonly string gloVeEmbeddingString = Directory.GetCurrentDirectory() + @"\glove-embedding\glove.6B.100d.txt";

        /// <summary>
        /// congnitive key
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
        /// corenlp server address
        /// </summary>
        public static string CoreNLPAddress
        {
            get
            {
                return GetConfigValueByKey("CoreNLPAddress", corenlpaddress);
            }
            set
            {
                UpdateConfigKeyValue("CoreNLPAddress", value);
            }
        }

        public static string CoreNLPPort
        {
            get
            {
                return GetConfigValueByKey("CoreNLPPort", corenlpport);
            }
            set
            {
                UpdateConfigKeyValue("CoreNLPPort", value);
            }
        }

        #endregion

        #region  Scenario Expert

        /// <summary>
        /// 致灾因子
        /// </summary>
        public static string HazardString
        {
            get
            {
                return GetConfigValueByKey("HazardString", "");
            }
            set
            {
                UpdateConfigKeyValue("HazardString", value);
            }
        }

        /// <summary>
        /// 承灾体
        /// </summary>
        public static string ExposureString
        {
            get
            {
                return GetConfigValueByKey("ExposureString", "");
            }
            set
            {
                UpdateConfigKeyValue("ExposureString", value);
            }
        }

        /// <summary>
        /// 人类活动
        /// </summary>
        public static string HumanBehaviorString
        {
            get
            {
                return GetConfigValueByKey("HumanBehaviorString", "");
            }
            set
            {
                UpdateConfigKeyValue("HumanBehaviorString", value);
            }
        }

        #endregion

    }
}
