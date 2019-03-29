using System;
using System.Collections.Generic;

using Ganss.Text;

namespace TinyPinyin
{
    public class MatchComparer : IComparer<WordMatch>
    {
        public Int32 Compare(WordMatch x, WordMatch y)
        {
            if (x.Index == y.Index) return y.Word.Length.CompareTo(x.Word.Length);

            return x.Index.CompareTo(y.Index);
        }
    }
}
