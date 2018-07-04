using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace Engine.Brain.AI
{
    public class CNN
    {
        /// <summary>
        /// 
        /// </summary>
        long[] _filters = new long[] { 16, 16, 32, 64 };
        /// <summary>
        /// 
        /// </summary>
        TFGraph _graph;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputWidth">输入图像的宽</param>
        /// <param name="inputHeight">输入图像的高</param>
        /// <param name="actions_num">图像分类操作（对应类别数）</param>
        public CNN(int inputWidth, int inputHeight, int actions_num)
        {
            _graph = new TFGraph();
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
        private TFOutput Conv(TFOutput x, int filter_size, int in_filters, int out_filters, long[] strides, string scopeName = "Conv")
        {
            _graph.WithScope(scopeName);
            var shape = new TFShape(filter_size, filter_size, in_filters, out_filters);
            var filter = _graph.VariableV2(shape, TFDataType.Float);
            return _graph.Conv2D(x, filter, strides, "SAME");
        }
        /// <summary>
        /// Relu操作
        /// </summary>
        /// <param name="x"></param>
        /// <param name="leakiness"></param>
        /// <returns></returns>
        private TFOutput Relu(TFOutput x,double leakiness = 0.0)
        {
            _graph.WithScope("Relu");
            //写法1,使用条件判断
            var c = _graph.Const(0);
            var condition = _graph.Less(x, c);
            return _graph.Where(condition, c, x);
           //写法2，直接relu
            //return _graph.Relu(x);
        }



    }
}
