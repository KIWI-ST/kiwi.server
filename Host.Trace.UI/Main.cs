using Engine.GIS;
using Engine.GIS.Read;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Host.Trace.UI
{
    public partial class Main : Form
    {

        IOsmReaderPBF _pOsmReaderPBF;

        public Main()
        {
            InitializeComponent();
            //ReadPBF();
            //TraceModel mode = new TraceModel();
            //mode.Run();
            ReadShp();
        }

        private void TrainTensorflow()
        {
          
        }

        private void ReadShp()
        {
            string shpPath = System.IO.Directory.GetCurrentDirectory() + @"\DATA\boundary\china\CHN_adm1.shp";
            ShpReader shpReader = new ShpReader(shpPath);
            shpReader.Read();
            //1.获取广东省矩形边界
            var polygon = shpReader.LoadPolygon(new Point(112, 23.5)) as IPolygon;
            //2.裁剪pbf文件
            ReadPBF(polygon);
        }

        private void ReadPBF(IPolygon polygon)
        {
            string fpbf = System.IO.Directory.GetCurrentDirectory() + @"\DATA\street\target.osm.pbf";
            _pOsmReaderPBF = new OsmReaderPBF(fpbf);
            _pOsmReaderPBF.OnComplete += _pOsmReaderPBF_OnComplete;
            //
            _pOsmReaderPBF.Read(polygon);
        }

        private void _pOsmReaderPBF_OnComplete(List<OsmSharp.OsmGeo> nodes, List<OsmSharp.OsmGeo> ways, List<OsmSharp.OsmGeo> relations)
        {
            var s = nodes;

            var sss = ways;
        }
    }
}
