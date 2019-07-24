using System;
using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Clustering;
using Accord.Math;

namespace Engine.Brain.Utils
{
    public partial class NP
    {

        /// <summary>
        /// http://accord-framework.net/docs/html/T_Accord_MachineLearning_Clustering_TSNE.htm
        /// </summary>
        /// <param name=""></param>
        public static double[] TSNE1(double[][] observations)
        {
            Accord.Math.Random.Generator.Seed = 0;
            TSNE tSNE = new TSNE()
            {
                NumberOfOutputs = 1,
                Perplexity = 1.5
            };
            double[][] output = tSNE.Transform(observations);
            double[] y = output.Reshape();
            return y;
        }

        /// <summary>
        /// http://accord-framework.net/docs/html/T_Accord_MachineLearning_Clustering_TSNE.htm
        /// </summary>
        /// <param name="observations"></param>
        /// <returns></returns>
        public static float[][] TSNE2(float[][] observations)
        {
            Accord.Math.Random.Generator.Seed = 0;
            TSNE tSNE = new TSNE()
            {
                NumberOfOutputs = 2,
                Perplexity = 1
            };
            double[][] output = tSNE.Transform(NP.FloatArrayToDoubleArray(observations));
            return NP.DoubleArrayToFloatArray(output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Dot(double[] x, double[] y)
        {
            if (x.Length != y.Length) throw new Exception("vector x and y length must be equal");
            double dot = 0.0;
            for (int i = 0; i < x.Length; i++)
                dot += x[i] * y[i];
            return dot;
        }

        /// <summary>
        /// indicate prediction vector equals lable vector
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool Equal(float[] pred, float[] label)
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
                action[i] = Random(2);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Random(int maxValue)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(0, maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        public static float[] ToOneHot(int hotIndex, int hotLength)
        {
            float[] oneHot = new float[hotLength];
            for (int i = 0; i < hotLength; i++)
                oneHot[i] = i == hotIndex ? 1 : 0;
            return oneHot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float[] ToOneDimensional(float[][] input)
        {
            var list = input.ToList();
            int rows = list.Count;
            int cols = list[0].Length;
            int totalCount = rows * cols;
            float[] output = new float[totalCount];
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
        public static List<float> CreateLabels(int batchSzie = 15, int oneHot = 10)
        {
            var inputs = new List<float[]>();
            //构建多样本的输出label
            for (int i = 0; i < batchSzie; i++)
            {
                var label = Random(10);
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
            for (int i = 0; i < dim0; i++)
            {
                float[] arr = new float[dim1];
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predict"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static double CalcuteAccuracy(double[] predict, double[] target)
        {
            int predCount = predict.GetLength(0);
            int labelCount = predict.GetLength(0);
            if (predCount != labelCount)
                return 0.0;
            double right = 0.0;
            for (int i = 0; i < predCount; i++)
                right += (predict[i] == target[i]) ? 1 : 0;
            return right / predCount;
        }

        /// <summary>
        /// 余弦计算文本相似度
        /// https://www.cnblogs.com/airnew/p/9563703.html
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float Cosine(float[] a, float[] b)
        {
            if (a.Length != b.Length) throw new Exception("Error: Distance, source and target length must be same");
            float si = 0.0f;
            for (int i = 0; i < a.Length; i++)
                si += a[i] * b[i];
            return si / (Len(a) * Len(b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double[] Plus(double[] source, double[] target)
        {
            if (source.Length != target.Length) throw new Exception("Error: Distance, source and target length must be same");
            double[] r = new double[source.Length];
            for (int i = 0; i < source.Length; i++)
                r[i] = source[i] + target[i];
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double[] Sub(double[] source, double[] target)
        {
            if (source.Length != target.Length) throw new Exception("Error: Distance, source and target length must be same");
            double[] r = new double[source.Length];
            for (int i = 0; i < source.Length; i++)
                r[i] = source[i] - target[i];
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float Len(float[] source)
        {
            float sum = 0.0f;
            for (int i = 0; i < source.Length; i++)
                sum += source[i] * source[i];
            return (float)Math.Sqrt(sum);
        }

        /// <summary>
        ///  https://stackoverflow.com/a/110570
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void Shuffle<T>(T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                var k = NP.Random(n--);
                Swap(array, n, k);
            }
        }

        /// <summary>
        /// 乱序样本集， array1 为输入 array2是label
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        public static void Shuffle<T1, T2>(T1[] array1, T2[] array2)
        {
            System.Diagnostics.Debug.Assert(array1.Length == array2.Length);
            var n = array1.Length;
            while (n > 1)
            {
                var k = NP.Random(n--);
                Swap(array1, n, k);
                Swap(array2, n, k);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="n"></param>
        /// <param name="k"></param>
        public static void Swap<T>(T[] array, int n, int k)
        {
            var temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int[] ShuffledIndex(int n)
        {
            var array = new int[n];
            for (int i = 0; i < n; i++) { array[i] = i; }
            Shuffle(array);
            return array;
        }

        /// <summary>
        /// 对输入的样本集，乱序提取出batchSize个样本对
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static (float[][] inputs, float[] labels) ShuffleBatch(float[][] inputs, float[] outputs, int batchSize)
        {
            float[][] x = new float[batchSize][];
            float[] y = new float[batchSize];
            Shuffle(inputs, outputs);
            for (int i = 0; i < batchSize; i++)
            {
                x[i] = inputs[i];
                y[i] = outputs[i];
            }
            return (x, y);
        }

        /// <summary>
        /// 对输入的样本集，乱序提取出batchSize个样本对
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static (float[][] inputs, float[][] labels) ShuffleBatch(float[][] inputs, float[][] outputs, int batchSize)
        {
            float[][] x = new float[batchSize][];
            float[][] y = new float[batchSize][];
            Shuffle(inputs, outputs);
            for (int i = 0; i < batchSize; i++)
            {
                x[i] = inputs[i];
                y[i] = outputs[i];
            }
            return (x, y);
        }

        /// <summary>
        /// concat arrrays to one array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumetns"></param>
        /// <returns></returns>
        public static T[] Concat<T>(params T[][] argumetns) where T : struct
        {
            var list = new List<T>();
            for (int i = 0; i < argumetns.Length; i++)
                list.AddRange(argumetns[i]);
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter_shape"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] PadToShape<T>(int[] filter_shape, T value) where T : struct
        {
            var result = new T[filter_shape.Length];
            for (int i = 0; i < result.Length; i++) { result[i] = value; }
            return result;
        }

        /// <summary>
        /// double数值转换成float数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float[] DoubleArrayToFloatArray(double[] array)
        {
            float[] arr = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
                arr[i] = (float)array[i];
            return arr;
        }

        /// <summary>
        /// float数值转换成double数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float[][] DoubleArrayToFloatArray(double[][] array)
        {
            int n0 = array.GetLength(0);
            float[][] arr = new float[n0][];
            for (int i = 0; i < n0; i++)
            {
                int n1 = array[i].Length;
                arr[i] = new float[n1];
                for (int k = 0; k < n1; k++)
                    arr[i][k] = (float)array[i][k];
            }
            return arr;
        }

        /// <summary>
        /// float数值转换成double数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[] FloatArrayToDoubleArray(float[] array)
        {
            double[] arr = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                arr[i] = Convert.ToDouble(array[i]);
            return arr;
        }

        /// <summary>
        /// float数值转换成double数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[][] FloatArrayToDoubleArray(float[][] array)
        {
            int n0 = array.GetLength(0);
            double[][] arr = new double[n0][];
            for (int i = 0; i < n0; i++)
            {
                int n1 = array[i].Length;
                arr[i] = new double[n1];
                for (int k = 0; k < n1; k++)
                    arr[i][k] = Convert.ToDouble(array[i][k]);
            }
            return arr;
        }

    }
}