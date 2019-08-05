using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CNTK;
using Engine.Brain.Method;
using Engine.Brain.Method.Convolution;

namespace Engine.Brain.Utils
{
    /// <summary>
    /// NP helper for CNTK model
    /// </summary>
    public partial class NP
    {
        public static class CNTKHelper
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
            static readonly Dictionary<string, DeviceDescriptor> devices = DeviceDescriptor.AllDevices().ToDictionary(device => string.Format("{0}-{1}", device.Id, device.Type), device => device);
            
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
            public static IConvNet LoadModel(string modelFilename, string deviceName)
            {
                string modelType = System.IO.Path.GetFileNameWithoutExtension(modelFilename).Split('_').Last();
                var device = devices[deviceName];
                var outputClassifier = Function.Load(modelFilename, device);
                if (modelType == typeof(FullyChannelNet9).Name)
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
            public static float[][] LoadBinary(string binaryFilename, int numRows, int numCols)
            {
                var buffer = new byte[sizeof(float) * numRows * numCols];
                var dst = new float[numRows][];
                using (var reader = new System.IO.BinaryReader(System.IO.File.OpenRead(binaryFilename)))
                    reader.Read(buffer, 0, buffer.Length);
                for (int row = 0; row < dst.Length; row++)
                {
                    dst[row] = new float[numCols];
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
            /// <param name="input"></param>
            /// <param name="num_output_channels"></param>
            /// <param name="filter_shape"></param>
            /// <param name="device"></param>
            /// <param name="activation"></param>
            /// <param name="use_padding"></param>
            /// <param name="use_bias"></param>
            /// <param name="strides"></param>
            /// <param name="outputName"></param>
            /// <returns></returns>
            public static Function Convolution2D(
                Variable input,
                int num_output_channels,
                int[] filter_shape,
                DeviceDescriptor device,
                Func<Variable, string, Function> activation = null,
                bool use_padding = false,
                bool use_bias = true,
                int[] strides = null,
                string outputName = "")
            {
                var convolution_map_size = new int[] { filter_shape[0], filter_shape[1], NDShape.InferredDimension, num_output_channels };
                strides = strides ?? (new int[] { 1 });
                var rtrn = Convolution(input, convolution_map_size, device, use_padding, use_bias, strides, activation, outputName);
                return rtrn;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <param name="convolution_map_size"></param>
            /// <param name="device"></param>
            /// <param name="use_padding"></param>
            /// <param name="use_bias"></param>
            /// <param name="strides"></param>
            /// <param name="activation"></param>
            /// <param name="outputName"></param>
            /// <returns></returns>
            public static Function Convolution(
                Variable input,
                int[] convolution_map_size,
                DeviceDescriptor device,
                bool use_padding,
                bool use_bias,
                int[] strides,
                Func<Variable, string, Function> activation = null,
                string outputName = "")
            {
                var W = new Parameter(
                    NDShape.CreateNDShape(convolution_map_size),
                    DataType.Float,
                    CNTKLib.GlorotUniformInitializer(CNTKLib.DefaultParamInitScale, CNTKLib.SentinelValueForInferParamInitRank, CNTKLib.SentinelValueForInferParamInitRank, 1),
                    device,
                    outputName + "_W");
                //y = Wx+b
                Variable y = CNTKLib.Convolution(W, input, strides, new BoolVector(new bool[] { true }) /* sharing */, new BoolVector(new bool[] { use_padding }));
                //apply bias
                if (use_bias)
                {
                    var num_output_channels = convolution_map_size[convolution_map_size.Length - 1];
                    var b_shape = Concat(Enumerable.Repeat(1, convolution_map_size.Length - 2).ToArray(), new int[] { num_output_channels });
                    var b = new Parameter(b_shape, 0.0f, device, outputName + "_b");
                    y = CNTKLib.Plus(y, b);
                }
                //apply activation
                if (activation != null)
                {
                    y = activation(y, outputName);
                }
                return y;
            }

            /// <summary>
            /// https://microsoft.github.io/CNTK-R/reference/ConvolutionTranspose.html
            /// </summary>
            /// <param name="input"></param>
            /// <param name="filter_shape"></param>
            /// <param name="num_filters"></param>
            /// <param name="activation"></param>
            /// <param name="device"></param>
            /// <param name="use_padding"></param>
            /// <param name="strides"></param>
            /// <param name="use_bias"></param>
            /// <param name="output_shape"></param>
            /// <param name="dilation"></param>
            /// <param name="max_temp_mem_size_in_samples"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public static Function ConvolutionTranspose(
                Variable input,
                int[] filter_shape,
                int num_filters,
                DeviceDescriptor device,
                Func<Variable, Function> activation = null,
                bool use_padding =true,
                int[] strides =null,
                bool use_bias = true,
                int[] output_shape=null,
                int[] dilation = null,
                uint max_temp_mem_size_in_samples = 0,
                string name = "")
            {
                strides = strides?? new int[] { 1 };
                var sharing = PadToShape(filter_shape, true);
                var padding = PadToShape(filter_shape, use_padding);
                padding = Concat(padding, new bool[] { false });
                if (dilation == null)
                    dilation = PadToShape(filter_shape, 1);
                var output_channels_shape = new int[] { num_filters };
                var kernel_shape = Concat(filter_shape, output_channels_shape, new int[] { NDShape.InferredDimension });
                var output_full_shape = output_shape;
                if (output_full_shape != null)
                    output_full_shape = Concat(output_shape, output_channels_shape);
                var filter_rank = filter_shape.Length;
                var init = CNTKLib.GlorotUniformInitializer(CNTKLib.DefaultParamInitScale, CNTKLib.SentinelValueForInferParamInitRank, CNTKLib.SentinelValueForInferParamInitRank, 1);
                var W = new Parameter(kernel_shape, DataType.Float, init, device, name = "W");
                var r = CNTKLib.ConvolutionTranspose(
                  convolutionMap: W,
                  operand: input,
                  strides: strides,
                  sharing: new BoolVector(sharing),
                  autoPadding: new BoolVector(padding),
                  outputShape: output_full_shape,
                  dilation: dilation,
                  reductionRank: 1, // in this case, the reductionRank must be equals 1
                  maxTempMemSizeInSamples: max_temp_mem_size_in_samples);
                if (use_bias)
                {
                    var b_shape = Concat(Enumerable.Repeat(1, filter_shape.Length).ToArray(), output_channels_shape);
                    var b = new Parameter(b_shape, 0.0, device, "B");
                    r = CNTKLib.Plus(r, b);
                }
                if (activation != null)
                    r = activation(r);
                return r;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="src"></param>
            /// <returns></returns>
            public static T[] ConvertJaggedArrayToSingleDimensionalArray<T>(T[][] src) where T : struct
            {
                var numRows = src.Length;
                var numColumns = src[0].Length;
                var numBytesInRow = numColumns * SizeOf<T>();
                var dst = new T[numRows * numColumns];
                var dstOffset = 0;
                for (int row = 0; row < numRows; row++)
                {
                    System.Diagnostics.Debug.Assert(src[row].Length == numColumns);
                    Buffer.BlockCopy(src[row], 0, dst, dstOffset, numBytesInRow);
                    dstOffset += numBytesInRow;
                }
                return dst;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
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
                Variable input,
                int kernelWidth,
                int kernelHeight,
                int inputChannel,
                int outputChannel,
                int hStride, int vStride,
                int poolingWindowWidth,
                int poolingWindowHeight,
                DeviceDescriptor device,
                Func<Variable, Function> activation = null,
                string outputName = "")
            {
                float convWScale = 0.26f;
                var W = new Parameter(
                    new int[] { kernelWidth, kernelHeight, inputChannel, outputChannel },
                    DataType.Float,
                    CNTKLib.GlorotUniformInitializer(convWScale, -1, 2),
                    device);
                Function y = CNTKLib.Convolution(W, input, new int[] { 1, 1, inputChannel });
                y = activation != null ? activation(y) : y;
                //pooling 
                y = CNTKLib.Pooling(y,
                    PoolingType.Max,
                    new int[] { poolingWindowWidth, poolingWindowHeight },
                    new int[] { hStride, vStride },
                    new bool[] { true });
                return y;
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
                var timesParam = new Parameter(i, DataType.Float, CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank, 1), device, "timesParam");
                var timesFunction = CNTKLib.Times(timesParam, input, "times");
                int[] o = { outputDim };
                var plusParam = new Parameter(o, 0.0f, device, "plusParam");
                return CNTKLib.Plus(plusParam, timesFunction, outputName);
            }

            /// <summary>
            /// y = ax +b , y1 = activate(y) 
            /// </summary>
            /// <param name="input"></param>
            /// <param name="outputDim"></param>
            /// <param name="device"></param>
            /// <param name="outputName"></param>
            /// <returns></returns>
            public static Function Dense(Variable input, int outputDim, DeviceDescriptor device, Func<Variable, Function> activation = null, string outputName = "")
            {
                if (input.Shape.Rank != 1)
                {
                    int reshapeDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                    input = CNTKLib.Reshape(input, new int[] { reshapeDim });
                }
                Function fc = FullyConnectedLinearLayer(input, outputDim, device, outputName);
                fc = activation != null ? activation(fc) : fc;
                return fc;
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
            public static Function Embedding(Variable x, int shape, DeviceDescriptor device, float[][] weights = null, string name = "")
            {
                if (weights == null)
                {
                    var weightShape = new int[] { shape, NDShape.InferredDimension };
                    var E = new Parameter(
                        weightShape,
                        DataType.Float,
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
            public static Dictionary<Variable, Value> CreateMiniBatch(float[][] inputs, float[][] outputs, Variable inputVariable, Variable outputVariable, DeviceDescriptor device)
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
                    float[] projectionMapValues = new float[inputDim * outputDim];
                    for (int i = 0; i < inputDim * outputDim; i++)
                        projectionMapValues[i] = 0;
                    for (int i = 0; i < inputDim; ++i)
                        projectionMapValues[(i * inputDim) + i] = 1.0f;
                    var projectionMap = new NDArrayView(DataType.Float, new int[] { 1, 1, inputDim, outputDim }, device);
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
                public static Function ProjectLayer(Variable wProj, Variable input, int hStride, int vStride, float bValue, float scValue, int bnTimeConst, DeviceDescriptor device)
                {
                    int outFeatureMapCount = wProj.Shape[0];
                    var b = new Parameter(new int[] { outFeatureMapCount }, bValue, device, "");
                    var sc = new Parameter(new int[] { outFeatureMapCount }, scValue, device, "");
                    var m = new Constant(new int[] { outFeatureMapCount }, DataType.Float, 0.0f, device);
                    var v = new Constant(new int[] { outFeatureMapCount }, DataType.Float, 0.0f, device);
                    var n = Constant.Scalar(DataType.Float, 0.0f, device);
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
                public static Function ResNetNodeInc(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, float wScale, float bValue, float scValue, int bnTimeConst, bool spatial, Variable wProj, DeviceDescriptor device)
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
                public static Function ResNetNode(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, float wScale, float bValue, float scValue, int bnTimeConst, bool spatial, DeviceDescriptor device)
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
                public static Function ConvBatchNormalizationReLULayer(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, int hStride, int vStride, float wScale, float bValue, float scValue, int bnTimeConst, bool spatial, DeviceDescriptor device)
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
                public static Function ConvBatchNormalizationLayer(Variable input, int outFeatureMapCount, int kernelWidth, int kernelHeight, int hStride, int vStride, float wScale, float bValue, float scValue, int bnTimeConst, bool spatial, DeviceDescriptor device)
                {
                    int numInputChannels = input.Shape[input.Shape.Rank - 1];
                    var convParams = new Parameter(new int[] { kernelWidth, kernelHeight, numInputChannels, outFeatureMapCount }, DataType.Float, CNTKLib.GlorotUniformInitializer(wScale, -1, 2), device);
                    var convFunction = CNTKLib.Convolution(convParams, input, new int[] { hStride, vStride, numInputChannels });
                    var biasParams = new Parameter(new int[] { NDShape.InferredDimension }, bValue, device, "");
                    var scaleParams = new Parameter(new int[] { NDShape.InferredDimension }, scValue, device, "");
                    var runningMean = new Constant(new int[] { NDShape.InferredDimension }, DataType.Float, 0.0f, device);
                    var runningInvStd = new Constant(new int[] { NDShape.InferredDimension }, DataType.Float, 0.0f, device);
                    var runningCount = Constant.Scalar(DataType.Float, 0.0f, device);
                    return CNTKLib.BatchNormalization(convFunction, scaleParams, biasParams, runningMean, runningInvStd, runningCount, spatial, bnTimeConst, 0.0, 1e-5 /* epsilon */);
                }
            }
        }
    }
}
