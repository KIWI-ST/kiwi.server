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
            _w1 = _graph.VariableV2(new TFShape(n_features + n_actions, n_actions), TFDataType.Float, operName: "w1");
            _b1 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float, operName: "b1");
            _l1 = _graph.Add(_graph.MatMul(_input_features, _w1), _b1);
            //_l1 = _graph.Relu(y1, operName: "l1");
            //layer2
            _w2 = _graph.VariableV2(new TFShape(n_actions, n_actions), TFDataType.Float);
            _b2 = _graph.VariableV2(new TFShape(1, n_actions), TFDataType.Float);
            _l2 = _graph.Add(_graph.MatMul(_l1, _w2), _b2);
            //_l2 = _graph.Relu(y2);
            //layer3
            _w3 = _graph.VariableV2(new TFShape(n_actions, n_actions / 2), TFDataType.Float);
            _b3 = _graph.VariableV2(new TFShape(1, n_actions / 2), TFDataType.Float);
            _l3 = _graph.Add(_graph.MatMul(_l2, _w3), _b3);
            //_l3 = _graph.Relu(y3);
            //layer4 
            _w4 = _graph.VariableV2(new TFShape(n_actions / 2, 1), TFDataType.Float);
            _b4 = _graph.VariableV2(new TFShape(1, 1), TFDataType.Float);
            _l4 = _graph.Add(_graph.MatMul(_l3, _w4), _b4);
            //_l4 = _graph.Relu(y4);
            //calcute reward
            _output_qvalue = _l4;
            //loss and train
            //_loss = _graph.Neg(_graph.ReduceSum(_graph.Mul(_input_qvalue, _graph.Log(_l4))));
            _loss = _graph.ReduceMean(_graph.Abs(_graph.Sub(_input_qvalue, _l4)));
            //_loss = _graph.ReduceMean(_graph.SquaredDifference(_input_qvalue, _l4));
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
                _graph.ApplyGradientDescent(_w1,_graph.Const(0.001f),_grad[0]).Operation,
                _graph.ApplyGradientDescent(_b1,_graph.Const(0.001f),_grad[1]).Operation,
                _graph.ApplyGradientDescent(_w2,_graph.Const(0.001f),_grad[2]).Operation,
                _graph.ApplyGradientDescent(_b2,_graph.Const(0.001f),_grad[3]).Operation,
                _graph.ApplyGradientDescent(_w3,_graph.Const(0.001f),_grad[4]).Operation,
                _graph.ApplyGradientDescent(_b3,_graph.Const(0.001f),_grad[5]).Operation,
                _graph.ApplyGradientDescent(_w4,_graph.Const(0.001f),_grad[6]).Operation,
                _graph.ApplyGradientDescent(_b4,_graph.Const(0.001f),_grad[7]).Operation,
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
        public float Train(TFTensor input_feature_tensor, TFTensor input_qvalue_tensor)
        {
            var result = _session.GetRunner().
                AddInput(_input_features, input_feature_tensor).
                AddInput(_input_qvalue, input_qvalue_tensor).
                AddTarget(_optimize).
                Fetch(_loss).
                Fetch(_l1, _l2, _l3, _l4).
                Fetch(_grad).
                Run();
            float loss = (float)result[0].GetValue();
            History.Add(loss);
            return loss;
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
        /// <summary>
        /// copy sourceNet parameters
        /// </summary>
        /// <param name="sourNet"></param>
        /// <returns></returns>
        public void Accept(DNet sourceNet)
        {
            //sourceNet variables
            float[] w1, b1, w2, b2, w3, b3, w4, b4;
            (w1, b1, w2, b2, w3, b3, w4, b4) = sourceNet.GetVariables();
            //}{ debug print to watch
            float[] w1_, b1_, w2_, b2_, w3_, b3_, w4_, b4_;
            (w1_, b1_, w2_, b2_, w3_, b3_, w4_, b4_) = GetVariables();
            //convert to TFOutput
            var variation = new[]{
                 _graph.Assign(_w1,_graph.Const(TFTensor.FromBuffer(new TFShape(n_features+n_actions,n_actions), w1, 0, w1.Length))).Operation,
                 _graph.Assign(_b1,_graph.Const(TFTensor.FromBuffer(new TFShape(1,n_actions), b1, 0, b1.Length))).Operation,
                 _graph.Assign(_w2,_graph.Const(TFTensor.FromBuffer(new TFShape(n_actions,n_actions), w2, 0, w2.Length))).Operation,
                 _graph.Assign(_b2,_graph.Const(TFTensor.FromBuffer(new TFShape(1,n_actions), b2, 0, b2.Length))).Operation,
                 _graph.Assign(_w3,_graph.Const(TFTensor.FromBuffer(new TFShape(n_actions,n_actions/2), w3, 0, w3.Length))).Operation,
                 _graph.Assign(_b3,_graph.Const(TFTensor.FromBuffer(new TFShape(1,n_actions/2), b3, 0, b3.Length))).Operation,
                 _graph.Assign(_w4,_graph.Const(TFTensor.FromBuffer(new TFShape(n_actions/2,1), w4, 0, w4.Length))).Operation,
                _graph.Assign(_b4,_graph.Const(TFTensor.FromBuffer(new TFShape(1,1), b4, 0, b4.Length))).Operation,
            };
            //
            _session.GetRunner().AddTarget(variation).Run();
            (w1_, b1_, w2_, b2_, w3_, b3_, w4_, b4_) = GetVariables();
        }

        public (float[] w1, float[] b1, float[] w2, float[] b2, float[] w3, float[] b3, float[] w4, float[] b4) GetVariables()
        {
            var variables = _session.GetRunner().Fetch(_w1, _b1, _w2, _b2, _w3, _b3, _w4, _b4).Run();

            float[] w1 = NP.Pad((float[,])variables[0].GetValue());
            float[] b1 = NP.Pad((float[,])variables[1].GetValue());
            float[] w2 = NP.Pad((float[,])variables[2].GetValue());
            float[] b2 = NP.Pad((float[,])variables[3].GetValue());
            float[] w3 = NP.Pad((float[,])variables[4].GetValue());
            float[] b3 = NP.Pad((float[,])variables[5].GetValue());
            float[] w4 = NP.Pad((float[,])variables[6].GetValue());
            float[] b4 = NP.Pad((float[,])variables[7].GetValue());

            return (w1, b1, w2, b2, w3, b3, w4, b4);
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
