using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Host.UI.Util
{
    /// <summary>
    /// host 辅助类库，提供
    /// 1. 模型统一存储
    /// 2. 模型统一载入
    /// 3. 样本统一命名
    /// 
    /// </summary>
    public class HostHelper
    {
        /// <summary>
        /// 匹配样本文件名参数规则的正则
        /// </summary>
        private static Regex _reg = new Regex(@"_\d+");

        /// <summary>
        /// datetime now string
        /// </summary>
        public static string Now { get { return DateTime.Now.ToFileTimeUtc().ToString(); } }

        /// <summary>
        /// 使用模型
        /// </summary>
        public static void Useage()
        {

        }

        /// <summary>
        /// 拼装样本文件，根据当 file time utc
        /// </summary>
        /// <returns></returns>
        public static string PackSampleFile(int row, int col, int band)
        {
            return string.Format("{0}_{1}_{2}_{3}", Now, row, col, band) + ".txt";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static (int row, int col, int band) UnPackSampleFile(string filename)
        {
            string name = Path.GetFileName(filename);
            MatchCollection collection = _reg.Matches(name);
            int row = Convert.ToInt32(collection[0].Value.Replace("_", "")),
                col = Convert.ToInt32(collection[1].Value.Replace("_", "")),
                band = Convert.ToInt32(collection[2].Value.Replace("_", ""));
            return (row, col, band);
        }

    }
}
