using Engine.Brain.Entity;
using Engine.Lexicon.Extend;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Engine.Brain.Model.DL
{

    [Serializable]
    public class LSTMNetwork
    {
        /// <summary>
        /// 网络结构
        /// </summary>
        Language.Layer layer1, layer2, layer3;

        /// <summary>
        /// 
        /// </summary>
        int _hiddenNeuronsCount;

        /// <summary>
        /// 
        /// </summary>
        int _bufferSize;

        /// <summary>
        /// 字典文件
        /// </summary>
        int _vocaSize;

        /// <summary>
        /// loss计算
        /// </summary>
        public double Loss { get; private set; } = 0.0;

        /// <summary>
        /// 自动存储路径
        /// </summary>
        string _autoSave = Directory.GetCurrentDirectory() + @"\tmp\autolstm.bin";

        /// <summary>
        /// 
        /// </summary>
        double _targetLoss;

        /// <summary>
        /// 
        /// </summary>
        public double Process { get; private set; } = 0.0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vocaSize"></param>
        /// <param name="buffersize"></param>
        /// <param name="hiddenNeuronsCount"></param>
        /// <param name="learningRate"></param>
        /// <param name="targetLoss"></param>
        public LSTMNetwork(int vocaSize, int buffersize = 24, int hiddenNeuronsCount = 300, double learningRate = 0.001, double targetLoss = 0.01)
        {
            _vocaSize = vocaSize;
            _bufferSize = buffersize;
            _targetLoss = targetLoss;
            _hiddenNeuronsCount = hiddenNeuronsCount;
            layer1 = new Language.LSTM(_vocaSize, _hiddenNeuronsCount, _bufferSize);
            layer1.LearningRate = learningRate;
            layer2 = new Language.LSTM(_hiddenNeuronsCount, _hiddenNeuronsCount, _bufferSize);
            layer2.LearningRate = learningRate;
            layer3 = new Language.SoftMax(_hiddenNeuronsCount, _vocaSize, _bufferSize);
            layer3.LearningRate = learningRate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            Save(stream);
            stream.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        private void Save(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static LSTMNetwork Load(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            LSTMNetwork network = Load(stream);
            stream.Close();
            return network;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static LSTMNetwork Load(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            LSTMNetwork network = (LSTMNetwork)formatter.Deserialize(stream);
            return network;
        }
        public int liter { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textFullFilename"></param>
        /// <param name="_lexicon"></param>
        public void LearnFromRawText(string textFullFilename, Lexicon.Entity.Lexicon _lexicon)
        {
            double loss_p = Math.Log(_lexicon.VocaSize);
            //迭代次数和自动存储的迭代轮次
            liter = 1;
            int saveInterval = 5;
            using (StreamReader sr = new StreamReader(textFullFilename))
            {
                string rawText = "";
                while (!sr.EndOfStream)
                    rawText += sr.ReadLine().Trim().ClearPunctuation();
                int bufferSize = 24;
                string[] text = _lexicon.Sgement(rawText);
                while (true)
                {
                    int pos = 0;
                    while (pos + bufferSize < text.Length)
                    {
                        // Fill buffer.
                        var buffer = FillBuffer(pos, bufferSize, text, _lexicon);
                        var reset = pos == 0;
                        var probs = layer3.Forward(layer2.Forward(layer1.Forward(buffer, reset), reset), reset);
                        // Advance buffer.  
                        var vx = new double[_lexicon.VocaSize];
                        pos += _bufferSize - 1;
                        vx[_lexicon.DictIndex[text[pos]]] = 1;
                        AdvanceBuffer(buffer, vx, bufferSize);
                        // calcute loss
                        var grads = Cost(probs, buffer, bufferSize, _lexicon.VocaSize);
                        // backward
                        layer1.Backward(layer2.Backward(layer3.Backward(grads)));
                        //
                        Process = (double)pos / text.Length;
                    }
                    if (loss_p - Loss > 0)
                        layer1.LearningRate = layer2.LearningRate = layer3.LearningRate = layer1.LearningRate * 1.01;
                    else
                        layer1.LearningRate = layer2.LearningRate = layer3.LearningRate = layer1.LearningRate * 0.98;
                    loss_p = loss_p * 0.8 + Loss * 0.2;
                    liter++;
                    //
                    if (Loss < _targetLoss)
                    {
                        Save(_autoSave);
                        break;
                    }
                    else if (liter % saveInterval == 0)
                        Save(_autoSave);
                }
            }
        }
        /// <summary>
        /// write a brief text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lexicon"></param>
        /// <returns></returns>
        public string WriteText(string[] text, Lexicon.Entity.Lexicon lexicon)
        {
            //string generateText = Generate(text.Length+1, text, lexicon, layer3, layer2, layer1);
            string generateText = Generate(_bufferSize, text, lexicon, layer3, layer2, layer1);
            return generateText;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <param name="text"></param>
        /// <param name="lexicon"></param>
        /// <param name="layer3"></param>
        /// <param name="layer2"></param>
        /// <param name="layer1"></param>
        /// <returns></returns>
        private string Generate(int bufferSize, string[] text, Lexicon.Entity.Lexicon lexicon, Language.Layer layer3, Language.Layer layer2, Language.Layer layer1)
        {
            var buffer = FillBuffer(0, bufferSize, text, lexicon);
            string generatedText = "";
            for (var pos = 0; pos < 24; pos++)
            {
                var reset = pos == 0;
                var probs = layer3.Forward(layer2.Forward(layer1.Forward(buffer, reset), reset), reset);
                int ix = WeightedChoice(probs[_bufferSize - 1]);
                double[] vx = new double[lexicon.VocaSize];
                vx[ix] = 1;
                AdvanceBuffer(buffer, vx, bufferSize);
                generatedText += lexicon.Decode(ix);
            }
            return generatedText;
        }

        private int WeightedChoice(double[] vy)
        {
            double val = NP.Random();
            for (var i = 0; i < vy.Length; i++)
            {
                if (val <= vy[i]) return i;
                val -= vy[i];
            }
            throw new Exception("Not in dictionary!");
        }

        private void AdvanceBuffer(double[][] buffer, double[] vx, int bufferSize)
        {
            for (var b = 1; b < bufferSize - 1; b++)
                buffer[b] = buffer[b + 1];
            buffer[_bufferSize - 1] = vx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="probs"></param>
        /// <param name="targets"></param>
        /// <param name="BufferSize"></param>
        /// <param name="size_vocab"></param>
        /// <returns></returns>
        private double[][] Cost(double[][] probs, double[][] targets, int BufferSize, int size_vocab)
        {
            var ls = 0.0;
            var grads = new double[BufferSize][];
            for (var t = 1; t < _bufferSize; t++)
            {
                grads[t] = probs[t].ToArray();
                for (var i = 0; i < size_vocab; i++)
                {
                    ls += -Math.Log(probs[t][i]) * targets[t][i];
                    grads[t][i] -= targets[t][i];
                }
            }
            ls = ls / (_bufferSize - 1);
            Loss = Loss * 0.99 + ls * 0.01;
            return grads;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="bufferSize"></param>
        /// <param name="text"></param>
        /// <param name="lexicon"></param>
        /// <returns></returns>
        private double[][] FillBuffer(int offset, int bufferSize, string[] text, Lexicon.Entity.Lexicon lexicon)
        {
            double[][] buffer = new double[bufferSize][];
            for (int pos = 1; pos < bufferSize; pos++)
            {
                buffer[pos] = new double[lexicon.VocaSize];
                buffer[pos][lexicon.DictIndex[text[pos + offset - 1]]] = 1;
            }
            return buffer;
        }

    }
}
