using System.Collections.Generic;
using WordCloud.Models;

namespace WordCloud.Services
{
    public interface IAdminService
    {
        IEnumerable<WordViewModel> GetTopWords();
    }
}
