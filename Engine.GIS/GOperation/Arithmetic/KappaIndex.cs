using Accord.Statistics.Analysis;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.GIS.GOperation.Arithmetic
{
    public class KappaIndex
    {

        public static (int[,] matrix, double kappa, int actionsNumber) Calcute(GRasterLayer truthLayer, GRasterLayer predLayer)
        {
            //statical label band graph
            IBandStasticTool pBandStaticTool = new GBandStasticTool();
            pBandStaticTool.Visit(truthLayer.BandCollection[0]);
            Dictionary<int,List<Point>> memory = pBandStaticTool.StaisticalRawGraph;
            //key index
            List<int> Keys = memory.Keys.ToList();
            int actionsNumber = Keys.Count;
            int[,] matrix = new int[actionsNumber, actionsNumber];
            IBandCursorTool pBandCursorTool = new GBandCursorTool();
            pBandCursorTool.Visit(predLayer.BandCollection[0]);
            //
            pBandStaticTool.Visit(predLayer.BandCollection[0]);
            var m = pBandStaticTool.StaisticalRawGraph;
            //
            for (int i = 0; i < actionsNumber; i++)
            {
                int key = Keys[i];
                List<Point> points = memory[key];
                //计算realKey类分类结果,存入混淆矩阵
                points.ForEach(p =>
                {
                    int rawType = (int)pBandCursorTool.PickRawValue(p.X, p.Y);
                    int indexType = Keys.IndexOf(rawType);
                    if (indexType != -1)
                        matrix[i, indexType]++;
                });
            }
            // Create a new multi-class Confusion Matrix
            var cm = new GeneralConfusionMatrix(matrix);
            //
            int totalNum = cm.NumberOfSamples;
            //p0
            double p0 = 0;
            for (int i = 0; i < actionsNumber; i++)
                p0 += Convert.ToDouble(matrix[i, i]);
            //pc
            double pc = 0;
            for (int i = 0; i < actionsNumber; i++)
                pc += Convert.ToDouble(cm.ColumnTotals[i]) * Convert.ToDouble(cm.RowTotals[i]);
            pc = pc / totalNum;
            //
            double kappa = (p0 - pc) / (totalNum - pc);
            //
            return (matrix, kappa, actionsNumber);
        }

    }
}
