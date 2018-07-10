using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace Engine.Brain.AI
{
    /// <summary>
    /// 卷积网络
    /// </summary>
    public class CNN
    {
        float learningRate = 0.01f;
        /// <summary>
        /// loss function
        /// </summary>
        TFOutput _corss_entropy;
        /// <summary>
        /// gradients
        /// </summary>
        TFOutput[] _grad;
        /// <summary>
        /// input tensor s (placeholder)
        /// </summary>
        TFOutput _x;
        /// <summary>
        /// action tensor a (placeholder)
        /// </summary>
        TFOutput _y;
        /// <summary>
        /// dropout操作，减少过拟合，其实就是降低上一层某些输入的权重scale，甚至置为0，升高某些输入的权值，甚至置为2，防止评测曲线出现震荡，个人觉得样本较少时很必要
        /// </summary>
        TFOutput _keep;
        /// <summary>
        /// 
        /// </summary>
        TFOutput _r;
        /// <summary>
        /// 
        /// </summary>
        TFGraph _graph;
        /// <summary>
        /// 全局session
        /// </summary>
        TFSession _session;
        /// <summary>
        /// 梯度修正操作
        /// </summary>
        TFOperation[] _optimize;
        /// <summary>
        /// 初始化操作
        /// </summary>
        TFOperation[] _inits;
        /// <summary>
        /// </summary>
        /// <param name="inputWidth">输入图像的宽</param>
        /// <param name="inputHeight">输入图像的高</param>
        /// <param name="actions_num">图像分类操作（对应类别数）</param>
        public CNN(int inputWidth, int inputHeight, int actions_num)
        {
            _graph = new TFGraph();
            //craete envrionment state placeholder
            _x = _graph.Placeholder(TFDataType.Float, new TFShape(-1, inputHeight * inputWidth), "state");
            //create agent's action
            _y = _graph.Placeholder(TFDataType.Float, new TFShape(-1, actions_num), "action");
            //create reward
            //_r = _graph.Placeholder(TFDataType.Float, new TFShape(-1, 1), "reward");
            //dropout operation
            _keep = _graph.Placeholder(TFDataType.Float, TFShape.Scalar);
            //The sparse matrix eigenvector is obtained by processing _s
            //1.reshape _s  with unlimit count/1 brand
            var _s_image = _graph.Reshape(_x, ReShapeInput(-1, inputWidth, inputHeight, 1));
            //2.build  cnn layer
            BuildCNNLayer(_s_image);
        }
        /// <summary>
        /// https://blog.csdn.net/xukaiwen_2016/article/details/70880694
        /// </summary>
        /// <param name="x"></param>
        private void BuildCNNLayer(TFOutput x)
        {
            //uniformization bit data
            var x_normal = _graph.Mul(x, _graph.Const(1 / 255f));
            //convolution layer 1
            var w1 = _graph.VariableV2(new TFShape(5, 5, 1, 32), TFDataType.Float);
            var b1 = _graph.VariableV2(new TFShape(32), TFDataType.Float);
            var c1 = _graph.Relu(_graph.Add(Conv2d(x_normal, w1), b1));
            var p1 = MaxPool2x2(c1);
            //convolution layer 2
            var w2 = _graph.VariableV2(new TFShape(5, 5, 32, 64), TFDataType.Float);
            var b2 = _graph.VariableV2(new TFShape(64), TFDataType.Float);
            var c2 = _graph.Relu(_graph.Add(Conv2d(p1, w2), b2));
            var p2 = MaxPool2x2(c2);
            //full connection layer
            var w3 = _graph.VariableV2(new TFShape(7 * 7 * 64, 1024), TFDataType.Float);
            var b3 = _graph.VariableV2(new TFShape(1024), TFDataType.Float);
            var p3 = _graph.Reshape(p2, ReShapeInput(-1, 7 * 7 * 64));
            var c3 = _graph.Relu(_graph.Add(_graph.MatMul(p3, w3), b3));
            var c3_dropout = _graph.Dropout(c3, _keep);
            //output layer
            var w4 = _graph.VariableV2(new TFShape(1024, 10), TFDataType.Float);
            var b4 = _graph.VariableV2(new TFShape(10), TFDataType.Float);
            var y_hat = _graph.Softmax(_graph.Add(_graph.MatMul(c3_dropout, w4), b4));
            //loss function
            _corss_entropy = _graph.Neg(_graph.ReduceSum(_graph.Mul(_y, _graph.Log(y_hat))));
            //gradient descent
            _grad = _graph.AddGradients(new TFOutput[] { _corss_entropy }, new TFOutput[] {
                w1,b1,
                w2,b2,
                w3,b3,
                w4,b4
            });
            //init variables 
            _inits = new[]
            {
                _graph.Assign(w1, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(5,5,1,32)))).Operation,
                _graph.Assign(b1, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(32)))).Operation,
                _graph.Assign(w2, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(5,5,32,64)))).Operation,
                _graph.Assign(b2, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(64)))).Operation,
                 _graph.Assign(w3, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(7 * 7 * 64,1024)))).Operation,
                _graph.Assign(b3, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(1024)))).Operation,
                _graph.Assign(w4, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(1024,10)))).Operation,
                _graph.Assign(b4, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(10)))).Operation,
            };
            //optimize gradient descent
            _optimize = new[]{
                _graph.AssignSub(w1, _graph.Mul(_grad[0], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(b1, _graph.Mul(_grad[1], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(w2, _graph.Mul(_grad[2], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(b2, _graph.Mul(_grad[3], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(w3, _graph.Mul(_grad[4], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(b3, _graph.Mul(_grad[5], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(w4, _graph.Mul(_grad[6], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(b4, _graph.Mul(_grad[7], _graph.Const(0.01f,TFDataType.Float))).Operation,
            };
        }
        /// <summary>
        /// 加入训练
        /// </summary>
        public void Train(int batchSize, int width, int height)
        {
            using (_session = new TFSession(_graph))
            {
                var xData = Samples.CreateInputs(batchaSize: batchSize, oneDimensionCount: width * height);
                var yData = Samples.CreateLabels(batchSzie: batchSize);

                TFTensor tensorX = TFTensor.FromBuffer(new TFShape(batchSize, width * height), xData.ToArray(), 0, xData.Count);
                TFTensor tensorY = TFTensor.FromBuffer(new TFShape(batchSize, 10), yData.ToArray(), 0, yData.Count);

                _session.GetRunner().AddTarget(_inits);

                var s = tensorY.GetValue();

                var result = _session.GetRunner()
                     .AddInput(_x, tensorX)
                     .AddInput(_y, tensorY)
                     .AddTarget(_optimize)
                     .Fetch(_corss_entropy)
                     .Fetch(_grad)
                     .Run();
            }
        }

        /// <summary>
        /// 构建卷积操作
        /// </summary>
        /// <param name="tensor"></param>
        /// <param name="filter_size"></param>
        /// <param name="in_filters"></param>
        /// <param name="out_filters"></param>
        /// <param name="strides"></param>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        private TFOutput Conv2d(TFOutput x, TFOutput W, string scopeName = "Conv2d")
        {
            return _graph.Conv2D(x, W, new long[4] { 1, 1, 1, 1 }, "SAME");
        }
        /// <summary>
        /// 池化卷积结果（conv2d）池化层采用kernel大小为2*2，步数也为2，周围补0，取最大值。数据量缩小了4倍
        /// </summary>
        /// <param name="x"></param>
        private TFOutput MaxPool2x2(TFOutput x)
        {
            return _graph.MaxPool(x, new long[4] { 1, 2, 2, 1 }, new long[4] { 1, 2, 2, 1 }, "SAME");
        }

        private TFOutput ReShapeInput(params long[] shapes)
        {
            var shape = _graph.Shape(_graph.Placeholder(TFDataType.Float, new TFShape(shapes)));
            return shape;
        }

    }
}
