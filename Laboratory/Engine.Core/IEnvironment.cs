using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Engine.Core
{
    /// <summary>
    /// 环境配置，用于定义、读取各自方法的资源库
    /// </summary>
    public interface IEnvironment
    {
        //委托实现
        void PoolFunc(object state);
    }

    public delegate void WaitCallback(object state);

}
