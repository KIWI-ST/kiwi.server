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
        TFOutput _s;
        TFOutput y_hat;
        /// <summary>
        /// 
        /// </summary>
        int _inputWidth, _inputHeight, _actions_num;
        /// <summary>
        /// 
        /// </summary>
        public DQN(int inputWidth, int inputHeight, int actions_num)
        {
            _s = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, _inputWidth * _inputHeight), "state");
            var _a = _graph.PlaceholderV2(TFDataType.Double, new TFShape(-1, _actions_num), "action");
            //
            _inputWidth = inputHeight;
             _inputHeight = inputHeight;
             _actions_num = actions_num;
            //initialization graph
            _graph = new TFGraph();
            //calculate image feature vector by convolution
            TFOutput img_input = BuildCNNLayer();
            //concat tensor to one dimension
            TFOutput inputs = _graph.ConcatV2(new TFOutput[] { img_input,_a },new TFOutput());
            //

        }

        private TFOutput BuildCNNLayer()
        {
            //define graph0
            var _keep = _graph.PlaceholderV2(TFDataType.Double, TFShape.Scalar);
            //reshape _s
            var x = _graph.Reshape(_s, _graph.VariableV2(new TFShape(-1, _inputWidth, _inputHeight, 1), TFDataType.Double));
            //uniformization bit data
            var normalX = _graph.Mul(x, _graph.Const(1 / 255));
            //convolution layer 1
            var w1 = _graph.VariableV2(new TFShape(5, 5, 1, 32), TFDataType.Double);
            var b1 = _graph.VariableV2(new TFShape(32), TFDataType.Double);
            var c1 = _graph.Relu(_graph.Add(Conv2d(normalX, w1), b1));
            var p1 = MaxPool(c1);
            //convolution layer 2
            var w2 = _graph.VariableV2(new TFShape(5, 5, 32, 64), TFDataType.Double);
            var b2 = _graph.VariableV2(new TFShape(64), TFDataType.Double);
            var c2 = _graph.Relu(_graph.Add(Conv2d(p1, w2), b2));
            var p2 = MaxPool(c2);
            //full connection layer
            var w3 = _graph.VariableV2(new TFShape(7 * 7 * 64, 1024), TFDataType.Double);
            var b3 = _graph.VariableV2(new TFShape(1024), TFDataType.Double);
            var p3 = _graph.Reshape(p2, _graph.VariableV2(new TFShape(-1, 7 * 7 * 64), TFDataType.Double));
            var c3 = _graph.Relu(_graph.Add(Conv2d(p3, w3), b3));
            var c3_dropout = _graph.Dropout(c3, _keep);
            //output layer
            var w4 = _graph.VariableV2(new TFShape(1024, 10), TFDataType.Double);
            var b4 = _graph.VariableV2(new TFShape(10), TFDataType.Double);
            y_hat = _graph.Softmax(_graph.Add(_graph.MatMul(c3_dropout, w4), b4));
            //prediction of feature vector
            return y_hat;
        }


        /// <summary>
        /// 增加初始化操作
        /// </summary>
        /// <param name="operation"></param>
        private void IncreateInitOperation(TFOperation operation)
        {

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
