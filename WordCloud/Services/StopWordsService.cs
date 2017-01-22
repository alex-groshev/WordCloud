using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordCloud.Services
{
    public class StopWordsService : IStopWordsService
    {
        private readonly string _fileName;

        public StopWordsService(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(nameof(fileName));
            _fileName = fileName;
        }

        public IEnumerable<string> GetStopWords(int minLength)
        {
            return File.ReadAllLines(_fileName).Where(x => x.Length >= minLength);
        }
    }
}