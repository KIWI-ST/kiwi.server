using Engine.GIS.GLayer.GRasterLayer;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GOperation.Tools
{

    public class RPCInfo
    {
        public double dfSAMP_SCALE { get; set; }
        public double dfSAMP_OFF { get; set; }
        public double dfLONG_OFF { get; set; }
        public double dfLONG_SCALE { get; set; }
        public double dfLAT_OFF { get; set; }
        public double dfLAT_SCALE { get; set; }
        public double dfHEIGHT_OFF { get; set; }
        public double dfHEIGHT_SCALE { get; set; }
        public double dfLINE_SCALE { get; internal set; }
        public double dfLINE_OFF { get; internal set; }
        public double[] adfSAMP_NUM_COEFF { get; set; }
        public double[] adfSAMP_DEN_COEFF { get; set; }
        public double[] adfLINE_NUM_COEFF { get; set; }
        public double[] adfLINE_DEN_COEFF { get; set; }

    }

    /// <summary>
    /// reference:
    /// https://www.cnblogs.com/rainbow70626/p/6254406.html
    /// </summary>
    public class GRasterRPCTool : IRasterRPCTool
    {

        /// <summary>
        /// layer
        /// </summary>
        GRasterLayer _pLayer;

        /// <summary>
        /// rpc paramaters
        /// </summary>
        RPCInfo _rpcInfo;

        /// <summary>
        /// 
        /// </summary>
        GRasterBandCursorTool _pBandCursorTool;

        public GRasterRPCTool(double[] a, double[] b, double[] c, double[] d, Dictionary<string, double> paramaters)
        {
            _rpcInfo = new RPCInfo()
            {
                adfLINE_NUM_COEFF = a,
                adfLINE_DEN_COEFF = b,
                adfSAMP_NUM_COEFF = c,
                adfSAMP_DEN_COEFF = d,
                dfLONG_OFF = paramaters["longOffset"],
                dfLONG_SCALE = paramaters["longScale"],
                dfLAT_OFF = paramaters["latOffset"],
                dfLAT_SCALE = paramaters["latScale"],
                dfHEIGHT_OFF = paramaters["heightOffset"],
                dfHEIGHT_SCALE = paramaters["heightScale"],
                dfSAMP_SCALE = paramaters["sampScale"],
                dfSAMP_OFF = paramaters["sampOffset"],
                dfLINE_SCALE = paramaters["lineScale"],
                dfLINE_OFF = paramaters["lineOffset"]
            };

            _pBandCursorTool = new GRasterBandCursorTool();
        }

        public void DoRPCRectify()
        {
            foreach (var pBand in _pLayer.BandCollection)
                DoRPCTransformInEachBand(pBand);
        }

        public void Visit(GRasterLayer pLayer)
        {
            _pLayer = pLayer;
        }

        double[] RPCComputeTerms(double dfLong, double dfLat, double dfHeight)
        {
            double[] padfTerms = new double[20];

            padfTerms[0] = 1.0;
            padfTerms[1] = dfLong;
            padfTerms[2] = dfLat;
            padfTerms[3] = dfHeight;
            padfTerms[4] = dfLong * dfLat;
            padfTerms[5] = dfLong * dfHeight;
            padfTerms[6] = dfLat * dfHeight;
            padfTerms[7] = dfLong * dfLong;
            padfTerms[8] = dfLat * dfLat;
            padfTerms[9] = dfHeight * dfHeight;

            padfTerms[10] = dfLong * dfLat * dfHeight;
            padfTerms[11] = dfLong * dfLong * dfLong;
            padfTerms[12] = dfLong * dfLat * dfLat;
            padfTerms[13] = dfLong * dfHeight * dfHeight;
            padfTerms[14] = dfLong * dfLong * dfLat;
            padfTerms[15] = dfLat * dfLat * dfLat;
            padfTerms[16] = dfLat * dfHeight * dfHeight;
            padfTerms[17] = dfLong * dfLong * dfHeight;
            padfTerms[18] = dfLat * dfLat * dfHeight;
            padfTerms[19] = dfHeight * dfHeight * dfHeight;

            return padfTerms;
        }

        /// <summary>
        /// 基于rpc配置文件，计算图四至坐标
        /// </summary>
        /// <param name="rpcInfo">rpc配置参数</param>
        /// <param name="geoTransform">image文件GeoTransform属性</param>
        /// <param name="r">行数，对应图像像素点横坐标</param>
        /// <param name="c">列数，对应图像像素点纵坐标</param>
        /// <param name="h">高度，默认50</param>
        (double[] extent, double georesolution) CalcuteImageBoundary(RPCInfo rpcInfo, GRasterLayer pLayer)
        {
            //使用平均海拔
            double averageElevation = rpcInfo.dfHEIGHT_OFF;
            //外包矩形
            double[] extent = new double[4];
            //通过gdal读取的geotransform参数
            double[] geoTransform = _pLayer.GeoTransform;
            //临时记录转换后的坐标
            double[] dbX = new double[4];
            double[] dbY = new double[4];
            //转换像素区域到投影坐标区域，假定高程使用Rpc平均海拔米（如果有dem数据，则需要获取位置的dem高度）
            (dbX[0], dbY[0]) = RPCInversePoint(_rpcInfo, 0, 0, averageElevation);
            (dbX[1], dbY[1]) = RPCInversePoint(_rpcInfo, _pLayer.XSize, 0, averageElevation);
            (dbX[2], dbY[2]) = RPCInversePoint(_rpcInfo, _pLayer.XSize, _pLayer.YSize, averageElevation);
            (dbX[3], dbY[3]) = RPCInversePoint(_rpcInfo, 0, _pLayer.YSize, averageElevation);
            //计算坐标并赋值
            extent[0] = Math.Min(Math.Min(dbX[0], dbX[1]), Math.Min(dbX[2], dbX[3]));
            extent[1] = Math.Max(Math.Max(dbX[0], dbX[1]), Math.Max(dbX[2], dbX[3]));
            extent[2] = Math.Min(Math.Min(dbY[0], dbY[1]), Math.Min(dbY[2], dbY[3]));
            extent[3] = Math.Max(Math.Max(dbY[0], dbY[1]), Math.Max(dbY[2], dbY[3]));
            //计算地理分辨率
            double lfArea = 0.0;
            for (int i = 0; i < 4; i++)
                lfArea += (dbX[i] - dbX[(i + 1) % 4]) * (dbY[(i + 1) % 4] + dbY[i]) / 2.0;
            double georesolution = Math.Sqrt(Math.Abs(lfArea) / (pLayer.XSize * pLayer.YSize));
            return (extent, georesolution);
        }


        void DoRPCTransformInEachBand(GRasterBand pBand)
        {
            //
            _pBandCursorTool.Visit(pBand);
            //计算输出图像四至范围、大小、仿射变换六参数等信息
            double[] nGeoTrans = new double[6];
            //adExtent lngmin, lngmax, latmin, latmax
            var (adfExtent, georesolution) = CalcuteImageBoundary(_rpcInfo, _pLayer);
            //
            nGeoTrans[0] = adfExtent[0];
            nGeoTrans[3] = adfExtent[3];
            nGeoTrans[1] = georesolution;
            nGeoTrans[5] = -georesolution;
            //
            int width = Convert.ToInt32((adfExtent[1] - adfExtent[0]) / georesolution);
            int height = Convert.ToInt32((adfExtent[3] - adfExtent[2]) / georesolution);
            //
            double[] outputBuffer = new double[width * height];
            //
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    double nPointLng, nPointLat, nPointHeight;
                    nPointLng = nGeoTrans[0] + i * nGeoTrans[1] + j * nGeoTrans[2];
                    nPointLat = nGeoTrans[3] +i * nGeoTrans[4] + j * nGeoTrans[5];
                    nPointHeight = _rpcInfo.dfHEIGHT_OFF;
                    //计算经纬度得到行列号
                    var (x, y) = RPCTransformPoint(_rpcInfo, nPointLng, nPointLat, nPointHeight);

                    if (x >= 0 && x < pBand.Width && y >= 0 && y < pBand.Height)
                        outputBuffer[j * width + i] = _pBandCursorTool.PickRawValue((int)x, (int)y);
                    else
                        outputBuffer[j * width + i] = 0;
                }
            //
            Driver drv = Gdal.GetDriverByName("GTiff");
            string[] options = new string[] { "BLOCKXSIZE=" + width, "BLOCKYSIZE=" + height };
            Dataset ds = drv.Create(@"C:\Users\81596\Desktop\rpc\1.tif", width, height, 1, DataType.GDT_CFloat32, options);
            Band ba = ds.GetRasterBand(1);
            if (nGeoTrans != null)
                ds.SetGeoTransform(nGeoTrans);
            ba.WriteRaster(0, 0, width, height, outputBuffer, width, height, 0, 0);
            ds.FlushCache();
        }


        (double pdfPixel, double pdfLine) RPCTransformPoint(RPCInfo rpcInfo, double lng, double lat, double h)
        {
            double diffLong = lng - rpcInfo.dfLONG_OFF;
            if (diffLong < -270)
                diffLong += 360;
            else if (diffLong > 270)
                diffLong -= 360;
            //
            double dfNormalizedLong = diffLong / rpcInfo.dfLONG_SCALE;
            double dfNormalizedLat = (lat - rpcInfo.dfLAT_OFF) / rpcInfo.dfLAT_SCALE;
            double dfNormalizedHeight = (h - rpcInfo.dfHEIGHT_OFF) / rpcInfo.dfHEIGHT_SCALE;
            //
            double[] padfTerms = RPCComputeTerms(dfNormalizedLong, dfNormalizedLat, dfNormalizedHeight);
            double dfResultX = RPCEvaluate(padfTerms, rpcInfo.adfSAMP_NUM_COEFF) / RPCEvaluate(padfTerms, rpcInfo.adfSAMP_DEN_COEFF);
            double dfResultY = RPCEvaluate(padfTerms, rpcInfo.adfLINE_NUM_COEFF) / RPCEvaluate(padfTerms, rpcInfo.adfLINE_DEN_COEFF);
            //
            double i = dfResultX * rpcInfo.dfSAMP_SCALE + rpcInfo.dfSAMP_OFF + 0.5;
            double j = dfResultY * rpcInfo.dfLINE_SCALE + rpcInfo.dfLINE_OFF + 0.5;
            return (i, j);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rpcInfo"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        (double lng, double lat) RPCInversePoint(RPCInfo rpcInfo, double i, double j, double h)
        {
            //设置默认的经纬度
            double lng = rpcInfo.dfLONG_OFF, lat = rpcInfo.dfLAT_OFF;
            for (int iIter = 0; iIter < 20; iIter++)
            {
                var (di, dj) = RPCTransformPoint(rpcInfo, lng, lat, h);
                double deltaI = di - i;
                double deltaJ = dj - j;
                if (Math.Abs(deltaI) < 0.01 && Math.Abs(deltaJ) < 0.01) break;
                lng = lng + (deltaI) / rpcInfo.dfSAMP_SCALE * rpcInfo.dfLONG_SCALE;
                lat = lat + (deltaJ) / rpcInfo.dfLINE_SCALE * rpcInfo.dfLAT_SCALE;
            }
            return (lng, lat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="padfTerms"></param>
        /// <param name="padfCoefs"></param>
        /// <returns></returns>
        double RPCEvaluate(double[] padfTerms, double[] padfCoefs)
        {
            double dfSum = 0.0;
            for (int i = 0; i < 20; i++)
                dfSum += padfTerms[i] * padfCoefs[i];
            return dfSum;
        }

    }
}
