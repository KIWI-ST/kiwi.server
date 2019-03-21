using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using System.Collections.Generic;
using System;

namespace Test.Examples
{
    [TestClass]
    public class CNTKTest
    {
        static float[][] train_images;
        static float[][] test_images;
        static float[][] train_labels;
        static float[][] test_labels;

        public CNTKTest()
        {
            if (!System.IO.File.Exists(@"Datasets\train_images.bin"))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(@"Datasets\mnist_data.zip", @"Datasets\");
            }
            train_images = LoadBinaryFile(@"Datasets\train_images.bin", 60000, 28 * 28);
            test_images = LoadBinaryFile(@"Datasets\test_images.bin", 10000, 28 * 28);
            train_labels = LoadBinaryFile(@"Datasets\train_labels.bin", 60000, 10);
            test_labels = LoadBinaryFile(@"Datasets\test_labels.bin", 60000, 10);
        }

        /// <summary>
        /// read data
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="numRows"></param>
        /// <param name="numColoums"></param>
        /// <returns></returns>
        static float[][] LoadBinaryFile(string filepath, int numRows, int numColoums)
        {
            var buffer = new byte[sizeof(float) * numRows * numColoums];
            using (var reader = new System.IO.BinaryReader(System.IO.File.OpenRead(filepath)))
            {
                reader.Read(buffer, 0, buffer.Length);
            }
            var dst = new float[numRows][];
            for (int row = 0; row < dst.Length; row++)
            {
                dst[row] = new float[numColoums];
                System.Buffer.BlockCopy(buffer, row * numColoums, dst[row], 0, numColoums);
            }
            return dst;
        }
        /// <summary>
        /// 
        /// </summary>
        static Function CreateLinearModel(Variable input, int outputDim, DeviceDescriptor device)
        {
            int inputDim = input.Shape[0];
            var W = new Parameter(new int[] { outputDim, inputDim }, DataType.Float, 
                CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank, 
                    CNTKLib.SentinelValueForInferParamInitRank,
                    1), device, "w");
            var B = new Parameter(new int[] { outputDim }, DataType.Float, CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    1), device, "b");
            var Y = CNTKLib.Times(W, input) + B;
            return CNTKLib.Sigmoid(Y);
        }
        /// <summary>
        /// get device
        /// </summary>
        /// <returns></returns>
        static DeviceDescriptor GetComputeDevice()
        {
            foreach (var gpuDevice in CNTK.DeviceDescriptor.AllDevices())
            {
                if (gpuDevice.Type == CNTK.DeviceKind.GPU) { return gpuDevice; }
            }
            return CNTK.DeviceDescriptor.CPUDevice;
        }
        /// <summary>
        /// random take inputs and labels
        /// </summary>
        /// <returns></returns>
        static (float[] inputs, float[] onehotLabels) GenerateRawDataSamples(int sampleSize, int inputDim, int numOutputClasses)
        {
            Random random = new Random(0);
            float[] inputs = new float[sampleSize * inputDim];
            float[] onehotLabels = new float[sampleSize * numOutputClasses];
            for (int sample = 0; sample < sampleSize; sample++)
            {
                int index = random.Next(60000);
                for (int i = 0; i < inputDim; i++)
                {
                    inputs[sample * inputDim + i] = train_images[index][i];
                }
                for (int i = 0; i < numOutputClasses; i++)
                {
                    inputs[sample * numOutputClasses + i] = train_labels[index][i];
                }
            }
            return (inputs, onehotLabels);
        }

        static (Value inputs, Value labels) GenerateValueData(int sampleSize, int inputDim, int numOutputClasses, DeviceDescriptor device)
        {
            var (inputsf, labelsf) = GenerateRawDataSamples(sampleSize, inputDim, numOutputClasses);
            Value inputs = Value.CreateBatch<float>(new int[] { inputDim }, inputsf, device);
            Value labels = Value.CreateBatch<float>(new int[] { numOutputClasses }, labelsf, device);
            return (inputs, labels);
        }

        /// <summary>
        /// https://github.com/Microsoft/CNTK/blob/release/latest/Examples/TrainingCSharp/Common/LogisticRegression.cs
        /// </summary>
        [TestMethod]
        public void DeepNeuralNetwork()
        {
            var device = GetComputeDevice();
            //create network
            Variable inputs = Variable.InputVariable(new int[] { 28 * 28 }, DataType.Float);
            Variable labels = Variable.InputVariable(new int[] { 10 }, DataType.Float);

            Variable layer1 = CreateLinearModel(inputs, 392, device);
            Variable layer2 = CreateLinearModel(layer1, 196, device);
            Variable layer3 = CreateLinearModel(layer2, 98, device);
            Function classifierOutput = CreateLinearModel(layer3, 10, device);

            Variable loss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, labels);
            Variable evalError = CNTKLib.ClassificationError(classifierOutput, labels);
            //trainer
            TrainingParameterScheduleDouble learningRateSchedule = new TrainingParameterScheduleDouble(0.01, 1);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(classifierOutput.Parameters(), learningRateSchedule) };
            var trainer = Trainer.CreateTrainer(classifierOutput, loss, evalError, parameterLearners);
            //
            string sss = "";
            for (int i = 0; i < 100000; i++)
            {
                var (input, label) = GenerateValueData(29, 28 * 28, 10, device);
#pragma warning disable 618
                trainer.TrainMinibatch(new Dictionary<Variable, Value>() { { inputs, input }, { labels, label } }, device);
#pragma warning restore 618
                var outputMap = new Dictionary<Variable, Value>() { { classifierOutput.Output, null } };
                classifierOutput.Evaluate(new Dictionary<Variable, Value>() { { inputs, input } }, outputMap, device);
                var outputVal = outputMap[classifierOutput.Output];
                var outputData = outputVal.GetDenseData<float>(classifierOutput.Output);
                sss += trainer.PreviousMinibatchLossAverage().ToString() + "\r\n";
            }
            //
            string ssssss = "";
        }

    }
}
