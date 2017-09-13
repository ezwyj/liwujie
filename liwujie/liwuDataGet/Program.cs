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
            int no = 1048000;
            for(int i = 0; i < 100; i++)
            {
                Console.WriteLine("post {0}", no.ToString());
                AnalysisContent("http://www.liwushuo.com/posts/" + no.ToString());
                no = no + i;
                Console.WriteLine("----", no.ToString());
            }
            Console.ReadLine();
        }

        static void CheckOutContent()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = 256000;
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
                String url = "http://www.liwushuo.com/api/channels/1/items?limit=50";

                HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
                String result = response.Content.ReadAsStringAsync().Result;

                if (result.Length > 300)
                {
                    var liwuData = JsonConvert.DeserializeObject<liwuModel>(result);
                    foreach (var itemArchive in liwuData.data.items)
                    {
                        Console.WriteLine(itemArchive.content_url);
                        Console.WriteLine(itemArchive.introduction);
                        Console.WriteLine(itemArchive.title);
                        Console.WriteLine(ConvertIntDateTime(itemArchive.published_at));
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        static void AnalysisContent(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
                String result = response.Content.ReadAsStringAsync().Result;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                doc.LoadHtml(result);
                HtmlNodeCollection titleList = doc.DocumentNode.SelectNodes("//*[@class='item-title']/*[@class='ititle']");
                HtmlNodeCollection infoList = doc.DocumentNode.SelectNodes("//div[@class='item-info']");
                HtmlNodeCollection imgList = doc.DocumentNode.SelectNodes("//div[@class='content']/*/img");
                if (titleList != null)
                {
                    int i = 0;
                    string title, price, img, dataid;
                    foreach (HtmlNode item in titleList)
                    {
                        var priceNode = infoList[i].SelectNodes("//p[@class='item-info-price']");
                        price = priceNode[i].InnerText;
                        title = item.InnerText;
                        img = imgList[i].Attributes["src"].Value;
                        dataid = infoList[i].Attributes["data-id"].Value;

                        Console.WriteLine("title:{0},price{1},dataid:{2}", title, price, dataid);
                        i++;
                    }
                }
            }
            /*
                *
                * 测试通过http://www.liwushuo.com/posts/1048947      
                * 自测通过http://www.liwushuo.com/posts/1048337
            */
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
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
