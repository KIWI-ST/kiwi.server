using Accord.Statistics.Analysis;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GLayer.GRasterLayer.GBand;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.GIS.GOperation.Arithmetic
{
    public class KappaIndex
    {
        private static (int x,int y,int classIndex) SequentialAccessEnv(GRasterLayer labelRasterLayer)
        {
            //int _x, _y, _value;
            //do
            //{
            //    (_x, _y, _value) = labelRasterLayer.BandCollection[0].Next();
            //} while (_value == 0);//当值为0，即表示此像素为背景值，
            //return (_x, _y, _value - 1);
            return (0, 0, 0);
        }

        public static (int[,] matrix, double kappa, int actionsNumber) Calcute(GRasterLayer truthLayer, GRasterLayer predLayer)
        {
            GRasterBand labelBand = truthLayer.BandCollection[0];
            foreach ( var element in labelBand)
            {
                
            }


            Dictionary<int, List<Point>> Memory = new Dictionary<int, List<Point>>();
            int x, y, classIndex;
            do
            {
                (x, y, classIndex) = SequentialAccessEnv(truthLayer);
                if (Memory.ContainsKey(classIndex))
                    Memory[classIndex].Add(new Point(x, y));
                else
                    Memory.Add(classIndex, new List<Point>() { new Point(x, y) });
            } while (classIndex != -2);
            //remove empty value
            Memory.Remove(-2);
            Memory = Memory.Where(p => { return Convert.ToDouble(p.Key) < truthLayer.BandCollection[0].Max && Convert.ToDouble(p.Key) >= 0; }).OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            //reset cursor
           // truthLayer.BandCollection[0].ResetCursor();
            //key index
            List<int> Keys = Memory.Keys.ToList();
            //
            int actionsNumber = Keys.Count;
            int[,] matrix = new int[actionsNumber, actionsNumber];
            //
            for(int i=0;i<actionsNumber;i++)
            {
                int key = Keys[i];
                List<Point> points = Memory[key];
                //计算realKey类分类结果,存入混淆矩阵
                points.ForEach(p => {
                    int classificationType = (int)predLayer.BandCollection[0].GetRawValue(p.X, p.Y) - 1;
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
            return (matrix,kappa,actionsNumber);
        }

    }
}
