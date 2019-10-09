using Newtonsoft.Json.Linq;

namespace Engine.NLP.Process
{
    /// <summary>
    /// define regiment text (regroup) 
    /// </summary>
    public interface IRegimentTool:ITextTool
    {
        void RegimentTextByTimeline(string rawText);
    }
}
