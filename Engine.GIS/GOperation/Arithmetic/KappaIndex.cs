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
            Dictionary<int,List<Point>> Memory = pBandStaticTool.StaisticalRawGraph;
            //key index
            List<int> Keys = Memory.Keys.ToList();
            int actionsNumber = Keys.Count;
            int[,] matrix = new int[actionsNumber, actionsNumber];
            IBandCursorTool pBandCursorTool = new GBandCursorTool();
            pBandCursorTool.Visit(predLayer.BandCollection[0]);
            for (int i = 0; i < actionsNumber; i++)
            {
                int key = Keys[i];
                List<Point> points = Memory[key];
                //计算realKey类分类结果,存入混淆矩阵
                points.ForEach(p =>
                {
                    int classificationType = (int)pBandCursorTool.PickRawValue(p.X, p.Y);
                    classificationType = Keys.IndexOf(classificationType);
                    if (classificationType != -1)
                        matrix[i, classificationType]++;
                });
            }
            // Create a new multi-class Confusion Matrix
            var cm = new GeneralConfusionMatrix(matrix);
            //
            int totalNum = cm.NumberOfSamples;
            //p0
            double p0 = 0;
            for (int i = 0; i < actionsNumber; i++)
                p0 += matrix[i, i];
            //pc
            double pc = 0;
            for (int i = 0; i < actionsNumber; i++)
                pc += cm.ColumnTotals[i] * cm.RowTotals[i];
            pc = pc / totalNum;
            //
            double kappa = (p0 - pc) / (totalNum - pc);
            //
            return (matrix, kappa, actionsNumber);
        }

    }
}
