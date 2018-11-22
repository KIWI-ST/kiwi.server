using System;

namespace Engine.Brain.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class WordEntry : IComparable<WordEntry>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public float Score { get; private set; }

        public WordEntry(string name,float score)
        {
            Name = name;
            Score = score;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", Name, Score);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(WordEntry o)
        {
            return Score < o.Score ? 1 : -1;
        }

    }
}
