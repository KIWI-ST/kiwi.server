using Engine.Mongo.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine.Mongo.Operation
{
    /// <summary>
    /// 获取对象数据，载入内存
    /// </summary>
    public class PullToMemoryOperation : MongoOperation, IPullToMemoryOperation
    {
        /// <summary>
        /// 将mongodb数据拉取到内存
        /// </summary>
        /// <param name="dataName">数据库名</param>
        /// <param name="connectString">连接字符串</param>
        public PullToMemoryOperation(string dataName, string connectString) : base(dataName, connectString) { }
        /// <summary>
        ///  将数据全部拉取到内存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">待拉取表名</param>
        /// <param name="queryAllData">查询委托</param>
        /// <param name="callback">完成后的回调委托</param>
        public async void PullData<T>(string tableName, Action<List<T>> callback, FilterDefinition<T> filter = null, Func<string, FilterDefinition<T>, Task<List<T>>> queryDataMethod = null) where T : MongoEntity
        {
            //异步处理(委托处理方法为QueryDataMethod<T>
            var result = await QueryDataAsync(tableName, filter, queryDataMethod);
            callback?.Invoke(result);
        }
    }
}
