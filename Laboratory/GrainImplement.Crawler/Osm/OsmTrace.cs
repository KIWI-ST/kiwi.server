using Engine.Mongo.Entity;

namespace GrainImplement.Crawler.Osm
{
    /// <summary>
    /// osm 数据存储格式
    /// </summary>
    public class OsmTrace : MongoEntity
    {
        public string TraceId { get; set; }
        public string Filename {get;set;}
        public string Uploaded { get; set; }
        public string Points { get; set; }
        public string Startcoordinate { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Visibility { get; set; }
        public string Gpx { get; set; }
    }
}
