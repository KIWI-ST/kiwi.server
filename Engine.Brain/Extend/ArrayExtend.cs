namespace Engine.Brain.Extend
{
    public static class ArrayExtend
    {
        /// <summary>
        /// combine encode , example :
        /// [0,1,1,0,0] + [0,0,0,1,1] = [0,1,1,1,1]
        /// </summary>
        /// <param name="array"></param>
        /// <param name="otherArray"></param>
        /// <returns></returns>
        public static double[] CombineOneHot(this double[] array,double[] otherArray)
        {
            for (int i = 0; i < otherArray.Length; i++)
                array[i] = otherArray[i] > 0 ? otherArray[i] : array[i];
            return array;
        }
        /// <summary>
        /// calcute the product of array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int Product(this int[] array)
        {
            int product = 1;
            for (int i = 0; i < array.Length; i++)
                product *= array[i];
            return product;
        }
    }
}
