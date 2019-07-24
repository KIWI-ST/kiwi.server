using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.RL.Env
{
    /// <summary>
    /// 基于样本构建反馈环境
    /// </summary>
    public class SamplesEnv : IEnv
    {
        /// <summary>
        /// input data collection
        /// </summary>
        float[][] _inputs;

        /// <summary>
        /// label collection
        /// </summary>
        int[] _labels;

        /// <summary>
        /// sample size
        /// </summary>
        int _count;

        /// <summary>
        /// 指示agent每次只能做一个操作
        /// </summary>
        public bool SingleAction { get { return true; } }

        /// <summary>
        /// _current_inputIndex, input index
        /// </summary>
        int _current_inputIndex;

        /// <summary>
        /// _current_classIndex, label value to oneHot
        /// </summary>
        float[] _current_classIndex;

        /// <summary>
        /// build env according to the samples
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="labels"></param>
        public SamplesEnv(float[][] inputs, int[] labels)
        {
            if (inputs.Count() != labels.Count())
                throw new Exception("the inputs and lables must be at the same count");
            _inputs = inputs;
            _labels = labels;
            //preprocess
            Prepare();
        }

        /// <summary>
        /// release memory resource to reduce memory leak
        /// </summary>
        public void Dispose()
        {
            _inputs = null;
            _labels = null;
        }

        /// <summary>
        /// prepare parameters
        /// </summary>
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
        public int[] RandomSeedKeys { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int ActionNum { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] FeatureNum { get; private set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float[] RandomAction()
        {
            int action = NP.Random(ActionNum);
            return NP.ToOneHot(action, ActionNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public (int inputIndex, float[] classIndex) RandomAccessMemory()
        {
            //use actionNumber represent real types
            int inputIndex = NP.Random(_count);
            int lableValue = _labels[inputIndex];
            float[] classIndex = NP.ToOneHot(Array.IndexOf(RandomSeedKeys, lableValue), ActionNum);
            return (inputIndex, classIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public (List<float[]> states, float[][] labels) RandomEval(int batchSize = 64)
        {
            List<float[]> states = new List<float[]>();
            float[][] labels = new float[batchSize][];
            for (int i = 0; i < batchSize; i++)
            {
                var ( inputIndex, classIndex) = RandomAccessMemory();
                float[] normal = _inputs[inputIndex];
                states.Add(normal);
                labels[i] = classIndex;
            }
            return (states, labels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float[] Reset()
        {
            return Step(null).state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public (float[] state, float reward) Step(float[] action)
        {
            if (action == null)
            {
                var (_c_inputIndex, _c_classIndex) = (_current_inputIndex, _current_classIndex);
                (_current_inputIndex, _current_classIndex) = RandomAccessMemory();
                float[] raw = _inputs[_c_inputIndex];
                return (raw, 0.0f);
            }
            else
            {
                float reward = NP.Argmax(action) == NP.Argmax(_current_classIndex) ? 1.0f : -1.0f;
                (_current_inputIndex, _current_classIndex) = RandomAccessMemory();
                float[] raw = _inputs[_current_inputIndex];
                return (raw, reward);
            }
        }

    }
}
