using Abot.Crawler;
using Abot.Poco;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Crawler.Osm
{
    /// <summary>
    /// 获取trace信息委托
    /// </summary>
    public delegate void TraceInfoCompleteHandler(Dictionary<string, string> props);
    /// <summary>
    /// 获取trace数据
    /// </summary>
    public delegate void TraceDataCompleteHandler(string tarceId,string xmlContentText);

    public class OsmTraceCrawler
    {
        /// <summary>
        /// 获取traceInfo信息成功
        /// </summary>
       public  event TraceInfoCompleteHandler OnTraceInfoComplete;
        /// <summary>
        /// 获取xml格式trace信息成功
        /// </summary>
        public event TraceDataCompleteHandler OnTraceDataComplete;
        /// <summary>
        /// 网络爬虫
        /// </summary>
        PoliteWebCrawler _crawler;
        /// <summary>
        /// osm爬虫配置
        /// </summary>
        CrawlConfiguration _crawlConfig;
         
        public OsmTraceCrawler()
        {
            _crawlConfig = new CrawlConfiguration();
            _crawlConfig.MaxConcurrentThreads = 8;
            _crawlConfig.MaxCrawlDepth = int.MaxValue;
            _crawlConfig.MaxPagesToCrawl = int.MaxValue;
            //_crawlConfig.CrawlTimeoutSeconds = 100;
            _crawlConfig.IsExternalPageLinksCrawlingEnabled = true;
            _crawlConfig.UserAgentString = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
            _crawlConfig.DownloadableContentTypes = "text/html, text/plain, text/xml, application/gpx+xml";
            //
            _crawler = new PoliteWebCrawler(_crawlConfig, null, null, null, null, null, null, null, null);
            //
            _crawler.PageCrawlStartingAsync += _crawler_PageCrawlStartingAsync;
            _crawler.PageCrawlCompletedAsync += _crawler_PageCrawlCompletedAsync;
            _crawler.PageCrawlDisallowed += _crawler_PageCrawlDisallowed;
            _crawler.PageLinksCrawlDisallowed += _crawler_PageLinksCrawlDisallowed;
            //
            _crawler.ShouldCrawlPage((pageToCrawl, crawlContext) =>
            {
                CrawlDecision decision = new CrawlDecision { Allow = false };
                //不搜索以下内容

                //3.trace and data
                if (pageToCrawl.Uri.AbsoluteUri.Contains("trace") && pageToCrawl.Uri.AbsoluteUri.Contains("data"))
                    return new CrawlDecision { Allow = false };
                //搜索三种页面：
                //1.traces/page/
                if (pageToCrawl.Uri.AbsoluteUri.Contains("traces/page/"))
                    return new CrawlDecision { Allow = true };
                //2.user and traces
                if (pageToCrawl.Uri.AbsoluteUri.Contains("user") && pageToCrawl.Uri.AbsoluteUri.Contains("traces"))
                    return new CrawlDecision { Allow = true };
                if (pageToCrawl.Uri.AbsoluteUri.Contains("login?referer="))
                    return new CrawlDecision { Allow = true };
                if (pageToCrawl.Uri.AbsoluteUri.Contains("/tag/foot"))
                    return new CrawlDecision { Allow = true };
                if (pageToCrawl.Uri.AbsoluteUri.Contains("/traces/tag"))
                    return new CrawlDecision { Allow = true };
                return decision;
            });

            //_crawler.ShouldDownloadPageContent((crawledPage, crawlContext) =>
            //{
            //    CrawlDecision decision = new CrawlDecision { Allow = false, Reason = "Only download raw page content for .com tlds" };
            //    if (crawledPage.Uri.AbsoluteUri.Contains("/data") || crawledPage.Uri.AbsoluteUri.Contains("traces"))
            //        return new CrawlDecision { Allow = true };
            //    return decision;
            //});

        }

        private void _crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {

        }

        private void _crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {

        }

        private void _crawler_PageCrawlCompletedAsync(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            //数据页
            if (crawledPage.Uri.AbsoluteUri.Contains("user") && crawledPage.Uri.AbsoluteUri.Contains("traces")&& !crawledPage.Uri.AbsoluteUri.Contains("/tag") && !crawledPage.Uri.AbsoluteUri.Contains("login?referer=")&& !crawledPage.Uri.AbsoluteUri.Contains("new?referer="))
            {
                string traceId = crawledPage.Uri.Segments.Last();
                HtmlNodeCollection collection = crawledPage.HtmlDocument.DocumentNode.SelectNodes("//tr");
                Dictionary<string, string> props = new Dictionary<string, string>();
                props.Add("TraceId", traceId);
                foreach (var element in collection)
                {
                    string name = element.ChildNodes[1].InnerText.Replace(":","").Replace(" ","");
                    string value = element.ChildNodes[3].InnerText;
                    props.Add(name, value);
                }
                OnTraceInfoComplete(props);
            }
            //信息概览页
            else if (crawledPage.Uri.AbsoluteUri.Contains("trace") && crawledPage.Uri.AbsoluteUri.Contains("data"))
            {
                //string tarceId = crawledPage.Uri.Segments[2].Replace("/","");
                //string xmlContent = crawledPage.Content.Text;
                //OnTraceDataComplete(tarceId,xmlContent);
            }
        }

        private void _crawler_PageCrawlStartingAsync(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
        }

        public void Run()
        {
            try
            {
                CrawlResult result = _crawler.Crawl(new Uri("https://www.openstreetmap.org/traces/page/1"));

                var sss = result;
            }
            catch (Exception ex)
            {
                var sss = ex;
            }
        }


    }
}
