using System.Collections.Generic;
using System.Linq;

namespace WordCloud.Data
{
    public class WordRepository : IWordRepository
    {
        public List<Word> GetTop100()
        {
            using (var context = new WordCloudModel())
            {
                return context.Words.OrderByDescending(x => x.Count).Take(100).ToList();
            }
        }

        public void Upsert(List<Word> words)
        {
            //todo: bulk upsert
            using (var context = new WordCloudModel())
            {
                foreach (var word in words)
                {
                    var existing = context.Words.FirstOrDefault(x => x.SaltedHash == word.SaltedHash);
                    if (existing == null)
                    {
                        context.Words.Add(word);
                    }
                    else
                    {
                        existing.Count += word.Count;
                        context.Entry(existing).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                context.SaveChanges();
            }
        }
    }
}