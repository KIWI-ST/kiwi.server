﻿using System;
using System.IO;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers;
using ConvNetSharp.Core.Training;
using ConvNetSharp.Volume;
using Engine.Brain.AI.RL;
using Engine.Brain.Entity;

namespace Engine.Brain.AI.DL
{
    public class LeNet5 : IDNet
    {
        Net<double> _network;
        SgdTrainer<double> _trainer;
        private readonly int _channel;
        private int _width;
        private int _height;
        private int _classNum;
        bool _isToCharacteristicNetwork = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureNum"></param>
        /// <param name="classNum">分类总数</param>
        public LeNet5(int[] featureNum, int classNum)
        {
            //get channel
            _channel = featureNum[0];
            _width = featureNum[1];
            _height = featureNum[2];
            _classNum = classNum;
            //create cnn neural network
            _network = new Net<double>();
            _network.AddLayer(new InputLayer<double>(_width,_height,_channel));
            _network.AddLayer(new ConvLayer<double>(3, 3, _channel) { Stride = 1, Pad = 2, BiasPref = 0.1f });
            _network.AddLayer(new ReluLayer<double>());
            _network.AddLayer(new PoolLayer<double>(2, 2) { Stride = 2 });
            _network.AddLayer(new ConvLayer<double>(5, 5, 16) { Stride = 1, Pad = 2, BiasPref = 0.1f });
            _network.AddLayer(new ReluLayer<double>());
            _network.AddLayer(new PoolLayer<double>(3, 3) { Stride = 3 });
            _network.AddLayer(new FullyConnLayer<double>(_classNum));
            _network.AddLayer(new SoftmaxLayer<double>(_classNum));
            //create trainer
            _trainer = new SgdTrainer<double>(_network)
            {
                LearningRate = 0.01,
                L2Decay = 0.001,
                Momentum = 0.9
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceNet"></param>
        public void Accept(IDNet sourceNet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ToCharacteristicNetwork()
        {
            if (!_isToCharacteristicNetwork)
            {
                var layer = _network.Layers[_network.Layers.Count - 1];
                _network.Layers.Remove(layer);
                _isToCharacteristicNetwork = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Stream PersistenceMemory()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PersistencNative()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double[] Predict(double[] input)
        {
            var x = BuilderInstance<double>.Volume.From(input, new Shape(_width, _height, _channel));
            var y = _network.Forward(x);
            double[] output = new double[_classNum];
            for (int i = 0; i < _classNum; i++)
                output[i] = y.Get(i);
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        public double Train(double[][] inputs, double[][] outputs)
        {
            int batchSize = inputs.GetLength(0);
            _trainer.BatchSize = batchSize;
            var x = BuilderInstance<double>.Volume.From(NP.ToUnidimensional(inputs), new Shape(_width, _height, _channel, batchSize));
            var y = BuilderInstance<double>.Volume.From(NP.ToUnidimensional(outputs), new Shape(1, 1, _classNum, batchSize));
            _trainer.Train(x, y);
            return _trainer.Loss;
        }
    }
}
