using System.Collections.Generic;

namespace WordCloud.Services
{
    public interface IWordCloudService
    {
        IEnumerable<KeyValuePair<string, int>> GetWordCloud(string content, IEnumerable<string> stopWords);
    }
}
