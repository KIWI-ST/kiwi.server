using System.Collections.Generic;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// reference CNTK ResNet
    /// https://github.com/Microsoft/CNTK/blob/master/Examples/TrainingCSharp/Common/CifarResNetClassifier.cs
    /// </summary>
    public class ResNet
    {
        //input and output variable
        Variable inputVariable, outputVariable;
        int[] inputDim, outputDim;
        Function classifierOutput;
        Trainer trainer;
        DeviceDescriptor device;
        
        public ResNet(int width, int height, int channel, int ouputClasses, string deviceName)
        {
            device = NP.CNTK.GetDeviceByName(deviceName);
            inputDim = new int[] { width, height, channel };
            outputDim = new int[] { ouputClasses };
            inputVariable = CNTKLib.InputVariable(inputDim, DataType.Double, "Images");
            outputVariable = CNTKLib.InputVariable(outputDim, DataType.Double, "Labels");
            classifierOutput = CreateResNetModel(inputVariable, ouputClasses, device, "pred");
            var lossFunction = CNTKLib.CrossEntropyWithSoftmax(classifierOutput, outputVariable, "lossfunction");
            var prediction = CNTKLib.ClassificationError(classifierOutput, outputVariable, 5, "predictionError");
            var learningRatePerSample = new TrainingParameterScheduleDouble(0.0078125, 1);
            trainer = Trainer.CreateTrainer(classifierOutput, lossFunction, prediction, new List<Learner> { Learner.SGDLearner(classifierOutput.Parameters(), learningRatePerSample) });
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            var miniBatch = NP.CNTK.CreateMiniBatch(inputs, outputs, inputVariable, outputVariable, device);
#pragma warning disable 618
            trainer.TrainMinibatch(miniBatch, false, device);
#pragma warning restore 618
            return trainer.PreviousMinibatchLossAverage();
        }

        private Function CreateResNetModel(Variable input, int numOutputClasses, DeviceDescriptor device, string outputName)
        {
            double convWScale = 7.07;
            double convBValue = 0;
            double fc1WScale = 0.4;
            double fc1BValue = 0;
            double scValue = 1;
            int bnTimeConst = 4096;
            int kernelWidth = 3;
            int kernelHeight = 3;
            double conv1WScale = 0.26;
            int cMap1 = 16;
            var conv1 = NP.CNTK.ResNet.ConvBatchNormalizationReLULayer(input, cMap1, kernelWidth, kernelHeight, 1, 1, conv1WScale, convBValue, scValue, bnTimeConst, true /*spatial*/, device);
            var rn1_1 = NP.CNTK.ResNet.ResNetNode(conv1, cMap1, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, false /*spatial*/, device);
            var rn1_2 = NP.CNTK.ResNet.ResNetNode(rn1_1, cMap1, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, true /*spatial*/, device);
            var rn1_3 = NP.CNTK.ResNet.ResNetNode(rn1_2, cMap1, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, false /*spatial*/, device);
            int cMap2 = 32;
            var rn2_1_wProj = NP.CNTK.ResNet.GetProjectionMap(cMap2, cMap1, device);
            var rn2_1 = NP.CNTK.ResNet.ResNetNodeInc(rn1_3, cMap2, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, true /*spatial*/, rn2_1_wProj, device);
            var rn2_2 = NP.CNTK.ResNet.ResNetNode(rn2_1, cMap2, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, false /*spatial*/, device);
            var rn2_3 = NP.CNTK.ResNet.ResNetNode(rn2_2, cMap2, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, true /*spatial*/, device);
            int cMap3 = 64;
            var rn3_1_wProj = NP.CNTK.ResNet.GetProjectionMap(cMap3, cMap2, device);
            var rn3_1 = NP.CNTK.ResNet.ResNetNodeInc(rn2_3, cMap3, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, true /*spatial*/, rn3_1_wProj, device);
            var rn3_2 = NP.CNTK.ResNet.ResNetNode(rn3_1, cMap3, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, false /*spatial*/, device);
            var rn3_3 = NP.CNTK.ResNet.ResNetNode(rn3_2, cMap3, kernelWidth, kernelHeight, convWScale, convBValue, scValue, bnTimeConst, false /*spatial*/, device);
            // Global average pooling
            int poolW = 8;
            int poolH = 8;
            int poolhStride = 1;
            int poolvStride = 1;
            var pool = CNTKLib.Pooling(rn3_3, PoolingType.Average,
                new int[] { poolW, poolH, 1 }, new int[] { poolhStride, poolvStride, 1 });
            // Output DNN layer
            var outTimesParams = new Parameter(new int[] { numOutputClasses, 1, 1, cMap3 }, DataType.Double,
                CNTKLib.GlorotUniformInitializer(fc1WScale, 1, 0), device);
            var outBiasParams = new Parameter(new int[] { numOutputClasses }, fc1BValue, device, "");
            return CNTKLib.Plus(CNTKLib.Times(outTimesParams, pool), outBiasParams, outputName);
        }
    }
}
