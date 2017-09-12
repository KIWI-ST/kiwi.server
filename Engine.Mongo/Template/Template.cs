using Engine.Mongo.Entity;
using Engine.Mongo.Operation;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Engine.Mongo
{
    /// 提供 push-pull-sync-transaction 操作的分布式库
    /// 支持:
    /// 同步-数据拉取-写入
    /// @modify yellow date 2016.11.21
    /// </summary>
    public class Template
    {
        /// <summary>
        /// 返回当前最新时间
        /// </summary>
        protected string Now
        {
            get
            {
                return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            }
        }
        /// <summary>
        /// 操作数据库名
        /// </summary>
        protected string _dataBaseName = null;
        /// <summary>
        /// 标识是否初始化，默认false
        /// </summary>
        protected bool _hasInilized = false;
        /// <summary>
        /// 持久化操作
        /// </summary>
        protected PushToMongoOperation _push = null;
        /// <summary>
        /// 缓存化操作
        /// </summary>
        protected PullToMemoryOperation _pull = null;
        /// <summary>
        /// 数据修复
        /// </summary>
        public virtual void RepairData() { }
        /// <summary>
        /// 内存对象初始化
        /// </summary>
        public virtual void Inilization(string connectString)
        {
            _hasInilized = true;
            _pull = new PullToMemoryOperation(_dataBaseName, connectString);
            _push = new PushToMongoOperation(_dataBaseName, connectString);
        }
        /// <summary>
        /// 数据更新,可重写，虚函数
        /// </summary>
        public virtual void Persistence() { }
        /// <summary>
        /// 检查数据增量，并同步内存和数据库
        /// </summary>
        protected void SyncMemoryDataBase() { }
        /// <summary>
        /// 虚函数，导入数据库
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="tableName">表名</param>
        /// <param name="dataName">数据表名，默认为 _dataBaseName 编译常量 </param>
        protected async Task<bool> ImportToDataBase<T>(List<T> list, string tableName) where T : MongoEntity, new()
        {
            var task = await _push.PushData<T>(tableName, list);
            return task;
        }
        /// <summary>
        /// 根据dictionary构造查询器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterDictionary"></param>
        /// <returns></returns>
        protected BsonDocumentFilterDefinition<T> BuildFilter<T>(Dictionary<string, string> filterDictionary)
        {
            var simpleQuery = new BsonDocument();
            foreach (var element in filterDictionary)
                simpleQuery.Add(new BsonElement(element.Key, BsonValue.Create(element.Value)));
            return new BsonDocumentFilterDefinition<T>(simpleQuery);
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dataBaseName">数据集名</param>
        /// <param name="tableName">表名</param>
        /// <param name="pullAction">回调函数</param>
        protected void ExportFromDataBase<T>(string tableName, Action<List<T>> pullAction = null, Dictionary<string, string> filterDictionary = null) where T : MongoEntity
        {
            BsonDocumentFilterDefinition<T> filter = null;
            if (filterDictionary != null)
                filter = BuildFilter<T>(filterDictionary);
            _pull.PullData<T>(tableName, pullAction, filter);
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="targetObjectId">待修改对象id</param>
        /// <param name="targetObject">新的对象值</param>
        /// <param name="collection">数据集（内存副本）</param>
        /// <param name="tableName">表名</param>
        protected async Task<bool> Modify<T>(string targetObjectId, T targetObject, List<T> collection, string tableName, List<string> forbidden = null) where T : MongoEntity
        {
            try
            {
                bool isUpdate = false;
                T omf = collection.Find(p => p.objectId.Equals(targetObjectId));
                if (omf == null || !omf.Verify()) return false;
                Type type = targetObject.GetType();
                foreach (PropertyInfo element in type.GetProperties())
                {
                    //筛选派生类属性，基础类属性予以修改
                    if (element.DeclaringType.Equals(type))
                    {
                        var newValue = element.GetValue(targetObject);
                        if (newValue == null) continue;
                        var sourceValue = element.GetValue(omf);
                        //新值与原值不等
                        if (!newValue.Equals(sourceValue))
                        {
                            if (forbidden != null && forbidden.Contains(element.Name))
                                return false;
                            //string型
                            if (newValue.GetType().Equals(typeof(string)))
                            {
                                if (newValue != null && !newValue.Equals(""))
                                {
                                    element.SetValue(omf, newValue);
                                    isUpdate = true;
                                }
                            }
                            //int型
                            else if (newValue.GetType().Equals(typeof(int)))
                            {
                                element.SetValue(omf, newValue);
                                isUpdate = true;
                            }
                            //double型
                            else if (newValue.GetType().Equals(typeof(double)))
                            {
                                if (!newValue.Equals(0.000000))
                                {
                                    element.SetValue(omf, newValue);
                                    isUpdate = true;
                                }
                            }
                            //bool型
                            else if (newValue.GetType().Equals(typeof(bool)))
                            {
                                element.SetValue(omf, newValue);
                                isUpdate = true;
                            }
                            //list类型
                            else if (typeof(IList).IsAssignableFrom(element.PropertyType))
                            {
                                if (newValue != null)
                                {
                                    element.SetValue(omf, newValue);
                                    isUpdate = true;
                                }
                            }
                        }
                    }
                }
                bool isSuccess = false;
                if (isUpdate)
                    isSuccess = await _push.Update(omf, tableName);
                return isSuccess;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="targetObjectId">被删除数据objectId</param>
        /// <param name="collection">数据集（内存副本）</param>
        /// <param name="tableName">表名</param>
        protected async Task<bool> Close<T>(string targetObjectId, List<T> collection, string tableName) where T : MongoEntity
        {
            T omf = collection.Find(p => p.objectId.Equals(targetObjectId));
            if (omf == null) return false;
            omf.closed = true;
            var flag = await _push.Update<T>(omf, tableName);
            if (flag)
                collection.Remove(omf);
            return flag;
        }
    }
}
