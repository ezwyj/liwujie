using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet
{
    class Program
    {
        static void Main(string[] args)
        {
            AnalysisContent();
        }

        static void CheckOutContent()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            String url = "http://www.liwushuo.com/api/channels/1/items?limit=50";

            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            var liwuData = JsonConvert.DeserializeObject<liwuModel>(result);
            foreach (var itemArchive in liwuData.data.items)
            {
                Console.WriteLine(itemArchive.content_url);
                Console.WriteLine(itemArchive.introduction);
                Console.WriteLine(itemArchive.title);
                Console.WriteLine(ConvertIntDateTime(itemArchive.published_at));
            }
        }

        static void AnalysisContent()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            String url = "http://www.liwushuo.com/posts/1048947";


            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(result);
            HtmlNodeCollection hrefList = doc.DocumentNode.SelectNodes("/html/body/div/div[3]/div[1]/div/div[2]/div[1]/div[2]");

            if (hrefList != null)
            {
                foreach (HtmlNode href in hrefList)
                {
                    HtmlAttribute att = href.Attributes["href"];
                }
            }

        }

        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);
            return time;
        }

    }
}
