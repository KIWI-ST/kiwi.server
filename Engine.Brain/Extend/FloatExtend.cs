using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.Extend
{
    public static class FloatExtend
    {

        /// <summary>
        /// support float[] to double[]
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[] toDouble(this float[] array)
        {
            double[] arr = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                arr[i] = Convert.ToDouble(array[i]);
            return arr;
        }

        /// <summary>
        /// support float[][] to double[][]
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[][] toDouble(this float[][] array)
        {
            int n0 = array.GetLength(0);
            double[][] arr = new double[n0][];
            for (int i = 0; i < n0; i++)
            {
                int n1 = array[i].Length;
                arr[i] = new double[n1];
                for (int k = 0; k < n1; k++)
                    arr[i][k] = Convert.ToDouble(array[i][k]);
            }
            return arr;
        }

        /// <summary>
        /// support the first value of the arrary convert to Intger label value
        /// </summary>
        /// <param name="array"></param>
        /// <returns>int[]</returns>
        public static int[] toLabelInt(this float[][] array)
        {
            int n0 = array.GetLength(0);
            int[] arr = new int[n0];
            for (int i = 0; i < n0; i++)
            {
                int n1 = array[i].Length;
                double v0 = array[i][0];
                arr[i] = Convert.ToInt32(v0);
            }
            return arr;
        }

    }
}
