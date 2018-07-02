using Engine.Brain.Entity;
using System;
using TensorFlow;

namespace Engine.Brain.AI
{
    public class BinaryClassification
    {

        public BinaryClassification()
        {
            int batchSize = 20;
            int featureCount = 2;
            int predCount = 1;

            var xData = Samples.CreateInputs(count:20,dimension:2);
            var yData = Samples.CreateLabels(count:20);

            var random = new Random();
            //
            //var inputs = Samples.CreateInputs();
            //var labels = Samples.CreateLabels();
            //实现一个三层的神经网络
            var g = new TFGraph();
            //占位
            var x = g.Placeholder(TFDataType.Double, new TFShape(-1, featureCount));
            var y_ = g.Placeholder(TFDataType.Double, new TFShape(-1, predCount)); //实际结果
            //
            var W = g.VariableV2(new TFShape(featureCount, batchSize), TFDataType.Double, operName: "weight");
            var b = g.VariableV2(new TFShape(batchSize), TFDataType.Double, operName: "bias");
            //
            var y = g.Add(g.MatMul(x,W), b);    //预测结果
            //
            //var cross_entropy = g.Neg(g.ReduceSum(g.Mul(y_, g.Log(y))));
            var cross_entropy = g.ReduceMean(g.Square(g.Sub(y, y_)));
            //计算偏微分

            var grad = g.AddGradients(new TFOutput[] { cross_entropy }, new TFOutput[] { W, b });

            var optimize = new[]
          {
                g.AssignSub(W, g.Mul(grad[0], g.Const(0.01))).Operation,
                g.AssignSub(b, g.Mul(grad[1], g.Const(0.01))).Operation
            };

            using (var sess = new TFSession(g))
            {
                //
                var tensorW = g.Const(random.NextDouble());
                //
                var initW = g.Assign(W,g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(featureCount, batchSize))));
                var initb = g.Assign(b, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(batchSize))));

                sess.GetRunner().AddTarget(initW.Operation, initb.Operation).Run();

                for (var i = 0; i < 100000; i++)
                {
                    //
                    var tensorX = TFTensor.FromBuffer(new TFShape(batchSize, featureCount), xData.ToArray(), 0, xData.Count);
                    var tensorY = TFTensor.FromBuffer(new TFShape(batchSize, predCount), yData.ToArray(), 0, yData.Count);

                    var value2 = tensorX.GetValue();

                    //
                    var result = sess.GetRunner()
                   .AddInput(x, tensorX)
                   .AddInput(y_, tensorY)
                   .AddTarget(optimize)
                   .Fetch(cross_entropy, W, b).Run();
                    //
                    var t_loss = result[0].GetValue();
                    var t_w = result[1].GetValue();
                    var t_b = result[2].GetValue();
                }
                var ssss = "";
            }
        }


    }
}
