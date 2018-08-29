using Engine.Mongo.Entity;

namespace Engine.Mongo.Extend
{
    public static class StringExtend
    {
        /// <summary>
        /// 序列化mongo数据对象
        /// </summary>
        /// <typeparam name="T">mongo数据类型，继承BaseMongoType</typeparam>
        /// <param name="content">待序列化文本</param>
        /// <returns>T 类型对象</returns>
        public static T DeserializeMongoObject<T>(this string content) where T : MongoEntity
        {
            try
            {
                T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                if (!obj.Verify())
                    return null;
                return obj;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 序列化普通对象
        /// </summary>
        /// <typeparam name="T">可new的普通对象类型</typeparam>
        /// <param name="content">待序列化文本</param>
        /// <returns>T 类型对象</returns>
        public static T DeserializeObject<T>(this string content) where T : new()
        {
            try
            {
                T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                return obj;
            }
            catch
            {
                return default(T);
            }
        }

    }
}
