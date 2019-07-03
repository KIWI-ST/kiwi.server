namespace Engine.NLP
{
    /// <summary>
    /// annotation text
    /// </summary>
    public interface IAnnotation
    {
        void Process(string rawText);
    }
}
