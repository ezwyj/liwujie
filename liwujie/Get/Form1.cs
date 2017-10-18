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
using System.Xml;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Get
{
    public partial class Form1 : Form
    {
        private static PetaPoco.Database db = new PetaPoco.Database("dbConn");
        string appKey = "24635485"; // 可替换为您的沙箱环境应用的AppKey
        string appSecret = "a39690e4352957dc38e4141839f2af66"; // 可替换为您的沙箱环境应用的AppSecret
                                                               //string sessionKey = "test"; // 必须替换为沙箱账号授权得到的真实有效SessionKey

        //var db = new PetaPoco.Database("dbConn");
        //var toGet = db.Fetch<ProductItem>("select * from productitem where EndUrl=''");
        //foreach (var item in toGet)
        //{

        //}
        //db.Dispose();

        string serverUrl = "http://gw.api.taobao.com/router/rest";
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (var itemUrl in GlobalVariable.TargetUrlList)
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
            Debug.WriteLine("download Ok,next GetProductItem");
            GetProductItem4UID();
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


           

            
            var toDownloadPage = db.Fetch<SourcePageEntity>("select * from liewushuosourcepage where Status='Add'");
            foreach(var item in toDownloadPage)
            {
                HttpResponseMessage response = httpClient.GetAsync(item.Page).Result;
                StringBuilder sb = new StringBuilder( response.Content.ReadAsStringAsync().Result);
                sb.Replace("//static.liwushuo.com/", "http://static.liwushuo.com/");
                sb.Replace("require(\"posts/detail\");", "");
                StreamWriter w = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + item.LocalPage);
                w.Write(sb.ToString());
                w.Close();
                item.Status = "Download";
                db.Save(item);
            }
            db.Dispose();
        }

        private void GetProductItem4UID()
        {
            
            var toDownloadPage = db.Fetch<SourcePageEntity>("select * from liewushuosourcepage where Status='Download'");
            foreach (var item in toDownloadPage)
            {
                Analysis analysis = new Analysis(item.LocalPage, item.Page);
            }
            db.Dispose();
        }

        private void BuildNewUrl()
        {
           


            ITopClient client = new DefaultTopClient(serverUrl, appKey, appSecret);
            TbkUatmFavoritesItemGetRequest req = new TbkUatmFavoritesItemGetRequest();
            req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick,shop_title,zk_final_price_wap,click_url,coupon_click_url,tk_rate,status,type";
            req.AdzoneId = 136048077L;
            req.FavoritesId = 12913457L;


            TbkUatmFavoritesItemGetResponse rsp = client.Execute(req);
            //获取指定选品库中的商品列表
            //然后匹配数据库中的商品，如果有，并且EndUrl为空，则替换。
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rsp.Body);
            XmlNode rootNode = xmlDoc.SelectSingleNode("/tbk_uatm_favorites_item_get_response/results");
            foreach (XmlNode xxNode in rootNode.ChildNodes)
            {
                string num_iid = xxNode.SelectSingleNode("num_iid").InnerText;
                string click_url = xxNode.SelectSingleNode("click_url")!=null? xxNode.SelectSingleNode("click_url").InnerText:"";
                string pict_url = xxNode.SelectSingleNode("pict_url").InnerText;
                string price = xxNode.SelectSingleNode("zk_final_price_wap").InnerText;

                string sql = string.Format("update productitem set price={0},EndUrl='{1}',Image='{2}' where TaobaoUID='{3}'", price,click_url,pict_url,num_iid);
                db.Execute(sql);
            }

        }
        

        private void ReplaceProduct()
        {

        }
        /// <summary>
        /// 二次分析来源页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetUID_Click(object sender, EventArgs e)
        {
            
            var toGet = db.Fetch<SourcePageEntity>("select * from liewushuosourcepage where Status='error'");
            foreach (var item in toGet)
            {
                Analysis analysis = new Analysis(item.LocalPage, item.Page);
            }
            db.Dispose();
        }
        private void ShowProduct()
        {
            
            var toSet = db.Fetch<ProductItem>("select * from productitem where state='0' ");
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = toSet;
           
            db.Dispose();
        }
        //显示待显商品
        private void button2_Click(object sender, EventArgs e)
        {
            ShowProduct();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            changeState("2");
            //dataGridView1.Rows.Clear();
            ShowProduct();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            changeState("1");
            //dataGridView1.Rows.Clear();
            ShowProduct();
        }
        /// <summary>
        /// state:1 已查询未生成  state:2 已查询成功生成 
        /// </summary>
        /// <param name="state"></param>
        private void changeState(string state)
        {
            


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["SelIndex"].Value != null)
                {
                    if ((bool)dataGridView1.Rows[i].Cells["SelIndex"].Value == true)
                    {
                        //相应的操作
                        string Id = dataGridView1.Rows[i].Cells["ID"].Value.ToString();
                        db.Execute("update productitem set state='"+state+"' where id= " + Id);

                    }
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((bool)dataGridView1.Rows[e.RowIndex].Cells["SelIndex"].EditedFormattedValue == true)
            {
                dataGridView1.Rows[e.RowIndex].Cells["SelIndex"].Value = false;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Cells["SelIndex"].Value = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BuildNewUrl();


//            get
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ITopClient client = new DefaultTopClient(serverUrl, appKey, appKey);
            //TbkUatmFavoritesGetRequest req = new TbkUatmFavoritesGetRequest();

            //req.Fields = "favorites_title,favorites_id,type";
            //TbkUatmFavoritesGetResponse response = client.Execute(req);

            //Console.WriteLine(response.Body);
        }
    }
}
