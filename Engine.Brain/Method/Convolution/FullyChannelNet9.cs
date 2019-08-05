using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Method.Convolution
{
    /// <summary>
    /// fully channel net with 9 layers
    /// 4 pooling layer
    /// 4 conv layer
    /// 1 dense layer
    /// </summary>
    public class FullyChannelNet9 : IDConvNet
    {
        /// <summary>
        /// store traind epochs
        /// </summary>
        int traindEpochs = 0;

        /// <summary>
        /// trainer function
        /// </summary>
        readonly Trainer trainer;

        /// <summary>
        /// model
        /// </summary>
        readonly Function classifierOutput;

        private readonly Variable inputVariable;

        private readonly Variable outputVariable;

        /// <summary>
        /// 
        /// </summary>
        readonly DeviceDescriptor device;

        /// <summary>
        /// create model by w,h,c,outputClassNum
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        /// <param name="outputClassNum"></param>
        /// <param name="deviceName"></param>
        public FullyChannelNet9(int w, int h, int c, int outputClassNum, string deviceName)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            int[] inputDim = new int[] { w, h, c };
            int[] outputDim = new int[] { outputClassNum };
            inputVariable = Variable.InputVariable(NDShape.CreateNDShape(inputDim), DataType.Float, "inputVariable");
            outputVariable = Variable.InputVariable(NDShape.CreateNDShape(outputDim), DataType.Float, "labelVariable");
            classifierOutput = CreateFullyChannelNetwork(inputVariable, c, outputClassNum);
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, outputVariable);
            var prediction = CNTKLib.ClassificationError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        /// <summary>
        /// create from saved model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="deviceName"></param>
        public FullyChannelNet9(Function model, string deviceName)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            inputVariable = model.Inputs.First(v => v.Name == "inputVariable");
            outputVariable = Variable.InputVariable(model.Output.Shape, DataType.Float, "labelVariable");
            classifierOutput = model;
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, outputVariable);
            var prediction = CNTKLib.ClassificationError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        private Function CreateFullyChannelNetwork(Variable input, int inputChannel, int outputClassNum)
        {
            int[] channels = new int[] { inputChannel, inputChannel, Math.Max(inputChannel / 2, 3), Math.Max(inputChannel / 3, 3), Math.Max(inputChannel / 3, 3) };
            Function pooling1 = NP.CNTK.ConvolutionWithMaxPooling(input, 3, 3, channels[0], channels[1], 1, 1, 3, 3, device, CNTKLib.ReLU);
            Function pooling2 = NP.CNTK.ConvolutionWithMaxPooling(pooling1, 3, 3, channels[1], channels[2], 1, 1, 3, 3, device, CNTKLib.ReLU);
            Function pooling3 = NP.CNTK.ConvolutionWithMaxPooling(pooling2, 3, 3, channels[2], channels[3], 1, 1, 3, 3, device, CNTKLib.ReLU);
            Function pooling4 = NP.CNTK.ConvolutionWithMaxPooling(pooling3, 3, 3, channels[3], channels[4], 1, 1, 3, 3, device, CNTKLib.ReLU);
            return NP.CNTK.Dense(pooling4, outputClassNum, device, CNTKLib.ReLU, "ouput");
        }

        public double Train(float[][] inputs, float[][] outputs)
        {
            //ensure that data is destroyed after use
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToOneDimensional(inputs), device))
            using (Value outputsValue = Value.CreateBatch(outputVariable.Shape, NP.ToOneDimensional(outputs), device))
            {
                traindEpochs++;
                var miniBatch = new Dictionary<Variable, Value>() { { inputVariable, inputsValue }, { outputVariable, outputsValue } };
                trainer.TrainMinibatch(miniBatch, true, device);
                return trainer.PreviousMinibatchEvaluationAverage();
            }
        }

        public void Accept(INet sourceNet)
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

        public string PersistencNative(string modelFilename = null)
        {
            modelFilename = modelFilename ?? string.Format(@"{0}\tmp\{1}_{2}_{3}_{4}_{5}_{6}.net", Directory.GetCurrentDirectory(), DateTime.Now.ToFileTimeUtc(), inputVariable.Shape[0], inputVariable.Shape[1], inputVariable.Shape[2], traindEpochs, typeof(FullyChannelNet9).Name);
            classifierOutput.Save(modelFilename);
            return modelFilename;
        }

        /// <summary>
        ///  forward calcute, same as batch size equals one
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public float[] Predict(float[] input)
        {
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, input, device))
            {
                var inputDict = new Dictionary<Variable, Value>() { { inputVariable, inputsValue } };
                var outputDict = new Dictionary<Variable, Value>() { { classifierOutput.Output, null} };
                classifierOutput.Evaluate(inputDict, outputDict, device);
                IList<IList<float>> prdicts = outputDict[classifierOutput.Output].GetDenseData<float>(classifierOutput.Output);
                float[] result = prdicts[0].ToArray();
                return result;
            }
        }

       public float[][] Predicts(float[][] inputs)
        {
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToOneDimensional(inputs), device))
            {
                var inputDict = new Dictionary<Variable, Value>() { { inputVariable, inputsValue } };
                var outputDict = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
                classifierOutput.Evaluate(inputDict, outputDict, device);
                var prdict = outputDict[classifierOutput.Output].GetDenseData<float>(classifierOutput.Output);
                float[][] outputs = new float[inputs.Length][];
                for (int i = 0; i < inputs.Length; i++)
                    outputs[i] = prdict[i].ToArray();
                return outputs;
            }
        }
    }
}
