﻿using System.Collections.Generic;
using Engine.Brain.Utils;

namespace Engine.Brain.Extend
{
    public static class ListExtend
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="bacthSize"></param>
        /// <returns></returns>
        public static List<T> RandomTakeBatch<T>(this List<T> list, int limitSize = 200)
        {
            int num = list.Count;
            List<T> dist = new List<T>();
            for (int i = 0; i < limitSize; i++)
            {
                int key = NP.Random(num);
                dist.Add(list[key]);
            }
            return dist;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="limitSize"></param>
        /// <returns></returns>
        public static List<T> LerpTakeBatch<T>(this List<T> list, int limitSize=200)
        {
            int num = list.Count;
            if (num <= limitSize)
                return list;
            List<T> dist = new List<T>();
            int lerp = num / limitSize;
            for (int i = 0; i < num; i++)
                if (i % lerp == 0) dist.Add(list[i]);
            return dist;
        }
        /// <summary>
        /// 随机从数组中取出数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// 
        public static T RandomTake<T>(this List<T> list)
        {
            int index = NP.Random(list.Count);
            T item = list[index];
            return item;
        }

        public static void RandomRemove<T>(this List<T> list,int capacity)
        {
            if(list.Count>=capacity)
            {
                int index = NP.Random(list.Count);
                list.RemoveAt(index);
            }
        }

        public static void DequeRemove<T>(this List<T> list,int capacity)
        {
            if (list.Count > capacity)
                list.RemoveAt(0);
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
