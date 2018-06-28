using System;
using System.Collections.Generic;
using System.IO;

namespace Core.RL.DQN
{
    /// <summary>
    /// action枚举
    /// </summary>
    public enum EAction
    {
        /// <summary>
        /// 水稻
        /// </summary>
        RICE = 1,
        /// <summary>
        /// 小麦
        /// </summary>
        WHEAT = 2,
        /// <summary>
        /// 烟草
        /// </summary>
        TOBACCO = 3
    }

    /// <summary>
    /// 输入样本通过多次cnn卷积，输出一个相对简单的特征向量，用于计算(s)
    /// 任务：
    /// 1.观测并读取训练样本
    /// 2.多层cnn，降维生成样本特征向量
    /// 3.计算reward(给出reward的量化方法）
    /// </summary>
    public class DEnv
    {
        /// <summary>
        /// 被观察样本的根目录
        /// </summary>
        string _dir;
        /// <summary>
        /// 二级目录
        /// </summary>
        string[] _categories;
        /// <summary>
        /// 样本存储集合
        /// </summary>
        Dictionary<string, string[]> _sampleDictionary;
        /// <summary>
        /// 构建环境
        /// 样本存放格式形如：
        /// sampleDirectory
        ///              |
        ///          /       \
        ///    分类1    分类2 ...
        ///        |           |
        ///    /       \       \
        /// 样本a 样本b  样本c
        /// </summary>
        public DEnv(string sampleDirectory)
        {
            //样本根目录
            _dir = sampleDirectory;
            //获取样本分类目录
            _categories = Directory.GetDirectories(sampleDirectory);
            //样本集合
            _sampleDictionary = new Dictionary<string, string[]>();
            //初始化环境
            InitEnv(_categories, _sampleDictionary);
        }
        /// <summary>
        /// 构建样本字典
        /// </summary>
        private void InitEnv(string[] categories, Dictionary<string, string[]> sampleDictionary)
        {
            //构建dictory目录树
            Array.ForEach(categories, categoryDir =>
            {
                //获取样本全集
                string[] samples = Directory.GetFiles(categoryDir);
                //载入字典
                sampleDictionary.Add(categoryDir, samples);
            });
        }
        /// <summary>
        /// 随机观察分类操作，并获取reward
        /// </summary>
        private double[] AccessEnv()
        {
            //类别索引
            int classIndex = new Random().Next(_categories.Length);
            //样本索引
            int sampleIndex = new Random().Next(_sampleDictionary[_categories[classIndex]].Length);
            //样本文件地址
            string sampleFile =_sampleDictionary[_categories[classIndex]][sampleIndex];
            //构建样本输入环境s和辅助参考计算reward的class
            //1.读取灰度图，存储成byte[]

            //2.使用卷积，获取特征数据
            return new double[2] { 0, 0 };
        }
        /// <summary>
        /// 执行下一步操作
        /// 返回：操作后的环境s'和当前的reward
        /// 1. 根据当前的s计算reward
        /// 2. 计算下一个s（s'）
        /// </summary>
        public double[] Step(EAction action)
        {
            //1.
            //string s =


            //2.
            return null;
        }

    }


}
