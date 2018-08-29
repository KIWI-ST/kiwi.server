using MongoDB.Driver;

namespace Engine.Mongo.Operation
{
    /// <summary>
    /// MongoDB基础操作接口
    /// </summary>
    public interface IMongoOperation
    {
        /// <summary>
        /// 客户端连接
        /// </summary>
        IMongoClient Client { get; }
        /// <summary>
        /// 数据库连接
        /// </summary>
        IMongoDatabase DataBase { get; }
    }
}
