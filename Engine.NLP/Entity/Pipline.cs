using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.NLP.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Pipunit
    {

        List<string> _properties = new List<string>();

        public string Target { get; private set; }

        public Pipunit(string target)
        {
            Target = target;
        }

        public void AddDesc(string property)
        {
            _properties.Add(property);
        }

    }

    /// <summary>
    /// 记录情景内的一个独立流程
    /// 每个独立流程必然包含：
    /// 1. 主体， 例如: 油轮
    /// 2. 动态， 例如: 卸油
    /// 3. 相关描述，例如，邮轮-30吨， 卸油-15吨，卸油-2号管道，卸油-原油灌区
    /// </summary>
    public class Pipline
    {
        Pipunit _entity, _trend;

        public Pipline(Pipunit entity, Pipunit trend)
        {
            _entity = entity;
            _trend = trend;
        }

    }
}
