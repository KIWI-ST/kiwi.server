using System;
using System.Collections.Generic;
using TensorFlow;

namespace Engine.Brain.Entity
{
    public class Samples
    {



        /// <summary>
        /// 随机构建训练样本
        /// </summary>
        /// <param name="dimension">样本 features count</param>
        /// <param name="count">样本数量</param>
        /// <returns></returns>
        public static List<double> CreateInputs(int dimension = 64, int count = 10)
        {
            var inputs = new List<List<double>>();
            var random = new Random();
            //构建指定feature数目的多样本集合
            for (int i = 0; i < count; i++)
            {
                var input = new List<double>();
                for (int j = 0; j < dimension; j++)
                {
                    var num = random.Next(255);
                    input.Add(num);
                }
                inputs.Add(input);
            }
            //转换成一纬数组
            var outputs = new List<double>();
            inputs.ForEach(p =>
            {
                outputs.AddRange(p);
            });
            //返回一维数组，备用
            return outputs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">样本数量</param>
        /// <returns></returns>
        public static List<double> CreateLabels(int count = 10)
        {
            var labels = new List<double>();
            var random = new Random();
            //构建多样本的输出label
            for (int i = 0; i < count; i++)
            {
                var label = random.Next(10);
                labels.Add(label);
            }
            //返回一维数组，备用
            return labels;
        }
        /// <summary>
        /// 构建一个随机数组成的tensor
        /// </summary>
        /// <returns></returns>
        public static TFTensor CreateTensorWithRandomDouble(TFShape shape)
        {
            return null;
        }


    }
}
