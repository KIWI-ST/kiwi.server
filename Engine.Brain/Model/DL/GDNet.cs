using System.Collections.Generic;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    public class GDNet
    {
        DeviceDescriptor device;
        Trainer trainer;
        int inputCount;
        int outputCount;
        Variable featureVariable, labelVariable;
        public GDNet(int inputDim, int numOutputClasses,string deviceName)
        {
            inputCount = inputDim;
            outputCount = numOutputClasses;
            device = NP.CNTK.GetDeviceByName(deviceName);
            featureVariable = Variable.InputVariable(new int[] { inputDim }, DataType.Double);
            labelVariable = Variable.InputVariable(new int[] { numOutputClasses }, DataType.Double);
            var classifierOutput = CreateLinearModel(featureVariable, numOutputClasses, device);
            var loss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, labelVariable);
            var evalError = CNTKLib.ClassificationError(classifierOutput, labelVariable);

            // prepare for training
            var learningRatePerSample = new CNTK.TrainingParameterScheduleDouble(0.02, 1);
            var parameterLearners =
                new List<Learner>() { Learner.SGDLearner(classifierOutput.Parameters(), learningRatePerSample) };
            trainer = Trainer.CreateTrainer(classifierOutput, loss, evalError, parameterLearners);
        }

        private Function CreateLinearModel(Variable input, int outputDim, DeviceDescriptor device)
        {
            int inputDim = input.Shape[0];
            var weightParam = new Parameter(new int[] { outputDim, inputDim }, DataType.Double, 1, device, "w");
            var biasParam = new Parameter(new int[] { outputDim }, DataType.Double, 0, device, "b");

            return CNTKLib.Times(weightParam, input) + biasParam;
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            Value inputsValue = Value.CreateBatch(NDShape.CreateNDShape(new int[] { inputCount}), NP.ToUnidimensional(inputs), device);
            Value outputsValue = Value.CreateBatch(NDShape.CreateNDShape(new int[] { outputCount }), NP.ToUnidimensional(outputs), device);
            var miniBatch = new Dictionary<Variable, Value>()
            {
                {
                    featureVariable,
                    inputsValue
                },
                {
                    labelVariable,
                    outputsValue
                }
            };

            trainer.TrainMinibatch(miniBatch, false, device);
            return trainer.PreviousMinibatchLossAverage();
        }


    }
}
