using System;
using System.Collections.Generic;

using Ganss.Text;

using TinyPinyin.Dictionaries;

namespace TinyPinyin
{
    internal static class TrieHelper
    {
        internal static Trie FromDictionaries(IEnumerable<IPinyinDict> dicts)
        {
            if (dicts == null) throw new ArgumentNullException(nameof(dicts));

            var phrases = new HashSet<String>();

            foreach (var pinyinDict in dicts)
            {
                if (!(pinyinDict.Phrases is HashSet<String> subset)) continue;

                foreach (var phrase in subset)
                {
                    phrases.Add(phrase);
                }
            }

            var trie = new Trie();
            foreach (var phrase in phrases)
            {
                trie.Add(phrase);
            }

            return null;
        }
    }
}
