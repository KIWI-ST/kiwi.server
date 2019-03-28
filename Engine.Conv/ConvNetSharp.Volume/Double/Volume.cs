﻿using System;

namespace ConvNetSharp.Volume.Double
{
    public class Volume : Volume<double>
    {
        internal Volume(double[] array, Shape shape) : this(new NcwhVolumeStorage<double>(array, shape))
        {
        }

        internal Volume(VolumeStorage<double> storage) : base(storage)
        {
        }

        public override void Activation(ActivationType type, Volume<double> volume)
        {
            switch (type)
            {
                case ActivationType.Sigmoid:
                    this.Storage.Map(x => 1.0 / (1.0 + Math.Exp(-x)), volume.Storage);
                    return;
                case ActivationType.Relu:
                    Relu(volume);
                    break;
                case ActivationType.Tanh:
                    this.Storage.Map(Math.Tanh, volume.Storage);
                    break;
                case ActivationType.ClippedRelu:
                    throw new NotImplementedException();
            }
        }

        public override void ActivationGradient(Volume<double> input, Volume<double> outputGradient,
            ActivationType type,
            Volume<double> result)
        {
            switch (type)
            {
                case ActivationType.Sigmoid:
                    this.Storage.Map((output, outGradient) => output * (1.0 - output) * outGradient, outputGradient.Storage,
                        result.Storage);
                    return;
                case ActivationType.Relu:
                    ReluGradient(input, outputGradient, result);
                    break;
                case ActivationType.Tanh:
                    this.Storage.Map((output, outGradient) => (1.0 - output * output) * outGradient, outputGradient.Storage,
                        result.Storage);
                    return;
                case ActivationType.ClippedRelu:
                    throw new NotImplementedException();
            }
        }

        public override void Add(Volume<double> other, Volume<double> result)
        {
            this.Storage.MapEx((x, y) => x + y, other.Storage, result.Storage);
        }

        public override void Add(Volume<double> result)
        {
            this.Storage.MapEx((x, y) => x + y, result.Storage, result.Storage);
        }

        public override void BiasGradient(Volume<double> result)
        {
            var batchSize = this.Shape.Dimensions[3];

            var outputWidth = this.Shape.Dimensions[0];
            var outputHeight = this.Shape.Dimensions[1];
            var outputDepth = this.Shape.Dimensions[2];

            for (var n = 0; n < batchSize; n++)
            {
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    for (var ay = 0; ay < outputHeight; ay++)
                    {
                        for (var ax = 0; ax < outputWidth; ax++)
                        {
                            var chainGradient = Get(ax, ay, depth, n);

                            result.Storage.Set(0, 0, depth,
                                result.Storage.Get(0, 0, depth) + chainGradient);
                        }
                    }
                }
            }
        }

        public override void Concat(Volume<double> right, Volume<double> result)
        {
            var batchSize = Math.Max(this.Shape.Dimensions[3], right.Shape.Dimensions[3]);

            if (this.Shape.TotalLength > 1 && right.Shape.TotalLength > 1)
            {
                var left = ReShape(new Shape(1, 1, -1, batchSize));
                right = right.ReShape(new Shape(1, 1, -1, batchSize));

                var elementPerBatch = result.Shape.TotalLength / batchSize;
                var threshold = left.Shape.Dimensions[2];

                for (var n = 0; n < batchSize; n++)
                {
                    for (var i = 0; i < elementPerBatch; i++)
                    {
                        result.Set(0, 0, i, n, i < threshold ? left.Get(0, 0, i, n) : right.Get(0, 0, i - threshold, n));
                    }
                }
            }
            else if (this.Shape.TotalLength == 1 && right.Shape.TotalLength > 1)
            {
                // Left volume is actually a scalar => broadcast its value

                right = right.ReShape(new Shape(1, 1, -1, batchSize));
                var elementPerBatch = result.Shape.TotalLength / batchSize;
                var threshold = 1;

                for (var n = 0; n < batchSize; n++)
                {
                    for (var i = 0; i < elementPerBatch; i++)
                    {
                        result.Set(0, 0, i, n, i < threshold ? Get(0) : right.Get(0, 0, i - threshold, n));
                    }
                }
            }
            else
            {
                // Right volume is actually a scalar => broadcast its value

                var left = ReShape(new Shape(1, 1, -1, batchSize));
                var elementPerBatch = result.Shape.TotalLength / batchSize;
                var threshold = left.Shape.Dimensions[2];

                for (var n = 0; n < batchSize; n++)
                {
                    for (var i = 0; i < elementPerBatch; i++)
                    {
                        result.Set(0, 0, i, n, i < threshold ? left.Get(0, 0, i, n) : right.Get(0));
                    }
                }
            }
        }

        public override void Convolution(Volume<double> filters, int pad, int stride, Volume<double> result)
        {
            var batchSize = this.Shape.Dimensions[3];

            var inputWidth = this.Shape.Dimensions[0];
            var inputHeight = this.Shape.Dimensions[1];

            var outputWidth = result.Shape.Dimensions[0];
            var outputHeight = result.Shape.Dimensions[1];
            var outputDepth = result.Shape.Dimensions[2];

            var filterWidth = filters.Shape.Dimensions[0];
            var filterHeight = filters.Shape.Dimensions[1];
            var filterDepth = filters.Shape.Dimensions[2];

            for (var n = 0; n < batchSize; n++)
            {
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    var y = -pad;
                    for (var ay = 0; ay < outputHeight; y += stride, ay++)
                    {
                        var x = -pad;
                        for (var ax = 0; ax < outputWidth; x += stride, ax++)
                        {
                            // convolve centered at this particular location
                            var a = 0.0;
                            for (var fy = 0; fy < filterHeight; fy++)
                            {
                                var oy = y + fy; // coordinates in the original input array coordinates
                                for (var fx = 0; fx < filterWidth; fx++)
                                {
                                    var ox = x + fx;
                                    if (oy >= 0 && oy < inputHeight && ox >= 0 && ox < inputWidth)
                                    {
                                        for (var fd = 0; fd < filterDepth; fd++)
                                        {
                                            a += filters.Storage.Get(fx, fy, fd, depth) *
                                                 this.Storage.Get(ox, oy, fd, n);
                                        }
                                    }
                                }
                            }

                            result.Storage.Set(ax, ay, depth, n, a);
                        }
                    }
                }
            }
        }

        public override void ConvolutionGradient(Volume<double> filters, Volume<double> outputGradients,
            Volume<double> filterGradient, int pad,
            int stride,
            Volume<double> inputGradient)
        {
            inputGradient.Clear(); // zero out gradient wrt bottom data, we're about to fill it

            var batchSize = this.Shape.Dimensions[3];

            var inputWidth = this.Shape.Dimensions[0];
            var inputHeight = this.Shape.Dimensions[1];

            var outputWidth = outputGradients.Shape.Dimensions[0];
            var outputHeight = outputGradients.Shape.Dimensions[1];
            var outputDepth = outputGradients.Shape.Dimensions[2];

            var filterWidth = filters.Shape.Dimensions[0];
            var filterHeight = filters.Shape.Dimensions[1];
            var filterDepth = filters.Shape.Dimensions[2];

            for (var n = 0; n < batchSize; n++)
            {
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    var y = -pad;
                    for (var ay = 0; ay < outputHeight; y += stride, ay++)
                    {
                        var x = -pad;
                        for (var ax = 0; ax < outputWidth; x += stride, ax++)
                        {
                            // convolve centered at this particular location
                            var chainGradient = outputGradients.Get(ax, ay, depth, n);

                            // gradient from above, from chain rule
                            for (var fy = 0; fy < filterHeight; fy++)
                            {
                                var oy = y + fy; // coordinates in the original input array coordinates
                                for (var fx = 0; fx < filterWidth; fx++)
                                {
                                    var ox = x + fx;
                                    if (oy >= 0 && oy < inputHeight && ox >= 0 && ox < inputWidth)
                                    {
                                        for (var fd = 0; fd < filterDepth; fd++)
                                        {
                                            filterGradient.Set(fx, fy, fd, depth,
                                                filterGradient.Get(fx, fy, fd, depth) +
                                                Get(ox, oy, fd, n) * chainGradient);
                                            inputGradient.Set(ox, oy, fd, n,
                                                inputGradient.Get(ox, oy, fd, n) +
                                                filters.Get(fx, fy, fd, depth) * chainGradient);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Divide(Volume<double> other, Volume<double> result)
        {
            this.Storage.MapEx((left, right) => left / right, other.Storage, result.Storage);
        }

        public override void Dropout(double dropProbability, Volume<double> result)
        {
            if (((NcwhVolumeStorage<double>)this.Storage).Dropped == null || ((NcwhVolumeStorage<double>)this.Storage).Dropped.Length != this.Shape.TotalLength)
            {
                ((NcwhVolumeStorage<double>)this.Storage).Dropped = new bool[this.Shape.TotalLength];
            }

            if (dropProbability > 0.0)
            {
                // do dropout
                this.Storage.Map((x, i) =>
                {
                    var nextDouble = RandomUtilities.NextDouble();
                    if (nextDouble < dropProbability)
                    {
                        ((NcwhVolumeStorage<double>)this.Storage).Dropped[i] = true;
                        return 0;
                    }

                    ((NcwhVolumeStorage<double>)this.Storage).Dropped[i] = false;
                    return x / (1 - dropProbability); // Scale up so that magnitude remains constant accross training and testing
                }, result.Storage);
            }
            else
            {
                this.Storage.Map(x => x, result.Storage);
            }
        }

        public override void DropoutGradient(Volume<double> input, Volume<double> outputGradient, double dropProbability, Volume<double> inputGradient)
        {
            outputGradient.Storage.Map((x, i) =>
            {
                if (((NcwhVolumeStorage<double>)input.Storage).Dropped[i])
                {
                    return 0;
                }

                return x / (1.0 - dropProbability);
            }, inputGradient.Storage);
        }

        public override void Exp(Volume<double> result)
        {
            this.Storage.Map(Math.Exp, result.Storage);
        }

        public override void Extract(int length, int offset, Volume<double> result)
        {
            var input = ReShape(1, 1, Shape.None, Shape.Keep);

            if (input.Shape.TotalLength == 1)
            {
                var v = input.Get(0);
                this.Storage.Map(x => v, result.Storage);
            }
            else
            {
                var batchSize = this.Shape.Dimensions[3];
                for (var n = 0; n < batchSize; n++)
                {
                    for (var i = 0; i < length; i++)
                    {
                        result.Set(0, 0, i, n, input.Get(0, 0, i + offset, n));
                    }
                }
            }
        }

        public override void LeakyRelu(double alpha, Volume<double> volume)
        {
            this.Storage.Map(x => x > 0 ? x : alpha * x, volume.Storage);
        }

        public override void LeakyReluGradient(Volume<double> outputGradient, Volume<double> inputGradient, double alpha)
        {
            this.Storage.Map((x, y) => x >= 0 ? y : y * alpha, outputGradient.Storage, inputGradient.Storage);
        }

        public override void Log(Volume<double> result)
        {
            this.Storage.Map(x => Math.Log(x), result.Storage);
        }

        public override void MatMultiply(Volume<double> right, Volume<double> result)
        {
            if (this.Shape.Dimensions[2] != 1 || right.Shape.Dimensions[2] != 1)
            {
                throw new ArgumentException($"Left and right volumes should be [w, h, 1, b]. left = {this.Shape} right = {right.Shape}");
            }

            bool broadCastLeft = this.Shape.Dimensions[3] == 1;
            bool broadCastRight = right.Shape.Dimensions[3] == 1;
            if (this.Shape.Dimensions[3] != right.Shape.Dimensions[3] && !(broadCastLeft || broadCastRight))
            {
                throw new ArgumentException($"Left and right volumes should have the same batch size. left = {this.Shape.Dimensions[3]} right = {right.Shape.Dimensions[3]}");
            }

            var expectedShape = ComputeMatMultiplyShape(this.Shape, right.Shape);

            if (!result.Shape.Equals(expectedShape))
            {
                throw new ArgumentException($"Result shape should be {expectedShape} but is {result.Shape}");
            }

            for (var n = 0; n < this.Shape.Dimensions[3]; n++)
            {
                for (var i = 0; i < expectedShape.Dimensions[0]; i++)
                {
                    for (var j = 0; j < expectedShape.Dimensions[1]; j++)
                    {
                        var cell = 0.0;
                        for (var k = 0; k < this.Shape.Dimensions[0]; k++)
                        {
                            cell = cell + Get(k, j, 0, broadCastLeft ? 0 : n) * right.Get(i, k, 0, broadCastRight ? 0 : n);
                        }

                        result.Set(i, j, 0, n, cell);
                    }
                }
            }
        }

        public override void Max(Volume<double> result)
        {
            var batchSize = this.Shape.Dimensions[3];
            var reshape = ReShape(-1, batchSize);

            var n = reshape.Shape.Dimensions[0];

            for (var i = 0; i < batchSize; i++)
            {
                var max = double.MinValue;

                for (var j = 0; j < n; j++)
                {
                    var d = reshape.Get(j, i);
                    if (d > max)
                    {
                        max = d;
                    }
                }

                result.Set(new[] { i }, max);
            }
        }

        public override void Min(Volume<double> result)
        {
            var batchSize = this.Shape.Dimensions[3];
            var reshape = ReShape(-1, batchSize);

            var n = reshape.Shape.Dimensions[0];

            for (var i = 0; i < batchSize; i++)
            {
                var min = double.MaxValue;

                for (var j = 0; j < n; j++)
                {
                    var d = reshape.Get(j, i);
                    if (d < min)
                    {
                        min = d;
                    }
                }

                result.Set(new[] { i }, min);
            }
        }

        public override void Multiply(double factor, Volume<double> result)
        {
            this.Storage.Map(x => x * factor, result.Storage);
        }

        public override void Multiply(Volume<double> right, Volume<double> result)
        {
            this.Storage.MapEx((x, y) => x * y, right.Storage, result.Storage);
        }

        public override void Negate(Volume<double> volume)
        {
            Multiply(-1.0, volume);
        }

        public override void Norm1(Volume<double> result)
        {
            var batchSize = this.Shape.Dimensions[3];
            var reshape = ReShape(-1, batchSize);

            var n = reshape.Shape.Dimensions[0];

            for (var i = 0; i < batchSize; i++)
            {
                var sum = 0.0;

                for (var j = 0; j < n; j++)
                {
                    var d = reshape.Get(j, i);
                    sum += Math.Abs(d);
                }

                result.Set(new[] { i }, sum);
            }
        }

        public override void Pool(int windowWidth, int windowHeight,
            int horizontalPad, int verticalPad, int horizontalStride, int verticalStride, Volume<double> result)
        {
            var inputWidth = this.Shape.Dimensions[0];
            var inputHeight = this.Shape.Dimensions[1];

            var outputWidth = result.Shape.Dimensions[0];
            var outputHeight = result.Shape.Dimensions[1];
            var outputDepth = result.Shape.Dimensions[2];
            var batchSize = result.Shape.Dimensions[3];

            for (var n = 0; n < batchSize; n++)
            {
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    var x = -horizontalPad;
                    for (var ax = 0; ax < outputWidth; x += verticalStride, ax++)
                    {
                        var y = -verticalPad;
                        for (var ay = 0; ay < outputHeight; y += horizontalStride, ay++)
                        {
                            var a = double.MinValue;

                            for (var fx = 0; fx < windowWidth; fx++)
                            {
                                for (var fy = 0; fy < windowHeight; fy++)
                                {
                                    var oy = y + fy;
                                    var ox = x + fx;
                                    if (oy >= 0 && oy < inputHeight && ox >= 0 && ox < inputWidth)
                                    {
                                        var v = Get(ox, oy, depth, n);
                                        // perform max pooling and store pointers to where
                                        // the max came from. This will speed up backprop 
                                        // and can help make nice visualizations in future
                                        if (v > a)
                                        {
                                            a = v;
                                        }
                                    }
                                }
                            }

                            result.Storage.Set(ax, ay, depth, n, a);
                        }
                    }
                }
            }
        }

        public override void PoolGradient(Volume<double> input, Volume<double> outputGradient,
            int windowWidth, int windowHeight,
            int horizontalPad, int verticalPad, int horizontalStride, int verticalStride,
            Volume<double> inputGradient)
        {
            var inputWidth = input.Shape.Dimensions[0];
            var inputHeight = input.Shape.Dimensions[1];

            var outputWidth = outputGradient.Shape.Dimensions[0];
            var outputHeight = outputGradient.Shape.Dimensions[1];
            var outputDepth = outputGradient.Shape.Dimensions[2];
            var batchSize = outputGradient.Shape.Dimensions[3];

            for (var n = 0; n < batchSize; n++)
            {
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    var x = -horizontalPad;
                    for (var ax = 0; ax < outputWidth; x += verticalStride, ax++)
                    {
                        var y = -verticalPad;
                        for (var ay = 0; ay < outputHeight; y += horizontalStride, ay++)
                        {
                            var a = double.MinValue;
                            int winx = -1, winy = -1;

                            for (var fx = 0; fx < windowWidth; fx++)
                            {
                                for (var fy = 0; fy < windowHeight; fy++)
                                {
                                    var oy = y + fy;
                                    var ox = x + fx;
                                    if (oy >= 0 && oy < inputHeight && ox >= 0 && ox < inputWidth)
                                    {
                                        var v = input.Get(ox, oy, depth, n);
                                        // perform max pooling and store pointers to where
                                        // the max came from. This will speed up backprop 
                                        // and can help make nice visualizations in future
                                        if (v > a)
                                        {
                                            a = v;
                                            winx = ox;
                                            winy = oy;
                                        }
                                    }
                                }
                            }

                            var chainGradient = outputGradient.Get(ax, ay, depth, n);
                            inputGradient.Storage.Set(winx, winy, depth, n, chainGradient);
                        }
                    }
                }
            }
        }

        public override void Power(Volume<double> power, Volume<double> result)
        {
            this.Storage.MapEx(Math.Pow, power.Storage, result.Storage);
        }

        public override void Reduce(TensorReduceOp op, Volume<double> result)
        {
            if (this.Shape.Equals(result.Shape))
            {
                result.Storage.CopyFrom(this.Storage);
                return;
            }

            switch (op)
            {
                case TensorReduceOp.Add:
                    Sum(result);
                    break;
                case TensorReduceOp.Mul:
                    throw new NotImplementedException();
                case TensorReduceOp.Min:
                    throw new NotImplementedException();
                case TensorReduceOp.Max:
                    Max(result);
                    break;
                case TensorReduceOp.AMax:
                    throw new NotImplementedException();
                case TensorReduceOp.Avg:
                    throw new NotImplementedException();
                case TensorReduceOp.Norm1:
                    Norm1(result);
                    break;
                case TensorReduceOp.Norm2:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        public override void Relu(Volume<double> volume)
        {
            this.Storage.Map(x => x <= 0 ? 0 : x, volume.Storage);
        }

        public override void ReluGradient(Volume<double> input, Volume<double> outputGradient,
            Volume<double> inputGradient)
        {
            this.Storage.Map((x, y) => x > 0 ? y : 0, outputGradient.Storage, inputGradient.Storage);
        }

        public override void Sigmoid(Volume<double> volume)
        {
            this.Storage.Map(x => 1.0 / (1.0 + Math.Exp(-x)), volume.Storage);
        }

        public override void SigmoidGradient(Volume<double> input, Volume<double> outputGradient,
            Volume<double> inputGradient)
        {
            this.Storage.Map((output, outGradient) => output * (1.0 - output) * outGradient, outputGradient.Storage,
                inputGradient.Storage);
        }

        public override void Softmax(Volume<double> result)
        {
            var batchSize = this.Shape.Dimensions[3];

            var outputWidth = this.Shape.Dimensions[0];
            var outputHeight = this.Shape.Dimensions[1];
            var outputDepth = this.Shape.Dimensions[2];

            for (var n = 0; n < batchSize; n++)
            {
                // compute max activation
                var amax = double.MinValue;
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    for (var ay = 0; ay < outputHeight; ay++)
                    {
                        for (var ax = 0; ax < outputWidth; ax++)
                        {
                            var v = Get(ax, ay, depth, n);
                            if (v > amax)
                            {
                                amax = v;
                            }
                        }
                    }
                }

                // compute exponentials (carefully to not blow up)
                var es = new double[outputDepth * outputHeight * outputWidth];
                var esum = 0.0;

                for (var depth = 0; depth < outputDepth; depth++)
                {
                    for (var ay = 0; ay < outputHeight; ay++)
                    {
                        for (var ax = 0; ax < outputWidth; ax++)
                        {
                            var e = Math.Exp(Get(ax, ay, depth, n) - amax);
                            esum += e;
                            es[ax + ay * outputWidth + depth * outputWidth * outputHeight] = e;
                        }
                    }
                }

                // normalize and output to sum to one
                for (var depth = 0; depth < outputDepth; depth++)
                {
                    for (var ay = 0; ay < outputHeight; ay++)
                    {
                        for (var ax = 0; ax < outputWidth; ax++)
                        {
                            es[ax + ay * outputWidth + depth * outputWidth * outputHeight] /= esum;

                            result.Storage.Set(ax, ay, depth, n,
                                es[ax + ay * outputWidth + depth * outputWidth * outputHeight]);
                        }
                    }
                }
            }
        }

        public override void SoftmaxGradient(Volume<double> outputGradient, Volume<double> inputGradient)
        {
            var batchSize = this.Shape.Dimensions[3];

            var outputReshape = ReShape(-1, batchSize);
            var outputGradientReshape = outputGradient.ReShape(-1, batchSize);
            var inputGradientReshape = inputGradient.ReShape(-1, batchSize);

            var firstDim = outputReshape.Shape.Dimensions[0];

            for (var b = 0; b < batchSize; b++)
            {
                var classIndex = -1;

                for (var i = 0; i < firstDim; i++)
                {
                    var yi = outputGradientReshape.Get(i, b);

                    if (yi == 1.0)
                    {
                        classIndex = i;
                    }
                }

                var pj = outputReshape.Get(classIndex, b);

                // input gradient:
                // pi(1 - pi) if i = class index
                // -pipj if i != class index
                for (var i = 0; i < firstDim; i++)
                {
                    var pi = outputReshape.Get(i, b);

                    if (i == classIndex)
                    {
                        inputGradientReshape.Set(i, b, pj * (1.0 - pj));
                    }
                    else
                    {
                        inputGradientReshape.Set(i, b, -pj * pi);
                    }
                }
            }
        }

        public override void Sqrt(Volume<double> result)
        {
            this.Storage.Map(Math.Sqrt, result.Storage);
        }

        public override void SubtractFrom(Volume<double> other, Volume<double> result)
        {
            this.Storage.MapEx((x, y) => y - x, other.Storage, result.Storage);
        }

        public override void Sum(Volume<double> result)
        {
            var batchsize = this.Shape.Dimensions[3];
            var channel = this.Shape.Dimensions[2];
            var height = this.Shape.Dimensions[1];
            var width = this.Shape.Dimensions[0];

            var resultWIsOne = result.Shape.Dimensions[0] == 1;
            var resultHIsOne = result.Shape.Dimensions[1] == 1;
            var resultCIsOne = result.Shape.Dimensions[2] == 1;
            var resultNIsOne = result.Shape.Dimensions[3] == 1;

            for (var n = 0; n < batchsize; n++)
            {
                for (var c = 0; c < channel; c++)
                {
                    for (var h = 0; h < height; h++)
                    {
                        for (var w = 0; w < width; w++)
                        {
                            var val = Get(w, h, c, n);

                            var resultW = resultWIsOne ? 0 : w;
                            var resultH = resultHIsOne ? 0 : h;
                            var resultC = resultCIsOne ? 0 : c;
                            var resultN = resultNIsOne ? 0 : n;

                            var current = result.Get(resultW, resultH, resultC, resultN);
                            result.Set(resultW, resultH, resultC, resultN, current + val);
                        }
                    }
                }
            }
        }

        public override void Tanh(Volume<double> volume)
        {
            this.Storage.Map(Math.Tanh, volume.Storage);
        }

        public override void TanhGradient(Volume<double> input, Volume<double> outputGradient,
            Volume<double> inputGradient)
        {
            this.Storage.Map((output, outGradient) => (1.0 - output * output) * outGradient, outputGradient.Storage,
                inputGradient.Storage);
        }

        public override void Tile(Volume<double> reps, Volume<double> result)
        {
            var batchsize = this.Shape.Dimensions[3];
            var channel = this.Shape.Dimensions[2];
            var height = this.Shape.Dimensions[1];
            var width = this.Shape.Dimensions[0];

            var outputBatchSize = result.Shape.Dimensions[3];
            var outputChannel = result.Shape.Dimensions[2];
            var outputHeight = result.Shape.Dimensions[1];
            var outputWidth = result.Shape.Dimensions[0];

            for (var n = 0; n < outputBatchSize; n++)
            {
                for (var c = 0; c < outputChannel; c++)
                {
                    for (var h = 0; h < outputHeight; h++)
                    {
                        for (var w = 0; w < outputWidth; w++)
                        {
                            result.Set(w, h, c, n, Get(w % width, h % height, c % channel, n % batchsize));
                        }
                    }
                }
            }
        }

        public override void Transpose(Volume<double> result)
        {
            var expectedShape = new Shape(this.Shape.Dimensions[1], this.Shape.Dimensions[0], 1, this.Shape.Dimensions[3]);

            if (!result.Shape.Equals(expectedShape))
            {
                throw new ArgumentException($"Result shape should be {expectedShape}");
            }

            for (var n = 0; n < this.Shape.Dimensions[3]; n++)
            {
                for (var j = 0; j < this.Shape.Dimensions[1]; j++)
                {
                    for (var i = 0; i < this.Shape.Dimensions[0]; i++)
                    {
                        result.Set(j, i, 0, n, Get(i, j, 0, n));
                    }
                }
            }
        }
    }
}