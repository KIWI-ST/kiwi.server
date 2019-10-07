using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using NPOI.XWPF.UserModel;

namespace Host.UI.Util
{
    /// <summary>
    /// IOPF text file
    /// </summary>
    public class IOPF
    {
        public string[] _incident, _impact, _response;
        /// <summary>
        /// 
        /// </summary>
        public string IncidentText
        {
            get
            {
                if (_incident == null)
                    return null;
                else
                    return string.Join(".", _incident);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImpactText
        {
            get
            {
                if (_impact == null)
                    return null;
                else
                    return string.Join(".", _impact);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResponseText
        {
            get
            {
                if (_response == null)
                    return null;
                else
                    return string.Join(".", _response);
            }
        }
    }

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
        /// 全文保存
        /// </summary>
        public static string FullText { get; private set; } = "";

        #region IOPF文件解析

        /// <summary>
        /// 
        /// </summary>
        public static IOPF Iopf { get; private set; } = new IOPF();

        #endregion

        /// <summary>
        /// 匹配样本文件名参数规则的正则
        /// </summary>
        private static Regex _reg = new Regex(@"_\d+");

        /// <summary>
        /// datetime to utc filename string
        /// </summary>
        public static string NowFile { get { return DateTime.Now.ToFileTimeUtc().ToString(); } }

        /// <summary>
        /// now time string
        /// </summary>
        public static string Now { get { return DateTime.Now.ToLongTimeString().ToString(); } }

        /// <summary>
        /// 使用模型
        /// </summary>
        public static void Useage()
        {

        }

        /// <summary>
        /// load and analysis file
        /// @example 
        ///  var (indcident, impact, response) = HostHelper.ReadIOPF(opg.FileName);
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        public static (string[] incident, string[] impact, string[] response) ReadIOPF(string fullFilename)
        {
            //https://itextpdf.com/en/resources/books/itext-7-jump-start-tutorial-net/chapter-5-manipulating-existing-pdf-document
            using (PdfDocument pdf = new PdfDocument(new PdfReader(fullFilename)))
            {
                int count = pdf.GetNumberOfPages();
                string text = "";
                for (int i = 1; i <= count; i++)
                    text += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i)) + "\n";
                string[] lines = Regex.Split(text, "\n");
                //提取 incident, impact 和 response
                int incidentIdx = -1, impactIdx = -1, responseIdx = -1, applicabilityIdx = -1;
                //1. extract form text 
                for (int i = 0; i < lines.Count(); i++)
                {
                    string line = lines[i];
                    if (line == "Incident")
                        incidentIdx = i;
                    else if (line == "Impact")
                        impactIdx = i;
                    else if (line == "Response operations")
                        responseIdx = i;
                    else if (line == "Applicability of the Conventions")
                        applicabilityIdx = i;
                }
                //fix: if there is misssing response operations
                responseIdx = responseIdx == -1 ? applicabilityIdx : responseIdx;
                //2. return result
                List<string> collection = lines.ToList();
                collection.RemoveRange(0, incidentIdx);
                string[] incident = collection.Take(impactIdx - incidentIdx).ToArray();
                collection.RemoveRange(0, impactIdx - incidentIdx);
                string[] impact = collection.Take(responseIdx - impactIdx).ToArray();
                collection.RemoveRange(0, responseIdx - impactIdx);
                string[] response = collection.Take(applicabilityIdx - responseIdx).ToArray();
                //cache
                Iopf._incident = incident;
                Iopf._impact = impact;
                Iopf._response = response;
                return (incident, impact, response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        public static string[] ReadFlatText(string fullFilename)
        {
            using (FileStream fs = new FileStream(fullFilename, FileMode.Open))
            {
                FullText = "";
                XWPFDocument doc = new XWPFDocument(fs);
                List<string> strs = new List<string>();
                foreach (var paragraph in doc.Paragraphs)
                {
                    string text = paragraph.ParagraphText;
                    strs.Add(text);
                    FullText += text;
                }
                return strs.ToArray();
            }
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
            return string.Format(@"{0}\{1}{2}", dir, NowFile, extention);
        }

        /// <summary>
        /// 拼装样本文件，根据当 file time utc
        /// </summary>
        /// <returns></returns>
        public static string PackSampleFile(int row, int col, int band)
        {
            return string.Format("{0}_{1}_{2}_{3}", NowFile, row, col, band) + ".txt";
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
