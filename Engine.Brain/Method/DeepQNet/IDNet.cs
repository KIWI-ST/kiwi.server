using System.IO;

namespace Engine.Brain.Method.DeepQNet
{
    /// <summary>
    /// support dqn training and apply
    /// </summary>
    public interface IDNet : INet
    {
        /// <summary>
        /// persistence in memory
        /// </summary>
        Stream PersistenceMemory();

        /// <summary>
        /// copy sourceNet parameters to this Net
        /// </summary>
        /// <param name="sourceNet"></param>
        void Accept(IDNet sourceNet);
    }
}
