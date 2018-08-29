using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Mongo.Entity
{
    /// <summary>
    /// 每条记录的时间和日期
    /// </summary>
    public interface IDate
    {
        string date { get; set; }
        string time { get; set; }
    }
}
