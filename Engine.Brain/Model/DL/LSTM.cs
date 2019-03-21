using System;
using System.Collections.Generic;
using System.IO;
using CNTK;
using Engine.Brain.AI.RL;

namespace Engine.Brain.Model.DL
{
    /// <summary>
    /// based on CNTK
    /// https://github.com/Microsoft/CNTK/blob/release/latest/Examples/TrainingCSharp/Common/LSTMSequenceClassifier.cs
    /// </summary>
    public class LSTM: INet
    {
        public LSTM()
        {
            const int inputDim = 2000;
            const int numOutputClasses = 5;
            string featuresName = "features", labelsName = "labels";
            //build model
            var features = Variable.InputVariable(new int[] { inputDim }, DataType.Float, featuresName, null, true /*isSparse*/);
            var labels = Variable.InputVariable(new int[] { numOutputClasses }, DataType.Float, labelsName, new List<Axis>() { Axis.DefaultBatchAxis() }, true);
           
        }
        #region lstm build function
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDim"></param>
        /// <param name="device"></param>
        /// <param name="outputName"></param>
        /// <returns></returns>
        private Function FullyConnectedLinearLayer(Variable input, int outputDim, DeviceDescriptor device, string outputName = "")
        {
            System.Diagnostics.Debug.Assert(input.Shape.Rank == 1);
            int inputDim = input.Shape[0];

            int[] s = { outputDim, inputDim };
            var timesParam = new Parameter((NDShape)s, DataType.Float,
                CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank, 1),
                device, "timesParam");
            var timesFunction = CNTKLib.Times(timesParam, input, "times");

            int[] s2 = { outputDim };
            var plusParam = new Parameter(s2, 0.0f, device, "plusParam");
            return CNTKLib.Plus(plusParam, timesFunction, outputName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ElementType"></typeparam>
        /// <param name="x"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        private Function Stabilize<ElementType>(Variable x, DeviceDescriptor device)
        {
            bool isFloatType = typeof(ElementType).Equals(typeof(float));
            Constant f, fInv;
            if (isFloatType)
            {
                f = Constant.Scalar(4.0f, device);
                fInv = Constant.Scalar(f.DataType, 1.0 / 4.0f);
            }
            else
            {
                f = Constant.Scalar(4.0, device);
                fInv = Constant.Scalar(f.DataType, 1.0 / 4.0f);
            }

            var beta = CNTKLib.ElementTimes(
                fInv,
                CNTKLib.Log(
                    Constant.Scalar(f.DataType, 1.0) +
                    CNTKLib.Exp(CNTKLib.ElementTimes(f, new Parameter(new NDShape(), f.DataType, 0.99537863 /* 1/f*ln (e^f-1) */, device)))));
            return CNTKLib.ElementTimes(beta, x);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ElementType"></typeparam>
        /// <param name="input"></param>
        /// <param name="prevOutput"></param>
        /// <param name="prevCellState"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        private Tuple<Function, Function> LSTMPCellWithSelfStabilization<ElementType>(Variable input, Variable prevOutput, Variable prevCellState, DeviceDescriptor device)
        {
            int outputDim = prevOutput.Shape[0];
            int cellDim = prevCellState.Shape[0];
            bool isFloatType = typeof(ElementType).Equals(typeof(float));
            DataType dataType = isFloatType ? DataType.Float : DataType.Double;

            Func<int, Parameter> createBiasParam;
            if (isFloatType)
                createBiasParam = (dim) => new Parameter(new int[] { dim }, 0.01f, device, "");
            else
                createBiasParam = (dim) => new Parameter(new int[] { dim }, 0.01, device, "");

            uint seed2 = 1;
            Func<int, Parameter> createProjectionParam = (oDim) => new Parameter(new int[] { oDim, NDShape.InferredDimension },
                    dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0, seed2++), device);

            Func<int, Parameter> createDiagWeightParam = (dim) =>
                new Parameter(new int[] { dim }, dataType, CNTKLib.GlorotUniformInitializer(1.0, 1, 0, seed2++), device);

            Function stabilizedPrevOutput = Stabilize<ElementType>(prevOutput, device);
            Function stabilizedPrevCellState = Stabilize<ElementType>(prevCellState, device);

            Func<Variable> projectInput = () =>
                createBiasParam(cellDim) + (createProjectionParam(cellDim) * input);

            // Input gate
            Function it =
                CNTKLib.Sigmoid(
                    (Variable)(projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)) +
                    CNTKLib.ElementTimes(createDiagWeightParam(cellDim), stabilizedPrevCellState));
            Function bit = CNTKLib.ElementTimes(
                it,
                CNTKLib.Tanh(projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)));

            // Forget-me-not gate
            Function ft = CNTKLib.Sigmoid(
                (Variable)(
                        projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)) +
                        CNTKLib.ElementTimes(createDiagWeightParam(cellDim), stabilizedPrevCellState));
            Function bft = CNTKLib.ElementTimes(ft, prevCellState);

            Function ct = (Variable)bft + bit;

            // Output gate
            Function ot = CNTKLib.Sigmoid(
                (Variable)(projectInput() + (createProjectionParam(cellDim) * stabilizedPrevOutput)) +
                CNTKLib.ElementTimes(createDiagWeightParam(cellDim), Stabilize<ElementType>(ct, device)));
            Function ht = CNTKLib.ElementTimes(ot, CNTKLib.Tanh(ct));

            Function c = ct;
            Function h = (outputDim != cellDim) ? (createProjectionParam(outputDim) * Stabilize<ElementType>(ht, device)) : ht;

            return new Tuple<Function, Function>(h, c);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ElementType"></typeparam>
        /// <param name="input"></param>
        /// <param name="outputShape"></param>
        /// <param name="cellShape"></param>
        /// <param name="recurrenceHookH"></param>
        /// <param name="recurrenceHookC"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        private Tuple<Function, Function> LSTMPComponentWithSelfStabilization<ElementType>(Variable input,
            NDShape outputShape, NDShape cellShape,
            Func<Variable, Function> recurrenceHookH,
            Func<Variable, Function> recurrenceHookC,
            DeviceDescriptor device)
        {
            var dh = Variable.PlaceholderVariable(outputShape, input.DynamicAxes);
            var dc = Variable.PlaceholderVariable(cellShape, input.DynamicAxes);
            var LSTMCell = LSTMPCellWithSelfStabilization<ElementType>(input, dh, dc, device);
            var actualDh = recurrenceHookH(LSTMCell.Item1);
            var actualDc = recurrenceHookC(LSTMCell.Item2);
            (LSTMCell.Item1).ReplacePlaceholders(new Dictionary<Variable, Variable> { { dh, actualDh }, { dc, actualDc } });
            return new Tuple<Function, Function>(LSTMCell.Item1, LSTMCell.Item2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="embeddingDim"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        private Function Embedding(Variable input, int embeddingDim, DeviceDescriptor device)
        {
            System.Diagnostics.Debug.Assert(input.Shape.Rank == 1);
            int inputDim = input.Shape[0];
            var embeddingParameters = new Parameter(new int[] { embeddingDim, inputDim }, DataType.Float, CNTKLib.GlorotUniformInitializer(), device);
            return CNTKLib.Times(embeddingParameters, input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="numOutputClasses"></param>
        /// <param name="embeddingDim"></param>
        /// <param name="LSTMDim"></param>
        /// <param name="cellDim"></param>
        /// <param name="device"></param>
        /// <param name="outputName"></param>
        /// <returns></returns>
        private Function LSTMSequenceClassifierNet(Variable input, int numOutputClasses, int embeddingDim, int LSTMDim, int cellDim, DeviceDescriptor device, string outputName)
        {
            Function embeddingFunction = Embedding(input, embeddingDim, device);
            Func<Variable, Function> pastValueRecurrenceHook = (x) => CNTKLib.PastValue(x);
            Function LSTMFunction = LSTMPComponentWithSelfStabilization<float>(
               embeddingFunction,
               new int[] { LSTMDim },
               new int[] { cellDim },
               pastValueRecurrenceHook,
               pastValueRecurrenceHook,
               device).Item1;
            Function thoughtVectorFunction = CNTKLib.SequenceLast(LSTMFunction);
            return FullyConnectedLinearLayer(thoughtVectorFunction, numOutputClasses, device, outputName);
        }
#endregion

        public double Train(double[][] inputs, double[][] outputs)
        {
            throw new NotImplementedException();
        }

        public string PersistencNative()
        {
            throw new NotImplementedException();
        }

        public Stream PersistenceMemory()
        {
            throw new NotImplementedException();
        }

        public double[] Predict(double[] input)
        {
            throw new NotImplementedException();
        }

        public void Accept(INet sourceNet)
        {
            throw new NotImplementedException();
        }

    }
}
