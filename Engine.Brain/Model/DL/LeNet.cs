using System;
using System.Collections.Generic;
using System.IO;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    public class LeNet : IDConvNet
    {
        Variable inputVariable, outputVariable;

        int[] inputDim, outputDim;

        DeviceDescriptor device;

        Trainer trainer;

        public LeNet(int w, int h, int c, int outputClassNum, string deviceName)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            inputDim = new int[] { w, h, c };
            outputDim = new int[] { outputClassNum };
            inputVariable = Variable.InputVariable(NDShape.CreateNDShape(inputDim), DataType.Double, "inputVariable");
            outputVariable = Variable.InputVariable(NDShape.CreateNDShape(outputDim), DataType.Double, "outputVariable");
            var classifierOutput = CreateFullyChannelNetwork(inputVariable, c, outputClassNum);
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, outputVariable);
            var prediction = CNTKLib.ClassificationError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.003125, 1);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(classifierOutput.Parameters(), learningRatePerSample) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        private Function CreateFullyChannelNetwork(Variable input, int inputChannel, int outputClassNum)
        {
            int[] channels = new int[] { inputChannel, Math.Max(inputChannel/2, 3), Math.Max(inputChannel/3, 3), Math.Max(inputChannel/4, 3) };
            Function pooling1 = NP.CNTK.ConvolutionWithMaxPooling(input, 3, 1, channels[0], channels[1], 1, 1, 3, 3, device);
            Function pooling2 = NP.CNTK.ConvolutionWithMaxPooling(pooling1, 1, 3, channels[1], channels[2], 1, 1, 3, 3, device);
            Function pooling3 = NP.CNTK.ConvolutionWithMaxPooling(pooling2, 3, 3, channels[2], channels[3], 1, 1, 3, 3, device);
            Function pooling4 = NP.CNTK.ConvolutionWithMaxPooling(pooling3, 3, 3, channels[3], channels[4], 1, 1, 3, 3, device);
            return NP.CNTK.Dense(pooling4, outputClassNum, device, "ouput");
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            Value inputsValue = Value.CreateBatch(NDShape.CreateNDShape(inputDim), NP.ToUnidimensional(inputs), device);
            Value outputsValue = Value.CreateBatch(NDShape.CreateNDShape(outputDim), NP.ToUnidimensional(outputs), device);
            var miniBatch = new Dictionary<Variable, Value>()
            {
                {
                    inputVariable,
                    inputsValue
                },
                {
                    outputVariable,
                    outputsValue
                }
            };
#pragma warning disable 618
            trainer.TrainMinibatch(miniBatch, false, device);
#pragma warning restore 618
            return trainer.PreviousMinibatchLossAverage();
        }


        public void Accept(IDNet sourceNet)
        {
            throw new NotImplementedException();
        }

        public void ConvertToExtractNetwork()
        {
            throw new NotImplementedException();
        }

        public Stream PersistenceMemory()
        {
            throw new NotImplementedException();
        }

        public string PersistencNative()
        {
            throw new NotImplementedException();
        }

        public double[] Predict(double[] input)
        {
            throw new NotImplementedException();
        }

    }
}
