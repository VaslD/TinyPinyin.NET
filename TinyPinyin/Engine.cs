using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ganss.Text;

using TinyPinyin.Dictionaries;

namespace TinyPinyin
{
    public static class Engine
    {
        public static String Transliterate(String             input, AhoCorasick           tree,
                                           IList<IPinyinDict> dicts, ISegmentationSelector selector,
                                           String             separator)
        {
            if (String.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));

            if (dicts == null || selector == null)
            {
                return String.Join(separator, input.Select(PinyinConverter.ToPinyin));
            }

            var matches = selector.SelectFrom(tree.Search(input)).ToList();

            var resultBuilder = new StringBuilder();
            var nextHitIndex  = 0;

            for (var i = 0; i < input.Length;)
            {
                var hit = matches[nextHitIndex];
                if (nextHitIndex < matches.Count && i == hit.Index)
                {
                    var fromDicts = GetPinyinFromDicts(hit.Word, dicts).ToList();
                    resultBuilder.Append(String.Join(separator, fromDicts.Select(x => x.ToUpperInvariant())));

                    i += hit.Index + hit.Word.Length;
                    nextHitIndex++;
                }
                else
                {
                    resultBuilder.Append(PinyinConverter.ToPinyin(input[i]));
                    i++;
                }

                if (i != input.Length)
                {
                    resultBuilder.Append(separator);
                }
            }

            return resultBuilder.ToString();
        }

        private static IEnumerable<String> GetPinyinFromDicts(String phrase, IEnumerable<IPinyinDict> dicts)
        {
            if (String.IsNullOrEmpty(phrase)) throw new ArgumentNullException(nameof(phrase));

            if (dicts == null) throw new ArgumentNullException(nameof(dicts));

            foreach (var pinyinDict in dicts)
            {
                if (pinyinDict?.Phrases?.Contains(phrase) == true) return pinyinDict.Transliterate(phrase);
            }

            throw new InvalidOperationException("No matching phrase found in provided dictionaries.");
        }
    }
}
