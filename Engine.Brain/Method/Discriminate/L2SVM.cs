using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Engine.Brain.Extend;

namespace Engine.Brain.Method.Discriminate
{
    /// <summary>
    /// support vector machines
    /// </summary>
    public class L2SVM: IDiscriminate
    {
        MulticlassSupportVectorMachine<Linear> _ksvm;

        MulticlassSupportVectorLearning<Linear> _teacher;

        public L2SVM()
        {
            _teacher = new MulticlassSupportVectorLearning<Linear>
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
            double[][] dInputs = inputs.toDouble();
            _ksvm = _teacher.Learn(dInputs, outputs);
            return 0.0;
        }

        public int Predict(float[] input)
        {
            double[] dInput = input.toDouble();
            int predicted = _ksvm.Decide(dInput);
            return predicted;
        }

    }
}
