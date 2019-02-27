using Engine.Brain.AI.RL;
using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

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
        double[] _current_classIndex;
        /// <summary>
        /// build env according to the samples
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="labels"></param>
        public SamplesEnv(double[][] inputs, int[] labels)
        {
            if (inputs.Count() != labels.Count())
                throw new Exception("the inputs and lables must be at the same count");
            _inputs = inputs;
            _labels = labels;
            //preprocess
            Prepare();
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
        public double[] RandomAction()
        {
            int action = NP.Random(ActionNum);
            return NP.ToOneHot(action, ActionNum);
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
                var ( inputIndex, classIndex) = RandomAccessMemory();
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
        /// <param name="action"></param>
        /// <returns></returns>
        public (double[] state, double reward) Step(double[] action)
        {
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
    }
}
