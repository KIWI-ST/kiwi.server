using Engine.Brain.Utils;
using System.IO;
using TensorFlow;

namespace Engine.Brain.Bootstrap
{
    public class TensorflowBootstrap : IBootstrap
    {
        string _modalFilename;
        TFGraph _graph;
        byte[] _model;
        TFSession _session;

        public TensorflowBootstrap(string modalFilename)
        {
            _modalFilename = modalFilename;
            _graph = new TFGraph();
            _model = File.ReadAllBytes(modalFilename);
            _graph.Import(new TFBuffer(_model));
            _session = new TFSession(_graph);
        }

        public long Classify(float[] input, ShapeEnum shapeEnum)
        {
            var tensor = TensorFactory.Create(input, ShapeEnum.TEN_TEN);
            var runner = _session.GetRunner();
            var t0 = _graph["input"][0];
            runner.AddInput(_graph["input"][0], tensor).Fetch(_graph["logit/output"][0]);
            var output = runner.Run();
            return output[0].Shape[0];
        }

    }
}
