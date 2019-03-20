using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using System.Collections.Generic;

namespace Test.Examples
{
    [TestClass]
    public class CNTKTest
    {
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
            var W = new Parameter(new int[] { outputDim, inputDim }, DataType.Float, 1, device, "w");
            var B = new Parameter(new int[] { outputDim }, DataType.Float, 0, device, "b");
            return CNTKLib.Times(W, input) + B;
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
        /// https://github.com/Microsoft/CNTK/blob/release/latest/Examples/TrainingCSharp/Common/LogisticRegression.cs
        /// </summary>
        [TestMethod]
        public void DeepNeuralNetwork()
        {
            float[][] train_images;
            float[][] test_images;
            float[][] train_labels;
            float[][] test_labels;
            if (!System.IO.File.Exists(@"Datasets\train_images.bin"))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(@"Datasets\mnist_data.zip", @"Datasets\");
            }
            train_images = LoadBinaryFile(@"Datasets\train_images.bin", 60000, 28 * 28);
            test_images = LoadBinaryFile(@"Datasets\test_images.bin", 10000, 28 * 28);
            train_labels = LoadBinaryFile(@"Datasets\train_labels.bin", 60000, 10);
            test_labels = LoadBinaryFile(@"Datasets\test_labels.bin", 60000, 10);
            //create network
            Variable inputs = Variable.InputVariable(new int[] { 28 * 28 }, DataType.Float);
            Variable labels = Variable.InputVariable(new int[10], DataType.Float);
            Function classifierOutput = CreateLinearModel(inputs, 10, GetComputeDevice());
            Variable loss = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, labels);
            Variable evalError = CNTKLib.ClassificationError(classifierOutput, labels);
            //trainer
            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.02, 1);
            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(classifierOutput.Parameters(), learningRatePerSample) };
            var trainer = Trainer.CreateTrainer(classifierOutput, loss, evalError, parameterLearners);
            //

            //
        }

    }
}
