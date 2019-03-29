using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pinyin4net;
using Pinyin4net.Format;

using TinyPinyin;
using TinyPinyin.ChinaCities;
using TinyPinyin.Data;
using TinyPinyin.Dictionaries;

namespace TinyPinyinTest
{
    [TestClass]
    public class StandardPinyinTest
    {
        public TestContext TestContext { get; set; }

        private static readonly HanyuPinyinOutputFormat Config = new HanyuPinyinOutputFormat
                                                                 {
                                                                     CaseType  = HanyuPinyinCaseType.UPPERCASE,
                                                                     ToneType  = HanyuPinyinToneType.WITHOUT_TONE,
                                                                     VCharType = HanyuPinyinVCharType.WITH_V
                                                                 };

        [TestMethod]
        public void FullCoverage()
        {
            for (var i = PinyinData.MinChar; i <= PinyinData.MaxChar; i++)
            {
                var character = (Char) i;

                if (!PinyinConverter.IsChinese(character, out _)) continue;

                var standard            = PinyinHelper.ToHanyuPinyinStringArray(character, Config);
                var generatedFromChar   = PinyinConverter.ToPinyin(character);
                var generatedFromString = PinyinConverter.ToStandardPinyin(character.ToString());

                TestContext.WriteLine($"Character: {character} / Pinyin: {generatedFromChar}");

                Assert.AreEqual(standard[0], generatedFromChar);
                Assert.AreEqual(standard[0], generatedFromString);
            }
        }

        [TestMethod]
        public void CityNames()
        {
            var cityNames = new CityNamesPinyinMap();
            var converter = new PinyinConverter(new PinyinConverter.Config(new List<IPinyinDict>
                                                                           {
                                                                               cityNames
                                                                           }));

            foreach (var name in cityNames.Phrases)
            {
                var standard = cityNames.Map[name];
                var generated = converter.ToPinyin(name);

                TestContext.WriteLine($"Name: {name} / Pinyin: {generated}");

                Assert.AreEqual(String.Join(String.Empty, standard), generated);
            }
        }
    }
}
