using TensorFlow;

namespace Engine.Brain.AI.RL
{
    public interface IDNet
    {
        (float[] w1, float[] b1, float[] w2, float[] b2, float[] w3, float[] b3, float[] w4, float[] b4) TrainVariables { get; }
        void Accept(DNet sourceNet);
        object Predict(TFTensor feature_tensor);
        void Save();
        float Train(TFTensor input_feature_tensor, TFTensor input_qvalue_tensor);
    }
}