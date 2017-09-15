using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.Crawler.Osm
{
    public interface IOsmTrace: IGrainWithIntegerKey
    {
        /// <summary>
        /// 启动爬虫
        /// </summary>
        Task<bool> StartCrawler();
    }
}
