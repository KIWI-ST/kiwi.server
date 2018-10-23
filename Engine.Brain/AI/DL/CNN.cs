using System;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Fluent;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Core.Training;
using ConvNetSharp.Core.Training.Double;
using ConvNetSharp.Volume;
using ConvNetSharp.Volume.Double;
using Engine.Brain.Entity;

namespace Engine.Brain.AI.RL.Net
{
    public class CNet:INet
    {

        FluentNet<double> _network;

        SgdTrainer<double> _trainer;

        public CNet(int[] featureNum,int actionNum)
        {
            //get channel
            int channel = featureNum[3];
            //create cnn neural network
            _network = FluentNet<double>.Create(24, 24, channel)
                     .Conv(5, 5, 8).Stride(1).Pad(2)
                     .Relu()
                     .Pool(2, 2).Stride(2)
                     .Conv(5, 5, 16).Stride(1).Pad(2)
                     .Relu()
                     .Pool(3, 3).Stride(3)
                     .FullyConn(10)
                     .Softmax(10)
                     .Build();
            //create trainer
            _trainer = new SgdTrainer<double>(_network) {
                LearningRate = 0.01,
                BatchSize = 20,
                L2Decay = 0.001,
                Momentum = 0.9
            };
        }

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
      

            throw new NotImplementedException();
        }

        public double Train(double[][] inputs, double[][] outputs)
        {
           // var x = BuilderInstance.Volume.From
            throw new NotImplementedException();
        }

    }
}
