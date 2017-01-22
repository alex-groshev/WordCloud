using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordCloud.Services
{
    public class WordCloudService : IWordCloudService
    {
        public IEnumerable<KeyValuePair<string, int>> GetWordCloud(string content, IEnumerable<string> stopWords)
        {
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var word in ExtractWords(content, new HashSet<string>(stopWords, StringComparer.OrdinalIgnoreCase)))
            {
                if (result.ContainsKey(word))
                    result[word]++;
                else
                    result[word] = 1;
            }
            return result.OrderByDescending(x => x.Value).Take(100);
        }

        private IEnumerable<string> ExtractWords(string content, HashSet<string> stopWords)
        {
            var words = Regex.Split(content, "[^a-zA-Z]+");
            foreach (var word in words.Where(x => x.Length > 2))
            {
                if (!stopWords.Contains(word))
                    yield return word;
            }
        }
    }
}