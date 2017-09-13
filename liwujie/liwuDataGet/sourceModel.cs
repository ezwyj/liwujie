using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet
{
    public class Feature_list
    {
        int id { get; set; }
    }

    public class Label_ids
    {
    }

    public class Items
    {
        public string ad_monitors { get; set; }
        public string approved_at { get; set; }
        public string comments_count { get; set; }
        public string content_type { get; set; }
        public string content_url { get; set; }
        public string cover_image_url { get; set; }
        public string created_at { get; set; }
        public string editor_id { get; set; }

        public string id { get; set; }
        public string introduction { get; set; }

        public string liked { get; set; }
        public string likes_count { get; set; }
        public string limit_end_at { get; set; }
        public string media_type { get; set; }
        public long published_at { get; set; }
        public string share_msg { get; set; }
        public string shares_count { get; set; }
        public string short_title { get; set; }
        public string status { get; set; }
        public string template { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public long updated_at { get; set; }
        public string url { get; set; }
    }

    public class Data
    {
        public List<Items> items { get; set; }
    }

    public class liwuModel
    {
        public string code { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }
}
