namespace Engine.Word.Utils
{
    /// <summary>
    /// Calcute WordHash
    /// </summary>
    public class WordHash
    {
        /// <summary>
        /// vocabulary size
        /// </summary>
        public const int VOCABHASHSIZE = 30000000;

        public static uint GetWordHash(string word)
        {
            int a;
            ulong hash = 0;
            for (a = 0; a < word.Length; a++)
                hash = hash * 257 + word[a];
            hash = hash % 30000000;
            return (uint)hash;
        }
    }
}
