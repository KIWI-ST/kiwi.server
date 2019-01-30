using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.Extend
{
    public static class DictionaryExtend
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="capaciaty">the maximum sample size of each class</param>
        /// <returns></returns>
        public static Dictionary<T1, List<T2>> LimitedDictionaryCapcaity<T1, T2>(this Dictionary<T1, List<T2>> dict, int capaciaty = 200, bool learpPick = true) where T2 : new()
        {
            Dictionary<T1, List<T2>> newDic = new Dictionary<T1, List<T2>>();
            foreach (var element in dict)
            {
                if(learpPick)
                    newDic[element.Key] = element.Value.LerpTakeBatch(capaciaty);
                else
                    newDic[element.Key] = element.Value.RandomTakeBatch(capaciaty);
            }
                
            return newDic;
        }
    }
}
