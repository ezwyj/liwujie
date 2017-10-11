using Get.Entity;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Get
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach(var itemUrl in GlobalVariable.TargetUrlList)
            {
                GetList(itemUrl);
            }
            foreach (var itemUrl in GlobalVariable.SceneUrlList)
            {
                GetList(itemUrl);
            }
            foreach (var itemUrl in GlobalVariable.PersonalityUrlList)
            {
                GetList(itemUrl);
            }
            Debug.WriteLine("Get url OK,next download");
            DownloadPage();
        }

        private void GetList(KeyValuePair<string,string> url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");


            HttpResponseMessage response = httpClient.GetAsync(url.Key).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            var liwuData = JsonConvert.DeserializeObject<Json.Page.SourcePageList>(result);

           

            string target=string.Empty, personality=string.Empty, scene=string.Empty;
            if (url.Key.IndexOf("target_id=") > 0)
            {
                target = url.Value;
            }
            if (url.Key.IndexOf("scene_id=") > 0)
            {
                scene = url.Value;
            }
            if (url.Key.IndexOf("personality_id=") > 0)
            {
                personality = url.Value;
            }
            Debug.WriteLine("target:{0},scene:{1},personality:{2}", target, scene, personality);

            var db = new PetaPoco.Database("dbConn");
            foreach (var item in liwuData.data.items)
            {
                string postUrl = item.url;
                Debug.WriteLine(postUrl);
                

                //数据库处理
                var checkResult = db.Fetch<SourcePageEntity>("Select * from liewushuosourcepage where page='" + postUrl + "'");
                if (checkResult.Count()==0)
                {
                    SourcePageEntity newPage = new Entity.SourcePageEntity();
                    newPage.cover_image_url = item.cover_image_url;
                    newPage.Status = "Add";
                    newPage.Title = item.title;
                    newPage.LocalPage = "Page_"+item.id + ".html";
                    newPage.Target = target;
                    newPage.Personality = personality;
                    newPage.Scene = scene;
                    newPage.Page = postUrl;
                    db.Save(newPage);
                }
                else
                {
                    SourcePageEntity oldPage = checkResult.First();
                    oldPage.Target = oldPage.Target + " " + target;
                    oldPage.Personality = oldPage.Personality + " " + personality;
                    oldPage.Scene = oldPage.Scene + " " + scene;
                    db.Save(oldPage);
                }
            }
            db.Dispose();
           
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DownloadPage()
        {

            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");


           

            var db = new PetaPoco.Database("dbConn");
            var toDownloadPage = db.Fetch<SourcePageEntity>("select * from liewushuosourcepage where Status='Add'");
            foreach(var item in toDownloadPage)
            {
                HttpResponseMessage response = httpClient.GetAsync(item.Page).Result;
                string html = response.Content.ReadAsStringAsync().Result;
                
                StreamWriter w = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + item.LocalPage);
                w.Write(html);
                w.Close();
                item.Status = "Download";
                db.Save(item);
            }
            db.Dispose();
        }

        private void GetProductItem()
        {
            var db = new PetaPoco.Database("dbConn");
            var toDownloadPage = db.Fetch<SourcePageEntity>("select * from liewushuosourcepage where Status='Download'");
            foreach (var item in toDownloadPage)
            {
                
                StreamWriter w = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + item.LocalPage);
                
                item.Status = "GetProductItem";
                db.Save(item);
            }
            db.Dispose();
        }

        private void ReplaceProduct()
        {

        }
    }
}
