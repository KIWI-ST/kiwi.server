using Engine.NLP.Entity;

namespace Engine.NLP
{
    /// <summary>
    /// annotation text
    /// </summary>
    public interface IAnnotation
    {
    }

    public interface IScenarioAnnotation
    {
        Scenario Process(string rawText);
    }

}
