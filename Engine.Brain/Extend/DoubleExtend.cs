using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.Extend
{
    public static class DoubleExtend
    {
        /// <summary>
        /// double[] to float support
        /// </summary>
        /// <param name="array"></param>
        /// <returns>float[]</returns>
        public static float[] toFloat(this double[] array)
        {
            float[] arr = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
                arr[i] = (float)array[i];
            return arr;
        }

        /// <summary>
        /// doulbe[][] to float[][]
        /// </summary>
        /// <param name="array"></param>
        /// <returns>float[][]</returns>
        public static float[][] toFloat(this double[][] array)
        {
            int n0 = array.GetLength(0);
            float[][] arr = new float[n0][];
            for (int i = 0; i < n0; i++)
            {
                int n1 = array[i].Length;
                arr[i] = new float[n1];
                for (int k = 0; k < n1; k++)
                    arr[i][k] = (float)array[i][k];
            }
            return arr;
        }

        /// <summary>
        /// support the first value of the arrary convert to Intger label value
        /// </summary>
        /// <param name="array"></param>
        /// <returns>int[]</returns>
        public static int[] toLabelInt(this double[][] array)
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
