namespace GrainImplement.Crawler.Helper
{
    public class CrawlerHelper
    {
        static Engine.Crawler.Osm.OsmTraceCrawler _osmTraceCrawler = new Engine.Crawler.Osm.OsmTraceCrawler();

        public static void Run()
        {
            _osmTraceCrawler.Run();
        }

    }
}
