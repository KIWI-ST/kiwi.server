using Engine.GIS;
using Engine.GIS.Grid;
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
            BuildGrid();
            //ReadPBF();
            //TraceModel mode = new TraceModel();
            //mode.Run();
            //ReadShp();
        }

        private void BuildGrid()
        {
            WebMercatorGrid grid = new WebMercatorGrid();

            List<Coordinate> coordinates = new List<Coordinate>();
            coordinates.Add(new Coordinate(114, 30));
            coordinates.Add(new Coordinate(115, 31));
            Bound bound = new Bound(coordinates);
            grid.Build(bound,14);
            //
            string shpPath = System.IO.Directory.GetCurrentDirectory() + @"\DATA\shp\china\gis.osm_roads_free_1.shp";
            ShpReader shpReader = new ShpReader(shpPath);
            shpReader.Read();
            //
            grid.CutShape(shpReader.GeometryCollection);
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
