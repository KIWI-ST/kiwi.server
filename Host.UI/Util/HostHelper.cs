using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static string PackRasterFile(int row, int col, int band)
        {
            return string.Format("{0}_{1}_{2}_{3}", Now, row, col, band) + ".txt";
        }

    }
}
