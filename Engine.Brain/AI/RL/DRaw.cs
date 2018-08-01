namespace Engine.Brain.AI.RL
{
    public class DRaw
    {
        /// <summary>
        /// state数据
        /// </summary>
        public float[] State { get; set; }
        /// <summary>
        /// one-hot vector
        /// </summary>
        public float[] Action { get; set; }
        /// <summary>
        /// reward
        /// </summary>
        public float Reward { get; set; }
    }
}
