using Engine.OSM.Read;
using Engine.TensorFlow.Models;
using NetTopologySuite.Features;
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
            TraceModel mode = new TraceModel();
            mode.Run();
        }

        private void TrainTensorflow()
        {
          
        }

        private void ReadPBF()
        {
            string fpbf = System.IO.Directory.GetCurrentDirectory() + @"\DATA\central-america-latest.osm.pbf";
            _pOsmReaderPBF = new OsmReaderPBF(fpbf);
            _pOsmReaderPBF.OnComplete += _pOsmReaderPBF_OnComplete;
            _pOsmReaderPBF.Read();
        }

        private void _pOsmReaderPBF_OnComplete(List<OsmSharp.OsmGeo> nodes, List<OsmSharp.OsmGeo> ways, List<OsmSharp.OsmGeo> relations)
        {
            var s = nodes;

            var sss = ways;
        }
    }
}
