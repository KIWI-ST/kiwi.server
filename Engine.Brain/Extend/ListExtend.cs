using Engine.Brain.Entity;
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
            int index = NP.Random(list.Count);
            T item = list[index];
            return item;
        }

        public static void RandomDispose<T>(this List<T> list) where T:IDisposable
        {
            T item = list[NP.Random(list.Count)];
            list.Remove(item);
            item.Dispose();
        }

        public static void RandomRemove<T>(this List<T> list)
        {
            int index = NP.Random(list.Count);
            list.RemoveAt(index);
        }

        public static T Next<T>(this List<T> list) where T:class
        {
            int count = list.Count;
            if (count > 0)
            {
                T item = list[0];
                list.Remove(item);
                return item;
            }
            else
                return null;
        }

    }
}
