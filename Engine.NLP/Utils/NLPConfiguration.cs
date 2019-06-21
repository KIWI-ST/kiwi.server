using System.Configuration;
using System.IO;
using System.Reflection;

namespace Engine.NLP.Utils
{
    public class NLPConfiguration
    {

        #region Properties

        /// <summary>
        /// congnitive key
        /// </summary>
        private static readonly string subscriptionKey = "d6eb316b989f4a6b8bfd13b9ae330878";

        /// <summary>
        /// congnitive server endpoint
        /// </summary>
        private static readonly string endpoint = "https://chinanorth.api.cognitive.azure.cn";

        /// <summary>
        ///  store configuration file
        /// </summary>
        private static readonly string configFilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NLPConfiguration.config";

        /// <summary>
        /// default golVe embedding string
        /// </summary>
        private static readonly string gloVeEmbeddingString = Directory.GetCurrentDirectory() + @"\glove-embedding\glove.6B.100d.txt";

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
        /// congnitive api key
        /// </summary>
        public static string SubscriptionKey
        {
            get
            {
                return GetConfigValueByKey("SubscriptionKey", subscriptionKey);
            }
            set
            {
                UpdateConfigKeyValue("SubscriptionKey", value);
            }
        }

        /// <summary>
        /// congnitive server endpoint
        /// </summary>
        public static string Endpoint
        {
            get
            {
                return GetConfigValueByKey("Endpoint", endpoint);
            }
            set
            {
                UpdateConfigKeyValue("Endpoint", value);
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
