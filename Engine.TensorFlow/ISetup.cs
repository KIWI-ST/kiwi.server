using TensorFlow;
using tf = TensorFlow;

namespace Engine.Brain
{
    public class BrainSetup
    {
        public static void Run()
        {
            using(var graph = new tf.TFGraph())
            {
                //variable
                var W = graph.VariableV2(tf.TFShape.Scalar, dtype: tf.TFDataType.Double, operName: "W");
                var b = graph.VariableV2(tf.TFShape.Scalar, dtype: tf.TFDataType.Double, operName: "b");
                //
                var pred = graph.Const(false);
                var init = graph.Cond(pred,
                    () => graph.Assign(W, graph.Const(1.0)),
                    () => graph.Assign(b, graph.Const(-0.3)));
                //
                using(var sess = new tf.TFSession(graph))
                {
                    var sw =  sess.GetRunner().Fetch(W).Run();

                    var sb = sess.GetRunner().Fetch(b).Run();

                    var rW = sess.GetRunner().Fetch(W).Run();

                }
            }

        }

    }
}
