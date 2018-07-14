using Engine.Brain.Entity;
using TensorFlow;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// DQN State Prediction NeuralNetwork 
    /// </summary>
    public class DNet
    {
        //runner
        private TFSession _session;
        //calcute graph
        private TFGraph _graph;
        //定义第一层网络的输出特征为10
        readonly int n_eval_l1_outfeature = 10;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n_features"></param>
        /// <param name="n_actions"></param>
        public DNet(int n_features, int n_actions)
        {
            //calcute graph
            _graph = new TFGraph();
            //input
            var x1 = _graph.Placeholder(TFDataType.Float, new TFShape(-1, n_features));
            var y1 = _graph.Placeholder(TFDataType.Float, new TFShape(-1, n_actions));
            //layer1
            var w1 = _graph.VariableV2(new TFShape(n_features, n_eval_l1_outfeature), TFDataType.Float);
            var b1 = _graph.VariableV2(new TFShape(1, n_eval_l1_outfeature), TFDataType.Float);
            var l1 = _graph.Relu(_graph.Add(_graph.MatMul(x1, w1), b1));
            //layer2
            var w2 = _graph.VariableV2(new TFShape(n_eval_l1_outfeature, n_actions), TFDataType.Float);
            var b2 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            var l2 = _graph.Relu(_graph.Add(_graph.MatMul(l1, w2), b2));
            //loss and train
            var corss_entropy = _graph.Neg(_graph.ReduceSum(_graph.Mul(y1, _graph.Log(l2))));
            //calute gradient 
            var grad = _graph.AddGradients(new TFOutput[] { corss_entropy }, new TFOutput[] {
                w1,b1,
                w2,b2
            });
            //init variables
            var inits = new[]{
                _graph.Assign(w1, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(5,5,1,32)))).Operation,
                _graph.Assign(b1, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(32)))).Operation,
                _graph.Assign(w2, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(5,5,32,64)))).Operation,
                _graph.Assign(b2, _graph.Const(Samples.CreateTensorWithRandomFloat(new TFShape(64)))).Operation,
            };
            //optimize gradient descent
            var optimize = new[]{
                _graph.AssignSub(w1, _graph.Mul(grad[0], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(b1, _graph.Mul(grad[1], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(w2, _graph.Mul(grad[2], _graph.Const(0.01f,TFDataType.Float))).Operation,
                _graph.AssignSub(b2, _graph.Mul(grad[3], _graph.Const(0.01f,TFDataType.Float))).Operation,
            };
            //inital varibales
            Initialize(inits);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Initialize(TFOperation[] operations)
        {
            //init session
            _session = new TFSession(_graph);
            _session.GetRunner().AddTarget(operations);
            _session.GetRunner().Run();
        }
        /// <summary>
        /// 增加学习记忆区
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="reward"></param>
        public void History(int state,int action,int reward)
        {

        }
        /// <summary>
        /// train model
        /// </summary>
        public void Train()
        {

        }
        /// <summary>
        /// 预测
        /// </summary>
        /// <returns></returns>
        public double Predict(double[] state)
        {
            return 1;
        }

    }
}
