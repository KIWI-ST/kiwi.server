using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
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
        //输入参数1，features
        private TFOutput _input_features;
        //输入参数2，action
        private TFOutput _input_actions;
        //输入参数3，[可选] 实际q值
        private TFOutput _input_qvalue;
        //输出参数，prediction
        private TFOutput _output_qvalue;
        //loss
        private TFOutput _corss_entropy;
        //
        private TFOperation[] _optimize;
        //
        private List<float> _history;

        TFOutput _cost, w1, b1, w2, b2, w3, b3;
        TFOutput[] _grad;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n_features"></param>
        /// <param name="n_actions"></param>
        public DNet(int n_features, int n_actions)
        {
            //
            _history = new List<float>();
            //calcute graph
            _graph = new TFGraph();
            //input
            _input_features = _graph.Placeholder(TFDataType.Float, new TFShape(-1, n_features + n_actions));
            _input_qvalue = _graph.Placeholder(TFDataType.Float, new TFShape(-1, 1));
            //layer1
            w1 = _graph.VariableV2(new TFShape(n_features + n_actions, n_actions), TFDataType.Float);
            b1 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            var l1 = _graph.Relu(_graph.Add(_graph.MatMul(_input_features, w1), b1));
            //layer2
            w2 = _graph.VariableV2(new TFShape(n_actions, n_actions), TFDataType.Float);
            b2 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            var l2 = _graph.Relu(_graph.Add(_graph.MatMul(l1, w2), b2));
            //layer3
            w3 = _graph.VariableV2(new TFShape(n_actions, 1), TFDataType.Float);
            b3 = _graph.VariableV2(new TFShape(1, 1), TFDataType.Float);
            var l3 = _graph.Relu(_graph.Add(_graph.MatMul(l2, w3), b3));
            //calcute reward
            _output_qvalue = l2;
            //loss and train
            //(_loss, _backprop) = _graph.SoftmaxCrossEntropyWithLogits(l3, _input_qvalue);
            _cost = _graph.ReduceMean(_graph.Neg(_graph.ReduceSum(_graph.Mul(_input_qvalue, _graph.Log(l3)))));
            //calute gradient 
            _grad = _graph.AddGradients(new TFOutput[] { _cost }, new TFOutput[] {
                w1,b1,
                w2,b2,
                w3,b3
            });
            //init variables
            var inits = new[]{
                _graph.Assign(w1, _graph.Const(NP.CreateTensorWithRandomFloat(new TFShape(n_features+n_actions,n_actions)))).Operation,
                _graph.Assign(b1, _graph.Const(NP.CreateTensorWithRandomFloat(new TFShape(1,n_actions)))).Operation,
                _graph.Assign(w2, _graph.Const(NP.CreateTensorWithRandomFloat(new TFShape(n_actions,n_actions)))).Operation,
                _graph.Assign(b2, _graph.Const(NP.CreateTensorWithRandomFloat(new TFShape(1,n_actions)))).Operation,
                _graph.Assign(w3, _graph.Const(NP.CreateTensorWithRandomFloat(new TFShape(n_actions,1)))).Operation,
                _graph.Assign(b3, _graph.Const(NP.CreateTensorWithRandomFloat(new TFShape(1,1)))).Operation,
            };
            //optimize gradient descent
            _optimize = new[]{
                _graph.AssignSub(w1, _graph.Mul(_grad[0], _graph.Const(0.0001f,TFDataType.Float))).Operation,
                _graph.AssignSub(b1, _graph.Mul(_grad[1], _graph.Const(0.0001f,TFDataType.Float))).Operation,
                _graph.AssignSub(w2, _graph.Mul(_grad[2], _graph.Const(0.0001f,TFDataType.Float))).Operation,
                _graph.AssignSub(b2, _graph.Mul(_grad[3], _graph.Const(0.0001f,TFDataType.Float))).Operation,
                _graph.AssignSub(w3, _graph.Mul(_grad[4], _graph.Const(0.0001f,TFDataType.Float))).Operation,
                _graph.AssignSub(b3, _graph.Mul(_grad[5], _graph.Const(0.0001f,TFDataType.Float))).Operation,
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
            _session.GetRunner().AddTarget(operations).Run();
        }
        /// <summary>
        /// 增加学习记忆区
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="reward"></param>
        public void History(int state, int action, int reward)
        {

        }
        /// <summary>
        /// train model
        /// </summary>
        public void Train(TFTensor input_feature_tensor, TFTensor input_qvalue_tensor)
        {
            for (int i = 0; i < 1000; i++)
            {
                //var result = _session.GetRunner().AddInput(_input_features, input_feature_tensor).AddInput(_input_qvalue, input_qvalue_tensor).AddTarget(_optimize).Fetch(_loss,_backprop).Run();
                var result = _session.GetRunner().AddInput(_input_features, input_feature_tensor).AddInput(_input_qvalue, input_qvalue_tensor).AddTarget(_optimize).Fetch(_cost).Fetch(_grad).Run();
                var loss = result[0].GetValue();
                _history.Add((float)loss);
            }
            Console.Write(_history);
        }
        /// <summary>
        /// 预测
        /// </summary>
        /// <returns></returns>
        public object Predict(TFTensor feature_tensor, TFTensor action_tensor)
        {
            var result = _session.GetRunner().AddInput(_input_features, feature_tensor).AddInput(_input_actions, action_tensor).Fetch(_output_qvalue).Run();
            var predict = result[0].GetValue();
            return predict;
        }

    }
}
