using Engine.Crawler.Osm;
using GrainImplement.Crawler.Osm;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GrainImplement.Crawler.Helper
{
    public delegate void CrawlerProgressHandler(int count, string msg);

    public class CrawlerHelper
    {
        public event CrawlerProgressHandler OnCrawlerProgress;

        public void Inilization(string connectString)
        {
            _osmTraceTamplate = new OsmTraceTamplate();
            _osmTraceCrawler = new OsmTraceCrawler();
            _osmTraceTamplate.Inilization(connectString);
        }
        /// <summary>
        /// 数据库寸
        /// </summary>
        OsmTraceTamplate _osmTraceTamplate;

        OsmTraceCrawler _osmTraceCrawler;

        public int Count
        {
            get { return _osmTraceTamplate.Count; }
        }

        public void Run()
        {
            _osmTraceCrawler.OnTraceInfoComplete += _osmTraceCrawler_OnTraceInfoComplete;
            _osmTraceCrawler.Run();
        }

        private async void _osmTraceCrawler_OnTraceInfoComplete(Dictionary<string, string> props)
        {
            string content = JsonConvert.SerializeObject(props);
            bool flag = await _osmTraceTamplate.Enqueue(content);
            if(flag)
                OnCrawlerProgress(_osmTraceTamplate.Count, JsonConvert.SerializeObject(props));
        }

    }
}
