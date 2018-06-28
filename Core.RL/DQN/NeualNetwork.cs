using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Learners;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.RL.DQN
{
    /// <summary>
    /// 训练数据类型
    /// </summary>
    public class LearningInput
    {
        /// <summary>
        /// 影像raw data
        /// </summary>
        [Column("0")]
        public double Rawdata1;
        [Column("1")]
        public double Rawdata2;
        [Column("2")]
        public double Rawdata3;
        [Column("3")]
        public double Rawdata4;


        [Column("4")]
        [ColumnName("Label")]
        public int Label;
    }

    public class LearningOutput
    {
        [ColumnName("PredictedLabel")]
        public int PredictedLabels;
    }

    /// <summary>
    /// 网络输入:
    /// 状态s+动作a
    /// 网络输出:
    /// q，即reward
    /// refrenece:
    /// https://zhuanlan.zhihu.com/p/32818105
    /// 
    /// 任务：
    /// 1.实现 environment 下的 convolution layer
    /// 2.实现
    /// 
    /// </summary>
    public class NeualNetwork
    {

        public NeualNetwork()
        {
            //初始化构建一个专用的pipline，用于传输操作
            LearningPipeline pipeline = new LearningPipeline();
            //1.加入数据
            ILearningPipelineLoader loaderItem = CollectionDataSource.Create<LearningInput>(new List<LearningInput>() {
                new LearningInput()
                {
                    Rawdata1 =1.0,
                    Rawdata2=2.0,
                    Rawdata3=3.0,
                    Rawdata4=4.0,
                    Label=1,
                }
            });
            pipeline.Add(loaderItem);
            //2.指定属性
            pipeline.Add(new ColumnConcatenator("Features", "Rawdata1", "Rawdata2", "Rawdata3", "Rawdata4"));
            //3.指定训练方法
            pipeline.Add(new LogisticRegressionClassifier() {

            });
            //4.指定prediction
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });


            var model = pipeline.Train<LearningInput, LearningOutput>();

            var pred = model.Predict(new LearningInput()
            {
                Rawdata1 = 1.0,
                Rawdata2 = 2.0,
                Rawdata3 = 3.0,
                Rawdata4 = 4.0,
            });


        }


        /// <summary>
        /// 输入s'和action,reward.
        /// 修正 nerual network 参数
        /// </summary>
        public void Learn()
        {

        }

        public void Transfer()
        {

        }
        /// <summary>
        /// 用于初始构建记忆池，记住观察学习的N步
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="reward"></param>
        public void Remember(string state, string action, string reward)
        {



        }


    }


}
