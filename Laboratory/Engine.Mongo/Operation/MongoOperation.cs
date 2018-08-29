using Engine.Mongo.Entity;
using Engine.Mongo.Extend;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Engine.Mongo.Operation
{
    /// <summary>
    /// 数据库操作基础类
    /// 默认数据库连接无需密码
    /// mongodb://admin:!admin_1@139.129.7.130
    /// </summary>
    public class MongoOperation : IMongoOperation
    {
        /// <summary>
        /// 连接客户端，创建本地连接
        /// </summary>
        IMongoClient _client;
        /// <summary>
        /// client 连接客户端
        /// </summary>
        public IMongoClient Client { get { return _client; } }
        /// <summary>
        ///  获取指定名称的DataBase
        /// </summary>
        private IMongoDatabase _dataBase;
        /// <summary>
        /// 数据库
        /// </summary>
        public IMongoDatabase DataBase { get { return _dataBase; } }
        /// <summary>
        /// 开启对某个数据库的操作
        /// </summary>
        /// <param name="dataName">数据库名，如"Poor"</param>
        /// [ImportingConstructor]
        public MongoOperation(string dataName, string connectString)
        {
            _client = new MongoClient(connectString);
            _dataBase = _client.GetDatabase(dataName);
        }

        #region 内部方法
        /// <summary>
        /// 以guid为核心创建唯一索引
        /// </summary>
        /// <typeparam name="T">数据映射类型</typeparam>
        /// <param name="tablename">需要建立索引的表名</param>
        /// <returns></returns>
        public bool BuildUniqueIndex<T>(string tablename) where T : MongoEntity
        {
            var collection = _dataBase.GetCollection<T>(tablename);
            //创建guide的索引
            var indexItem = Builders<T>.IndexKeys.Ascending(p => p.guid);
            var indexOption = new CreateIndexOptions() { Unique = true };
            //判断是否已索引
            var cursors = collection.Indexes.List().ToList();
            foreach (var element in cursors)
            {
                var st = element.GetElement("key").ToString().Equals("key={ \"guid\" : 1 }");
                if (st)
                    return true;
            }
            var name = collection.Indexes.CreateOne(indexItem, indexOption);
            return true;
        }
        #endregion

        #region 插入方法集
        /// <summary>
        /// 插入数据集，
        /// 1.判断单条数据是否已存在，如果存在则使用更新此信息操作
        /// 2.
        /// 1.如果插入的数据集
        /// 如果能确认数据均在数据库中未记录，可以直接使用
        /// InsertDataMany<T>(List<T> datas, string tableName)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="tableName"></param>
        public async Task<bool> InsertManyDataExcludeIdentical<T>(List<T> datas, string tableName) where T : MongoEntity, new()
        {
            try
            {
                var collection = _dataBase.GetCollection<T>(tableName);
                List<T> waitInsert = new List<T>();
                for (int i = 0, count = datas.Count; i < count; i++)
                {
                    //查询单挑数据
                    var queryResults = await collection.CountAsync(datas[i].ToFilter<T>());
                    //更新模式
                    if (queryResults > 0)
                        await UpdateOne<T>(datas[i], collection);
                    //直接插入数据模式
                    else
                        waitInsert.Add(datas[i]);
                }
                //插入未记录数据
                var result = await InsertDataMany<T>(waitInsert, tableName);
                return result;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 插入单挑数据
        /// 1.判断单条数据是否已存在，如果存在则使用更新此信息操作
        /// 如果能确认数据均在数据库中未记录，可以直接使用
        /// InsertData<T>(T data, string tableName)
        /// </summary>
        public async Task<bool> InsertOneDataExcludeIdentical<T>(T data, string tableName) where T : MongoEntity
        {
            bool result = false;
            var collection = _dataBase.GetCollection<T>(tableName);
            var queryFlag = await collection.CountAsync(data.ToFilter<T>());
            //更新模式
            if (queryFlag > 0)
                result = await UpdateOne<T>(data, collection);
            //直接插入数据模式
            else
                result = await InsertData<T>(data, tableName);
            return result;
        }
        /// <summary>
        /// 将数据对象转换为BsonDocument对象，并存在tableName表下
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName">表名</param>
        protected async Task<bool> InsertData<T>(T data, string tableName) where T : MongoEntity
        {
            try
            {
                //这里未修改，修正为：插入不重复的条目
                var collection = _dataBase.GetCollection<T>(tableName);
                await collection.InsertOneAsync(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 批量插入，基于并行原理，不影响性能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="tableName"></param>
        protected async Task<bool> InsertDataMany<T>(List<T> datas, string tableName) where T : MongoEntity, new()
        {
            try
            {
                var collection = _dataBase.GetCollection<T>(tableName);
                await collection.InsertManyAsync(datas);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 异步插入
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">插入数据项</param>
        /// <param name="tableName">表名</param>
        /// <param name="callback">回调插入结果</param>
        /// <param name="function">默认插入方法</param>
        protected async void InsertDataAsync<T>(T data, string tableName, Action<bool> callback, Func<T, string, Task<bool>> function = null) where T : MongoEntity, new()
        {
            //默认单条插入方法
            function = function == null ? InsertData : function;
            Func<Task<bool>> taskFunc = () =>
            {
                return Task.Run(() =>
                {
                    return function(data, tableName);
                });
            };
            bool rlt = false;
            try
            {
                rlt = await taskFunc();
            }
            catch
            {

            }
            finally
            {
                callback?.Invoke(rlt);
            }
        }
        /// <summary>
        /// 多条并行插入方法，不考虑去重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">待插入数据集</param>
        /// <param name="tableName">表名</param>
        /// <param name="callback">完成回调</param>
        /// <param name="function">可自定义插入方法，默认为 InsertDataMany 方法</param>
        protected async Task<bool> InsertDataManyAsync<T>(List<T> data, string tableName, Func<List<T>, string, Task<bool>> function = null) where T : MongoEntity, new()
        {
            //默认单条插入方法
            function = function == null ? InsertDataMany : function;
            Func<System.Threading.Tasks.Task<bool>> taskFunc = () =>
            {
                return System.Threading.Tasks.Task.Run(() =>
                {
                    return function(data, tableName);
                });
            };
            bool rlt = false;
            try
            {
                rlt = await taskFunc();
                return rlt;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 查询方法集

        /// <summary>
        /// 默认遍历规则/方法，不适用单条查询，默认查询未被关闭的数据集
        /// </summary>
        public async Task<List<T>> DefaultQueryDataMethod<T>(string tableName, FilterDefinition<T> filter) where T : MongoEntity
        {
            var collection = _dataBase.GetCollection<T>(tableName);
            //查询器，如果不存在则构造，如果存在则检查是否添加此规则
            if (filter == null)
            {
                var simpleQuery = new BsonDocument();
                simpleQuery.Add(new BsonElement("closed", BsonValue.Create(false)));
                filter = new BsonDocumentFilterDefinition<T>(simpleQuery);
            }
            else
            {
                var simpleQuery = filter.ToBsonDocument();
                if (!simpleQuery.Contains("closed"))
                    simpleQuery.Add(new BsonElement("closed", BsonValue.Create(false)));
                filter = new BsonDocumentFilterDefinition<T>(simpleQuery);
            }
            List<T> results = new List<T>();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        results.Add(document);
                    }
                }
            }
            return results;
        }
        /// <summary>
        /// 根据查询条件获取反馈结果，需要自行写查询代码
        /// 默认使用QueryData方法，带有回调
        /// </summary>
        /// <typeparam name="T">返回数据结构</typeparam>
        /// <param name="data">数据模板</param>
        /// <param name="tableName">表名</param>
        /// <param name="callback">回掉方法</param>
        /// <param name="function">查询方法，默认为游标法，可不填</param>
        protected async Task<List<T>> QueryDataAsync<T>(string tableName, FilterDefinition<T> filter = null, Func<string, FilterDefinition<T>, Task<List<T>>> QueryDataMethod = null) where T : MongoEntity
        {
            if (QueryDataMethod == null)
                QueryDataMethod = DefaultQueryDataMethod<T>;
            //默认采用QueryData方式查询（游标法）
            Func<Task<List<T>>> taskFunc = () =>
            {
                return System.Threading.Tasks.Task.Run(() =>
                {
                    return QueryDataMethod(tableName, filter);
                });
            };
            return await taskFunc();
        }

        #endregion

        #region 更新方法集

        /// <summary>
        /// 递归构建Update操作串
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="property"></param>
        /// <param name="propertyValue"></param>
        /// <param name="item"></param>
        /// <param name="father"></param>
        private void GenerateRecursion<T>(List<UpdateDefinition<T>> fieldList, PropertyInfo property, object propertyValue, T item, string father)
        {
            //复杂类型
            if (property.PropertyType.IsComplex() && propertyValue != null)
            {
                //处理简单类型集合
                if (propertyValue.GetType().IsList() && propertyValue.GetType().IsListChildPrimitive())
                {
                    var arr = propertyValue as IList;
                    for (int index = 0; index < arr.Count; index++)
                    {
                        if (string.IsNullOrWhiteSpace(father))
                            fieldList.Add(Builders<T>.Update.Set(property.Name + "." + index, arr[index]));
                        else
                            fieldList.Add(Builders<T>.Update.Set(father + "." + property.Name + "." + index, arr[index]));
                    }
                }
                //处理复杂类型集合
                else if (propertyValue.GetType().IsList() && propertyValue.GetType().IsListChildComplex())
                {
                    //判断list对象里的T是简单类型还是其他类型
                    IList arr1 = propertyValue as IList;
                    for (int index1 = 0; index1 < arr1.Count; index1++)
                    {
                        foreach (var subInner in arr1[index1].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            if (string.IsNullOrWhiteSpace(father))
                                GenerateRecursion(fieldList, subInner, subInner.GetValue(arr1[index1]), item, property.Name + "." + index1);
                            else
                                GenerateRecursion(fieldList, subInner, subInner.GetValue(arr1[index1]), item, father + "." + property.Name + "." + index1);
                        }
                    }
                }
                //普通对象
                else
                {
                    foreach (var sub in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {

                        if (string.IsNullOrWhiteSpace(father))
                            GenerateRecursion(fieldList, sub, sub.GetValue(propertyValue), item, property.Name);
                        else
                            GenerateRecursion(fieldList, sub, sub.GetValue(propertyValue), item, father + "." + property.Name);
                    }
                }
            }
            //简单类型
            else
            {
                //更新集中不能有实体键_id（EntityKey）
                if (property.Name != "_id" && property.Name != "objectId" && property.Name != "guid")
                {
                    if (string.IsNullOrWhiteSpace(father))
                        fieldList.Add(Builders<T>.Update.Set(property.Name, propertyValue));
                    else
                        fieldList.Add(Builders<T>.Update.Set(father + "." + property.Name, propertyValue));
                }
            }
        }
        /// <summary>
        /// 更新item
        /// </summary>
        /// <typeparam name="T">待更新数据类型</typeparam>
        /// <param name="item">更新项</param>
        /// <param name="collection">数据集</param>
        /// <returns>更新成功与否标识</returns>
        protected async Task<bool> UpdateOne<T>(T item, IMongoCollection<T> collection) where T : MongoEntity
        {
            try
            {
                var fieldList = new List<UpdateDefinition<T>>();
                foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    GenerateRecursion(fieldList, property, property.GetValue(item), item, string.Empty);
                var updateResult = await collection.UpdateOneAsync(item.ToFilter<T>(), Builders<T>.Update.Combine(fieldList));
                //没有找到指定项，即修改失败
                if (updateResult == null)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 替换更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected async Task<bool> ReplaceOne<T>(T item, IMongoCollection<T> collection) where T : MongoEntity
        {
            try
            {
                var replaceResult = await collection.ReplaceOneAsync(item.ToFilter<T>(), item);
                if (replaceResult.MatchedCount == 0)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 删除操作集

        protected async Task<bool> RemoveOne<T>(T item, IMongoCollection<T> collection) where T : MongoEntity
        {
            var filter = item.ToFilter<T>();
            var result = await collection.DeleteOneAsync(filter);
            if (result.DeletedCount == 0)
                return false;
            else
                return true;
        }

        protected async Task<bool> RemoveOne<T>(T item, string tableName) where T : MongoEntity
        {
            var result = await this.RemoveOne<T>(item, _dataBase.GetCollection<T>(tableName));
            return result;
        }

        protected async void RemoveOne<T>(T item, string tableName, Action<bool> callback, Func<T, string, Task<bool>> function = null) where T : MongoEntity
        {
            function = function == null ? RemoveOne : function;
            Func<System.Threading.Tasks.Task<bool>> taskFunc = () =>
            {
                return System.Threading.Tasks.Task.Run(() =>
                {
                    return function(item, tableName);
                });
            };
            bool rlt = await taskFunc();
            if (callback != null)
                callback(rlt);
        }

        #endregion

    }
}
