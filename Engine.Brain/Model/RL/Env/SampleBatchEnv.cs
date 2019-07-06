using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine.Brain.Extend;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.RL.Env
{
    /// <summary>
    /// 支持分批数据分次载入的DQN环境
    /// </summary>
    public class SamplesBatchEnv : IEnv
    {
        public bool SingleAction { get { return true; } }

        public int[] RandomSeedKeys { get; private set; }

        public int ActionNum { get; private set; }

        public int[] FeatureNum { get; private set; }

        /// <summary>
        /// input data collection
        /// </summary>
        double[][] _inputs;

        /// <summary>
        /// label collection
        /// </summary>
        int[] _labels;

        /// <summary>
        /// sample size
        /// </summary>
        int _count;

        /// <summary>
        /// _current_inputIndex, input index
        /// </summary>
        int _current_inputIndex;

        /// <summary>
        /// _current_classIndex, label value to oneHot
        /// </summary>
        double[] _current_classIndex;

        /// <summary>
        /// 
        /// </summary>
        List<DirectoryInfo> _samplesDirCollection;

        /// <summary>
        /// 当前已在环境中探索的步数
        /// </summary>
        int _stepCount = 0;

        /// <summary>
        /// 经过x次后，重新载入样本
        /// </summary>
        int _switchEpoch = 256*200;

        /// <summary>
        /// 文件夹组织逻辑：
        /// 1. SamplesDir 值所有样本存放目录， 例如 D:\Samples
        /// 2. 样本按照Batch存放在SamplesDir里，例如 D:\Samples\batch1, D:\Samples\batch2
        /// 3. Batch目录下对应的是具体样本文件，例如：D:\Samples\batch1\1_193_193_3.txt,
        ///     表示标签为1的输入维度为 193x193x3的样本集
        /// 4. 乱序操作内部定义
        /// </summary>
        /// <param name="SamplesRootDir"></param>
        public SamplesBatchEnv(string SamplesRootDir)
        {
            DirectoryInfo root = new DirectoryInfo(SamplesRootDir);
            _samplesDirCollection = root.GetDirectories().ToList();
            LoadSampleBatch(_samplesDirCollection.RandomTake());
        }

        private void LoadSampleBatch(DirectoryInfo sampleDir)
        {
            List<double[]> samples = new List<double[]>();
            List<int> labels = new List<int>();
            foreach (FileInfo file in sampleDir.GetFiles())
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string text = sr.ReadLine();
                    do
                    {
                        var (sample, label) = ConvertToSample(text);
                        samples.Add(sample);
                        labels.Add(label);
                        text = sr.ReadLine();
                    } while (text != null);
                }
            }
            _inputs = samples.ToArray();
            _labels = labels.ToArray();
            Prepare();
        }

        private (double[] sample, int label) ConvertToSample(string text)
        {
            string[] samplesText = text.Split(',');
            int label = Convert.ToInt32(samplesText.Last());
            double[] sample = new double[samplesText.Length - 1];
            for (int i = 0; i < samplesText.Length - 1; i++)
                sample[i] = Convert.ToDouble(samplesText[i]);
            return (sample, label);
        }

        private void Prepare()
        {
            _count = _labels.Count();
            List<int> keys = new List<int>();
            for (int i = 0; i < _count; i++)
                if (!keys.Contains(_labels[i])) keys.Add(_labels[i]);
            keys.Sort();
            //seedkey for convert the result
            RandomSeedKeys = keys.ToArray();
            //define the range of action values
            ActionNum = RandomSeedKeys.Count();
            //feature count
            FeatureNum = new int[] { _inputs[0].Length };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public (int inputIndex, double[] classIndex) RandomAccessMemory()
        {
            //use actionNumber represent real types
            int inputIndex = NP.Random(_count);
            int lableValue = _labels[inputIndex];
            double[] classIndex = NP.ToOneHot(Array.IndexOf(RandomSeedKeys, lableValue), ActionNum);
            return (inputIndex, classIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<double[]> states, double[][] labels) RandomEval(int batchSize = 64)
        {
            List<double[]> states = new List<double[]>();
            double[][] labels = new double[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                var (inputIndex, classIndex) = RandomAccessMemory();
                double[] normal = _inputs[inputIndex];
                states.Add(normal);
                labels[i] = classIndex;
            }
            return (states, labels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] Reset()
        {
            return Step(null).state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] RandomAction()
        {
            int action = NP.Random(ActionNum);
            return NP.ToOneHot(action, ActionNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public (double[] state, double reward) Step(double[] action)
        {
            if (_stepCount >= _switchEpoch)
            {
                //重置环境探索计数
                _stepCount = 0;
                //载入样本
                LoadSampleBatch(_samplesDirCollection.RandomTake());
            }
            _stepCount++;
            if (action == null)
            {
                var (_c_inputIndex, _c_classIndex) = (_current_inputIndex, _current_classIndex);
                (_current_inputIndex, _current_classIndex) = RandomAccessMemory();
                double[] raw = _inputs[_c_inputIndex];
                return (raw, 0.0);
            }
            else
            {
                double reward = NP.Argmax(action) == NP.Argmax(_current_classIndex) ? 1.0 : -1.0;
                (_current_inputIndex, _current_classIndex) = RandomAccessMemory();
                double[] raw = _inputs[_current_inputIndex];
                return (raw, reward);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Export(string fullFilename, int row = 1, int col = 1)
        {
            throw new NotImplementedException();
        }

    }
}
