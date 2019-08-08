using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Engine.Brain.Method;
using Engine.Brain.Method.Convolution;
using Engine.Brain.Method.DeepQNet;

namespace Engine.Brain.Utils
{

    class MachineEntity
    {
        /// <summary>
        /// 模型类型，与需要与类名一致
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 模型保存实体
    /// </summary>
    class NeuralNetEntity : MachineEntity
    {
        /// <summary>
        /// 模型的byte数据
        /// </summary>
        public byte[] ModelBuffer { get; set; }
    }

    /// <summary>
    /// support store deepQNet
    /// </summary>
    class DeepQNetEntity : MachineEntity
    {
        /// <summary>
        /// actor
        /// </summary>
        public byte[] ActorBuffer { get; set; }

        /// <summary>
        /// critic
        /// </summary>
        public byte[] CriticBuffer { get; set; }

        /// <summary>
        /// 内置critic and actor 类型
        /// </summary>
        public string InnerTypeName { get; set; }

        /// <summary>
        /// 环境类型索引
        /// </summary>
        public int[] ActionKeys{ get; set; }

        /// <summary>
        /// the number of actions (one-hot length)
        /// </summary>
        public int ActionsNum { get; set; }

        /// <summary>
        /// the lenght of input feature
        /// </summary>
        public int FeaturesNum { get; set; }

    }

    public partial class NP
    {
        /// <summary>
        /// support models in this library
        /// </summary>
        public static class SupportModel
        {
            /// <summary>
            /// get convnet support only
            /// </summary>
            public static List<string> ConvSupportCollection
            {
                get
                {
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IConvNet))).Select(s => s.ToString().Split('.').Last());
                    return typeNames.ToList();
                }
            }

            /// <summary>
            /// get deep reinforcement neural net support , for DQN
            /// </summary>
            public static List<string> ReinforceSupportCollection
            {
                get
                {
                    IEnumerable<string> typeNames = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISupportNet))).Select(s => s.ToString().Split('.').Last());
                    return typeNames.ToList();
                }
            }

            /// <summary>
            /// save model
            /// </summary>
            public static void SaveModel(INeuralNet network, string filename)
            {
                //get model bytes
                byte[] modelBytes = network.PersistenceMemory();
                string typeName = network.GetType().Name;
                //create saved model instance
                NeuralNetEntity entity = new NeuralNetEntity()
                {
                    TypeName = typeName,
                    ModelBuffer = modelBytes
                };
                //convert to string
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                //write file
                using (Stream ms = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(ms))
                using (FileStream fs = new FileStream(filename, FileMode.Create, System.IO.FileAccess.Write))
                {
                    sw.Write(jsonText);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] bytes2 = new byte[ms.Length];
                    ms.Read(bytes2, 0, (int)ms.Length);
                    fs.Write(bytes2, 0, bytes2.Length);
                }
            }

            /// <summary>
            /// save DQN network
            /// </summary>
            /// <param name="network"></param>
            /// <param name="filename"></param>
            public static void SaveModel(IDeepQNet network, string filename)
            {
                var (actorBuffer, criticBuffer, innerTypeName, actionsNum, featuresNum ,actionKeys) = network.PersistencMemory();
                string typeName = network.GetType().Name;
                DeepQNetEntity entity = new DeepQNetEntity()
                {
                    ActorBuffer = actorBuffer,
                    CriticBuffer = criticBuffer,
                    TypeName = typeName,
                    InnerTypeName = innerTypeName,
                    FeaturesNum = featuresNum,
                    ActionsNum = actionsNum,
                    ActionKeys = actionKeys
                };
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                using (Stream ms = new MemoryStream())
                using (StreamWriter sw = new StreamWriter(ms))
                using (FileStream fs = new FileStream(filename, FileMode.Create, System.IO.FileAccess.Write))
                {
                    sw.Write(jsonText);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] bytes2 = new byte[ms.Length];
                    ms.Read(bytes2, 0, (int)ms.Length);
                    fs.Write(bytes2, 0, bytes2.Length);
                }
            }

            public static IMachineLarning Load(string filename, string deviceName)
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    int fsLen = (int)fs.Length;
                    byte[] heByte = new byte[fsLen];
                    int r = fs.Read(heByte, 0, heByte.Length);
                    string jsonText = System.Text.Encoding.UTF8.GetString(heByte);
                    MachineEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<MachineEntity>(jsonText);
                    if (entity.TypeName == typeof(FullyChannelNet9).Name) //cnn
                    {
                        NeuralNetEntity neuralNetEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<NeuralNetEntity>(jsonText);
                        return FullyChannelNet9.Load(neuralNetEntity.ModelBuffer, deviceName);
                    }
                    else if (entity.TypeName == typeof(DQN).Name) //dqn
                    {
                        DeepQNetEntity deepQNetEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<DeepQNetEntity>(jsonText);
                        return DQN.Load(
                            deepQNetEntity.ActorBuffer, deepQNetEntity.CriticBuffer, 
                            deepQNetEntity.ActionsNum, deepQNetEntity.FeaturesNum, deepQNetEntity.ActionKeys, 
                            deepQNetEntity.InnerTypeName, 
                            deviceName);
                    }
                    //not support
                    else
                        return null;
                }
            }

        }
    }
}
