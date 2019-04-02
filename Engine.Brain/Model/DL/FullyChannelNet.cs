using System;
using System.Collections.Generic;
using System.IO;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    public class FullyChannelNet : IDConvNet
    {
        /// <summary>
        /// trainer function
        /// </summary>
        Trainer trainer;
        /// <summary>
        /// model
        /// </summary>
        Function classifierOutput;
        /// <summary>
        /// input and output
        /// </summary>
        Variable inputVariable, outputVariable;
        /// <summary>
        /// 
        /// </summary>
        readonly DeviceDescriptor device;

        public FullyChannelNet(int w, int h, int c, int outputClassNum, string deviceName)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            int[] inputDim = new int[] { w, h, c };
            int[] outputDim = new int[] { outputClassNum };
            inputVariable = Variable.InputVariable(NDShape.CreateNDShape(inputDim), DataType.Double, "inputVariable");
            outputVariable = Variable.InputVariable(NDShape.CreateNDShape(outputDim), DataType.Double, "outputVariable");
            classifierOutput = CreateFullyChannelNetwork(inputVariable, c, outputClassNum);
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, outputVariable);
            var prediction = CNTKLib.ClassificationError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.0005, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        private Function CreateFullyChannelNetwork(Variable input, int inputChannel, int outputClassNum)
        {
            int[] channels = new int[] { inputChannel, inputChannel, Math.Max(inputChannel / 2, 3), Math.Max(inputChannel / 3, 2), Math.Max(inputChannel / 3, 1) };
            Function pooling1 = NP.CNTK.ConvolutionWithMaxPooling(input, 3, 3, channels[0], channels[1], 1, 1, 3, 3, device);
            Function pooling2 = NP.CNTK.ConvolutionWithMaxPooling(pooling1, 3, 3, channels[1], channels[2], 1, 1, 3, 3, device);
            Function pooling3 = NP.CNTK.ConvolutionWithMaxPooling(pooling2, 3, 3, channels[2], channels[3], 1, 1, 3, 3, device);
            Function pooling4 = NP.CNTK.ConvolutionWithMaxPooling(pooling3, 3, 3, channels[3], channels[4], 1, 1, 3, 3, device);
            return NP.CNTK.Dense(pooling4, outputClassNum, device, "ouput");
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            //ensure that data is destroyed after use
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToUnidimensional(inputs), device))
            using (Value outputsValue = Value.CreateBatch(outputVariable.Shape, NP.ToUnidimensional(outputs), device))
            {
                var miniBatch = new Dictionary<Variable, Value>() { { inputVariable, inputsValue }, { outputVariable, outputsValue } };
                trainer.TrainMinibatch(miniBatch, true, device);
                return trainer.PreviousMinibatchEvaluationAverage();
            }
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
