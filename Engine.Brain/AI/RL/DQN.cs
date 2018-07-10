using TensorFlow;

namespace Engine.Brain.AI
{
    public class DQN
    {
        /// <summary>
        /// 输入数据dim
        /// </summary>
        int n_features;
        /// <summary>
        /// 
        /// </summary>
        int n_actions;
        /// <summary>
        /// dqn  主要参数
        /// </summary>
        TFOutput _s, _a, _q;
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
            n_features = inputHeight * inputWidth;
            n_actions = actions_num;

        }
        /// <summary>
        /// 构建神经网络，用于学习[s,a,q,s_]
        /// 
        /// </summary>
        public void BuildEvalNet()
        {

        }
        /// <summary>
        /// without train
        /// </summary>
        public void BuildTragetNet()
        {
            TFGraph graph = new TFGraph();
            //定义第一层网络的输出特征为10
            int n_eval_l1_outfeature = 10;
            //input
            var x1 = graph.Placeholder(TFDataType.Float, new TFShape(-1, n_features));
            var y1 = graph.Placeholder(TFDataType.Float, new TFShape(-1, n_actions));
            //layer1
            var w1 = graph.VariableV2(new TFShape(n_features, n_eval_l1_outfeature), TFDataType.Float);
            var b1 = graph.VariableV2(new TFShape(1, n_eval_l1_outfeature), TFDataType.Float);
            var l1 = graph.Relu(graph.Add(graph.MatMul(x1, w1), b1));
            //layer2
            var w2 = graph.VariableV2(new TFShape(n_eval_l1_outfeature, n_actions), TFDataType.Float);
            var b2 = graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            var l2 = graph.Relu(graph.Add(graph.MatMul(l1, w2), b2));
        }

    }
}
