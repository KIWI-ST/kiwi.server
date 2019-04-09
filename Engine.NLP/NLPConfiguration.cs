using System.Configuration;

namespace Engine.NLP
{
    public class NLPConfiguration
    {

        private static readonly string configFilename = System.Windows.Forms.Application.ExecutablePath;

        private static Configuration config = ConfigurationManager.OpenExeConfiguration(configFilename);

        private static void UpdateConfigKeyValue(string key, string value)
        {
            var element = config.AppSettings.Settings[key];
            if (element != null) config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
        }

    }
}
