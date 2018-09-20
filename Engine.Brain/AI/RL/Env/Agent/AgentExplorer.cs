using System.Collections.Generic;

namespace Engine.Brain.AI.RL.Env.Agent
{
    public class Achievement
    {
        /// <summary>
        /// action (one-hot)
        /// </summary>
        public double[] Action { get; set; }
        /// <summary>
        /// position x
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// position y
        /// </summary>
        public int Y { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AgentExplorer
    {
        /// <summary>
        /// 
        /// </summary>
        List<Achievement> _achievements = new List<Achievement>();
        /// <summary>
        /// 
        /// </summary>
        AgentManager _manager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentAgentExplorer"></param>
        /// <param name="manager"></param>
        public AgentExplorer(AgentExplorer parentAgentExplorer,AgentManager manager)
        {
            Parent = parentAgentExplorer;
            _manager = manager;
        }
        /// <summary>
        /// store previous explorer
        /// </summary>
        public AgentExplorer Parent { get;}
        /// <summary>
        /// next
        /// </summary>
        public List<AgentExplorer> Children { get;}



    }
}
