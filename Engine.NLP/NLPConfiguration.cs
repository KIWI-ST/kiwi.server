using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using Engine.NLP.Forms;

namespace Engine.NLP
{
    /// <summary>
    /// 1. set nlp configuration
    /// 2. start nlp server
    /// </summary>
    public class NLPConfiguration
    {

        #region Properties

        /// <summary>
        /// port number
        /// </summary>
        public static int PORT = 9000;
        /// <summary>
        /// default CoreNLP dir
        /// </summary>
        private static readonly string corenlpDir = Directory.GetCurrentDirectory() + @"\stanford-corenlp-full\";
        /// <summary>
        /// default start command string
        /// </summary>
        private static readonly string setupString = "-mx4g -cp * edu.stanford.nlp.pipeline.StanfordCoreNLPServer -port 9000 -timeout 999999";
        /// <summary>
        /// default golVe embedding string
        /// </summary>
        private static readonly string gloVeEmbeddingString = Directory.GetCurrentDirectory() + @"\glove-embedding\glove.6B.100d.txt";
        ///// <summary>
        /// 
        /// </summary>
        private static readonly string configFilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\NLPConfiguration.config";
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

        #endregion


        public static Process CreateCoreServerProcess()
        {
            if (PortInUse(PORT))
                return null;
            else
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = CoreNLPDirString;
                process.StartInfo.FileName = "java";
                process.StartInfo.Arguments = CoreNLPCommandString;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                return process;
            }
        }

        /// <summary>
        /// 启动 CoreNLP server
        /// </summary>
        /// <returns></returns>
        private static bool StartCoreServer()
        {
            if (PortInUse(PORT))
                return false;
            else
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = CoreNLPDirString;
                process.StartInfo.FileName = "java";
                process.StartInfo.Arguments = CoreNLPCommandString;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                NLPProcessForm processForm = new NLPProcessForm();
                processForm.SetProcess(process);
                processForm.Show();
                return true;
            }
        }

        /// <summary>
        /// check wether the port is in use
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool PortInUse(int port)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            List<IPEndPoint> endpoints = new List<IPEndPoint>();
            endpoints.AddRange(properties.GetActiveTcpListeners());
            endpoints.AddRange(properties.GetActiveUdpListeners());
            IPEndPoint p = endpoints.Find(pt => pt.Port == port);
            return p != null;
        }

    }
}
