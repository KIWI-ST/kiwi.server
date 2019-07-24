using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.ML
{
    /// <summary>
    /// support vector machines
    /// </summary>
    public class L2SVM: IDSupervised
    {
        MulticlassSupportVectorMachine<Linear> ksvm;

        MulticlassSupportVectorLearning<Linear> teacher;

        public L2SVM(int inputDimension, int outputClasses)
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

        public double Train(float[][] inputs, int[] outputs)
        {
            ksvm = teacher.Learn(NP.FloatArrayToDoubleArray(inputs), outputs);
            return 0.0;
        }

        public int[] Predict(float[][] inputs)
        {
            int[] predicted = ksvm.Decide(NP.FloatArrayToDoubleArray(inputs));
            return predicted;
        }

    }
}
