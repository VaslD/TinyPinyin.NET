using System.Collections.Generic;

using Ganss.Text;

namespace TinyPinyin
{
    public interface ISegmentationSelector
    {
        IEnumerable<WordMatch> SelectFrom(IEnumerable<WordMatch> matches);
    }
}
