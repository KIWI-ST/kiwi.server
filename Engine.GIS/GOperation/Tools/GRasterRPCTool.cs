using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GOperation.Tools
{
    public class GRasterRPCTool : IRasterRPCTool
    {

        double[] _a, _b, _c, _d;

        GRasterLayer _pLayer;

        Dictionary<string, double> _paramaters;

        public GRasterRPCTool(double[] a, double[] b, double[] c, double[] d, Dictionary<string, double> paramaters)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _paramaters = paramaters;
        }

        public void Visit(GRasterLayer pLayer)
        {
            _pLayer = pLayer;
        }

        private (double c, double r) DoRPC(double lng, double lat, double height)
        {
            //
            double P, L, H, C, R;
            P = (lat - _paramaters["latOffset"]) / _paramaters["latScale"];
            L = (lng - _paramaters["longOffset"]) / _paramaters["longScale"];
            H = (height - _paramaters["heightOffset"]) / _paramaters["heightScale"];
            double NumL = _a[0] + _a[1] * L + _a[2] * P + _a[3] * H + _a[4] * L * P + _a[5] * L * H + _a[6] * P * H + _a[7] * L * L + _a[8] * P * P + _a[9] * H * H + _a[10] * P * L * H + _a[11] * L * L * L + _a[12] * L * P * P + _a[13] * L * H * H + _a[14] * L * L * P + _a[15] * P * P * P + _a[16] * P * H * H + _a[17] * L * L * H + _a[18] * P * P * H + _a[19] * H * H * H;
            double DenL = _b[0] + _b[1] * L + _b[2] * P + _b[3] * H + _b[4] * L * P + _b[5] * L * H + _b[6] * P * H + _b[7] * L * L + _b[8] * P * P + _b[9] * H * H + _b[10] * P * L * H + _b[11] * L * L * L + _b[12] * L * P * P + _b[13] * L * H * H + _b[14] * L * L * P + _b[15] * P * P * P + _b[16] * P * H * H + _b[17] * L * L * H + _b[18] * P * P * H + _b[19] * H * H * H;
            double NumS = _c[0] + _c[1] * L + _c[2] * P + _c[3] * H + _c[4] * L * P + _c[5] * L * H + _c[6] * P * H + _c[7] * L * L + _c[8] * P * P + _c[9] * H * H + _c[10] * P * L * H + _c[11] * L * L * L + _c[12] * L * P * P + _c[13] * L * H * H + _c[14] * L * L * P + _c[15] * P * P * P + _c[16] * P * H * H + _c[17] * L * L * H + _c[18] * P * P * H + _c[19] * H * H * H;
            double DenS = _d[0] + _d[1] * L + _d[2] * P + _d[3] * H + _d[4] * L * P + _d[5] * L * H + _d[6] * P * H + _d[7] * L * L + _d[8] * P * P + _d[9] * H * H + _d[10] * P * L * H + _d[11] * L * L * L + _d[12] * L * P * P + _d[13] * L * H * H + _d[14] * L * L * P + _d[15] * P * P * P + _d[16] * P * H * H + _d[17] * L * L * H + _d[18] * P * P * H + _d[19] * H * H * H;
            R = NumL / DenL;
            C = NumS / DenS;
            double r = R * _paramaters["lineScale"] + _paramaters["lineOffset"];
            double c = C * _paramaters["sampScale"] + _paramaters["sampOffset"];
            return (c, r);
        }

        public void RPCTransform()
        {
            double[] nGeoTrans = _pLayer.GeoTransform;
            //
            Bitmap classificationBitmap = new Bitmap(_pLayer.XSize, _pLayer.YSize);
            Graphics g = Graphics.FromImage(classificationBitmap);
            //
            for (int i = 0; i < _pLayer.XSize; i++)
                for (int j = 0; j < _pLayer.YSize; j++)
                {
                    double nPointLng, nPointLat, nPointHeight;
                    nPointLng = j * nGeoTrans[1] + i * nGeoTrans[2] + nGeoTrans[0];
                    nPointLat = j * nGeoTrans[4] + i * nGeoTrans[5] + nGeoTrans[3];
                    nPointHeight = 0;
                    //
                    var (c, r) = DoRPC(nPointLng, nPointLat, nPointHeight);
                    //
                    int x, y;
                    x = (int)c;
                    y = (int)r;
                    double dx = c - x;
                    double dy = r - y;
                    //

                }
        }

    }
}
