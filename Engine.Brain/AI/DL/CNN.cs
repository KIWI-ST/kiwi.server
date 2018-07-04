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
        /// <summary>
        /// input tensor s (placeholder)
        /// </summary>
        TFOutput _s;
        /// <summary>
        /// action tensor a (placeholder)
        /// </summary>
        TFOutput _a;
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
        /// reference:
        /// https://blog.csdn.net/xukaiwen_2016/article/details/70880694
        /// </summary>
        /// <param name="inputWidth">输入图像的宽</param>
        /// <param name="inputHeight">输入图像的高</param>
        /// <param name="actions_num">图像分类操作（对应类别数）</param>
        public CNN(int inputWidth, int inputHeight, int actions_num)
        {
            _graph = new TFGraph();
            //craete envrionment state placeholder
            _s = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, inputHeight * inputWidth), "state");
            //create agent's action
            _a = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, actions_num), "action");
            //create reward
            _r = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, 1), "reward");
            //dropout operation
            _keep = _graph.PlaceholderV2(TFDataType.Double, TFShape.Scalar);
            //The sparse matrix eigenvector is obtained by processing _s
            BuildCnnLayer(_s);
        }

        private void BuildCnnLayer(TFOutput x)
        {
            //uniformization bit data
            var normalX = _graph.Mul(x, _graph.Const(1 / 255));
            //convolution layer 1
            var w1 = _graph.VariableV2(new TFShape(5, 5, 1, 32), TFDataType.Double);
            var b1 = _graph.VariableV2(new TFShape(32), TFDataType.Double);
            var conv1 = _graph.Relu(_graph.Add(Conv2d(normalX, w1), b1));
            var pool1 = MaxPool(conv1);
            //convolution layer 2
            var w2 = _graph.VariableV2(new TFShape(5, 5, 32, 64), TFDataType.Double);
            var b2 = _graph.VariableV2(new TFShape(64), TFDataType.Double);
            var conv2 = _graph.Relu(_graph.Add(Conv2d(pool1, w2), b2));
            var pool2 = MaxPool(conv2);
            //full connection layer
            var wfc1 = _graph.VariableV2(new TFShape(7 * 7 * 64, 1024), TFDataType.Double);
            var bfc1 = _graph.VariableV2(new TFShape(1024), TFDataType.Double);
            var pool2_flat = _graph.Reshape(pool2, _graph.VariableV2(new TFShape(-1, 7 * 7 * 64), TFDataType.Double));
            var convfc1 = _graph.Relu(_graph.Add(Conv2d(pool2_flat, wfc1), bfc1));
            var convfc1_dropout = _graph.Dropout(convfc1, _keep);
            //output layer
            var wfc2 = _graph.VariableV2(new TFShape(1024, 10), TFDataType.Double);
            var bfc2 = _graph.VariableV2(new TFShape(10), TFDataType.Double);
            var _y_ = _graph.Softmax(_graph.Add(_graph.MatMul(convfc1_dropout, wfc2), bfc2));
            //loss function
            var corss_entropy = _graph.Neg(_graph.ReduceSum(_graph.Mul(_r, _graph.Log(_y_))));
            //gradent descent
            var grad = _graph.AddGradients(new TFOutput[] { corss_entropy }, new TFOutput[] {
                w1,b1,w2,b2,wfc1,bfc1,wfc2,bfc2
            });
            //optimize gradent descent
            var optimize = new[]{
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01))).Operation,
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01))).Operation,
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01))).Operation,
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01))).Operation,
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01))).Operation,
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01))).Operation,
            };
            //convolution：lowpass filtering
        }

        private void Build()
        {
            var s = _graph.Placeholder(TFDataType.Float, new TFShape(64), "state");
            var q = _graph.Placeholder(TFDataType.Int32, new TFShape(13), "target");
        }

        private TFOutput Weight_Variable(TFShape shape)
        {
            var variable = _graph.VariableV2(shape, TFDataType.Double);
            var initial = _graph.TruncatedNormal(variable, TFDataType.Float);
            return variable;
        }

        private TFOutput Bias_Variable(TFShape shape)
        {
            var variable = _graph.VariableV2(shape, TFDataType.Double);
            var initial = _graph.Constant(0.1, shape);
            return variable;
        }

        /// <summary>
        /// 定义CNN网络结构
        /// </summary>
        private void BuildLayer()
        {

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
        private TFOutput Conv2d(TFOutput x, TFOutput filter, string scopeName = "Conv2d")
        {
            _graph.WithScope(scopeName);
            var strides = new long[] { 1, 1, 1, 1 };
            return _graph.Conv2D(x, filter, strides, "SAME");
        }
        /// <summary>
        /// Relu操作
        /// </summary>
        /// <param name="x"></param>
        /// <param name="leakiness"></param>
        /// <returns></returns>
        private TFOutput Relu(TFOutput x, double leakiness = 0.0)
        {
            _graph.WithScope("Relu");
            //写法1,使用条件判断
            var c = _graph.Const(0);
            var condition = _graph.Less(x, c);
            return _graph.Where(condition, c, x);
            //写法2，直接relu
            //return _graph.Relu(x);
        }

        private long[] Stride_arr(long stride)
        {
            return new long[] { 1, stride, stride, 1 };
        }
        /// <summary>
        /// 池化卷积结果（conv2d）池化层采用kernel大小为2*2，步数也为2，周围补0，取最大值。数据量缩小了4倍
        /// </summary>
        /// <param name="x"></param>
        private TFOutput MaxPool(TFOutput x)
        {
            return _graph.MaxPool(x, new long[] { 1, 2, 2, 1 }, new long[] { 1, 2, 2, 1 }, "SAME");
        }


    }
}
