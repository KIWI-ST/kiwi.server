/// <summary>
/// 切图时，读取xml文件定义的规则
///  黄奎   2016-7-8
/// </summary>
namespace Engine.Image
{
    public class geoEle
    {
        public string westBL { get; set; }
        public string eastBL { get; set; }
        public string northBL { get; set; }
        public string southBL { get; set; }
    }
    public class dataExt
    {
        public geoEle geoEle { get; set; }
    }
    public class dataIdInfo
    {
        public dataExt dataExt { get; set; }
    }
    public class metadata
    {
        public dataIdInfo dataIdInfo { get; set; }
    }
    public class XmlDescription
    {
        public metadata metadata { get; set; }
    }
}
