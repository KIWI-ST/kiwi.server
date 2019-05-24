using System.IO;

namespace Engine.Brain.Model
{
    /// <summary>
    /// support dqn training
    /// </summary>
    public interface IDSupportDQN:IDNet
    {
        /// <summary>
        /// persistence in memory
        /// </summary>
        Stream PersistenceMemory();

        /// <summary>
        /// copy sourceNet parameters to this Net
        /// </summary>
        /// <param name="sourceNet"></param>
        void Accept(IDSupportDQN sourceNet);
    }
}
