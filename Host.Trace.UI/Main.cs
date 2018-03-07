using Engine.GIS.File;
using Engine.GIS.GeoType;

using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
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
            BuildGrid();
            //ReadTrace();
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
            ShpReader shpReader = new ShpReader();
            shpReader.Read(shpPath);
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

        //private void ReadPBF(IPolygon polygon)
        //{
        //    string fpbf = System.IO.Directory.GetCurrentDirectory() + @"\DATA\street\target.osm.pbf";
        //    _pOsmReaderPBF = new OsmReader(fpbf);
        //    _pOsmReaderPBF.Read(polygon);
        //}

        //private void ReadTrace()
        //{
        //    //获取目录下的所有轨迹
        //    string dir = System.IO.Directory.GetCurrentDirectory() + @"\DATA\Trace\";
        //    string[] traceDir =  System.IO.Directory.GetDirectories(dir);
        //    //以此读取文件，并写入shp文件
        //    for(int i = 0; i < traceDir.Length; i++)
        //    {
        //        int idx = traceDir[i].LastIndexOf('\\');
        //        string dirName = traceDir[i].Substring(idx+1);
        //        string[] tracePaths = System.IO.Directory.GetFiles(traceDir[i]);
        //        for (int j = 0; j < tracePaths.Length; j++)
        //        {
        //            string tracePath = tracePaths[j];
        //            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(tracePath);
        //            ReadTraceFile(dirName, fileNameWithoutExtension, tracePath);
        //        }
        //    }
        //}

        //private void ReadTraceFile(string dirName,string fileName,string tracePath)
        //{
        //    string fileDir = System.IO.Directory.GetCurrentDirectory() + @"\DATA\Trace\shp\"+dirName+@"\";
        //    if (!System.IO.Directory.Exists(fileDir))
        //    {
        //        System.IO.Directory.CreateDirectory(fileDir);
        //    }
        //    ShpReader reader = new ShpReader();
        //    using (System.IO.StreamReader sr = new System.IO.StreamReader(tracePath))
        //    {
        //        string next = sr.ReadLine();
        //        while (next != null)
        //        {
        //            if (next.Length > 0)
        //            {
        //                string[] seg = next.Split(',');
        //                string id = seg[0];
        //                string evt = seg[1];
        //                string time = seg[3];
        //                double longitude =Convert.ToDouble(seg[4]);
        //                double latitude = Convert.ToDouble(seg[5]);
        //                string speed = seg[6];
        //                string direction = seg[7];
        //                string status = seg[8];
        //                //
        //                Point p = new Point(new Coordinate(longitude, latitude));
        //                AttributesTable table = new AttributesTable();
        //                table.AddAttribute("id", id);
        //                table.AddAttribute("evt", evt);
        //                table.AddAttribute("time", time);
        //                table.AddAttribute("longitude", longitude);
        //                table.AddAttribute("latitude", latitude);
        //                table.AddAttribute("speed", speed);
        //                table.AddAttribute("direction", direction);
        //                table.AddAttribute("status", status);
        //                reader.AddFeature(p, table);
        //            }
        //            next = sr.ReadLine();
        //        }
        //    }

        //    reader.Write(fileDir + fileName + ".shp");
        //}


    }
}
