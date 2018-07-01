using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace Engine.Brain.AI
{
    public class BinaryClassification
    {

        public BinaryClassification()
        {
            var xData = Samples.CreateInputs();
            var yData = Samples.CreateLabels();
            int featureCount = 8 * 8;
            int predCount = 1;
            var random = new Random();
            //
            //var inputs = Samples.CreateInputs();
            //var labels = Samples.CreateLabels();
            //实现一个三层的神经网络
            var g = new TFGraph();
            //占位
            var x = g.Placeholder(TFDataType.Double, new TFShape(-1, featureCount));
            var y = g.Placeholder(TFDataType.Double, new TFShape(-1, predCount));
            //
            var W = g.VariableV2(TFShape.Scalar, TFDataType.Double, operName: "weight");
            var b = g.VariableV2(TFShape.Scalar, TFDataType.Double, operName: "bias");
            //
            var output = g.Add(g.Mul(x,W), b);
            //
            var loss = g.ReduceMean(g.SigmoidCrossEntropyWithLogits(output, y));
            //计算偏微分
            var grad = g.AddGradients(new TFOutput[] { loss }, new TFOutput[] { W, b });
            //

            var optimize = new[]
          {
                g.AssignSub(W, g.Mul(grad[0], g.Const(0.1))).Operation,
                g.AssignSub(b, g.Mul(grad[1], g.Const(0.1))).Operation
            };
            using (var sess = new TFSession(g))
            {
                //
                var tensorW = g.Const(random.NextDouble());

                //var initW = g.Assign(W, );
                //var initb = g.Assign(b, g.Const(random.NextDouble()));

                //sess.GetRunner().AddTarget(initW.Operation, initb.Operation).Run();

                for (var i = 0; i < 1000; i++)
                {
                    //
                    var tensorX = TFTensor.FromBuffer(new TFShape(10, 64), xData.ToArray(), 0, xData.Count);
                    var tensorY = TFTensor.FromBuffer(new TFShape(10, 1), yData.ToArray(), 0, yData.Count);

                    var value2 = tensorX.GetValue();

                    //
                    var result = sess.GetRunner()
                   .AddInput(x, tensorX)
                   .AddInput(y, tensorY)
                   .AddTarget(optimize)
                   .Fetch(loss, W, b).Run();
                    //
                    var t_loss = result[0].GetValue();
                    var t_w = result[1].GetValue();
                    var t_b = result[2].GetValue();
                }

            }
        }


    }
}
