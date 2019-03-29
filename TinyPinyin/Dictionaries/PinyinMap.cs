using System;
using System.Collections.Generic;

namespace TinyPinyin.Dictionaries
{
    public abstract class PinyinMap : IPinyinDict
    {
        public abstract Dictionary<String, IEnumerable<String>> Map { get; }

        public HashSet<String> Phrases => new HashSet<String>(Map.Keys);

        public IEnumerable<String> Transliterate(String phrase)
        {
            return Map[phrase];
        }
    }
}
