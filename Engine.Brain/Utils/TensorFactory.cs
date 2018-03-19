using TensorFlow;

namespace Engine.Brain.Utils
{
    /// <summary>
    /// 分辨率枚举
    /// </summary>
    public enum ShapeEnum
    {
        THIRTEEN_TEN,
        TEN_TEN
    }

    public static class ImageUtil
    {
        //13x10
        const int W13 = 13;
        //10x10
        const int W10 = 10, H10 = 10;

        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        public static TFTensor CreateTensorFromImageFile(float[] contents, ShapeEnum resolution, TFDataType destinationDataType = TFDataType.Int32)
        {
            var shape = new TFShape(1, 10, 10, 1);
            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.FromBuffer(shape, contents,0,contents.Length);
            //TFOutput input, output;
            //// Construct a graph to normalize the image
            //using (var graph = ConstructGraphToNormalizeImage(out input, out output, resolution, destinationDataType))
            //{
            //    // Execute that graph to normalize this one image
            //    using (var session = new TFSession(graph))
            //    {
            //        var normalized = session.Run(
            //            inputs: new[] { input },
            //            inputValues: new[] { tensor },
            //            outputs: new[] { output });
            //        //
            //        return normalized[0];
            //    }
            //}
            return tensor;
        }
        // The inception model takes as input the image described by a Tensor in a very
        // specific normalized format (a particular image size, shape of the input tensor,
        // normalized pixel values etc.).
        //
        // This function constructs a graph of TensorFlow operations which takes as
        // input a JPEG-encoded string and returns a tensor suitable as input to the
        // inception model.
        private static TFGraph ConstructGraphToNormalizeImage(out TFOutput input, out TFOutput output, ShapeEnum resolution = ShapeEnum.TEN_TEN, TFDataType destinationDataType = TFDataType.Float)
        {
            // Some constants specific to the pre-trained model at:
            // https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip
            // - The model was trained after with images scaled to 224x224 pixels.
            // - The colors, represented as R, G, B in 1-byte each were converted to
            //   float using (value - Mean)/Scale.
            //比例
            const float Scale = 1;
            //均值
            const float Mean = 117;
            //形状
            int[] shapeSize;
            //分辨率判定
            if(resolution == ShapeEnum.TEN_TEN)
                shapeSize = new int[] { W10, H10 };
            else if (resolution == ShapeEnum.THIRTEEN_TEN)
                shapeSize = new int[] { W13, H10 };
            else
                shapeSize = new int[] { W10, H10 };

            var graph = new TFGraph();
            input = graph.Placeholder(TFDataType.String);

            output = graph.Cast(graph.Div(
                x: graph.Sub(
                    x: graph.ResizeBilinear(
                        images: graph.ExpandDims(
                              //input: graph.Cast(graph.DecodeJpeg(contents: input, channels: 1), DstT: TFDataType.Float),
                            input: graph.Cast(input, DstT: TFDataType.Float),
                            dim: graph.Const(0, "make_batch")),
                        size: graph.Const(shapeSize, "size")),
                    y: graph.Const(Mean, "mean")),
                y: graph.Const(Scale, "scale")), destinationDataType);

            return graph;
        }
    }

    public class TensorFactory
    {
        public static TFTensor Create(float[] input, ShapeEnum resolution)
        {
            return ImageUtil.CreateTensorFromImageFile(input, resolution);
        }
    }

}
