namespace Engine.Brain.Method
{
    /// <summary>
    /// loading percentage
    /// </summary>
    /// <param name="percentage"></param>
    public delegate void LoadingEventHandler(double percentage);

    public interface IDEmbeddingNet : IMachineLarning
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
        /// word mapping to vector
        /// </summary>
        /// <param name="word"></param>
        /// <returns>word vector</returns>
        float[] MappingToVector(string word);

        /// <summary>
        /// 
        /// </summary>
        event LoadingEventHandler OnLoading;
    }
}
