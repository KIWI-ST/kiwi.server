using ConvNetSharp.Core;
using ConvNetSharp.Core.Fluent;
using ConvNetSharp.Core.Layers;
using ConvNetSharp.Core.Training;
using ConvNetSharp.Volume;
using Engine.Brain.AI.RL;
using Engine.Brain.Entity;
using System;

namespace Engine.Brain.AI.DL

{
    public class CNN : INet
    {

        Net<double> _network;

        SgdTrainer<double> _trainer;

        int _channel, _width, _height, _actionNum, _batchSize;

        public CNN(int[] featureNum, int actionNum, int batchSize = 31)
        {
            //get channel
            _channel = featureNum[0];
            _width = featureNum[1];
            _height = featureNum[2];
            _actionNum = actionNum;
            _batchSize = batchSize;
            //create cnn neural network
            _network = new Net<double>();
            _network.AddLayer(new InputLayer<double>(_width,_height,_channel));
            _network.AddLayer(new ConvLayer<double>(1, 1, 2) { Stride = 1, Pad = 2, BiasPref = 0.1f });
            _network.AddLayer(new ReluLayer<double>());
            _network.AddLayer(new PoolLayer<double>(2, 2) { Stride = 2 });
            _network.AddLayer(new ConvLayer<double>(5, 5, 16) { Stride = 1, Pad = 2, BiasPref = 0.1f });
            _network.AddLayer(new ReluLayer<double>());
            _network.AddLayer(new PoolLayer<double>(3, 3) { Stride = 3 });
            _network.AddLayer(new FullyConnLayer<double>(_actionNum));
            _network.AddLayer(new SoftmaxLayer<double>(_actionNum));
            //create trainer
            _trainer = new SgdTrainer<double>(_network)
            {
                LearningRate = 0.001,
                BatchSize = batchSize,
                L2Decay = 0.001,
                Momentum = 0.9
            };
        }

        public int BatchSize { get => _batchSize;}

        public void Accept(INet sourceNet)
        {
            throw new NotImplementedException();
        }

        public string Persistence()
        {
            throw new NotImplementedException();
        }

        public double[] Predict(double[] input)
        {
            var x = BuilderInstance<double>.Volume.From(input, new Shape(_width, _height, _channel));
            var y = _network.Forward(x);
            double[] output = new double[_actionNum];
            for (int i = 0; i < _actionNum; i++)
                output[i] = y.Get(i);
            return output;
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
            int batchSize = inputs.GetLength(0);
            var x = BuilderInstance<double>.Volume.From(NP.ToUnidimensional(inputs), new Shape(_width, _height, _channel, batchSize));
            var y = BuilderInstance<double>.Volume.From(NP.ToUnidimensional(outputs), new Shape(1, 1, _actionNum, batchSize));
            _trainer.Train(x, y);
            return _trainer.Loss;
        }

    }
}
