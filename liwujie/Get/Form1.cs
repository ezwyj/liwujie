using Get.Entity;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            Console.WriteLine("ok");
        }

        private void GetList(KeyValuePair<string,string> url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");


            HttpResponseMessage response = httpClient.GetAsync(url.Key).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            var liwuData = JsonConvert.DeserializeObject<SourcePageList>(result);

            if (url.Key)
            { }

                foreach (var item in liwuData.data.items)
                {
                    string postUrl = item.url;
                    Console.WriteLine(postUrl);
                    

                //数据库处理
                    var checkResult = SourcePageEntity.GetListByProperty(a => a.Page, postUrl);
                    if (checkResult.Count == 0)
                    {
                        SourcePageEntity newPage = new Entity.SourcePageEntity();
                        newPage.cover_image_url = item.cover_image_url;
                        newPage.Status = "Add";
                        newPage.Title = item.title;
                        newPage.LocalPage = "Page_"+item.id + ".html";
                        newPage.Target = 
                    }
                    else
                    {
                        SourcePageEntity newPage
                    }
                }
           
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
