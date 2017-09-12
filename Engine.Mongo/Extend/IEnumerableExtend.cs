using Engine.Mongo.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Engine.Mongo.Extend
{
    public static class IEnumerableExtend
    {
        /// <summary>
        /// 转换为list对象，转换失败返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        public static List<T> ToList2<T>(this IEnumerable<T> enumerable)
        {
            try
            {
                if (enumerable.First() != null)
                    return enumerable.ToList<T>();
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// dynamic.linq 动态查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="condition"></param>
        public static IEnumerable<T> Condition<T>(this IEnumerable<T> enumerable, string condition) where T : MongoEntity
        {
            return enumerable.Where(condition);
        }
        /// <summary>
        /// 获取字符串组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="num"></param>
        /// <returns>List<T> 集合</returns>
        public static IEnumerable<T> GetRange<T>(this IEnumerable<T> list, int start, int num)
        {
            if (list == null)
                return null;
            int count = list.Count();
            if (count > start + num)
                return list.Skip(start).Take(num);
            else if (count > start && count <= start + num)
                return list.Skip(start).Take(count - start);
            return null;
        }
        /// <summary>
        /// 获取唯一值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static T One<T>(this IEnumerable<T> list) where T : MongoEntity
        {
            try
            {
                var one = list.Count() > 0 ? list.First() : null;
                return one;
            }
            catch
            {
                return null;
            }
        }
    }
}
