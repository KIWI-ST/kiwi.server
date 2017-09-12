using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Mongo.Entity
{
    /// <summary>
    /// 验证数据是否有效
    /// </summary>
    public interface IExprie
    {
        /// <summary>
        /// 校验数据是否有效，一般用于下单等
        /// </summary>
        /// <returns>bool，true为有效，false为无效</returns>
        bool Expire();
    }
}
