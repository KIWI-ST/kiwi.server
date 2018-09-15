using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GIS.Extend
{
    public static class ListExtend
    {
        /// <summary>
        /// 随机从数组中取出数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int IndexToKey(this List<int> list,int index)
        {
            return list[index];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int KeyToIndex(this List<int> list,int value)
        {
            var index =  list.IndexOf(value);
            return index;
        }
    }
}
       