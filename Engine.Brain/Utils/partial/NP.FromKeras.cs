using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Brain.Utils
{
    public partial class NP
    {
        public static class FromKeras
        {
            /// <summary>
            ///  https://github.com/keras-team/keras/blob/master/keras/preprocessing/sequence.py
            /// </summary>
            public class Preprocessing
            {
                /// <summary>
                /// https://github.com/keras-team/keras/blob/master/keras/preprocessing/sequence.py
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="sequences"></param>
                /// <param name="maxlen"></param>
                /// <returns></returns>
                static public double[][] pad_sequences<T>(List<List<T>> sequences, int maxlen)
                {
                    var padded_sequences = new double[sequences.Count][];
                    for (int i = 0; i < sequences.Count; i++)
                    {
                        padded_sequences[i] = new double[maxlen];
                        var src = sequences[i];
                        var dst = padded_sequences[i];
                        var offset = src.Count - dst.Length;
                        for (int dst_index = Math.Max(0, -offset); dst_index < dst.Length; dst_index++)
                        {
                            var src_index = dst_index + offset;
                            dst[dst_index] = Convert.ToSingle(src[src_index]);
                        }
                    }
                    return padded_sequences;
                }
            }
            /// <summary>
            /// https://github.com/python/cpython/blob/master/Lib/string.py
            /// </summary>
            public class Tokenizer
            {
                readonly int num_words;
                public Dictionary<string, int> word_index { get; }
                // See https://github.com/keras-team/keras/blob/master/keras/preprocessing/text.py
                static readonly char[] filters = " !\"#$%&()*+,-./:;<=>?@[\\]^_`{|}~\t\n".ToArray();

                public Tokenizer(int num_words)
                {
                    this.num_words = num_words;
                    word_index = new Dictionary<string, int>();
                }

                public static string python_string_printable()
                {
                    // https://github.com/python/cpython/blob/master/Lib/string.py
                    var whitespace = " \t\n\r\v\f";
                    var ascii_lowercase = "abcdefghijklmnopqrstuvwxyz";
                    var ascii_uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    var ascii_letters = ascii_lowercase + ascii_uppercase;
                    var digits = "0123456789";
                    var hexdigits = digits + "abcdef" + "ABCDEF";
                    var punctuation = "\"!#$%&()*+,-./:;<=>?@[\\]^_`{|}~";
                    var printable = digits + ascii_letters + punctuation + whitespace;
                    return printable;
                }
                public List<List<int>> texts_to_sequences(string[] samples)
                {
                    var results = new List<List<int>>();
                    for (int i = 0; i < samples.Length; i++)
                    {
                        var sequence = new List<int>();
                        foreach (var word in text_to_word_sequence(samples[i]))
                        {
                            int word_index;
                            this.word_index.TryGetValue(word, out word_index);
                            sequence.Add(word_index);
                        }
                        results.Add(sequence);
                    }
                    return results;
                }

                private string[] text_to_word_sequence(string text)
                {
                    return text.ToLower().Split(filters).Where(v => v.Length > 0).ToArray();
                }

                public void fit_on_texts(string[] samples)
                {
                    word_index.Clear();
                    for (int i = 0; i < samples.Length; i++)
                    {
                        foreach (var word in text_to_word_sequence(samples[i]))
                        {
                            var num_occurences = word_index.ContainsKey(word) ? word_index[word] : 0;
                            word_index[word] = num_occurences + 1;
                        }
                    }

                    if (word_index.Count >= this.num_words)
                    {
                        var words_in_descending_order_of_occurences = word_index.Keys.OrderByDescending(v => word_index[v]).ToArray();
                        for (int i = this.num_words; i < words_in_descending_order_of_occurences.Length; i++)
                        {
                            word_index.Remove(words_in_descending_order_of_occurences[i]);
                        }
                    }

                    var keys = word_index.Keys.ToArray();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        word_index[keys[i]] = i + 1;
                    }
                }

            }
        }
    }
}
