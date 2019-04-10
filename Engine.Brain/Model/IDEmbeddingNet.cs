namespace Engine.Brain.Model
{
    public interface IDEmbeddingNet:IDNet
    {
        /// <summary>
        /// get the weight
        /// </summary>
        double[][] W { get; }
    }
}
