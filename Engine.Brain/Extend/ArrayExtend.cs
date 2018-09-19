namespace Engine.Brain.Extend
{
    public static class ArrayExtend
    {
        /// <summary>
        /// 
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
    }
}
