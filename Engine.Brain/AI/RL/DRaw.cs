namespace Engine.Brain.AI.RL
{
    public class DRaw
    {
        /// <summary>
        /// state数据
        /// </summary>
        public double[] State { get; set; }
        /// <summary>
        /// one-hot vector
        /// </summary>
        public double[] Action { get; set; }
        /// <summary>
        /// reward
        /// </summary>
        public double Reward { get; set; }
    }
}
