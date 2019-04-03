using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// embedding net aims to use model instead of training it
    /// </summary>
    public class EmbeddingNet : IDNet
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
        string _modelFilename;

        public EmbeddingNet(string modelFilename)
        {
            _modelFilename = modelFilename;
        }

        public void Initialization()
        {
            embeddingsIndex = PreprocessEmbeddings(_modelFilename);
        }

        public Dictionary<string, double[]> PreprocessEmbeddings(string modelFilename)
        {
            var embeddings_index = new Dictionary<string, double[]>();
            foreach (var line in File.ReadLines(modelFilename, Encoding.UTF8))
            {
                var values = line.Split(' ');
                var word = values[0];
                var coefs = values.Skip(1).Select(v => double.Parse(v)).ToArray();
                embeddings_index[word] = coefs;
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
