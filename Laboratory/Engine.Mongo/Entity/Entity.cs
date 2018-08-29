using MongoDB.Bson;
using System;
using Newtonsoft.Json;

namespace Engine.Mongo.Entity
{
    /// <summary>
    /// mogodb 数据存储结构对象基础类
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 系统所需键值，根据时间序列生成，无需显示
        /// </summary>
        [JsonIgnore]
        public ObjectId _id { get; set; }
        /// <summary>
        /// 内置，是否关闭此条信息，默认为flase表示未关闭（不显示此字段）
        /// </summary>
        [JsonIgnore]
        public bool closed { get; set; }
        /// <summary>
        /// 构造时，创建objectId具体值
        /// </summary>
        public string guid { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        /// <summary>
        /// 返回objectId的字符串表达，set 保留功能，仅仅为序列化留存
        /// </summary>
        public string objectId { get { return guid; } }
    }
}
