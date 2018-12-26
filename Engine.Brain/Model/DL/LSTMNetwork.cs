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
        double _loss = 0.0;

        /// <summary>
        /// 自动存储路径
        /// </summary>
        string _autoSave = Directory.GetCurrentDirectory() + @"\Datasets\autoLstm.bin";

        /// <summary>
        /// 
        /// </summary>
        double _targetLoss;

        public LSTMNetwork(int vocaSize, int buffersize = 24, int hiddenNeuronsCount = 300, double learningRate = 0.001,double targetLoss =0.01)
        {
            _vocaSize = vocaSize;
            _bufferSize = buffersize;
            _targetLoss = targetLoss;
            _hiddenNeuronsCount = hiddenNeuronsCount;
            Language.Layer.LearningRate = learningRate;
            Language.Layer.BufferSize = buffersize;
            layer1 = new Language.LSTM(_vocaSize, _hiddenNeuronsCount);
            layer2 = new Language.LSTM(_hiddenNeuronsCount, _hiddenNeuronsCount);
            layer3 = new Language.SoftMax(_hiddenNeuronsCount, _vocaSize);

        }

        public void Save(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            Save(stream);
            stream.Close();
        }

        private void Save(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
        }

        public static LSTMNetwork Load(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            LSTMNetwork network = Load(stream);
            stream.Close();
            return network;
        }

        private static LSTMNetwork Load(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            LSTMNetwork network = (LSTMNetwork)formatter.Deserialize(stream);
            return network;
        }

        public void LearnFromRawText(string textFullFilename, Lexicon.Entity.Lexicon _lexicon)
        {
            double loss_p = Math.Log(_lexicon.VocaSize);
            //迭代次数和自动存储的迭代轮次
            int liter = 1, saveInterval = 10;
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
                        pos += Language.Layer.BufferSize - 1;
                        vx[_lexicon.DictIndex[text[pos]]] = 1;
                        AdvanceBuffer(buffer, vx, bufferSize);
                        // calcute loss
                        var grads = Loss(probs, buffer, bufferSize, _lexicon.VocaSize);
                        // backward
                        layer1.Backward(layer2.Backward(layer3.Backward(grads)));
                    }
                    if (loss_p - _loss > 0)
                        Language.Layer.LearningRate *= 1.01;
                    else
                        Language.Layer.LearningRate *= 0.98;
                    loss_p = loss_p * 0.8 + _loss * 0.2;
                    liter++;
                    //
                    if (_loss < _targetLoss || liter% saveInterval == 0)
                    {
                        Save(_autoSave);
                        break;
                    }
                }
            }
        }

        private string Generate(int bufferSize, string[] text, Lexicon.Entity.Lexicon lexicon, Language.Layer layer3, Language.Layer layer2, Language.Layer layer1)
        {
            var buffer = FillBuffer(0, bufferSize, text, lexicon);
            string generatedText = "";
            for (var pos = 0; pos < 500; pos++)
            {
                var reset = pos == 0;
                var probs = layer3.Forward(layer2.Forward(layer1.Forward(buffer, reset), reset), reset);
                int ix = WeightedChoice(probs[Language.Layer.BufferSize - 1]);
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
            buffer[Language.Layer.BufferSize - 1] = vx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="probs"></param>
        /// <param name="targets"></param>
        /// <param name="BufferSize"></param>
        /// <param name="size_vocab"></param>
        /// <returns></returns>
        private double[][] Loss(double[][] probs, double[][] targets, int BufferSize, int size_vocab)
        {
            var ls = 0.0;
            var grads = new double[BufferSize][];
            for (var t = 1; t < Language.Layer.BufferSize; t++)
            {
                grads[t] = probs[t].ToArray();
                for (var i = 0; i < size_vocab; i++)
                {
                    ls += -Math.Log(probs[t][i]) * targets[t][i];
                    grads[t][i] -= targets[t][i];
                }
            }
            ls = ls / (Language.Layer.BufferSize - 1);
            _loss = _loss * 0.99 + ls * 0.01;
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
