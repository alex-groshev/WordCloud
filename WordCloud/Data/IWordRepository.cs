using System.Collections.Generic;

namespace WordCloud.Data
{
    public interface IWordRepository
    {
        List<Word> GetTop100();
        void Upsert(List<Word> words);
    }
}
