using System;
using System.Linq;
using CNTK;

namespace Engine.Brain.Utils
{
    /// <summary>
    /// NP helper for CNTK model
    /// </summary>
    public partial class NP
    {
        public static class CNTK
        {
            /// <summary>
            /// the inputDim dim must less then outputDim
            /// </summary>
            /// <param name="outputDim"></param>
            /// <param name="inputDim"></param>
            /// <param name="device"></param>
            /// <returns></returns>
            public static Constant GetProjectionMap(int outputDim, int inputDim, DeviceDescriptor device)
            {
                if (inputDim > outputDim) throw new Exception("Can only project from lower to higher dimensionality");
                float[] projectionMapValues = new float[inputDim * outputDim];
                for (int i = 0; i < inputDim * outputDim; i++)
                    projectionMapValues[i] = 0;
                for (int i = 0; i < inputDim; ++i)
                    projectionMapValues[(i * (int)inputDim) + i] = 1.0f;
                var projectionMap = new NDArrayView(DataType.Float, new int[] { 1, 1, inputDim, outputDim }, device);
                projectionMap.CopyFrom(new NDArrayView(new int[] { 1, 1, inputDim, outputDim }, projectionMapValues, (uint)projectionMapValues.Count(), device));
                return new Constant(projectionMap);
            }
			/// <summary>
            /// RestNet Model Helper Function
            /// </summary>
			public static class ResNet
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="wProj"></param>
                /// <param name="input"></param>
                /// <param name="hStride"></param>
                /// <param name="vStride"></param>
                /// <param name="bValue"></param>
                /// <param name="scValue"></param>
                /// <param name="bnTimeConst"></param>
                /// <param name="device"></param>
                /// <returns></returns>
                public static Function ProjectLayer(Variable wProj, Variable input, int hStride, int vStride, double bValue, double scValue, int bnTimeConst, DeviceDescriptor device)
                {
                    int outFeatureMapCount = wProj.Shape[0];
                    var b = new Parameter(new int[] { outFeatureMapCount }, (float)bValue, device, "");
                    var sc = new Parameter(new int[] { outFeatureMapCount }, (float)scValue, device, "");
                    var m = new Constant(new int[] { outFeatureMapCount }, 0.0f, device);
                    var v = new Constant(new int[] { outFeatureMapCount }, 0.0f, device);
                    var n = Constant.Scalar(0.0f, device);
                    int numInputChannels = input.Shape[input.Shape.Rank - 1];
                    var c = CNTKLib.Convolution(wProj, input, new int[] { hStride, vStride, numInputChannels }, new bool[] { true }, new bool[] { false });
                    return CNTKLib.BatchNormalization(c, sc, b, m, v, n, true /*spatial*/, (double)bnTimeConst, 0, 1e-5, false);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="input"></param>
                /// <param name="outFeatureMapCount"></param>
                /// <param name="kernelWidth"></param>
                /// <param name="kernelHeight"></param>
                /// <param name="wScale"></param>
                /// <param name="bValue"></param>
                /// <param name="scValue"></param>
                /// <param name="bnTimeConst"></param>
                /// <param name="spatial"></param>
                /// <param name="wProj"></param>
                /// <param name="device"></param>
                /// <returns></returns>
                public static Function ResNetNodeInc(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, double wScale, double bValue, double scValue, int bnTimeConst, bool spatial, Variable wProj, DeviceDescriptor device)
                {
                    var c1 = ConvBatchNormalizationReLULayer(input, outFeatureMapCount, kernelWidth, kernelHeight, 2, 2, wScale, bValue, scValue, bnTimeConst, spatial, device);
                    var c2 = ConvBatchNormalizationLayer(c1, outFeatureMapCount, kernelWidth, kernelHeight, 1, 1, wScale, bValue, scValue, bnTimeConst, spatial, device);
                    var cProj = ProjectLayer(wProj, input, 2, 2, bValue, scValue, bnTimeConst, device);
                    var p = CNTKLib.Plus(c2, cProj);
                    return CNTKLib.ReLU(p);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="input"></param>
                /// <param name="outFeatureMapCount"></param>
                /// <param name="kernelWidth"></param>
                /// <param name="kernelHeight"></param>
                /// <param name="wScale"></param>
                /// <param name="bValue"></param>
                /// <param name="scValue"></param>
                /// <param name="bnTimeConst"></param>
                /// <param name="spatial"></param>
                /// <param name="device"></param>
                /// <returns></returns>
                public static Function ResNetNode(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, double wScale, double bValue, double scValue, int bnTimeConst, bool spatial, DeviceDescriptor device)
                {
                    var c1 = ConvBatchNormalizationReLULayer(input, outFeatureMapCount, kernelWidth, kernelHeight, 1, 1, wScale, bValue, scValue, bnTimeConst, spatial, device);
                    var c2 = ConvBatchNormalizationLayer(c1, outFeatureMapCount, kernelWidth, kernelHeight, 1, 1, wScale, bValue, scValue, bnTimeConst, spatial, device);
                    var p = CNTKLib.Plus(c2, input);
                    return CNTKLib.ReLU(p);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="input"></param>
                /// <param name="outFeatureMapCount"></param>
                /// <param name="kernelWidth"></param>
                /// <param name="kernelHeight"></param>
                /// <param name="hStride"></param>
                /// <param name="vStride"></param>
                /// <param name="wScale"></param>
                /// <param name="bValue"></param>
                /// <param name="scValue"></param>
                /// <param name="bnTimeConst"></param>
                /// <param name="spatial"></param>
                /// <param name="device"></param>
                /// <returns></returns>
                public static Function ConvBatchNormalizationReLULayer(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, int hStride, int vStride, double wScale, double bValue, double scValue, int bnTimeConst, bool spatial, DeviceDescriptor device)
                {
                    var convBNFunction = ConvBatchNormalizationLayer(input, outFeatureMapCount, kernelWidth, kernelHeight, hStride, vStride, wScale, bValue, scValue, bnTimeConst, spatial, device);
                    return CNTKLib.ReLU(convBNFunction);
                }
                /// <summary>
                /// https://github.com/Microsoft/CNTK/blob/764c8c4e313cd4831d5d43e9ee0605b06b35ebb1/Examples/TrainingCSharp/Common/CifarResNetClassifier.cs#L112
                /// </summary>
                /// <param name="input"></param>
                /// <param name="outFeatureMapCount"></param>
                /// <param name="kernelWidth"></param>
                /// <param name="kernelHeight"></param>
                /// <param name="hStride"></param>
                /// <param name="vStride"></param>
                /// <param name="wScale"></param>
                /// <param name="bValue"></param>
                /// <param name="scValue"></param>
                /// <param name="bnTimeConst"></param>
                /// <param name="spatial"></param>
                /// <param name="device"></param>
                /// <returns></returns>
                public static Function ConvBatchNormalizationLayer(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, int hStride, int vStride, double wScale, double bValue, double scValue, int bnTimeConst, bool spatial, DeviceDescriptor device)
                {
                    int numInputChannels = input.Shape[input.Shape.Rank - 1];
                    var convParams = new Parameter(new int[] { kernelWidth, kernelHeight, numInputChannels, outFeatureMapCount }, DataType.Float, CNTKLib.GlorotUniformInitializer(wScale, -1, 2), device);
                    var convFunction = CNTKLib.Convolution(convParams, input, new int[] { hStride, vStride, numInputChannels });
                    var biasParams = new Parameter(new int[] { NDShape.InferredDimension }, (float)bValue, device, "");
                    var scaleParams = new Parameter(new int[] { NDShape.InferredDimension }, (float)scValue, device, "");
                    var runningMean = new Constant(new int[] { NDShape.InferredDimension }, 0.0f, device);
                    var runningInvStd = new Constant(new int[] { NDShape.InferredDimension }, 0.0f, device);
                    var runningCount = Constant.Scalar(0.0f, device);
                    return CNTKLib.BatchNormalization(convFunction, scaleParams, biasParams, runningMean, runningInvStd, runningCount, spatial, bnTimeConst, 0.0, 1e-5 /* epsilon */);
                }
            }
        }
    }
}
