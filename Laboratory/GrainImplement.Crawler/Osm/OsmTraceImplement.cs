using GrainInterface.Crawler.Osm;
using Orleans;
using System.Threading.Tasks;

namespace GrainImplement.Crawler.Osm
{
    public class OsmTraceImplement : Grain, IOsmTrace
    {
        public Task<bool> StartCrawler()
        {
            //Helper.CrawlerHelper.Run();
            return Task.FromResult(true);
        }
    }
}
