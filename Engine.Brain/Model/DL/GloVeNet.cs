using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// reference:
    /// https://github.com/stanfordnlp/GloVe
    /// https://github.com/anastasios-stamoulis/deep-learning-with-csharp-and-cntk/tree/master/DeepLearning/Ch_06_Using_Word_Embeddings
    /// embedding net aims to use model instead of training it
    /// </summary>
    public class GloVeNet : IDEmbeddingNet
    {
        /// <summary>
        /// 
        /// </summary>
        public int TrainingSize { get; private set; } = 200;
        /// <summary>
        /// 
        /// </summary>
        public int ValidationSize { get; private set; } = 200;
        /// <summary>
        /// 
        /// </summary>
        public int MaxlenNum { get; private set; } = 100;
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
        public double[][] W { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, double[]> embeddingsIndex;
        /// <summary>
        /// 
        /// </summary>
        DeviceDescriptor device;
        /// <summary>
        /// learning rate scheduler
        /// </summary>
        NP.CNTK.ReduceLROnPlateau lrs;
        /// <summary>
        /// 
        /// </summary>
        Function model;
        /// <summary>
        /// 
        /// </summary>
        Variable x, y;
        /// <summary>
        /// 
        /// </summary>
        public double[][] embedding_weights = null;
        /// <summary>
        /// 
        /// </summary>
        Trainer trainer;
        /// <summary>
        /// create GloveNet instance and initialization weights by Glove pretrain file
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="gloVeFilename"></param>
        public GloVeNet(string deviceName, string gloVeFilename)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            embeddingsIndex = PreprocessEmbeddings(gloVeFilename);
            //initial w
            int seed = 0;
            W = new double[embeddingsIndex.Count][];
            foreach (var element in embeddingsIndex)
                W[seed++] = element.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDim"></param>
        private void CreateModel(int inputDim)
        {
            x = Variable.InputVariable(new int[] { inputDim }, DataType.Double);
            y = Variable.InputVariable(new int[] { 1 }, DataType.Double);
            //create model 
            model = CNTKLib.OneHotOp(x, numClass: (uint)MaxWordsNum, outputSparse: true, axis: new Axis(0));
            model = NP.CNTK.Embedding(model, EmbeddingDimNum, device, embedding_weights, "Embedding_1");
            model = NP.CNTK.Dense(model, 32, device, NP.CNTK.ActivateEnum.RELU);
            model = CNTKLib.ReLU(model);
            model = NP.CNTK.Dense(model, 1, device, NP.CNTK.ActivateEnum.RELU);
            model = CNTKLib.Sigmoid(model);
            //
            var loss_function = CNTKLib.BinaryCrossEntropy(model, y);
            var accuracy_function = loss_function;
            //create learner
            var parameterVector = new ParameterVector((System.Collections.ICollection)model.Parameters());
            var learner = CNTKLib.AdamLearner(
                parameterVector,
                new TrainingParameterScheduleDouble(0.001),
                new TrainingParameterScheduleDouble(0.9),
                unitGain: false);
            trainer = CNTKLib.CreateTrainer(model, loss_function, accuracy_function, new CNTK.LearnerVector() { learner });
            //lrs = new NP.CNTK.ReduceLROnPlateau(learner, 0.1);
        }
        /// <summary>
        /// train GloVeNet
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        public double Train(double[][] inputs, double[] outputs)
        {
            var metric = 0.0;
            var (xBatch, yBatch) = NP.ShuffleBatch(inputs, outputs, 1);
            for (int epoch = 0; epoch < 1000; epoch++)
            {
                var feed_dictionary = new Dictionary<Variable, Value>() {
                    { x,  Value.CreateBatch(x.Shape, NP.ToOneDimensional(xBatch),device)},
                    { y,  Value.CreateBatch(y.Shape, yBatch, device)},
                };
                trainer.TrainMinibatch(feed_dictionary, true, device);
                metric += trainer.PreviousMinibatchLossAverage();
            }
            return metric;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imdbDir"></param>
        public void UseGloVeWordEmebdding(string imdbDir, string gloVeFilename)
        {
            embeddingsIndex = PreprocessEmbeddings(gloVeFilename);
            //var (xTrain, yTrain, xValid, yValid, tokenizer, texts, labels) = PreprocessRawText(imdbDir);
            //embedding_weights = ComputeEmbeddingMatrix(tokenizer);
            //CreateModel(xTrain[0].Length);
            //Train(xTrain, yTrain);
        }
        /// <summary>
        /// 计算W矩阵
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private double[][] ComputeEmbeddingMatrix(NP.FromKeras.Tokenizer tokenizer)
        {
            var embedding_matrix = new double[MaxWordsNum][];
            foreach (var entry in tokenizer.word_index)
            {
                var word = entry.Key;
                var i = entry.Value;
                if (i >= MaxWordsNum) { continue; }
                embeddingsIndex.TryGetValue(word, out double[] embedding_vector);
                embedding_matrix[i] = embedding_vector != null && embedding_vector.Length == EmbeddingDimNum ? embedding_vector : new double[EmbeddingDimNum];
            }
            for (int i = 0; i < embedding_matrix.Length; i++)
            {
                if (embedding_matrix[i] != null) { continue; }
                embedding_matrix[i] = new double[EmbeddingDimNum];
            }
            return embedding_matrix;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelFilename"></param>
        /// <returns></returns>
        private Dictionary<string, double[]> PreprocessEmbeddings(string modelFilename)
        {
            var embeddings_index = new Dictionary<string, double[]>();
            foreach (var line in File.ReadLines(modelFilename, Encoding.UTF8))
            {
                var values = line.Split(' ');
                var word = values[0];
                var coefs = values.Skip(1).Select(v => double.Parse(v)).ToArray();
                var d = NP.Len(coefs);
                embeddings_index[word] = coefs.Select(v => v / d).ToArray();
                //embeddings_index[word] = coefs;
            }
            MaxWordsNum = embeddings_index.Keys.Count;
            EmbeddingDimNum = embeddings_index.Values.First().Length;
            return embeddings_index;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acimbFilename"></param>
        private (double[][] xTrain, double[] yTrain, double[][] xValid, double[] yValid, NP.FromKeras.Tokenizer tokenizer, List<string> texts, List<double> labels) PreprocessRawText(string imdbDir)
        {
            var (tokenizer, texts, labels) = PreprocessImdb(imdbDir);
            var sequences = tokenizer.texts_to_sequences(texts.ToArray());
            var word_index = tokenizer.word_index;
            //  Console.WriteLine($"Found {word_index.Keys.Count:n0} unique tokens.");
            var data_array = NP.FromKeras.Preprocessing.pad_sequences(sequences, MaxlenNum);
            var labels_array = labels.ToArray();
            NP.Shuffle(data_array, labels_array);
            //
            double[][] xTrain = data_array.Take(TrainingSize).ToArray();
            double[] yTrain = labels_array.Take(TrainingSize).ToArray();
            double[][] xValid = data_array.Skip(TrainingSize).Take(ValidationSize).ToArray();
            double[] yValid = labels_array.Skip(TrainingSize).Take(ValidationSize).ToArray();
            //
            return (xTrain, yTrain, xValid, yValid, tokenizer, texts, labels);
        }
        /// <summary>
        /// 
        /// </summary>
        private (NP.FromKeras.Tokenizer tokenizer, List<string> texts, List<double> labels) PreprocessImdb(string imdbDir)
        {
            //1. load  imdb text 
            var textLabelFilename = Path.Combine(imdbDir, "train");
            var (texts, labels) = ProcessTextLabels(textLabelFilename);
            //2. tokenizer 
            var tokenizer = new NP.FromKeras.Tokenizer(MaxWordsNum);
            tokenizer.fit_on_texts(texts.ToArray());
            //3. return process results
            return (tokenizer, texts, labels);
        }
        /// <summary>
        /// 
        /// </summary>
        private (List<string> texts, List<double> labels) ProcessTextLabels(string textLabelFilename)
        {
            List<string> texts = new List<string>();
            List<double> lables = new List<double>();
            var label_types = new string[] { "neg", "pos" };
            foreach (var label_type in label_types)
            {
                var dir_name = Path.Combine(textLabelFilename, label_type);
                foreach (var fname in Directory.GetFiles(dir_name))
                {
                    if (fname.EndsWith(".txt"))
                    {
                        texts.Add(File.ReadAllText(Path.Combine(dir_name, fname), Encoding.UTF8));
                        lables.Add((label_type == "neg") ? 0 : 1);
                    }
                }
            }
            return (texts, lables);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceNet"></param>
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
            return embeddingsIndex.Keys.Contains(input) ? embeddingsIndex[input] : new double[EmbeddingDimNum];
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            throw new System.NotImplementedException();
        }
    }
}
