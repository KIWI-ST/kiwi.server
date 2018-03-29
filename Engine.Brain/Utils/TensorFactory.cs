using TensorFlow;

namespace Engine.Brain.Utils
{
    /// <summary>
    /// 分辨率枚举
    /// </summary>
    public enum ShapeEnum
    {
        /// <summary>
        /// 13x10
        /// </summary>
        THIRTEEN_TEN,
        /// <summary>
        /// 10x10
        /// </summary>
        TEN_TEN,
        /// <summary>
        /// 8x8
        /// </summary>
        EIGHT_EIGHT
    }

    public static class ImageUtil
    {
        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        public static TFTensor CreateTensorFromImageFile(float[] contents, ShapeEnum resolution, TFDataType destinationDataType = TFDataType.Int32)
        {
            TFShape shape;
            if (resolution == ShapeEnum.TEN_TEN)
                shape = new TFShape(1, 10, 10, 1);
            else if (resolution == ShapeEnum.THIRTEEN_TEN)
                shape = new TFShape(1, 13, 10, 1);
            else if (resolution == ShapeEnum.EIGHT_EIGHT)
                shape = new TFShape(1, 8, 8, 1);
            else
                shape = new TFShape(1, 10, 10, 1);
            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.FromBuffer(shape, contents, 0, contents.Length);
            return tensor;
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
