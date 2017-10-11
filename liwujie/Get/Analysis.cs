using Get.Entity;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Get
{
    public class Analysis
    {
        public Analysis(string file,string url)
        {

            var productCount = AnalysisContent1(file, url);
            if (productCount == 0)
            {
                productCount = AnalysisContent2(file,url);
            }
            System.Threading.Thread.Sleep(1000);
        }

        private int AnalysisContent1(string result, string url)
        {

            //try
            //{
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(result);
            HtmlNodeCollection titleList = doc.DocumentNode.SelectNodes("//*[@class='item-title']/*[@class='ititle']");
            HtmlNodeCollection infoList = doc.DocumentNode.SelectNodes("//div[@class='item-info']");
            HtmlNodeCollection imgList = doc.DocumentNode.SelectNodes("//div[@class='content']/*/img");
            HtmlNodeCollection iteminfolinkList = doc.DocumentNode.SelectNodes("//div[@class='item-info']/*[@class='item-info-link']");



            PetaPoco.Database db = new PetaPoco.Database("dbConn");
            if (titleList != null)
            {
                int i = 0;
                string title, price, img, taobaoUrl, taobaoUID;
                foreach (HtmlNode item in titleList)
                {
                    var priceNode = infoList[i].SelectNodes("//p[@class='item-info-price']");
                    price = priceNode[i].InnerText;
                    title = item.InnerText;
                    img = imgList[i].Attributes["src"].Value;

                    taobaoUrl = GetTaobaoRealUrl(iteminfolinkList[i].Attributes["href"].Value);
                    taobaoUID = GetUid(taobaoUrl);
                    if (taobaoUID == "")
                    {
                        if (taobaoUrl.IndexOf("s.click.taobao.com") > 0)
                        {
                            taobaoUrl = GetTaobaoRealUrl(iteminfolinkList[i].Attributes["href"].Value);
                            taobaoUID = GetUid(taobaoUrl);
                        }
                        if (taobaoUrl.IndexOf("s.click.tmall.com") > 0)
                        {
                            taobaoUrl = GetTmallRealUrl(iteminfolinkList[i].Attributes["href"].Value);
                            taobaoUID = GetUid(taobaoUrl);
                        }
                        if (taobaoUID == "")
                        {
                            taobaoUrl = GetTaobaoRealUrl(iteminfolinkList[i].Attributes["href"].Value);
                            taobaoUID = GetUid(taobaoUrl);
                        }
                    }

                    var saveProduct = new ProductItem();
                    saveProduct.Price = float.Parse(price.Replace("￥", ""));
                    saveProduct.Title = title;
                    saveProduct.TaobaoUrl = taobaoUrl;
                    saveProduct.TaobaoUID = taobaoUID;
                    saveProduct.Image = img;
                    saveProduct.SourcePage = url;
                    saveProduct.InputTime = DateTime.Now;
                    Debug.WriteLine("titlt:{0},price:{1},TaobaoUID:{2}", saveProduct.Title, saveProduct.Price, saveProduct.TaobaoUID);
                    db.Save(saveProduct);
                    System.Threading.Thread.Sleep(500);
                    i++;
                }
            }
            else
            {
                return 0;
            }
            db.Dispose();

            return (titleList.Count + infoList.Count + imgList.Count) / 3;
            //}

            /*
                *
                * 测试通过http://www.liwushuo.com/posts/1048947      
                * 自测通过http://www.liwushuo.com/posts/1048337
            */
            //catch (Exception e)
            //{

            //    Debug.WriteLine("AnalysisContent1:" + e.Message);

            //    return 0;
            //}



        }
        private int AnalysisContent2(string result, string url)
        {
            int i = 0;
            //try
            //{

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(result);
            HtmlNodeCollection goodList = doc.DocumentNode.SelectNodes("//*[@class='hj-v2-list tpl-v2-list']");

            if (goodList != null)
            {
                PetaPoco.Database db = new PetaPoco.Database("dbConn");
                string title, price, img, taobaoUrl, taobaoUID;

                foreach (HtmlNode item in goodList)
                {
                    string itemJson = item.Attributes["data-payload"].Value.Replace("&#34;", "\"");
                    var goods = JsonConvert.DeserializeObject<Json.Product.SourceProductList>(itemJson);
                    foreach (var goodItem in goods.items)
                    {
                        title = goodItem.title.content;
                        price = goodItem.price;
                        img = goodItem.cover_image_url;
                        var taobaoJumpUrl = GetTaobaoPage(goodItem.url); ;
                        taobaoUrl = GetTaobaoRealUrl(taobaoJumpUrl);
                        taobaoUID = GetUid(taobaoUrl);
                        if (taobaoUID == "" && taobaoUrl.IndexOf("s.click.taobao.com") > 0)
                        {
                            taobaoUrl = GetTaobaoPage(taobaoUrl); ;
                            taobaoUID = GetUid(taobaoUrl);
                        }

                        var saveProduct = new ProductItem();
                        saveProduct.Price = float.Parse(price);
                        saveProduct.Title = title;
                        saveProduct.TaobaoUID = taobaoUID;
                        saveProduct.TaobaoUrl = taobaoUrl;

                        saveProduct.Image = img;
                        Debug.WriteLine("titlt:{0},prive:{1},TaobaoUID:{2}", saveProduct.Title, saveProduct.Price, saveProduct.TaobaoUID);
                        db.Save(saveProduct);
                        i++;
                    }
                }
            }
            return i;
            //}
            ///*
            //    *
            //    * 测试通过http://www.liwushuo.com/posts/1048947      
            //    * 自测通过http://www.liwushuo.com/posts/1048337
            //*/
            //catch(Exception e)
            //{
            //    Debug.WriteLine(e.Message);
            //    return 0;
            //}



        }
        private string GetTaobaoPage(string detialUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            HttpResponseMessage response = httpClient.GetAsync(new Uri(detialUrl)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(result);
            HtmlNodeCollection taobaoUrlNode = doc.DocumentNode.SelectNodes("//*[@class='btn-purchase']");
            string jumpUrl = taobaoUrlNode[0].Attributes["href"].Value;
            return jumpUrl;

        }
        private string GetUid(string url)
        {
            List<string> sp = url.Split('&').ToList();
            foreach (string s in sp)
            {
                if (s.ToLower().IndexOf("id=") >= 0)
                {
                    return s.Substring(s.ToLower().IndexOf("id=") + 3);
                }
            }
            return "";
        }
        private string GetTaobaoRealUrl(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowAutoRedirect = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            url = response.Headers["Location"];
            response.Close();

            Uri uri = new Uri(url);
            string analysisUrl = "";
            try
            {

                analysisUrl = uri.Query.Substring(4);
                url = HttpUtility.UrlDecode(analysisUrl);
                request = WebRequest.Create(url) as HttpWebRequest;

            }
            catch
            {
                analysisUrl = uri.Query.Substring(uri.Query.IndexOf("http_referer=") + 13);
                url = HttpUtility.UrlDecode(analysisUrl);
                request = WebRequest.Create(url) as HttpWebRequest;

            }
            request.AllowAutoRedirect = false;
            request.Referer = uri.ToString();
            response = request.GetResponse() as HttpWebResponse;
            url = response.Headers["Location"];
            response.Close();
            request.Abort();
            if (url.IndexOf("item.taobao.com") > 0 || url.IndexOf("detail.tmall.com/item.htm") > 0)
            {
                return url;
            }
            if (url.IndexOf("s.click.tmall.com/g?et") > 0)
            {
                url = HttpUtility.UrlDecode(url.Substring(url.IndexOf("tar=") + 4));
                //tps://s.click.tmall.com/g?et=OZ7QZMG2EtTB8TWbd6%2Bmn9BCyGL%2FgFp2&tar=https%3A%2F%2Fdetail.tmall.com%2Fitem.htm%3Fid%3D556715631322%26ali_trackid%3D2%3Amm_56503797_8596089_29498842%3A1506503659_294_1008954814%26sche%3Dliwushuo&op=1
                return url;
            }

            return "";
        }
        private string GetTmallRealUrl(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowAutoRedirect = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            url = response.Headers["Location"];
            response.Close();
            //Uri uri = new Uri(url);
            //url = HttpUtility.UrlDecode(uri.Query);
            //request = WebRequest.Create(url) as HttpWebRequest;
            //request.AllowAutoRedirect = false;
            //request.Referer = uri.ToString();
            //response = request.GetResponse() as HttpWebResponse;
            //url = response.Headers["Location"];
            //response.Close();
            return url;
        }
    }
}
