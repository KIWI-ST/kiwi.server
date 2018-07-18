using Engine.Brain.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.IO;

namespace Engine.Brain.AI
{

    public class DRaw
    {
        /// <summary>
        /// state数据
        /// </summary>
        public float[] State { get; set; }
        /// <summary>
        /// one-hot vector
        /// </summary>
        public float[] Action { get; set; }
        /// <summary>
        /// reward
        /// </summary>
        public float Reward { get; set; }
    }

    public class DEnv
    {  /// <summary>
       /// 被观察样本的根目录
       /// </summary>
        string _dir;
        /// <summary>
        /// 二级目录
        /// </summary>
        string[] _categories;
        /// <summary>
        /// 分类个数，对应action
        /// </summary>
        private int _classes_num;
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
            _classes_num = _categories.Length;
            //构建dictory目录树
            Array.ForEach(categories, categoryDir =>
            {
                //获取样本全集
                string[] samples = Directory.GetFiles(categoryDir, "*.jpeg");
                //载入字典
                sampleDictionary.Add(categoryDir, samples);
            });
        }
        /// <summary>
        /// 执行下一步操作
        /// 返回：操作后的环境s'和当前的reward
        /// 1. 根据当前的s计算reward
        /// 2. 计算下一个s（s'）
        /// </summary>
        public DRaw Step(int action)
        {
            //1.随机获取一次观察结果
            //类别索引
            int classIndex = new Random().Next(_classes_num);
            //样本索引
            int sampleIndex = new Random().Next(_sampleDictionary[_categories[classIndex]].Length);
            //样本文件地址
            string sampleFile = _sampleDictionary[_categories[classIndex]][sampleIndex];
            GRasterLayer rasterLayer = new GRasterLayer(sampleFile);
            byte[] raw = rasterLayer.BandCollection[0].GetByteBuffer();
            //归一化
            float[] noraml = NP.Normalize(raw);
            //得到样本reward
            float reawrd = action == classIndex ? 1.0f : 0.01f;
            //数据打包返回
            return new DRaw()
            {
                State = noraml,
                Action = NP.ToOneHot(action, _classes_num),
                Reward = reawrd,
            };
        }

    }
}
