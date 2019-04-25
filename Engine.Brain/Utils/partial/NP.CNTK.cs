﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CNTK;
using Engine.Brain.Model;
using Engine.Brain.Model.DL;

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
            /// device get
            /// </summary>
            /// <param name="deviceName"></param>
            /// <returns></returns>
            public static DeviceDescriptor GetDeviceByName(string deviceName) { return devices[deviceName]; }
            /// <summary>
            /// deivces collection
            /// </summary>
            static Dictionary<string, DeviceDescriptor> devices = DeviceDescriptor.AllDevices().ToDictionary(device => string.Format("{0}-{1}", device.Id, device.Type), device => device);
            /// <summary>
            /// get device map collection
            /// </summary>
            public static List<string> DeviceCollection { get { return devices.Keys.ToList(); } }
           /// <summary>
           /// load IConvNet type model
           /// </summary>
           /// <param name="modelFilename"></param>
           /// <param name="deviceName"></param>
           /// <returns></returns>
            public static IDConvNet LoadModel(string modelFilename, string deviceName)
            {
                string modelType = System.IO.Path.GetFileNameWithoutExtension(modelFilename).Split('_').Last();
                var device = devices[deviceName];
                var outputClassifier = Function.Load(modelFilename, device);
                if(modelType== typeof(FullyChannelNet9).Name)
                    return new FullyChannelNet9(outputClassifier, deviceName);
                else
                    return null;
            }
            /// <summary>
            /// load data from binary file
            /// </summary>
            /// <param name="binaryFilename"></param>
            /// <param name="numRows"></param>
            /// <param name="numCols"></param>
            /// <returns></returns>
            public static double[][] LoadBinary(string binaryFilename, int numRows, int numCols)
            {
                var buffer = new byte[sizeof(float) * numRows * numCols];
                var dst = new double[numRows][];
                using (var reader = new System.IO.BinaryReader(System.IO.File.OpenRead(binaryFilename)))
                    reader.Read(buffer, 0, buffer.Length);
                for (int row = 0; row < dst.Length; row++)
                {
                    dst[row] = new double[numCols];
                    Buffer.BlockCopy(buffer, row * numCols * sizeof(float), dst[row], 0, numCols * sizeof(float));
                }
                return dst;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static int SizeOf<T>() where T : struct
            {
                return Marshal.SizeOf(default(T));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="src"></param>
            /// <returns></returns>
            public static T[] ConvertJaggedArrayToSingleDimensionalArray<T>(T[][] src) where T:struct
            {
                var numRows = src.Length;
                var numColumns = src[0].Length;
                var numBytesInRow = numColumns * SizeOf<T>();
                var dst = new T[numRows * numColumns];
                var dstOffset = 0;
                for (int row = 0; row < numRows; row++)
                {
                    System.Diagnostics.Debug.Assert(src[row].Length == numColumns);
                    System.Buffer.BlockCopy(src[row], 0, dst, dstOffset, numBytesInRow);
                    dstOffset += numBytesInRow;
                }
                return dst;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fetures"></param>
            /// <param name="kernelWidth"></param>
            /// <param name="kernelHeight"></param>
            /// <param name="inputChannel"></param>
            /// <param name="outputChannel"></param>
            /// <param name="hStride"></param>
            /// <param name="vStride"></param>
            /// <param name="poolingWindowWidth"></param>
            /// <param name="poolingWindowHeight"></param>
            /// <param name="device"></param>
            /// <returns></returns>
            public static Function ConvolutionWithMaxPooling(
                Variable fetures,
                int kernelWidth, int kernelHeight, int inputChannel, int outputChannel,
                int hStride, int vStride,
                int poolingWindowWidth, int poolingWindowHeight,
                DeviceDescriptor device)
            {
                double convWScale = 0.26;
                var convParameters = new Parameter(
                    new int[] { kernelWidth, kernelHeight, inputChannel, outputChannel },
                    DataType.Double,
                    CNTKLib.GlorotUniformInitializer(convWScale, -1, 2),
                    device);
                Function convFunction = CNTKLib.Convolution(convParameters, fetures, new int[] { 1, 1, inputChannel });
                //use Selu
                Function reluFunction = CNTKLib.SELU(convFunction);
                Function poolling = CNTKLib.Pooling(
                    reluFunction, PoolingType.Max,
                    new int[] { poolingWindowWidth, poolingWindowHeight },
                    new int[] { hStride, vStride },
                    new bool[] { true });
                return poolling;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <param name="outputDim"></param>
            /// <param name="device"></param>
            /// <param name="outputName"></param>
            /// <returns></returns>
            public static Function FullyConnectedLinearLayer(Variable input, int outputDim, DeviceDescriptor device, string outputName = "")
            {
                int inputDim = input.Shape[0];
                int[] i = { outputDim, inputDim };
                var timesParam = new Parameter(i, DataType.Double,
                CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank, 1),
                device, "timesParam");
                var timesFunction = CNTKLib.Times(timesParam, input, "times");
                int[] o = { outputDim };
                var plusParam = new Parameter(o, 0.0, device, "plusParam");
                return CNTKLib.Plus(plusParam, timesFunction, outputName);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <param name="outputDim"></param>
            /// <param name="device"></param>
            /// <param name="outputName"></param>
            /// <returns></returns>
            public static Function Dense(Variable input, int outputDim, DeviceDescriptor device, string outputName = "")
            {
                if (input.Shape.Rank != 1)
                {
                    int reshapeDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                    input = CNTKLib.Reshape(input, new int[] { reshapeDim });
                }
                Function fc = FullyConnectedLinearLayer(input, outputDim, device, outputName);
                return CNTKLib.SELU(fc);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="shape"></param>
            /// <param name="device"></param>
            /// <param name="weights"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public static Function Embedding(Variable x, int shape, DeviceDescriptor device, double[][] weights = null, string name = "")
            {
                if(weights==null)
                {
                    var weightShape = new int[] { shape, NDShape.InferredDimension };
                    var E = new Parameter(
                        weightShape,
                        DataType.Double,
                        CNTKLib.GlorotUniformInitializer(),
                        device,
                        name);
                    return CNTKLib.Times(E, x);
                }
                else
                {
                    var weight_shape = new int[] { shape, x.Shape.Dimensions[0] };
                    System.Diagnostics.Debug.Assert(shape == weights[0].Length);
                    System.Diagnostics.Debug.Assert(weight_shape[1] == weights.Length);
                    var w = ConvertJaggedArrayToSingleDimensionalArray(weights);
                    var ndArrayView = new NDArrayView(weight_shape, w, device, readOnly: true);
                    var E = new Constant(ndArrayView, name: "fixed_embedding_" + name);
                    return CNTKLib.Times(E, x);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="inputs"></param>
            /// <param name="outputs"></param>
            /// <returns></returns>
            public static Dictionary<Variable, Value> CreateMiniBatch(double[][] inputs, double[][] outputs, Variable inputVariable, Variable outputVariable, DeviceDescriptor device)
            {
                Value inputsValue = Value.CreateBatch(inputVariable.Shape, NP.ToOneDimensional(inputs), device);
                Value outputsValue = Value.CreateBatch(outputVariable.Shape, NP.ToOneDimensional(outputs), device);
                var miniBatch = new Dictionary<Variable, Value>() { { inputVariable, inputsValue }, { outputVariable, outputsValue } };
                return miniBatch;
            }
            /// <summary>
            /// learning rate reduce
            /// </summary>
            public class ReduceLROnPlateau
            {
                readonly Learner learner;
                double lr = 0;
                double best_metric = 1e-5;
                int slot_since_last_update = 0;

                public ReduceLROnPlateau(Learner learner, double lr)
                {
                    this.learner = learner;
                    this.lr = lr;
                }
                public bool update(double current_metric)
                {
                    bool should_stop = false;
                    if (current_metric < best_metric)
                    {
                        best_metric = current_metric;
                        slot_since_last_update = 0;
                        return should_stop;
                    }
                    slot_since_last_update++;
                    if (slot_since_last_update > 10)
                    {
                        lr *= 0.75;
                        learner.ResetLearningRate(new TrainingParameterScheduleDouble(lr));
                        Console.WriteLine($"Learning rate set to {lr}");
                        slot_since_last_update = 0;
                        should_stop = (lr < 1e-6);
                    }
                    return should_stop;
                }
            }
            /// <summary>
            /// RestNet Model Helper Function
            /// </summary>
            public static class ResNet
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
                    double[] projectionMapValues = new double[inputDim * outputDim];
                    for (int i = 0; i < inputDim * outputDim; i++)
                        projectionMapValues[i] = 0;
                    for (int i = 0; i < inputDim; ++i)
                        projectionMapValues[(i * inputDim) + i] = 1.0;
                    var projectionMap = new NDArrayView(DataType.Double, new int[] { 1, 1, inputDim, outputDim }, device);
                    projectionMap.CopyFrom(new NDArrayView(new int[] { 1, 1, inputDim, outputDim }, projectionMapValues, (uint)projectionMapValues.Count(), device));
                    return new Constant(projectionMap);
                }
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
                    var b = new Parameter(new int[] { outFeatureMapCount }, bValue, device, "");
                    var sc = new Parameter(new int[] { outFeatureMapCount }, scValue, device, "");
                    var m = new Constant(new int[] { outFeatureMapCount }, DataType.Double, 0.0, device);
                    var v = new Constant(new int[] { outFeatureMapCount }, DataType.Double, 0.0, device);
                    var n = Constant.Scalar(DataType.Double, 0.0, device);
                    int numInputChannels = input.Shape[input.Shape.Rank - 1];
                    var c = CNTKLib.Convolution(wProj, input, new int[] { hStride, vStride, numInputChannels }, new bool[] { true }, new bool[] { false });
                    return CNTKLib.BatchNormalization(c, sc, b, m, v, n, true /*spatial*/, bnTimeConst, 0, 1e-5, false);
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
                    var convParams = new Parameter(new int[] { kernelWidth, kernelHeight, numInputChannels, outFeatureMapCount }, DataType.Double, CNTKLib.GlorotUniformInitializer(wScale, -1, 2), device);
                    var convFunction = CNTKLib.Convolution(convParams, input, new int[] { hStride, vStride, numInputChannels });
                    var biasParams = new Parameter(new int[] { NDShape.InferredDimension }, bValue, device, "");
                    var scaleParams = new Parameter(new int[] { NDShape.InferredDimension }, scValue, device, "");
                    var runningMean = new Constant(new int[] { NDShape.InferredDimension }, DataType.Double, 0.0, device);
                    var runningInvStd = new Constant(new int[] { NDShape.InferredDimension }, DataType.Double, 0.0, device);
                    var runningCount = Constant.Scalar(DataType.Double, 0.0, device);
                    return CNTKLib.BatchNormalization(convFunction, scaleParams, biasParams, runningMean, runningInvStd, runningCount, spatial, bnTimeConst, 0.0, 1e-5 /* epsilon */);
                }
            }
        }
    }
}
