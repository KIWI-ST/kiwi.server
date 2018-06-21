﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.RL.DQN
{
    /// <summary>
    /// 输入样本通过多次cnn卷积，输出一个相对简单的特征向量，用于计算(s)
    /// 任务：
    /// 1.观测并读取训练样本
    /// 2.多层cnn，降维生成样本特征向量
    /// 3.计算reward(给出reward的量化方法）
    /// </summary>
    public class Environment
    {
    }
}
