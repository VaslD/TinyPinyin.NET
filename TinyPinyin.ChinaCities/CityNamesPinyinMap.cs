using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using TinyPinyin.Dictionaries;

namespace TinyPinyin.ChinaCities
{
    public class CityNamesPinyinMap : PinyinMap
    {
        private const String EmbeddedFile = "TinyPinyin.ChinaCities.ChinaCities.txt";

        public override Dictionary<String, IEnumerable<String>> Map { get; }

        public CityNamesPinyinMap()
        {
            Map = new Dictionary<String, IEnumerable<String>>();

            var assembly = typeof(CityNamesPinyinMap).GetTypeInfo().Assembly;
            if (assembly.GetManifestResourceNames().Contains(EmbeddedFile))
            {
                using (var stream = assembly.GetManifestResourceStream(EmbeddedFile))
                {
                    Init(stream);
                }
            }
        }

        public CityNamesPinyinMap(Stream cityNames)
        {
            Init(cityNames);
        }

        private void Init(Stream input)
        {
            using (var reader = new StreamReader(input, Encoding.UTF8))
            {
                while (reader.ReadLine() is String line)
                {
                    if (String.IsNullOrEmpty(line)) return;

                    if (line.Split(' ') is String[] pair &&
                        pair.Length == 2)
                    {
                        Map.Add(pair[1], pair[0].Split('\''));
                    }
                    else
                    {
                        throw new InvalidOperationException($"Line \"{line}\" is not properly formatted.");
                    }
                }
            }
        }
    }
}
