using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Mongo.Entity
{
    /// <summary>
    /// 用于验证数据结构是否完整
    /// </summary>
    public interface IVerify
    {
        /// <summary>
        /// 校验数据结构完整（非空字段等逻辑）
        /// </summary>
        /// <returns>bool，true表示完整，false表示不完整</returns>
        bool Verify();
        /// <summary>
        /// 通过反射为值赋值，只支持基础类型，string,int,double,List （注意不支持float）
        /// </summary>
        bool SetValue(string propertyName, object value);
    }
}
