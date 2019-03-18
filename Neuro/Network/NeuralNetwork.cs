using NEURO;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neuro.Networks
{
    public class NeuralNetwork : INetwork
    {
        public List<ILayer> Layers { get; private set; } = new List<ILayer>();
        public double[] Output { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public NeuralNetwork() { }

        #region 保存和修改模型

        public INetwork Load(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                INetwork network = Load(stream);
                return network;
            }
        }
        public INetwork Load(Stream stream)
        {
            using (stream)
            {
                IFormatter formatter = new BinaryFormatter();
                INetwork network = (INetwork)formatter.Deserialize(stream);
                return network;
            }
        }
        public void Save(Stream stream)
        {
            using (stream)
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
            }
        }
        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Save(stream);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayer(ILayer layer)
        {
            Layers.Add(layer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double[] Compute(double[] input)
        {
            double[] output = input;
            Layers.ForEach(layer =>{
                output = layer.Compute(output);
            });
            Output = output;
            return Output;
        }
    }
}
