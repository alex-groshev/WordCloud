using System;
using System.Net;
using HtmlAgilityPack;

namespace WordCloud.Services
{
    public class FetchService : IFetchService
    {
        public string Body(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(nameof(url));

            using (var client = new WebClient())
            {
                var html = client.DownloadString(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                return htmlDocument.DocumentNode.SelectSingleNode("//body").InnerText.Trim();
            }
        }
    }
}