using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// use Fully Channel Convoluation Neural Network instead of Deep Neural Network
    /// </summary>
    public class DNet2 : IDNet
    {
        /// <summary>
        /// log trained epochs
        /// </summary>
        private int traindEpochs = 0;

        /// <summary>
        /// trainer function
        /// </summary>
        readonly Trainer trainer;

        /// <summary>
        /// model 
        /// </summary>
        readonly Function classifierOutput;

        /// <summary>
        /// 
        /// </summary>
        private readonly Variable inputVariable;

        /// <summary>
        /// 
        /// </summary>
        private readonly Variable outputVariable;

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
        public DNet2(string deviceName, int w, int h, int c, int o)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            int[] inputDim = new int[] { w, h, c };
            int[] outputDim = new int[] { o };
            inputVariable = Variable.InputVariable(NDShape.CreateNDShape(inputDim), DataType.Double, "inputVariable");
            outputVariable = Variable.InputVariable(NDShape.CreateNDShape(outputDim), DataType.Double, "labelVariable");
            classifierOutput = CreateFullyChannelNetwork(inputVariable, c, o);
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, outputVariable);
            var prediction = CNTKLib.ClassificationError(classifierOutput, outputVariable);
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
            Function pooling1 = NP.CNTK.ConvolutionWithMaxPooling(input, 3, 3, channels[0], channels[1], 1, 1, 3, 3, device);
            Function pooling2 = NP.CNTK.ConvolutionWithMaxPooling(pooling1, 3, 3, channels[1], channels[2], 1, 1, 3, 3, device);
            Function pooling3 = NP.CNTK.ConvolutionWithMaxPooling(pooling2, 3, 3, channels[2], channels[3], 1, 1, 3, 3, device);
            Function pooling4 = NP.CNTK.ConvolutionWithMaxPooling(pooling3, 3, 3, channels[3], channels[4], 1, 1, 3, 3, device);
            return NP.CNTK.Dense(pooling4, outputClassNum, device, "ouput");
        }
        /// <summary>
        /// async parameters
        /// </summary>
        /// <param name="sourceNet"></param>
        public void Accept(IDNet sourceNet)
        {
            //convert to bytes 
            Stream modelStream = sourceNet.PersistenceMemory();
            byte[] bytes = new byte[modelStream.Length];
            modelStream.Read(bytes, 0, bytes.Length);
            modelStream.Seek(0, SeekOrigin.Begin);
            //read model and set parameters
            Function model = Function.Load(bytes, device);
            int count = model.Parameters().Count;
            //copy
            for(int i = 0; i < count; i++)
                classifierOutput.Parameters()[i].SetValue(model.Parameters()[i].Value());
        }
        /// <summary>
        /// store in memeory
        /// </summary>
        /// <returns></returns>
        public Stream PersistenceMemory()
        {
            byte[] model = classifierOutput.Save();
            Stream stream = new MemoryStream(model);
            return stream;
        }

        public string PersistencNative(string modelFilename = null)
        {
            throw new NotImplementedException();
        }

        public double[] Predict(params object[] inputs)
        {
            double[] input = inputs[0] as double[];
            using (Value inputsValue = Value.CreateBatch(inputVariable.Shape, input, device))
            {
                var inputDict = new Dictionary<Variable, Value>() { { inputVariable, inputsValue } };
                var outputDict = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
                classifierOutput.Evaluate(inputDict, outputDict, device);
                var prdict = outputDict[classifierOutput.Output].GetDenseData<double>(classifierOutput.Output);
                return prdict[0].ToArray();
            }
        }

        public double Train(double[][] inputs, double[][] outputs)
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
    }
}
