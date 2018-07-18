using System;
using System.Collections.Generic;
using System.Linq;
using TensorFlow;

namespace Engine.Brain.Entity
{
    public class NP
    {
        /// <summary>
        /// 归一化
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static float[] Normalize(byte[] inputs)
        {
            int count = inputs.Length;
            float[] normal = new float[inputs.Length];
            for (int i = 0; i < count; i++)
                normal[i] = inputs[i] / 255.0f;
            return normal;
        }
        /// <summary>
        /// onehot编码
        /// </summary>
        /// <param name="hotIndex"></param>
        /// <param name="hotLength"></param>
        /// <returns></returns>
        public static float[] ToOneHot(int hotIndex, int hotLength)
        {
            float[] oneHot = new float[hotLength];
            for (int i = 0; i < hotLength; i++)
                oneHot[i] = i == (hotIndex - 1) ? 1 : 0;
            return oneHot;
        }
        /// <summary>
        /// 随机构建训练样本
        /// </summary>
        /// <param name="oneDimensionCount">样本 features count</param>
        /// <param name="batchSize">样本数量</param>
        /// <returns></returns>
        public static List<float> CreateInputs(int oneDimensionCount = 64, int batchSize = 15)
        {
            var inputs = new List<List<float>>();
            var random = new Random();
            //构建指定feature数目的多样本集合
            for (int i = 0; i < batchSize; i++)
            {
                var input = new List<float>();
                for (int j = 0; j < oneDimensionCount; j++)
                {
                    var num = random.Next(10);
                    input.Add(num);
                }
                inputs.Add(input);
            }
            //转换成一纬数组
            var outputs = new List<float>();
            inputs.ForEach(p =>
            {
                outputs.AddRange(p);
            });
            //返回一维数组，备用
            return outputs;
        }
        /// <summary>
        /// 构建10个长度的oneHot编码结果样本集
        /// </summary>
        /// <param name="batchSzie">样本数量</param>
        /// <returns></returns>
        public static List<float> CreateLabels(int batchSzie = 15, int oneHot = 10)
        {
            var inputs = new List<float[]>();
            var random = new Random();
            //构建多样本的输出label
            for (int i = 0; i < batchSzie; i++)
            {
                var label = random.Next(10);
                inputs.Add(ToOneHot(label, oneHot));
            }
            var outputs = new List<float>();
            inputs.ForEach(p =>
            {
                outputs.AddRange(p);
            });
            //返回一维数组，备用
            return outputs;
        }
        /// <summary>
        /// 构建一个随机数组成的tensor
        /// </summary>
        /// <returns></returns>
        public static TFTensor CreateTensorWithRandomFloat(TFShape shape)
        {
            var random = new Random();
            var dimensions = shape.NumDimensions;
            int length = 1;
            List<float> array = new List<float>();
            for (var i = 0; i < dimensions; i++)
                length *= Convert.ToInt32(shape[i]);
            for (var i = 0; i < length; i++)
                array.Add((float)random.NextDouble());
            var tensor = TFTensor.FromBuffer(shape, array.ToArray(), 0, array.Count);
            return tensor;
        }

        public static int Argmax(float[] inputs)
        {
            float max = inputs.Max();
            return Array.IndexOf(inputs, max);
        }
        /// <summary>
        /// 非一维数组无法引用Array.Copy
        /// 可考虑转为one dim 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static int[] Argmax(float[,] inputs)
        {
            int dim0 = inputs.GetLength(0);
            int dim1 = inputs.GetLength(1);
            int[] output = new int[dim0];
            for (int i=0;i< dim0; i++)
            {
                float[] arr = new float[dim1];
                for(int j = 0; j < dim1; j++)
                    arr[j] = inputs[i, j];
                output[i] = Argmax(arr);
            }
            return output;
        }

    }
}
