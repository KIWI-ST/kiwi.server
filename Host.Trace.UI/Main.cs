using Engine.GIS.Entity;
using Engine.GIS.GeoType;
using Engine.GIS.Grid;
using Engine.GIS.Read;
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Host.Trace.UI
{
    public partial class Main : Form
    {

        IOsmReader _pOsmReaderPBF;

        public Main()
        {
            InitializeComponent();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BuildGrid();
            ReadTrace();
        }

        private void BuildGrid()
        {
            WebMercatorGrid grid = new WebMercatorGrid();
            //
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
            grid.CutShape(shpReader.FeaureCollection, outputDir);
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
            _pOsmReaderPBF = new OsmReader(fpbf);
            _pOsmReaderPBF.Read(polygon);
        }

        private void ReadTrace()
        {
            //获取目录下的所有轨迹
            string dir = System.IO.Directory.GetCurrentDirectory() + @"\DATA\Trace\20121130\";
            string[] traceDir =  System.IO.Directory.GetDirectories(dir);
            //以此读取文件，并写入shp文件
            for(int i = 0; i < traceDir.Length; i++)
            {
                string trace = traceDir[i];
                using(System.IO.StreamReader sr = new System.IO.StreamReader(trace))
                {
                    string next =  sr.ReadLine();
                    while (next != null)
                    {
                        if (next.Length > 0)
                        {
                            string[] seg = next.Split(',');
                            string id = seg[0];
                            string evt = seg[1];
                            string time = seg[2];
                            string longitude = seg[3];
                            string latitude = seg[4];
                            string speed = seg[5];
                            string direction = seg[6];
                            string status = seg[7];
                            //

                        }
                        next = sr.ReadLine();
                    }
                }
            }
        }




    }
}
