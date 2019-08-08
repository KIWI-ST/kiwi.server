using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Method.DeepQNet.Net
{
    /// <summary>
    /// use Fully Channel Convoluation Neural Network instead of Deep Neural Network
    /// </summary>
    public class DNetCNN : ISupportNet
    {
        /// <summary>
        /// log trained epochs
        /// </summary>
        private int traindEpochs = 0;

        /// <summary>
        /// trainer function
        /// </summary>
        Trainer trainer;

        /// <summary>
        /// model 
        /// </summary>
        Function classifierOutput;

        /// <summary>
        /// 
        /// </summary>
        private Variable inputVariable;

        /// <summary>
        /// 
        /// </summary>
        private Variable outputVariable;

        /// <summary>
        /// device
        /// </summary>
        readonly DeviceDescriptor device;

        /// <summary>
        /// select device to run model
        /// </summary>
        /// <param name="deviceName">select device to run model</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="c">channel</param>
        /// <param name="o">output class num</param>
        public DNetCNN(string deviceName, int w, int h, int c, int o)
        {
            device = NP.CNTKHelper.GetDeviceByName(deviceName);
            int[] inputDim = new int[] { w, h, c };
            int[] outputDim = new int[] { o };
            inputVariable = Variable.InputVariable(NDShape.CreateNDShape(inputDim), DataType.Float, "inputVariable");
            outputVariable = Variable.InputVariable(NDShape.CreateNDShape(outputDim), DataType.Float, "labelVariable");
            classifierOutput = CreateFullyChannelNetwork(inputVariable, c, o);
            var trainingLoss = CNTKLib.SquaredError(classifierOutput, outputVariable);
            var prediction = CNTKLib.SquaredError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        public DNetCNN(byte[] buffer, string deviceName)
        {
            device = NP.CNTKHelper.GetDeviceByName(deviceName);
            //read model and set parameters
            classifierOutput = Function.Load(buffer, device);
            inputVariable = classifierOutput.Inputs.First(v => v.Name == "inputVariable");
            outputVariable = Variable.InputVariable(classifierOutput.Output.Shape, DataType.Float, "labelVariable");
            var trainingLoss = CNTKLib.SquaredError(classifierOutput, outputVariable);
            var prediction = CNTKLib.SquaredError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputChannel"></param>
        /// <param name="outputClassNum"></param>
        /// <returns></returns>
        private Function CreateFullyChannelNetwork(Variable input, int inputChannel, int outputClassNum)
        {
            int[] channels = new int[] { inputChannel, inputChannel, Math.Max(inputChannel / 2, 3), Math.Max(inputChannel / 3, 3), Math.Max(inputChannel / 3, 3) };
            Function pooling1 = NP.CNTKHelper.ConvolutionWithMaxPooling(input, 3, 3, channels[0], channels[1], 1, 1, 3, 3, device, CNTKLib.SELU);
            Function pooling2 = NP.CNTKHelper.ConvolutionWithMaxPooling(pooling1, 3, 3, channels[1], channels[2], 1, 1, 3, 3, device, CNTKLib.SELU);
            Function pooling3 = NP.CNTKHelper.ConvolutionWithMaxPooling(pooling2, 3, 3, channels[2], channels[3], 1, 1, 3, 3, device, CNTKLib.SELU);
            Function pooling4 = NP.CNTKHelper.ConvolutionWithMaxPooling(pooling3, 3, 3, channels[3], channels[4], 1, 1, 3, 3, device, CNTKLib.SELU);
            return NP.CNTKHelper.Dense(pooling4, outputClassNum, device, CNTKLib.SELU, "ouput");
        }

        /// <summary>
        /// async parameters
        /// </summary>
        /// <param name="sourceNet"></param>
        public void Accept(ISupportNet sourceNet)
        {
            //convert to bytes 
            byte[] bytes = sourceNet.PersistenceMemory();
            //read model and set parameters
            classifierOutput = Function.Load(bytes, device);
            inputVariable = classifierOutput.Inputs.First(v => v.Name == "inputVariable");
            outputVariable = Variable.InputVariable(classifierOutput.Output.Shape, DataType.Float, "labelVariable");
            var trainingLoss = CNTKLib.SquaredError(classifierOutput, outputVariable);
            var prediction = CNTKLib.SquaredError(classifierOutput, outputVariable);
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.00178125, 1); //0.00178125
            TrainingParameterScheduleDouble momentumTimeConstant = CNTKLib.MomentumAsTimeConstantSchedule(256);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.MomentumSGDLearner(classifierOutput.Parameters(), learningRatePerSample, momentumTimeConstant, true) };
            trainer = Trainer.CreateTrainer(classifierOutput, trainingLoss, prediction, parameterLearners);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] PersistenceMemory()
        {
            byte[] modelBytes = classifierOutput.Save();
            return modelBytes;
        }

        public string PersistencNative(string modelFilename = null)
        {
            byte[] bytes = PersistenceMemory();
            Stream modelStream = new MemoryStream(bytes);
            using (FileStream fileStream = File.Create(modelFilename))
            {
                modelStream.CopyTo(fileStream);
            }
            return modelFilename;
        }

        /// <summary>
        /// 载入方法
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static DNetCNN Load(byte[] bytes, string deviceName)
        {
            return new DNetCNN(bytes, deviceName);
        }

        /// <summary>
        /// calcute forward once
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public float[] Predict(float[] input)
        {
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, input, device))
            {
                var inputDict = new Dictionary<Variable, Value>() { { inputVariable, inputsValue } };
                var outputDict = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
                classifierOutput.Evaluate(inputDict, outputDict, device);
                IList<IList<float>> prdicts = outputDict[classifierOutput.Output].GetDenseData<float>(classifierOutput.Output);
                float[] result = prdicts[0].ToArray();
                return result;
            }
        }

        public double Train(float[][] inputs, float[][] outputs)
        {
            //ensure that data is destroyed after use
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToOneDimensional(inputs), device))
            using (Value outputsValue = Value.CreateBatch(outputVariable.Shape, NP.ToOneDimensional(outputs), device))
            {
                traindEpochs++;
                var miniBatch = new Dictionary<Variable, Value>() { { inputVariable, inputsValue }, { outputVariable, outputsValue } };
                trainer.TrainMinibatch(miniBatch, false, device);
                return trainer.PreviousMinibatchEvaluationAverage();
            }
        }
    }
}
