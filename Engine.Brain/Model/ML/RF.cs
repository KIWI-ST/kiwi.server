using Accord.MachineLearning.DecisionTrees;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.ML
{
    /// <summary>
    /// random forest
    /// </summary>
    public class RF: IDSupervised
    {
        RandomForestLearning _teacher;

        RandomForest _forest;

        public RF(int treeCount = 10)
        {
            Accord.Math.Random.Generator.Seed = 1;

            _teacher = new RandomForestLearning()
            {
                NumberOfTrees = treeCount
            };
        }

        public double Train(float[][] inputs,int[] outputs)
        {
            _forest = _teacher.Learn(NP.FloatArrayToDoubleArray(inputs), outputs);
            return 0.0;
        }

        public int[] Predict(float[][] inputs)
        {
            int[] predicted = _forest.Decide(NP.FloatArrayToDoubleArray(inputs));
            return predicted;
        }

    }
}
