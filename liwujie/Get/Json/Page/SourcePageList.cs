using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.Json.Page
{
    public class Items
    {
        public string comments_count { get; set; }
        public string cover_image_url { get; set; }
        public string id { get; set; }
        public string liked { get; set; }
        public string likes_count { get; set; }
        public string share_msg { get; set; }
        public string shares_count { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Data
    {
        public List<Items> items { get; set; }
    }

    public class SourcePageList
    {
        public string code { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }
}
