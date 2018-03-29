using Engine.Brain.Utils;
using System;
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
            var tensor = TensorFactory.Create(input, shapeEnum);
            var runner = _session.GetRunner();
            var t0 = _graph["input"][0];
            runner.AddInput(_graph["input"][0], tensor).Fetch(_graph["logit/output"][0]);
            var output = runner.Run();
            long[] reslut = output[0].GetValue(jagged: false) as long[];
            return reslut[0];
        }

    }
}
