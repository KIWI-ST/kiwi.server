using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Crawler.Osm
{
    public class OsmTraceCrawler
    {

        PoliteWebCrawler _crawler;

        public OsmTraceCrawler()
        {
            CrawlConfiguration crawlConfig = AbotConfigurationSectionHandler.LoadFromXml().Convert();
            _crawler = new PoliteWebCrawler(crawlConfig);
        }



    }
}
