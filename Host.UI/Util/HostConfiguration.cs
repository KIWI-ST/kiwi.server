using System.Configuration;
using System.IO;
using System.Reflection;

namespace Host.UI.Util
{
    public class HostConfiguration
    {

        /// <summary>
        ///  store configuration file
        /// </summary>
        private static readonly string configFilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\App.config";

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

        public static string MainTabViewIndex
        {
            get
            {
                return GetConfigValueByKey("MainTabViewIndex", null);
            }
            set
            {
                UpdateConfigKeyValue("MainTabViewIndex", value);
            }
        }

    }
}
