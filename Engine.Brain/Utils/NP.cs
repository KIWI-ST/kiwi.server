using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Brain.Utils
{
    public partial class NP
    {
        /// <summary>
        /// indicate prediction vector equals lable vector
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool Equal(double[] pred, double[] label)
        {
            int predCount = pred.Length;
            int labelCount = label.Length;
            if (predCount != labelCount)
                return false;
            bool result = true;
            for (int i = 0; i < predCount; i++)
                result &= pred[i] == label[i];
            return result;
        }
        /// <summary>
        /// create ont hot array stochastic
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double[] StochasticOnehot(int length)
        {
            double[] action = new double[length];
            for (int i = 0; i < length; i++)
                action[i] = NP.Random(2);
            return action;
        }
        /// <summary>
        /// random with seed
        /// </summary>
        /// <param name="seeds"></param>
        /// <returns>the seed value</returns>
        public static int Random(int[] seeds)
        {
            int index = Random(seeds.Length);
            return seeds[index];
        }

        public static int Random(int maxValue)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(0, maxValue);
        }

        public static double Random()
        {
            return new Random(Guid.NewGuid().GetHashCode()).NextDouble();
        }
        /// <summary>
        /// 归一化
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static double[] Normalize(byte[] inputs)
        {
            int count = inputs.Length;
            double[] normal = new double[inputs.Length];
            for (int i = 0; i < count; i++)
                normal[i] = inputs[i] / 255f;
            return normal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static double[] Normalize(double[] inputs, double max)
        {
            int count = inputs.Length;
            double[] normal = new double[inputs.Length];
            for (int i = 0; i < count; i++)
                normal[i] = inputs[i] / max;
            return normal;
        }
        /// <summary>
        /// onehot编码
        /// </summary>
        /// <param name="hotIndex"></param>
        /// <param name="hotLength"></param>
        /// <returns></returns>
        public static double[] ToOneHot(int hotIndex, int hotLength)
        {
            double[] oneHot = new double[hotLength];
            for (int i = 0; i < hotLength; i++)
                oneHot[i] = i == hotIndex ? 1 : 0;
            return oneHot;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] ToUnidimensional(double[][] input)
        {
            var list = input.ToList();
            int rows = list.Count;
            int cols = list[0].Length;
            int totalCount = rows * cols;
            double[] output = new double[totalCount];
            for (int i = 0; i < totalCount; i++)
                output[i] = input[i / cols][i % cols];
            return output;
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
            //构建指定feature数目的多样本集合
            for (int i = 0; i < batchSize; i++)
            {
                var input = new List<float>();
                for (int j = 0; j < oneDimensionCount; j++)
                {
                    var num = NP.Random(10);
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
        public static List<double> CreateLabels(int batchSzie = 15, int oneHot = 10)
        {
            var inputs = new List<double[]>();
            //构建多样本的输出label
            for (int i = 0; i < batchSzie; i++)
            {
                var label = NP.Random(10);
                inputs.Add(ToOneHot(label, oneHot));
            }
            var outputs = new List<double>();
            inputs.ForEach(p =>
            {
                outputs.AddRange(p);
            });
            //返回一维数组，备用
            return outputs;
        }
        /// <summary>
        /// 生成符合正态分部的随机数组
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static float[] CreateRandomNormalFloat(int length)
        {
            float[] arry = new float[length];
            for (int i = 0; i < length; i++)
                arry[i] = Normal(Random(), Random());
            return arry;
        }
        /// <summary>
        /// 标准正态分部期望0，方差1
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <param name="e">期望，</param>
        /// <param name="d">方差</param>
        /// <returns></returns>
        public static float Normal(double u1, double u2, double e = 0, double d = 1)
        {
            double result = e + Math.Sqrt(d) * Math.Sqrt((-2) * (Math.Log(u1) / Math.Log(Math.E))) * Math.Cos(2 * Math.PI * u2);
            return (float)result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static int Argmax(double[] inputs)
        {
            double max = inputs.Max();
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
            for (int i = 0; i < dim0; i++)
            {
                double[] arr = new double[dim1];
                for (int j = 0; j < dim1; j++)
                    arr[j] = inputs[i, j];
                output[i] = Argmax(arr);
            }
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static double Max(double[] inputs)
        {
            double max = inputs.Max();
            return max;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static float[] Max(float[,] inputs)
        {
            int dim0 = inputs.GetLength(0);
            int dim1 = inputs.GetLength(1);
            float[] output = new float[dim0];
            for (int i = 0; i < dim0; i++)
            {
                float[] arr = new float[dim1];
                for (int j = 0; j < dim1; j++)
                    arr[j] = inputs[i, j];
                output[i] = arr.Max();
            }
            return output;
        }

        public static double CalcuteAccuracy(double[][] predict, double[][] label)
        {
            int predCount = predict.GetLength(0);
            int labelCount = predict.GetLength(0);
            if (predCount != labelCount)
                return 0.0;
            double right = 0.0;
            for (int i = 0; i < predCount; i++)
                right += Equal(predict[i], label[i]) ? 1 : 0;
            return right / predCount;
        }

        public static double CalcuteAccuracy(float[] predict, float[] label)
        {
            int count = predict.Length;
            float right = 0f;
            for (int i = 0; i < count; i++)
                right += Math.Abs(predict[i] - label[i]) < 0.1f ? 1f : 0f;
            return right / count;
        }

        public static float CalcuteAccuracy(float[,] predict, float[,] label)
        {
            int dim0 = predict.GetLength(0);
            int dim1 = predict.GetLength(1);
            float right = 0f;
            for (int i = 0; i < dim0; i++)
                for (int j = 0; j < dim1; j++)
                    right += Math.Abs(predict[i, j] - label[i, j]) < 0.1f ? 1f : 0f;
            return right / (dim0 * dim1);
        }
        /// <summary>
        /// 余弦计算文本相似度
        /// https://www.cnblogs.com/airnew/p/9563703.html
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double Cosine(double[] source, double[] target)
        {
            if (source.Length != target.Length) throw new Exception("Error: Distance, source and target length must be same");
            return 0.0;
        }

        public static double[] Plus(double[] source, double[] target)
        {
            if (source.Length != target.Length) throw new Exception("Error: Distance, source and target length must be same");
            double[] r = new double[source.Length];
            for (int i = 0; i < source.Length; i++)
                r[i] = source[i] + target[i];
            return r;
        }

        public static double[] Sub(double[] source, double[] target)
        {
            if (source.Length != target.Length) throw new Exception("Error: Distance, source and target length must be same");
            double[] r = new double[source.Length];
            for (int i = 0; i < source.Length; i++)
                r[i] = source[i] - target[i];
            return r;
        }

        public static double Len(double[] source)
        {
            double sum = 0;
            for (int i = 0; i < source.Length; i++)
                sum += source[i]* source[i];
            return Math.Sqrt(sum);
        }

    }
}