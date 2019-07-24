namespace Engine.Brain.Model
{
    /// <summary>
    /// loading percentage
    /// </summary>
    /// <param name="percentage"></param>
    public delegate void LoadingEventHandler(double percentage);

    public interface IDEmbeddingNet:IDNet
    {
        /// <summary>
        /// get the weight
        /// </summary>
        float[][] W { get; }

        /// <summary>
        /// load model
        /// </summary>
        void Load();

        /// <summary>
        /// 
        /// </summary>
        event LoadingEventHandler OnLoading;
    }
}
