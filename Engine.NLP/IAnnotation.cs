using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.NLP
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAnnotation
    {
        void Process(string rawText);
    }
}
