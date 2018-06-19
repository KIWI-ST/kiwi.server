using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.DL.Entity
{
    /// <summary>
    /// 定义神经网络基本结构
    /// TInput input 数据类
    /// TOutput predictiton 数据类
    /// </summary>
    public interface INeuralNetwork<TInput, TOutput>
        where TInput : class
        where TOutput : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<PredictionModel<TInput, TOutput>> TrainAsync();
    }
}
