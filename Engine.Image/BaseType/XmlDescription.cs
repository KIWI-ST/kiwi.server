using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Image.BaseType
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
