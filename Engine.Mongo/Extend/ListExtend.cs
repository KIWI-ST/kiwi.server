using Engine.Mongo.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;

namespace Engine.Mongo.Extend
{
    /// <summary>
    /// 扩展原生list方法的getRange，返回不超过上限的item枚举
    /// </summary>
    public static class ListExtend
    {
        /// <summary>
        /// 获取字符串组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="num"></param>
        /// <returns>List<T> 集合</returns>
        public static List<T> GetRange<T>(this List<T> list, int start, int num)
        {
            if (list == null)
                return null;
            int count = list.Count;
            if (count > start + num)
                return list.GetRange(start, num);
            else if (count > start && count <= start + num)
                return list.GetRange(start, count - start);
            return null;
        }
        /// <summary>
        /// 扩展linq查询，直接组合某些string字段的值，拼成一个大字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="splitStr"></param>
        /// <returns>string类型</returns>
        public static string Join<T>(this List<T> list, string splitStr = ",")
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in list)
            {
                builder.Append(item);
                builder.Append(splitStr);
            }
            return builder.ToString().Trim(splitStr.ToCharArray());
        }
        /// <summary>
        /// dynamic.linq 动态查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="condition"></param>
        public static IEnumerable<T> Condition<T>(this List<T> list, string condition) where T : MongoEntity
        {
            try
            {
                return list.Where(condition);
            }
            catch
            {
                return null;
            }
        }
    }
}
