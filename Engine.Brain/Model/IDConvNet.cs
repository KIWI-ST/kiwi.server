﻿namespace Engine.Brain.Model
{
    /// <summary>
    /// suitable for deep feature extract
    /// </summary>
    public interface IDConvNet : IDNet
    {
        /// <summary>
        /// remove softmax, convert it to Extract Feature Network
        /// </summary>
        void ConvertToExtractNetwork();
    }
}