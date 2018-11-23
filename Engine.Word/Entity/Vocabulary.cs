using System;

namespace Engine.Word.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Vocabulary:IComparable<Vocabulary>
    {
        /// <summary>
        /// 
        /// </summary>
        public long Cn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public char[] Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CodeLen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int[] Point { get; set; }

        public int CompareTo(Vocabulary o)
        {
            return (int)(Cn - o.Cn);
        }

    }
}
