using System.Collections.Generic;
using System.Linq;
using WordCloud.Data;
using WordCloud.Models;

namespace WordCloud.Services
{
    public class AdminService : IAdminService
    {
        private IWordRepository _wordRepository;
        private IEncryptionService _encryptionService;

        public AdminService()
            : this(new WordRepository(), new EncryptionService()) { }

        public AdminService(IWordRepository wordRepository, IEncryptionService encryptionService)
        {
            _wordRepository = wordRepository;
            _encryptionService = encryptionService;
        }

        public IEnumerable<WordViewModel> GetTopWords()
        {
            return (from word in _wordRepository.GetTop100()
                    select new WordViewModel
                    {
                        Word = _encryptionService.Decrypt(word.EncryptedValue),
                        Count = word.Count
                    }).ToList();
        }
    }
}