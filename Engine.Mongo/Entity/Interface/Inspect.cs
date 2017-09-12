using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Mongo.Entity
{
    /// <summary>
    /// 自检状态接口
    /// </summary>
    public interface Inspect
    {
        bool SelfInspection();
    }
}
