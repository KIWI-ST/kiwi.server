namespace Engine.Mongo.Entity
{
    /// <summary>
    /// 需要自检状态的类型
    /// </summary>
    public class MongoEntity2: MongoEntity,Inspect
    {
        /// <summary>
        /// 自检状态函数
        /// </summary>
        public virtual bool SelfInspection() { return true; }
    }
}
