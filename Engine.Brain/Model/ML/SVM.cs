using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace Engine.Brain.Model.ML
{
    public class SVM
    {
        MulticlassSupportVectorMachine<Linear> ksvm;

        MulticlassSupportVectorLearning<Linear> teacher;

        public SVM(int inputDimension, int outputClasses)
        {
            teacher = new MulticlassSupportVectorLearning<Linear>
            {
                // using LIBLINEAR's L2-loss SVC dual for each SVM
                //Learner = (p) => new Accord.MachineLearning.VectorMachines.Learning.LinearCoordinateDescent()
                Learner = (p) => new LinearDualCoordinateDescent()
                {
                    Loss = Loss.L2
                }
            };
        }

        public double Train(double[][] inputs, int[] outputs)
        {
            ksvm = teacher.Learn(inputs, outputs);
            return 0.0;
        }

        public int[] Predict(double[][] inputs)
        {
            int[] predicted = ksvm.Decide(inputs);
            return predicted;
        }

    }
}
