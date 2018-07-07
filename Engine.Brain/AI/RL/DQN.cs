using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace Engine.Brain.AI
{
    public class DQN
    {
        /// <summary>
        /// 
        /// </summary>
        TFGraph _graph;
        /// <summary>
        /// 
        /// </summary>
        int _inputWidth, _inputHeight, _actions_num;
        /// <summary>
        /// 
        /// </summary>
        public DQN(int inputWidth, int inputHeight, int actions_num)
        {
            //
             _inputWidth = inputHeight;
             _inputHeight = inputHeight;
             _actions_num = actions_num;
            //initialization graph
            _graph = new TFGraph();
        }

        private TFOutput BuilCNNLayer()
        {
            //define graph0
            var _s = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, _inputWidth * _inputHeight), "state");
            var _a = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, _actions_num), "action");
            var _keep = _graph.PlaceholderV2(TFDataType.Double, TFShape.Scalar);
            //reshape _s
            var x = _graph.Reshape(_s, _graph.VariableV2(new TFShape(-1, _inputWidth, _inputHeight, 1), TFDataType.Double));
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
            return _y_;
        }


        private TFOutput Conv2d(TFOutput x, TFOutput filter, string scopeName = "Conv2d")
        {
            _graph.WithScope(scopeName);
            var strides = new long[] { 1, 1, 1, 1 };
            return _graph.Conv2D(x, filter, strides, "SAME");
        }

        private TFOutput MaxPool(TFOutput x)
        {
            return _graph.MaxPool(x, new long[] { 1, 2, 2, 1 }, new long[] { 1, 2, 2, 1 }, "SAME");
        }


    }
}
