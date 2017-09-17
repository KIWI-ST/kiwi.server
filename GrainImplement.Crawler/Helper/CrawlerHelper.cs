using Engine.Crawler.Osm;
using GrainImplement.Crawler.Osm;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GrainImplement.Crawler.Helper
{
    public class CrawlerHelper
    {
        public static void Inilization(string connectString)
        {
            _osmTraceTamplate.Inilization(connectString);
        }
        /// <summary>
        /// 数据库寸
        /// </summary>
        static OsmTraceTamplate _osmTraceTamplate = new OsmTraceTamplate();

        static OsmTraceCrawler _osmTraceCrawler = new OsmTraceCrawler();

        public static void Run()
        {
            _osmTraceCrawler.OnTraceDataComplete += _osmTraceCrawler_OnTraceDataComplete;
            _osmTraceCrawler.OnTraceInfoComplete += _osmTraceCrawler_OnTraceInfoComplete;
            _osmTraceCrawler.Run();
        }

        private async static void _osmTraceCrawler_OnTraceInfoComplete(Dictionary<string, string> props)
        {
            string content = JsonConvert.SerializeObject(props);
           await _osmTraceTamplate.Enqueue(content);
        }

        private static void _osmTraceCrawler_OnTraceDataComplete(string traceId, string xmlContentText)
        {
          
        }
    }
}
