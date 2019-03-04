using System;
using System.Collections.Generic;
using System.Linq;

using Ganss.Text;

namespace TinyPinyin
{
    public class ForwardLongestSelector : ISegmentationSelector
    {
        public IComparer<WordMatch> Comparer { get; }

        public ForwardLongestSelector()
        {
            Comparer = new MatchComparer();
        }

        public ForwardLongestSelector(IComparer<WordMatch> customComparer)
        {
            Comparer = customComparer;
        }

        public IEnumerable<WordMatch> SelectFrom(IEnumerable<WordMatch> matches)
        {
            if (matches == null) throw new ArgumentNullException(nameof(matches));

            var sorted = matches.ToList();
            sorted.Sort(Comparer);

            var endValueToRemove = -1;
            var matchesToRemove  = new HashSet<WordMatch>();
            foreach (var wordMatch in sorted)
            {
                var end = wordMatch.Index + wordMatch.Word.Length;
                if (wordMatch.Index > endValueToRemove && end > endValueToRemove)
                    endValueToRemove = end;
                else
                    matchesToRemove.Add(wordMatch);
            }

            foreach (var match in matchesToRemove)
            {
                sorted.Remove(match);
            }

            sorted.Sort(Comparer);
            return sorted;
        }
    }
}
