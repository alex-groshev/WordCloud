using System.Collections.Generic;

namespace WordCloud.Services
{
    public interface IStopWordsService
    {
        IEnumerable<string> GetStopWords(int minLength);
    }
}
