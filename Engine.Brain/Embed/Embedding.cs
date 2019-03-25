using CNTK;

namespace Engine.Brain.Embed
{
    /// <summary>
    /// https://github.com/axmand/deep-learning-with-csharp-and-cntk/tree/master/DeepLearning/Ch_06_Using_Word_Embeddings
    /// </summary>
    public class Embedding
    {
        static public Function Embed(Variable x, int shape, DeviceDescriptor device, float[][] weights = null, string opName = "")
        {
            //if(weights == null)
            //{
            var weightShape = new int[] { shape, NDShape.InferredDimension };
            var E = new Parameter(weightShape, DataType.Float, CNTKLib.GlorotUniformInitializer(), device, "embedding_" + opName);
            return CNTKLib.Times(E, x);
            //}
            //else
            //{
            //    var weight_shape = new int[] { shape, x.Shape.Dimensions[0] };
            //    System.Diagnostics.Debug.Assert(shape == weights[0].Length);
            //    System.Diagnostics.Debug.Assert(weight_shape[1] == weights.Length);
            //    var w = convert_jagged_array_to_single_dimensional_array(weights);
            //    var ndArrayView = new NDArrayView(weight_shape, w, device, readOnly: true);
            //    var E = new Constant(ndArrayView, name: "fixed_embedding_" + opName);
            //    return = CNTKLib.Times(E, x);
            //}
        }
    }
}
