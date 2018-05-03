using Engine.Brain.Utils;
using TensorFlow;

namespace Engine.Brain
{
    public interface IBootstrap
    {
        /// <summary>
        /// 执行graph分类操作
        /// </summary>
        /// <param name="input"></param>
        /// <param name="shapeEnum"></param>
        /// <returns></returns>
        long Classify(float[] input, ShapeEnum shapeEnum);
        /// <summary>
        /// tensorflow模型对象
        /// </summary>
        TFGraph Graph { get; }
        /// <summary>
        /// 模型名
        /// </summary>
        string ModalFilename { get; }
    }
}
