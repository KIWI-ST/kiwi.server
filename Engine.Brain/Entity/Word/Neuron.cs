using System;

namespace Engine.Brain.Entity
{
    /// <summary>
    /// 用于分析语料的基本单元
    /// </summary>
    public class Neuron:IComparable<Neuron>
    {
        /// <summary>
        /// 
        /// </summary>
        public double Freq { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public Neuron Parent { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; protected set; }
        /// <summary>
        /// 预料分类
        /// </summary>
        public int Category { get; protected set; } = -1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="neuron"></param>
        /// <returns></returns>
        public int CompareTo(Neuron neuron)
        {
            if (Category == neuron.Category)
                return Freq > neuron.Freq ? 1 : -1;
            else if (Category > neuron.Category)
                return 1;
            else
                return -1;
        }

    }
}
