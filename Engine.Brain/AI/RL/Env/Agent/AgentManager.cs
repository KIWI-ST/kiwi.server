using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Brain.AI.RL.Env.Agent
{
    public class AgentManager
    {
        AgentExplorer _seedAgent;

        List<AgentExplorer> CurrentAgents { get; }

        public AgentExplorer ResetExplorer (int x,int y)
        {
            _seedAgent = new AgentExplorer(null,this);
            CurrentAgents.Clear();
            CurrentAgents.Add(_seedAgent);
            return _seedAgent;
        }

        public AgentExplorer Create(AgentExplorer agentExplorer)
        {
            AgentExplorer explorer = new AgentExplorer(agentExplorer, this);
            CurrentAgents.Add(explorer);
            return explorer;
        }


    }
}
