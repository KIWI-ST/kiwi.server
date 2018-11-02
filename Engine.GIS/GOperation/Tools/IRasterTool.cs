using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GIS.GOperation.Tools
{
    /// <summary>
    /// raster tool
    /// </summary>
    public interface IRasterTool
    {
       
    }
    /// <summary>
    /// raster band tool(aim band)
    /// </summary>
    public interface IRasterBandTool : IRasterTool
    {
        void Visit(GRasterBand pBand);
    }
    /// <summary>
    /// raster layer tool(aim layer)
    /// </summary>
    public interface IRasterLayerTool : IRasterTool
    {
        void Visit(GRasterLayer pLayer);
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
    public interface IBandCursorTool : IRasterBandTool
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
    }

    public interface IBandStasticTool : IRasterBandTool
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

}
