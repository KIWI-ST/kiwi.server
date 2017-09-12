using Engine.Mongo.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine.Mongo.Operation
{
    /// <summary>
    /// 将内存数据存入数据库
    /// </summary>
    public class PushToMongoOperation : MongoOperation, IPushToMongoOperation
    {
        /// <summary>
        /// 构造函数，初始化持久化数据库
        /// </summary>
        /// <param name="dataName">数据库名</param>
        /// <param name="connectString">连接字符串</param>
        public PushToMongoOperation(string dataName, string connectString) : base(dataName, connectString) { }
        /// <summary>
        /// 更新模式写入数据集
        /// </summary>
        /// <typeparam name="T">类型名称</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="list">数据集</param>
        public async Task<bool> PushData<T>(string tableName, List<T> list, bool updateModal = true) where T : MongoEntity, new()
        {
            try
            {
                bool rlt = false;
                if (updateModal) //更新模式下，插入数据集需要逐条检查数据
                    rlt = await this.InsertManyDataExcludeIdentical<T>(list, tableName);
                else
                    rlt = await this.InsertDataManyAsync<T>(list, tableName);
                return rlt;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 更新模式写入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="data">单条数据内容</param>
        /// <param name="updateModal">更新模式，ture表示更新模式，false 表示数据insert模式</param>
        public async Task<bool> PushData<T>(string tableName, T data, bool updateModal = true) where T : MongoEntity
        {
            try
            {
                bool rlt = false;
                if (updateModal)
                    rlt = await this.InsertOneDataExcludeIdentical<T>(data, tableName);
                else
                    rlt = await this.InsertData(data, tableName);
                return rlt;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// replace 方式更新单条数据
        /// 针对多重嵌套数组类型，可能会新增旁支list时使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="data">替换数据</param>
        public async Task<bool> ReplaceData<T>(string tableName, T data) where T : MongoEntity
        {
            try
            {
                bool rlt = false;
                rlt = await this.ReplaceOne<T>(data, DataBase.GetCollection<T>(tableName));
                return rlt;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">待更新数据</param>
        /// <param name="tableName">表名</param>
        public async Task<bool> Update<T>(T data, string tableName) where T : MongoEntity
        {
            return await UpdateOne<T>(data, DataBase.GetCollection<T>(tableName));
        }
    }
}
