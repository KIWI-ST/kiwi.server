using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

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
        /// load and analysis file
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        public static (string incident, string impact, string response) ReadIOPF(string fullFilename)
        {
            //https://itextpdf.com/en/resources/books/itext-7-jump-start-tutorial-net/chapter-5-manipulating-existing-pdf-document
            using (PdfDocument pdf = new PdfDocument(new PdfReader(fullFilename)))
            {
                int count = pdf.GetNumberOfPages();
                string text = "";
                for (int i = 1; i <= count; i++)
                    text += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
                string[] lines = Regex.Split(text, "\n");
                //提取 incident, impact 和 response
                string incident = "";
                //1. extract form

                //PdfAcroForm form = pdf. .GetAcroForm(pdf, false);
                //IDictionary<String, PdfFormField> fields = form.GetFormFields();
            }
            return ("", "", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extention"></param>
        /// <returns></returns>
        public static string GetTemporaryFilename(string extention)
        {
            string dir = Directory.GetCurrentDirectory() + @"\tmp";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return string.Format(@"{0}\{1}{2}", dir, Now, extention);
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
