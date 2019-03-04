using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Ganss.Text;

using TinyPinyin.Data;
using TinyPinyin.Dictionaries;

namespace TinyPinyin
{
    public class PinyinConverter
    {
        public AhoCorasick           Tree         { get; } = new AhoCorasick();
        public ISegmentationSelector Selector     { get; }
        public IList<IPinyinDict>    Dictionaries { get; }

        public PinyinConverter(Config config)
        {
            Selector     = config.Selector;
            Dictionaries = config.Dictionaries.ToList();
        }

        public String ToPinyin(String text, String separator = "")
        {
            return Engine.Transliterate(text, Tree, Dictionaries, Selector, separator);
        }

        public static String ToStandardPinyin(String text, String separator = "")
        {
            return Engine.Transliterate(text, null, null, null, separator);
        }

        public static String ToPinyin(Char character)
        {
            if (IsChinese(character, out var code))
            {
                return character == PinyinData.Zero
                           ? PinyinData.ZeroPinyin
                           : PinyinData.PinyinTable[code];
            }

            return character.ToString(CultureInfo.InvariantCulture);
        }

        public static Boolean IsChinese(Char character, out Int32 code)
        {
            code = 0;
            return character                >= PinyinData.MinChar &&
                   character                <= PinyinData.MaxChar &&
                   (code = GetPinyinCode(character)) > 0 ||
                   character == PinyinData.Zero;
        }

        private static Int32 GetPinyinCode(Char character)
        {
            var offset = character - PinyinData.MinChar;
            if (0 <= offset && offset < PinyinData.Page1Offset)
            {
                return DecodeIndex(PinyinCodes1.Paddings, PinyinCodes1.Codes, offset);
            }

            if (PinyinData.Page1Offset <= offset && offset < PinyinData.Page2Offset)
            {
                return DecodeIndex(PinyinCodes2.Paddings, PinyinCodes2.Codes,
                                   offset - PinyinData.Page1Offset);
            }

            return DecodeIndex(PinyinCodes3.Paddings, PinyinCodes3.Codes,
                               offset - PinyinData.Page2Offset);
        }

        private static Int16 DecodeIndex(SByte[] paddings, SByte[] indexes, Int32 offset)
        {
            var divisionResult    = offset / 8;
            var divisionRemainder = offset % 8;

            var index = (Int16) (indexes[offset] & 0xFF);
            if ((paddings[divisionResult] & PinyinData.Masks[divisionRemainder]) != 0)
            {
                index = (Int16) (index | PinyinData.PaddingMask);
            }

            return index;
        }

        public class Config
        {
            public ISegmentationSelector Selector     { get; }
            public ISet<IPinyinDict>     Dictionaries { get; }

            public Boolean IsValid => Selector != null && Dictionaries != null;

            public Config(IEnumerable<IPinyinDict> dicts)
            {
                Dictionaries = new HashSet<IPinyinDict>(dicts);
                Selector     = new ForwardLongestSelector();
            }

            public Config(IEnumerable<IPinyinDict> dicts, ISegmentationSelector selector)
            {
                Dictionaries = new HashSet<IPinyinDict>(dicts);
                Selector     = selector ?? throw new ArgumentNullException(nameof(selector));
            }

            public void Merge(IPinyinDict additionalDict)
            {
                Dictionaries.Add(additionalDict);
            }
        }
    }
}
