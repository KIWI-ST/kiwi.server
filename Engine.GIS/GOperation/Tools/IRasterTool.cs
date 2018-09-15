using Engine.GIS.GLayer.GRasterLayer.GBand;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GIS.GOperation.Tools
{
    public interface IRasterTool
    {
        void Visit(GRasterBand pBand);
    }
    /// <summary>
    /// cursor tool
    /// </summary>
    public interface IBandCursorTool : IRasterTool
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
        double[] PickNormalValueByMask(int x, int y, int row = 5, int col = 5);
    }

    public interface IBandStasticTool : IRasterTool
    {
        /// <summary>
        /// static raw value graph
        /// </summary>
        Dictionary<int, List<Point>> StaisticalRawGraph { get; }
    }

}
