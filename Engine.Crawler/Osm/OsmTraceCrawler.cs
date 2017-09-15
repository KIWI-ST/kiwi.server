using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using System;

namespace Engine.Crawler.Osm
{
    public class OsmTraceCrawler
    {

        PoliteWebCrawler _crawler;
        CrawlConfiguration _crawlConfig;

        public OsmTraceCrawler()
        {
            try
            {
                _crawlConfig = new CrawlConfiguration();
                _crawlConfig.MaxConcurrentThreads = 10;
                _crawlConfig.CrawlTimeoutSeconds = 100;
                _crawlConfig.MaxPagesToCrawl = 1000;
                _crawlConfig.UserAgentString = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
                _crawlConfig.DownloadableContentTypes = "text/html, text/plain";
                //
                _crawler = new PoliteWebCrawler(_crawlConfig,null, null, null, null, null, null, null, null);
                //
                _crawler.PageCrawlStartingAsync += _crawler_PageCrawlStartingAsync;
                _crawler.PageCrawlCompletedAsync += _crawler_PageCrawlCompletedAsync;
                _crawler.PageCrawlDisallowed += _crawler_PageCrawlDisallowed;
                _crawler.PageLinksCrawlDisallowed += _crawler_PageLinksCrawlDisallowed;
            }
            catch(Exception ex)
            {
                var sssss = ex;
            }
        }

        private void _crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            throw new NotImplementedException();
        }

        private void _crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            throw new NotImplementedException();
        }

        private void _crawler_PageCrawlCompletedAsync(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
        }

        private void _crawler_PageCrawlStartingAsync(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;

        }

        public void Run()
        {
            try
            {
                CrawlResult result = _crawler.Crawl(new Uri("http://www.openstreetmap.org/traces/page/1"));
            }
            catch (Exception ex)
            {
                var sss = ex;
            }
        }


    }
}
