using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using WordCloud.Data;
using WordCloud.Services;

namespace WordCloud.Controllers
{
    public class WordController : ApiController
    {
        private static string _stopWordsfile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data"), "stopwords.txt");
        private IWordRepository _wordRepository;
        private IStopWordsService _stopWordsService;
        private IFetchService _fetchService;
        private IWordCloudService _wordCloudService;
        private IEncryptionService _encryptionService;

        public WordController()
        {
            //todo: inject
            _wordRepository = new WordRepository();
            _wordCloudService = new WordCloudService();
            _fetchService = new FetchService();
            _encryptionService = new EncryptionService();
            _stopWordsService = new StopWordsService(_stopWordsfile);
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, int>> Get(string url)
        {
            try
            {
                var kvps = _wordCloudService.GetWordCloud(_fetchService.Body(url), _stopWordsService.GetStopWords(3));
                _wordRepository.Upsert(ToWords(kvps));
                return kvps;
            }
            catch (WebException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<Word> ToWords(IEnumerable<KeyValuePair<string, int>> words)
        {
            return (from word in words.AsParallel()
                    let encryptedData = _encryptionService.Encrypt(word.Key)
                    select new Word
                    {
                        SaltedHash = encryptedData.Item1,
                        EncryptedValue = encryptedData.Item2,
                        Count = word.Value
                    }).ToList();
        }
    }
}
