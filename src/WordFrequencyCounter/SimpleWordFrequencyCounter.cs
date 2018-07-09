using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordFrequencyCounter
{
    public class SimpleWordFrequencyCounter : IWordFrequencyCounter
    {
        public IDictionary<string, int> Process(string filePath)
        {
            var dictionary = new Dictionary<string, int>();
            foreach (var word in File.ReadLines(filePath).SelectMany(line => line.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)))
            {
                if(dictionary.ContainsKey(word))dictionary[word]++;
                else dictionary.Add(word.ToLower(), 1);
            }
            return dictionary;
        }
    }
}