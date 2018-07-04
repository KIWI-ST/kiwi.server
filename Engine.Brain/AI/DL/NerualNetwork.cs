using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace Engine.Brain.AI
{
    /// <summary>
    /// 构建普通的多层神经网络
    /// reference:
    /// https://github.com/bytedeco/javacpp-presets/blob/master/tensorflow/samples/src/main/java/org/bytedeco/javacpp/samples/tensorflow/CarPricePredictionExample.java
    /// </summary>
    public class NerualNetwork
    {
        TFGraph _graph;

        public NerualNetwork()
        {
            //构建图
            _graph = new TFGraph();
            //1.
            //var x = _graph.PlaceholderV2(TFDataType)


        }





    }
}
