using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Engine.Mongo.Entity
{
    /// <summary>
    /// 基础mongo数据oop类型，如果需要存储进数据库，需要继承自此类
    /// </summary>
    public class MongoEntity : Entity, IDate, IVerify, IExprie, ICloneable
    {
        /// <summary>
        /// 数据生成日期
        /// </summary>
        public string date { set; get; } = DateTime.Now.ToLongDateString();
        /// <summary>
        /// 数据生成时间
        /// </summary>
        public string time { set; get; } = DateTime.Now.ToLongTimeString();

        #region 内置方法

        /// <summary>
        /// 构造函数，初始化参数，默认为全区域可用
        /// </summary>
        public MongoEntity() { }
        /// <summary>
        /// 用于验证数据完整性，默认只检查regionId，返回true为通过
        /// </summary>
        public virtual bool Verify() { return true; }
        /// <summary>
        /// 校验数据有效性
        /// </summary>
        public virtual bool Expire() { return true; }
        /// <summary>
        /// 设置类型属性
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        public bool SetValue(string propertyName, object value)
        {
            try
            {
                Type t = this.GetType();
                t.GetProperty(propertyName).SetValue(this, value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// copy类，只copy public 类型，且为浅拷贝
        /// 拷贝成功则为新的对象，失败则为 null
        /// </summary>
        public T Copy<T>() where T : MongoEntity, new()
        {
            var result = new T();
            try
            {
                Type t = typeof(T);
                foreach (var element in t.GetProperties())
                {
                    if (element.CanWrite)
                    {
                        element.SetValue(result, element.GetValue(this));
                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 默认创建guid唯一值filter
        /// </summary>
        public BsonDocumentFilterDefinition<T> ToFilter<T>(List<string> keyNames = null) where T : MongoEntity
        {
            var qType = this.GetType();
            var simpleQuery = new BsonDocument();

            if (keyNames == null)
                keyNames = new List<string>();
            if (!keyNames.Contains("guid"))
                keyNames.Add("guid");

            foreach (var item in qType.GetProperties())
            {
                if (item.PropertyType.IsClass && item.PropertyType != typeof(string))
                    continue;
                else
                {
                    if (item.GetValue(this) == null || item.GetValue(this).ToString().Equals("") || !keyNames.Contains(item.Name) || !item.CanRead)
                        continue;
                    //简单类型,ValueType和string
                    simpleQuery.Add(new BsonElement(item.Name, BsonValue.Create(item.GetValue(this))));
                }
            }
            return new BsonDocumentFilterDefinition<T>(simpleQuery);
        }
        /// <summary>
        /// 内存浅表副本
        /// </summary>
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}
