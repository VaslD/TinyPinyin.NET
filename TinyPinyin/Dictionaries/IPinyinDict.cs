using System;
using System.Collections.Generic;

namespace TinyPinyin.Dictionaries
{
    public interface IPinyinDict
    {
        HashSet<String> Phrases { get; }

        IEnumerable<String> Transliterate(String phrase);
    }
}
