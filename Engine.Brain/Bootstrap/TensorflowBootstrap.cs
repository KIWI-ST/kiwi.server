using System.IO;
using TensorFlow;

namespace Engine.Brain.Bootstrap
{
    public class TensorflowBootstrap : IBootstrap
    {
        string _modalFilename;

        public TensorflowBootstrap(string modalFilename)
        {
            _modalFilename = modalFilename;
            //
            using (var graph = new TFGraph())
            {
                var model = File.ReadAllBytes(modalFilename); 
                graph.Import(new TFBuffer(model));
                using (var session = new TFSession(graph))
                {
                    var tensor = ImageUtil.CreateTensorFromImageFile(@"D:\coordinate_systems.jpg", TFDataType.Float);
                    //1.get runner
                    var runner = session.GetRunner();
                    var t0 =  graph["input"][0];
                    //2.add input out put
                    runner.AddInput(graph["input"][0], tensor).Fetch(graph["logit/output"][0]);
                    var output = runner.Run();
                }
            }
        }
    }
}
