using System;
using System.IO;

namespace Engine.OSM.Read
{
    public class OsmReader:IOsmReader,IDisposable
    {

       protected  FileStream _steam;

        public OsmReader(string path)
        {
            _steam = File.OpenRead(path);
        }

        public virtual void Dispose()
        {
            _steam.Dispose();
        }

    }
}
