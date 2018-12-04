using Engine.GIS.GLayer.GRasterLayer;
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
        public double[] adfSAMP_NUM_COEFF { get; set; }
        public double[] adfSAMP_DEN_COEFF { get; set; }
        public double[] adfLINE_NUM_COEFF { get; set; }
        public double[] adfLINE_DEN_COEFF { get; set; }
    }

    /// <summary>
    /// reference:
    /// http://www.mamicode.com/info-detail-1668491.html
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
                dfSAMP_OFF = paramaters["sampOffset"]
            };

            _pBandCursorTool = new GRasterBandCursorTool();
        }

        public void DoRPCRectify()
        {
            foreach (var pBand in _pLayer.BandCollection)
                Run(pBand);
        }

        public void Visit(GRasterLayer pLayer)
        {
            _pLayer = pLayer;
        }

        void RPCComputeCoeffTerms(double dfLong, double dfLat, double dfHeight, double[] padfTerms)
        {
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
        }

        double RPCEvaluateSum(double[] padfTerms, double[] padfCoefs)
        {
            double dfSum = 0.0;
            int i;
            for (i = 0; i < 20; i++)
                dfSum += padfTerms[i] * padfCoefs[i];
            return dfSum;
        }

        (double pdfLong, double pdfLat) RPCInverseTransform(RPCInfo pRPC, double[] dbGeoTran, double dfPixel, double dfLine, double dfHeight)
        {
            double dfResultX, dfResultY;
            //初始值使用放射变换系数求解  
            dfResultX = dbGeoTran[0] + dbGeoTran[1] * dfPixel + dbGeoTran[2] * dfLine;
            dfResultY = dbGeoTran[3] + dbGeoTran[4] * dfPixel + dbGeoTran[5] * dfLine;
            //开始用正变换的函数进行迭代  
            double dfPixelDeltaX = 0.0, dfPixelDeltaY = 0.0;
            int nIter;
            for (nIter = 0; nIter < 20; nIter++)
            {
                var (dfBackPixel, dfBackLine) = RPCTransform(pRPC, dfResultX, dfResultY, dfHeight);
                dfPixelDeltaX = dfBackPixel - dfPixel;
                dfPixelDeltaY = dfBackLine - dfLine;
                dfResultX = dfResultX
                    - dfPixelDeltaX * dbGeoTran[1] - dfPixelDeltaY * dbGeoTran[2];
                dfResultY = dfResultY
                    - dfPixelDeltaX * dbGeoTran[4] - dfPixelDeltaY * dbGeoTran[5];
                if (Math.Abs(dfPixelDeltaX) < 0.001 && Math.Abs(dfPixelDeltaY) < 0.001) break;
            }
            return (dfResultX, dfResultY);
        }

        (double dfBackPixel, double dfBackLine) RPCTransform(RPCInfo pRPC, double dfLong, double dfLat, double dfHeight)
        {
            double dfResultX, dfResultY;
            double[] adfTerms = new double[20];

            RPCComputeCoeffTerms(
                (dfLong - pRPC.dfLONG_OFF) / pRPC.dfLONG_SCALE,
                (dfLat - pRPC.dfLAT_OFF) / pRPC.dfLAT_SCALE,
                (dfHeight - pRPC.dfHEIGHT_OFF) / pRPC.dfHEIGHT_SCALE,
                adfTerms);

            dfResultX = RPCEvaluateSum(adfTerms, pRPC.adfSAMP_NUM_COEFF)
                / RPCEvaluateSum(adfTerms, pRPC.adfSAMP_DEN_COEFF);

            dfResultY = RPCEvaluateSum(adfTerms, pRPC.adfLINE_NUM_COEFF)
                / RPCEvaluateSum(adfTerms, pRPC.adfLINE_DEN_COEFF);

            double pdfPixel = dfResultX * pRPC.dfSAMP_SCALE + pRPC.dfSAMP_OFF;
            double pdfLine = dfResultY * pRPC.dfLONG_SCALE + pRPC.dfLONG_OFF;

            return (pdfPixel, pdfLine);
        }

        void Run(GRasterBand pBand)
        {
            _pBandCursorTool.Visit(pBand);
            double[] dbGeonTran = _pLayer.GeoTransform;
            //计算输出图像四至范围、大小、仿射变换六参数等信息
            double[] adfGeoTransform = new double[6];
            double[] adfExtent = new double[4];
            int nPixels = 0, nLines = 0;
            double[] dbX = new double[4];
            double[] dbY = new double[4];
            //
            (dbX[0], dbY[0]) = RPCInverseTransform(_rpcInfo, dbGeonTran, 0, 0, 100);
            (dbX[1], dbY[1]) = RPCInverseTransform(_rpcInfo, dbGeonTran, pBand.Width, 0, 100);
            (dbX[2], dbY[2]) = RPCInverseTransform(_rpcInfo, dbGeonTran, pBand.Width, pBand.Height, 100);
            (dbX[3], dbY[3]) = RPCInverseTransform(_rpcInfo, dbGeonTran, 0, pBand.Height, 100);
            //
            adfExtent[0] = Math.Min(Math.Min(dbX[0], dbX[1]), Math.Min(dbX[2], dbX[3]));
            adfExtent[1] = Math.Max(Math.Max(dbX[0], dbX[1]), Math.Max(dbX[2], dbX[3]));
            adfExtent[2] = Math.Min(Math.Min(dbY[0], dbY[1]), Math.Min(dbY[2], dbY[3]));
            adfExtent[3] = Math.Max(Math.Max(dbY[0], dbY[1]), Math.Max(dbY[2], dbY[3]));
            //
            int nMinCellSize = (int)Math.Min(dbGeonTran[1], Math.Abs(dbGeonTran[5]));
            //
            nPixels = (int)Math.Ceiling(Math.Abs(adfExtent[1] - adfExtent[0]) / dbGeonTran[1]);
            nLines = (int)Math.Ceiling(Math.Abs(adfExtent[3] - adfExtent[2]) / Math.Abs(dbGeonTran[5]));
            adfGeoTransform[0] = adfExtent[0];
            adfGeoTransform[3] = adfExtent[3];
            adfGeoTransform[1] = dbGeonTran[1];
            adfGeoTransform[5] = dbGeonTran[5];
            //创建输出图像
            Bitmap bitmap = new Bitmap(pBand.Width, pBand.Height);
            Graphics g = Graphics.FromImage(bitmap);
            //reference https://www.cnblogs.com/lovebay/p/5335431.html
            //左上
            double dbX1 = adfGeoTransform[0];
            double dbY1 = adfGeoTransform[3];
            //右下
            double dbX2 = adfGeoTransform[0] + nPixels * adfGeoTransform[1] + nLines * adfGeoTransform[2];
            double dbY2 = adfGeoTransform[3] + nPixels * adfGeoTransform[4] + nLines * adfGeoTransform[5];
            //右上
            double dbX3 = dbX2;
            double dbY3 = dbY1;
            //左下
            double dbX4 = dbX1;
            double dbY4 = dbY2;
            //
            double dfPixel = 0;
            double dfLine = 0;
            //由输出的图像地理坐标系变换到原始的像素坐标系  
            (dfPixel, dfLine) = RPCTransform(_rpcInfo, dbX1, dbY1, 100);
            int nCol1 = (int)(dfPixel + 0.5);
            int nRow1 = (int)(dfLine + 0.5);
            //
            (dfPixel, dfLine) = RPCTransform(_rpcInfo, dbX2, dbY2, 100);
            int nCol2 = (int)(dfPixel + 0.5);
            int nRow2 = (int)(dfLine + 0.5);
            //
            (dfPixel, dfLine) = RPCTransform(_rpcInfo, dbX3, dbY3, 100);
            int nCol3 = (int)(dfPixel + 0.5);
            int nRow3 = (int)(dfLine + 0.5);
            //
            (dfPixel, dfLine) = RPCTransform(_rpcInfo, dbX4, dbY4, 100);
            int nCol4 = (int)(dfPixel + 0.5);
            int nRow4 = (int)(dfLine + 0.5);
            //
            int nMinRow = Math.Min(Math.Min(nRow1, nRow2), Math.Min(nRow3, nRow4));
            if (nMinRow < 0) nMinRow = 0;
            int nMaxRow = Math.Max(Math.Max(nRow1, nRow2), Math.Max(nRow3, nRow4));
            if (nMaxRow >= pBand.Height) nMaxRow = pBand.Height - 1;
            int nHeight = nMaxRow - nMinRow + 1;
            //图像重采用
            double[] pSrcValues = new double[pBand.Width * pBand.Height];
            //
            for (int i = 0; i < pBand.Width; i++)
                for (int j = 0; j < pBand.Height; j++)
                {
                    double x = adfGeoTransform[0] + j * adfGeoTransform[1] + i * adfGeoTransform[2];
                    double y = adfGeoTransform[3] + j * adfGeoTransform[4] + i * adfGeoTransform[5];
                    var (dx, dy) = RPCTransform(_rpcInfo, x, y, 100);
                    int nColIndex = (int)(dx + 0.5);
                    int nRowIndex = (int)(dy + 0.5);
                    int nOffsetSrc = ((nRowIndex - nMinRow) * pBand.Width + nColIndex);
                    if (nColIndex < 0 || nColIndex >= pBand.Width || nRowIndex < 0 || nRowIndex >= pBand.Height)
                        pSrcValues[j * pBand.Width + i] = 0;
                    else
                        pSrcValues[j * pBand.Width + i] = _pBandCursorTool.PickRawValue(i, j);
                }
            //}{debug
            int seed = 0;
            Array.ForEach(pSrcValues, p =>
            {
                if (p != 0.0)
                    seed++;
            });
            var s = "";
            //save bitmap file
            //bitmap.Save(@"C:\Users\81596\Desktop\111.png");
        }

    }
}
