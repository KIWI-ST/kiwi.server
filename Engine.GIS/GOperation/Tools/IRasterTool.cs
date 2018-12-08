using Engine.GIS.GLayer.GRasterLayer;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GOperation.Tools
{
    /// <summary>
    /// raster tool
    /// </summary>
    public interface IRasterTool:IDisposable
    {
       
    }
    /// <summary>
    /// raster band tool(aim band)
    /// base interface
    /// </summary>
    public interface IRasterBandTool : IRasterTool
    {
        void Visit(GRasterBand pBand);
    }
    /// <summary>
    /// raster layer tool(aim layer)
    /// base interface
    /// </summary>
    public interface IRasterLayerTool : IRasterTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLayer"></param>
        void Visit(GRasterLayer pLayer);
    }

    /// <summary>
    /// rpc tool
    /// </summary>
    public interface IRasterRPCTool : IRasterLayerTool
    {
        void DoRPCRectify();
    }

    /// <summary>
    /// pick normal value at posiont (x,y) in eachlayer
    /// </summary>
    public interface IRasterLayerCursorTool : IRasterLayerTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double[] PickRawValue(int x, int y);
        /// <summary>
        /// pick normalized value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double[] PickNormalValue(int x, int y);
        /// <summary>
        /// pick normalized with range
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        double[] PickRagneNormalValue(int x, int y, int row = 5, int col = 5);
    }
    /// <summary>
    /// cursor tool
    /// </summary>
    public interface IRasterBandCursorTool : IRasterBandTool
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<(int, int, double)> ValidatedRawCollection { get; }
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<(int, int, double)> ValidatedNormalCollection { get; }
        /// <summary>
        /// pick raw value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double PickRawValue(int x, int y);
        /// <summary>
        /// pick normalized value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double PickNormalValue(int x, int y);
        /// <summary>
        /// pick value by conv
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        double[] PickRangeNormalValue(int x, int y, int row = 5, int col = 5);
        /// <summary>
        /// pick raw value by cov
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        double[] PickRangeRawValue(int x, int y, int row = 5, int col = 5);
    }

    public interface IRasterBandStatisticTool : IRasterBandTool
    {
        /// <summary>
        /// static raw value query table (convert one-dim to [x,y] two dim]
        /// </summary>
        double[,] StatisticalRawQueryTable { get; }
        /// <summary>
        /// static raw value graph
        /// </summary>
        Dictionary<int, List<Point>> StaisticalRawGraph { get; }
    }

    public interface IRasterExportTool : IRasterTool
    {
        /// <summary>
        /// prepare for export raster
        /// </summary>
        /// <returns></returns>
        GRasterExportTool Prepare();
        /// <summary>
        /// combine bands
        /// </summary>
        /// <param name="outputBuffer"></param>
        /// <returns></returns>
        GRasterExportTool CombineBand(double[] outputBuffer);
        /// <summary>
        /// export raster layer
        /// </summary>
        /// <param name="nGeoTrans"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fullFilename"></param>
        /// <param name="dateType"></param>
        void Export(double[] nGeoTrans, int width, int height, string fullFilename, DataType dateType = DataType.GDT_CFloat32);
    }

}
