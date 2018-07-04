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
            int featureCount = 8 * 8;
            int predCount = 1;

            var xData = Samples.CreateInputs(count: batchSize, dimension: featureCount);
            var yData = Samples.CreateLabels(count: batchSize);

            var random = new Random();

            //实现一个单层的神经网络
            var g = new TFGraph();
            //占位
            var x = g.Placeholder(TFDataType.Double, new TFShape(-1, featureCount));
            var y = g.Placeholder(TFDataType.Double, new TFShape(-1, predCount)); //实际结果
            //layer1
            var W1 = g.VariableV2(new TFShape(featureCount, batchSize), TFDataType.Double, operName: "W1");
            var b1 = g.VariableV2(new TFShape(batchSize), TFDataType.Double, operName: "b1");
            var l1 = g.Add(g.MatMul(x, W1), b1);    //预测结果
            //layer2(output,x=batsize,y=customer)
            var W2 = g.VariableV2(new TFShape(batchSize, 15), TFDataType.Double, operName: "W2");
            var b2 = g.VariableV2(new TFShape(15), TFDataType.Double, operName: "b2");
            var l2 = g.Add(g.MatMul(l1, W2), b2);    //预测结果

            var cost = g.Neg(g.ReduceSum(g.Mul(y, g.Log(l2))));
            //
            var grad = g.AddGradients(new TFOutput[] { cost }, new TFOutput[] { W1, b1, W2, b2 });
            //var cross_entropy = g.Neg(g.ReduceSum(g.Mul(y_, g.Log(y))));
            //计算loss基于交叉熵

            //计算偏微分
            //var grad = g.AddGradients(new TFOutput[] { cost }, new TFOutput[] { W, b });

            //g.ApplyGradientDescent(W1, grad[0], g.Const(0.1));
            //g.ApplyGradientDescent(b1, grad[1], g.Const(0.1));
            //g.ApplyGradientDescent(W2, grad[2], g.Const(0.1));
            //g.ApplyGradientDescent(b2, grad[3], g.Const(0.1));


            var optimize = new[]{
                g.AssignSub(W1, g.Mul(grad[0], g.Const(0.01))).Operation,
                g.AssignSub(b1, g.Mul(grad[1], g.Const(0.01))).Operation,
                g.AssignSub(W2, g.Mul(grad[2], g.Const(0.01))).Operation,
                g.AssignSub(b2, g.Mul(grad[3], g.Const(0.01))).Operation
            };

            using (var sess = new TFSession(g))
            {
                var tensorW = g.Const(random.NextDouble());
                var initW1 = g.Assign(W1, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(featureCount, batchSize))));
                var initb1 = g.Assign(b1, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(batchSize))));
                //
                var initW2 = g.Assign(W2, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(batchSize, 15))));
                var initb2 = g.Assign(b2, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(15))));
                //
                sess.GetRunner().AddTarget(initW1.Operation, initb1.Operation, initW2.Operation, initb2.Operation).Run();
                //

                for (var i = 0; i < 100000; i++)
                {
                    //
                    var tensorX = TFTensor.FromBuffer(new TFShape(batchSize, featureCount), xData.ToArray(), 0, xData.Count);
                    var tensorY = TFTensor.FromBuffer(new TFShape(batchSize, predCount), yData.ToArray(), 0, yData.Count);
                    //
                    var value2 = tensorX.GetValue();
                    //
                    var result = sess.GetRunner()
                   .AddInput(x, tensorX)
                   .AddInput(y, tensorY)
                   .AddTarget(optimize)
                   .Fetch(cost)
                   .Fetch(grad)
                   .Run();
                    //
                    var t_loss = result[0].GetValue();
                    var t_w = result[1].GetValue();
                    var t_b = result[2].GetValue();
                    var t_y = result[3].GetValue();
                    var t_g = result[4].GetValue();
                }
            }
        }

        /// <summary>
        /// 备份代码
        /// </summary>
        public void Old()
        {
            int batchSize = 20;
            int featureCount = 8 * 8;
            int predCount = 1;

            var xData = Samples.CreateInputs(count: batchSize, dimension: featureCount);
            var yData = Samples.CreateLabels(count: batchSize);

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
            var y = g.Add(g.MatMul(x, W), b);    //预测结果
            //
            //var cross_entropy = g.Neg(g.ReduceSum(g.Mul(y_, g.Log(y))));
            //计算loss基于交叉熵
            var cross_entropy = g.Neg(g.ReduceSum(g.Mul(y_, g.Log(y))));
            //计算cost
            var cost = g.ReduceMean(cross_entropy);
            //计算偏微分
            var grad = g.AddGradients(new TFOutput[] { cost }, new TFOutput[] { W, b });

            var optimize = new[]{
                g.AssignSub(W, g.Mul(grad[0], g.Const(0.01))).Operation,
                g.AssignSub(b, g.Mul(grad[1], g.Const(0.01))).Operation
            };

            using (var sess = new TFSession(g))
            {
                var tensorW = g.Const(random.NextDouble());
                var initW = g.Assign(W, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(featureCount, batchSize))));
                var initb = g.Assign(b, g.Const(Samples.CreateTensorWithRandomDouble(new TFShape(batchSize))));
                //
                sess.GetRunner().AddTarget(initW.Operation, initb.Operation).Run();
                //

                for (var i = 0; i < 100000; i++)
                {
                    //
                    var tensorX = TFTensor.FromBuffer(new TFShape(batchSize, featureCount), xData.ToArray(), 0, xData.Count);
                    var tensorY = TFTensor.FromBuffer(new TFShape(batchSize, predCount), yData.ToArray(), 0, yData.Count);
                    //
                    var value2 = tensorX.GetValue();
                    //
                    var result = sess.GetRunner()
                   .AddInput(x, tensorX)
                   .AddInput(y_, tensorY)
                   .AddTarget(optimize)
                   .Fetch(cost, W, b, y).Run();
                    //
                    var t_loss = result[0].GetValue();
                    var t_w = result[1].GetValue();
                    var t_b = result[2].GetValue();
                    var t_y = result[3].GetValue();
                }
            }
        }



    }
}
