using Engine.Brain.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TensorFlow;

namespace Engine.Brain.AI.RL
{
    /// <summary>
    /// DQN State Prediction NeuralNetwork 
    /// </summary>
    public class DNet
    {

        private int n_features, n_actions;

        public List<float> History { get; }

        #region 神经网络相关
        public TFOutput _w1, _b1, _w2, _b2, _w3, _b3, _w4, _b4;

        //calcute graph
        private TFGraph _graph;
        //输入参数1，features
        private TFOutput _input_features;
        //输入参数3，[可选] 实际q值
        private TFOutput _input_qvalue;
        //输出参数，prediction
        private TFOutput _output_qvalue;
        //中间操作，梯度修正
        TFOperation[] _optimize;
        //中间操作，输出层 l1,l2,l3,l4
        TFOutput _l1, _l2, _l3, _l4;
        //loss
        TFOutput _loss;
        //梯度修正
        TFOutput[] _grad;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n_features"></param>
        /// <param name="n_actions"></param>
        public DNet(int features_num, int actions_num)
        {
            //
            n_features = features_num;
            //
            n_actions = actions_num;
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
            _l1 = _graph.Selu(_graph.Add(_graph.MatMul(_input_features, _w1), _b1));
            //layer2
            _w2 = _graph.VariableV2(new TFShape(n_actions, n_actions), TFDataType.Float);
            _b2 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            _l2 = _graph.Selu(_graph.Add(_graph.MatMul(_l1, _w2), _b2));
            //layer3
            _w3 = _graph.VariableV2(new TFShape(n_actions, n_actions / 2), TFDataType.Float);
            _b3 = _graph.VariableV2(new TFShape(1, n_actions / 2), TFDataType.Float);
            _l3 = _graph.Selu(_graph.Add(_graph.MatMul(_l2, _w3), _b3));
            //layer4 
            _w4 = _graph.VariableV2(new TFShape(n_actions / 2, 1), TFDataType.Float);
            _b4 = _graph.VariableV2(new TFShape(1, 1), TFDataType.Float);
            _l4 = _graph.Selu(_graph.Add(_graph.MatMul(_l3, _w4), _b4));
            //calcute reward
            _output_qvalue = _l4;
            //loss and train
            //_loss = _graph.Neg(_graph.ReduceMean(_graph.ReduceSum(_graph.Mul(_input_qvalue, _graph.Log(_l4)))));
            //_loss = _graph.ReduceMean(_graph.Sub(_input_qvalue, _l4));
            //_loss = _graph.ReduceMean(_graph.SquaredDifference(_input_qvalue, _l4));
            _loss = _graph.ReduceMean(_graph.Mul(_graph.Const(0.5f), _graph.Square(_graph.Sub(_input_qvalue, _l4))));
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
            };
            //inital varibales
            Initialize(inits);
        }
        /// <summary>
        /// init session
        /// </summary>
        private void Initialize(TFOperation[] operations)
        {
            using (var session = new TFSession(_graph))
            {
                session.GetRunner().AddTarget(operations).Run();
                Freeze(session);
            }
        }

        float[] _w1_, _b1_, _w2_, _b2_, _w3_, _b3_, _w4_, _b4_;

        public (float[] w1, float[] b1, float[] w2, float[] b2, float[] w3, float[] b3, float[] w4, float[] b4) TrainVariables
        {
            get { return (_w1_, _b1_, _w2_, _b2_, _w3_, _b3_, _w4_, _b4_); }
        }

        private void Freeze(TFSession session)
        {
            var variables = session.GetRunner().Fetch(_w1, _b1, _w2, _b2, _w3, _b3, _w4, _b4).Run();
            _w1_ = NP.Pad((float[,])variables[0].GetValue());
            _b1_ = NP.Pad((float[,])variables[1].GetValue());
            _w2_ = NP.Pad((float[,])variables[2].GetValue());
            _b2_ = NP.Pad((float[,])variables[3].GetValue());
            _w3_ = NP.Pad((float[,])variables[4].GetValue());
            _b3_ = NP.Pad((float[,])variables[5].GetValue());
            _w4_ = NP.Pad((float[,])variables[6].GetValue());
            _b4_ = NP.Pad((float[,])variables[7].GetValue());
        }

        private void UnFreeze(TFSession session)
        {
            var variation = new[]{
                 _graph.Assign(_w1,_graph.Const(TFTensor.FromBuffer(new TFShape(n_features+n_actions,n_actions), _w1_, 0, _w1_.Length))).Operation,
                 _graph.Assign(_b1,_graph.Const(TFTensor.FromBuffer(new TFShape(1,n_actions), _b1_, 0, _b1_.Length))).Operation,
                 _graph.Assign(_w2,_graph.Const(TFTensor.FromBuffer(new TFShape(n_actions,n_actions), _w2_, 0, _w2_.Length))).Operation,
                 _graph.Assign(_b2,_graph.Const(TFTensor.FromBuffer(new TFShape(1,n_actions), _b2_, 0, _b2_.Length))).Operation,
                 _graph.Assign(_w3,_graph.Const(TFTensor.FromBuffer(new TFShape(n_actions,n_actions/2), _w3_, 0, _w3_.Length))).Operation,
                 _graph.Assign(_b3,_graph.Const(TFTensor.FromBuffer(new TFShape(1,n_actions/2), _b3_, 0, _b3_.Length))).Operation,
                 _graph.Assign(_w4,_graph.Const(TFTensor.FromBuffer(new TFShape(n_actions/2,1), _w4_, 0, _w4_.Length))).Operation,
                _graph.Assign(_b4,_graph.Const(TFTensor.FromBuffer(new TFShape(1,1), _b4_, 0, _b4_.Length))).Operation,
            };
            session.GetRunner().AddTarget(variation).Run();
        }
        /// <summary>
        /// train model
        /// </summary>
        public float Train(TFTensor input_feature_tensor, TFTensor input_qvalue_tensor)
        {
            float loss = 0.0f;
            using (var session = new TFSession(_graph))
            {
                UnFreeze(session);
                var result = session.GetRunner().
                AddInput(_input_features, input_feature_tensor).
                AddInput(_input_qvalue, input_qvalue_tensor).
                AddTarget(_optimize).
                Fetch(_loss).
                Fetch(_l1, _l2, _l3, _l4).
                Fetch(_grad).
                Run();
                loss = (float)result[0].GetValue();
                Freeze(session);
            }
            return loss;
         
        }
        /// <summary>
        /// 预测
        /// </summary>
        /// <returns></returns>
        public object Predict(TFTensor feature_tensor)
        {
            object predict = null;
            using (var session = new TFSession(_graph))
            {
                UnFreeze(session);
                var result = session.GetRunner().AddInput(_input_features, feature_tensor).Fetch(_output_qvalue).Run();
                predict = result[0].GetValue();
            }
            return predict;
        }
        /// <summary>
        /// copy sourceNet parameters
        /// </summary>
        /// <param name="sourNet"></param>
        /// <returns></returns>
        public void Accept(DNet sourceNet)
        {
            (_w1_, _b1_, _w2_, _b2_, _w3_, _b3_, _w4_, _b4_) = sourceNet.TrainVariables;
            using (var session = new TFSession(_graph))
                UnFreeze(session);
        }
        /// <summary>
        /// save model
        /// </summary>
        public void Save()
        {
            string root = System.IO.Directory.GetCurrentDirectory() + @"\model\";
            using (var buffer = new TFBuffer())
            {
                _graph.ToGraphDef(buffer);
                var bytes = buffer.ToArray();
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);
                var filePath = root + "graph.meta";
                File.WriteAllBytes(filePath, bytes);
            }
            TFOutput rootOutput = _graph.Const(TFTensor.CreateString(Encoding.UTF8.GetBytes(root + "min.ckpt")), TFDataType.String);
            TFOutput variableNames = _graph.Const(TFTensor.CreateString(Encoding.UTF8.GetBytes("w1 b1 l1")), TFDataType.String);
            TFOutput shape_and_slices = _graph.Const(TFTensor.CreateString(Encoding.UTF8.GetBytes("w1 b1 l1")), TFDataType.String);
            _graph.Save(rootOutput, variableNames, new[] { _w1, _b1, _l1 });
            //_session.SaveTensors(root+"min.tsf", ("w1",_w1),("l1", _l1));
        }

    }
}
