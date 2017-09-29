using OsmSharp;

namespace Engine.OSM.Read
{
    public interface IOsmReaderPBF : IOsmReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Read();
        /// <summary>
        /// 
        /// </summary>
        event ReadCompleteHandle OnComplete;
    }
}
