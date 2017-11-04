using Engine.GIS;
using Engine.GIS.Grid;
using Engine.GIS.Read;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Host.Trace.UI
{
    public partial class Main : Form
    {

        IOsmReaderPBF _pOsmReaderPBF;

        public Main()
        {
            InitializeComponent();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BuildGrid();
        }

        private void BuildGrid()
        {
            WebMercatorGrid grid = new WebMercatorGrid();

            List<Coordinate> coordinates = new List<Coordinate>();
            coordinates.Add(new Coordinate(109, 20));
            coordinates.Add(new Coordinate(117.2, 25.6));
            Bound bound = new Bound(coordinates);
            //构建尺度6格网
            for (int i=10;i<17;i++)
                grid.Build(bound,i);
            //shpfile路径
            string shpPath = System.IO.Directory.GetCurrentDirectory() + @"\DATA\shp\china\guangdong.shp";
            ShpReader shpReader = new ShpReader(shpPath);
            shpReader.Read();
            //
            string outputDir = System.IO.Directory.GetCurrentDirectory() + @"\dataset";
            grid.CutShape(shpReader.GeometryCollection, outputDir);
        }


        private void TrainTensorflow()
        {
          
        }

        private void ReadShp()
        {
         

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
