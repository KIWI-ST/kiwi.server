using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// reference:
    /// https://github.com/stanfordnlp/GloVe
    /// https://github.com/anastasios-stamoulis/deep-learning-with-csharp-and-cntk/tree/master/DeepLearning/Ch_06_Using_Word_Embeddings
    /// embedding net aims to use model instead of training it
    /// </summary>
    public class GloVeNet : IDNet
    {
        /// <summary>
        /// 
        /// </summary>
        public int MaxWordsNum { get; private set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int EmbeddingDimNum { get; private set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, double[]>  embeddingsIndex;

        /// <summary>
        /// 
        /// </summary>
        readonly string _modelFilename;

        public GloVeNet(string modelFilename)
        {
            _modelFilename = modelFilename;
        }

        public void Initialization()
        {
            //embeddingsIndex = PreprocessEmbeddings(_modelFilename);
        }
        /// <summary>
        /// 计算W矩阵
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private double[][] ComputeEmbeddingMatrix(NP.FromKeras.Tokenizer tokenizer)
        {
            var embedding_matrix = new double[MaxWordsNum][];
            var embeddings_index = PreprocessEmbeddings(_modelFilename);
            foreach (var entry in tokenizer.word_index)
            {
                var word = entry.Key;
                var i = entry.Value;
                if (i >= MaxWordsNum) { continue; }
                double[] embedding_vector;
                embeddings_index.TryGetValue(word, out embedding_vector);
                if (embedding_vector == null)
                {
                    // Words not found in embedding index will be all-zeros.
                    embedding_vector = new double[EmbeddingDimNum];
                }
                else
                {
                    System.Diagnostics.Debug.Assert(embedding_vector.Length == EmbeddingDimNum);
                }
                embedding_matrix[i] = embedding_vector;
            }
            for (int i = 0; i < embedding_matrix.Length; i++)
            {
                if (embedding_matrix[i] != null) { continue; }
                embedding_matrix[i] = new double[EmbeddingDimNum];
            }
            return embedding_matrix;
        }

        private Dictionary<string, double[]> PreprocessEmbeddings(string modelFilename)
        {
            var embeddings_index = new Dictionary<string, double[]>();
            foreach (var line in File.ReadLines(modelFilename, Encoding.UTF8))
            {
                var values = line.Split(' ');
                var word = values[0];
                var coefs = values.Skip(1).Select(v => double.Parse(v)).ToArray();
                var d = NP.Len(coefs);
                embeddings_index[word] = coefs.Select(v=>v/d).ToArray();
            }
            MaxWordsNum = embeddings_index.Keys.Count;
            EmbeddingDimNum = embeddings_index.Values.First().Length;
            return embeddings_index;
        }

        public void Accept(IDNet sourceNet)
        {
            throw new System.NotImplementedException();
        }

        public Stream PersistenceMemory()
        {
            throw new System.NotImplementedException();
        }

        public string PersistencNative(string modelFilename = null)
        {
            throw new System.NotImplementedException();
        }

        public double[] Predict(params object[] inputs)
        {
            string input = inputs[0] as string;
            return embeddingsIndex.Keys.Contains(input)?embeddingsIndex[input]:new double[EmbeddingDimNum];
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            throw new System.NotImplementedException();
        }
    }
}
