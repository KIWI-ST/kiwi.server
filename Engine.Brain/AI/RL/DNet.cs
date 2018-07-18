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
        //输入参数3，[可选] 实际q值
        private TFOutput _input_qvalue;
        //输出参数，prediction
        private TFOutput _output_qvalue;
        //
        private TFOperation[] _optimize;
        TFOutput _w1, _b1, _w2, _b2, _w3, _b3, _w4, _b4;
        TFOutput _l1, _l2, _l3, _l4;
        TFOutput _loss, _backprop;
        TFOutput[] _grad;

        public List<float> History { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n_features"></param>
        /// <param name="n_actions"></param>
        public DNet(int n_features, int n_actions)
        {
            //
            History = new List<float>();
            //calcute graph
            _graph = new TFGraph();
            //input
            _input_features = _graph.Placeholder(TFDataType.Float, new TFShape(-1, n_features + n_actions));
            _input_qvalue = _graph.Placeholder(TFDataType.Float, new TFShape(-1, 1));
            //layer1
            _w1 = _graph.VariableV2(new TFShape(n_features + n_actions, n_actions), TFDataType.Float);
            _b1 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            var y1 = _graph.Add(_graph.MatMul(_input_features, _w1), _b1);
            _l1 = _graph.Sigmoid(y1);
            //layer2
            _w2 = _graph.VariableV2(new TFShape(n_actions, n_actions), TFDataType.Float);
            _b2 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            var y2 = _graph.Add(_graph.MatMul(_l1, _w2), _b2);
            _l2 = _graph.Sigmoid(y2);
            //layer3
            _w3 = _graph.VariableV2(new TFShape(n_actions, n_actions / 2), TFDataType.Float);
            _b3 = _graph.VariableV2(new TFShape(1, n_actions / 2), TFDataType.Float);
            var y3 = _graph.Add(_graph.MatMul(_l2, _w3), _b3);
            _l3 = _graph.Sigmoid(y3);
            //layer4 
            _w4 = _graph.VariableV2(new TFShape(n_actions / 2, 1), TFDataType.Float);
            _b4 = _graph.VariableV2(new TFShape(1, 1), TFDataType.Float);
            var y4 = _graph.Add(_graph.MatMul(_l3, _w4), _b4);
            _l4 = _graph.Sigmoid(y4);
            //calcute reward
            _output_qvalue = _l4;
            //loss and train
            _loss = _graph.Neg(_graph.ReduceSum(_graph.Mul(_input_qvalue, _graph.Log(_l4))));
            //calute gradient 
            _grad = _graph.AddGradients(new TFOutput[] { _loss }, new TFOutput[] {
                _w1,_b1,
                _w2,_b2,
                _w3,_b3,
                _w4,_b4
            });
            //init variables
            var inits = new[]{
                 _graph.Assign(_w1, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(n_features+n_actions,n_actions)))).Operation,
                _graph.Assign(_b1, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(1,n_actions)))).Operation,
                _graph.Assign(_w2, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(n_actions,n_actions)))).Operation,
                _graph.Assign(_b2, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(1,n_actions)))).Operation,
                _graph.Assign(_w3, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(n_actions,n_actions/2)))).Operation,
                _graph.Assign(_b3, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(1,n_actions/2)))).Operation,
                _graph.Assign(_w4, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(n_actions/2,1)))).Operation,
                _graph.Assign(_b4, _graph.Const(NP.CreateTensorWithRandomNormalFloat(new TFShape(1,1)))).Operation,
            };
            //optimize gradient descent
            _optimize = new[]{
                _graph.ApplyGradientDescent(_w1,_graph.Const(0.01f),_grad[0]).Operation,
                _graph.ApplyGradientDescent(_b1,_graph.Const(0.01f),_grad[1]).Operation,
                _graph.ApplyGradientDescent(_w2,_graph.Const(0.01f),_grad[2]).Operation,
                _graph.ApplyGradientDescent(_b2,_graph.Const(0.01f),_grad[3]).Operation,
                _graph.ApplyGradientDescent(_w3,_graph.Const(0.01f),_grad[4]).Operation,
                _graph.ApplyGradientDescent(_b3,_graph.Const(0.01f),_grad[5]).Operation,
                _graph.ApplyGradientDescent(_w4,_graph.Const(0.01f),_grad[6]).Operation,
                _graph.ApplyGradientDescent(_b4,_graph.Const(0.01f),_grad[7]).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_w1,_w1),()=>_graph.AssignSub(_w1, _graph.Mul(_grad[0], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_b1,_b1),()=>_graph.AssignSub(_b1, _graph.Mul(_grad[1], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_w2,_w2),()=>_graph.AssignSub(_w2, _graph.Mul(_grad[2], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_b2,_b2),()=>_graph.AssignSub(_b2, _graph.Mul(_grad[3], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_w3,_w3),()=>_graph.AssignSub(_w3, _graph.Mul(_grad[4], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_b3,_b3),()=>_graph.AssignSub(_b3, _graph.Mul(_grad[5], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_w4,_w4),()=>_graph.AssignSub(_w4, _graph.Mul(_grad[6], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                //_graph.Cond(_graph.IsNan(_loss),()=>_graph.Assign(_b4,_b4),()=>_graph.AssignSub(_b4, _graph.Mul(_grad[7], _graph.Const(0.00001f,TFDataType.Float)))).Operation,
                // _graph.AssignSub(_w1, _graph.Mul(_grad[0], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_b1, _graph.Mul(_grad[1], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_w2, _graph.Mul(_grad[2], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_b2, _graph.Mul(_grad[3], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_w3, _graph.Mul(_grad[4], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_b3, _graph.Mul(_grad[5], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_w4, _graph.Mul(_grad[6], _graph.Const(0.01f,TFDataType.Float))).Operation,
                //_graph.AssignSub(_b4, _graph.Mul(_grad[7], _graph.Const(0.01f,TFDataType.Float))).Operation,
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
        /// train model
        /// </summary>
        public void Train(TFTensor input_feature_tensor, TFTensor input_qvalue_tensor)
        {
            //input output
            var input = input_feature_tensor.GetValue();
            var output = input_qvalue_tensor.GetValue();
            for (int i = 0; i < 10000; i++)
            {
                //
                var variables = _session.GetRunner().Fetch(_w1, _b1, _w2, _b2, _w3, _b3, _w4, _b4).Run();
                var w1 = variables[0].GetValue();
                var b1 = variables[1].GetValue();
                var w2 = variables[2].GetValue();
                var b2 = variables[3].GetValue();
                var w3 = variables[4].GetValue();
                var b3 = variables[5].GetValue();
                var w4 = variables[6].GetValue();
                var b4 = variables[7].GetValue();
                //var result = _session.GetRunner().AddInput(_input_features, input_feature_tensor).AddInput(_input_qvalue, input_qvalue_tensor).AddTarget(_optimize).Fetch(_loss).Fetch(_grad).Fetch(_l1,_l2,_l3,_l4,_backprop).Run();
                var result = _session.GetRunner().
                    AddInput(_input_features, input_feature_tensor).
                    AddInput(_input_qvalue, input_qvalue_tensor).
                    AddTarget(_optimize).
                    Fetch(_loss).
                    Fetch(_l1, _l2, _l3, _l4).
                    Fetch(_grad).
                    Run();
                //
                var loss = result[0].GetValue();
                //
                var l1 = result[1].GetValue();
                var l2 = result[2].GetValue();
                var l3 = result[3].GetValue();
                var l4 = result[4].GetValue();
                //
                var gard1 = result[5].GetValue();
                var gard2 = result[6].GetValue();
                var gard3 = result[7].GetValue();
                var gard4 = result[8].GetValue();
                var gard5 = result[9].GetValue();
                var gard6 = result[10].GetValue();
                var gard7 = result[11].GetValue();
                var gard8 = result[12].GetValue();
                //
                History.Add((float)loss);
            }
        }
        /// <summary>
        /// 预测
        /// </summary>
        /// <returns></returns>
        public object Predict(TFTensor feature_tensor)
        {
            var result = _session.GetRunner().AddInput(_input_features, feature_tensor).Fetch(_output_qvalue).Run();
            var predict = result[0].GetValue();
            return predict;
        }

    }
}
