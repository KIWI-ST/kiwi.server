using Core.DL.Entity;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.DL.Network
{
    /// <summary>
    /// 定义图像分割神经网络
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class MulitClassifyNetwork<TInput, TOutput> : INeuralNetwork<TInput, TOutput>
        where TInput : class, new()
        where TOutput : class, new()
    {
        /// <summary>
        /// 训练数据地址
        /// </summary>
        string _trainFile;
        /// <summary>
        /// 运行环境目录，用于保存模型等操作
        /// </summary>
        string _workDirectory;

        /// <summary>
        /// 设置默认参数
        /// </summary>
        public MulitClassifyNetwork(string trainFile, string workDirectory = null)
        {
            //设置训练数据地址
            _trainFile = trainFile;
            //设置运行文件目录
            if (workDirectory == null)
            {
                string dir = Directory.GetCurrentDirectory() + @"/Models/";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                _workDirectory = dir;
            }
            else
            {
                _workDirectory = workDirectory;
            }
        }

        public async Task<PredictionModel<TInput, TOutput>> TrainAsync()
        {
            //1.通过反射获取Tinput的attributes
            Type ti = typeof(TInput), to = typeof(TOutput);
            //2.获取input属性
            List<string> inputColoums = new List<string>();
            Array.ForEach(ti.GetFields(), new Action<System.Reflection.FieldInfo>(p =>
            {
                inputColoums.Add(p.Name);
            }));
            //3.获取output属性
            string outputColoum = to.GetFields().Length > 0 ? to.GetFields()[0].Name : null;
            //4.聚合输入输出层参数名称
            ColumnConcatenator coloums = new ColumnConcatenator(outputColoum, inputColoums.ToArray());
            //4.构建学习机
            //LearningPipeline pipeline = new LearningPipeline();
            //pipeline.Add()\
            //CollectionDataSource.Create(new List<Input>() { new Input { Number1 = 1, String1 = "1" } })

            LearningPipeline pipeline = new LearningPipeline
            {
                coloums,
                new LogisticRegressionBinaryClassifier()
            };
            PredictionModel<TInput, TOutput> model = pipeline.Train<TInput, TOutput>();
            //model写入zip file
            await model.WriteAsync(_workDirectory+System.DateTime.Now.ToLongDateString()+".zip");
            //返回model对象
            return model;
        }
    }
}
