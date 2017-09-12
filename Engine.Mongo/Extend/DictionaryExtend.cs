using System;
using System.Collections.Generic;

namespace Engine.Mongo.Extend
{
    public static class DictionaryExtend
    {
        private static SortedDictionary<string, string> GetSortedDictionary(this SortedDictionary<string, string> dict, Func<string, bool> filter = null)
        {
            //获取排序的键值对  
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            if (dict != null && dict.Count > 0)
            {
                foreach (var k in dict.Keys)
                {
                    if (filter == null || !filter(k))
                    {
                        //如果没设置过滤条件或者无需过滤  
                        dic.Add(k, dict[k]);
                    }
                }
            }
            return dic;
        }
    }
}
