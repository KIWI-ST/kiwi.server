using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.Extend
{
    public static class ListExtend
    {
        /// <summary>
        /// 随机从数组中取出数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T RandomTake<T>(this List<T> list)
        {
            int index = new Random().Next(list.Count);
            T item = list[index];
            return item;
        } 

    }
}
