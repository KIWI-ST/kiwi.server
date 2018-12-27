using Engine.Brain.Entity;
using Engine.Lexicon.Extend;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// event handler
    /// </summary>
    /// <param name="loss"></param>
    /// <param name="liter"></param>
    public delegate void OnTrainingEventHanlder(double loss, int liter, double progress);

    [Serializable]
    public class LSTMNetwork
    {
        /// <summary>
        /// 
        /// </summary>
        public event OnTrainingEventHanlder OnTrainingProgress;

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
        string _autoSave = Directory.GetCurrentDirectory() + @"\tmp\autolstm.bin";

        /// <summary>
        /// 
        /// </summary>
        double _targetLoss;



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
                        pos += _bufferSize - 1;
                        vx[_lexicon.DictIndex[text[pos]]] = 1;
                        AdvanceBuffer(buffer, vx, bufferSize);
                        // calcute loss
                        var grads = Loss(probs, buffer, bufferSize, _lexicon.VocaSize);
                        // backward
                        layer1.Backward(layer2.Backward(layer3.Backward(grads)));
                        //
                        OnTrainingProgress?.Invoke(_loss, liter, (double)pos/ text.Length);
                    }
                    if (loss_p - _loss > 0)
                        layer1.LearningRate = layer2.LearningRate = layer3.LearningRate = layer1.LearningRate * 1.01;
                    else
                        layer1.LearningRate = layer2.LearningRate = layer3.LearningRate = layer1.LearningRate * 0.98;
                    loss_p = loss_p * 0.8 + _loss * 0.2;
                    liter++;
                    //
                    if (_loss < _targetLoss)
                    {
                        Save(_autoSave);
                        break;
                    }
                    else if (liter % saveInterval == 0)
                    {
                        Save(_autoSave);
                    }
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
            string generateText = Generate(_bufferSize, text, lexicon, layer3, layer2, layer1);
            return generateText;
        }

        private string Generate(int bufferSize, string[] text, Lexicon.Entity.Lexicon lexicon, Language.Layer layer3, Language.Layer layer2, Language.Layer layer1)
        {
            var buffer = FillBuffer(0, bufferSize, text, lexicon);
            string generatedText = "";
            for (var pos = 0; pos < 500; pos++)
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
        private double[][] Loss(double[][] probs, double[][] targets, int BufferSize, int size_vocab)
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
