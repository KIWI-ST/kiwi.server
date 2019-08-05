using Accord.MachineLearning.DecisionTrees;
using Engine.Brain.Extend;

namespace Engine.Brain.Method.Discriminate
{
    /// <summary>
    /// random forest
    /// </summary>
    public class RandomForest: IDiscriminate
    {
        RandomForestLearning _teacher;

        Accord.MachineLearning.DecisionTrees.RandomForest _forest;

        public RandomForest(int treeCount = 10)
        {
            Accord.Math.Random.Generator.Seed = 1;

            _teacher = new RandomForestLearning()
            {
                NumberOfTrees = treeCount
            };
        }

        public double Train(float[][] inputs, int[] outputs)
        {
            double[][] dInputs = inputs.toDouble();
            _forest = _teacher.Learn(dInputs, outputs);
            return 0.0;
        }

        public int Predict(float[] input)
        {
            double[] dInput = input.toDouble();
            int predicted = _forest.Decide(dInput);
            return predicted;
        }
    }
}
