using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet.entity
{
    




    public class Cover_image
    {
        public string width { get; set; }
        public string height { get; set; }
        public string url { get; set; }
    }

    public class Images
    {
        public string width { get; set; }
        public string height { get; set; }
        public string url { get; set; }
    }

    public class Title
    {
        public string index { get; set; }
        public string content { get; set; }
    }

    public class Items
    {
        public string brand_id { get; set; }
        public string brand_order { get; set; }
        public string category_id { get; set; }

        public string commission_rate { get; set; }
        public string cover_image_url { get; set; }
        public string created_at { get; set; }
        public string desc_pinyin { get; set; }
        public string description { get; set; }
        public string editor_id { get; set; }
        public string favorites_count { get; set; }
        public string id { get; set; }

        public string in_rank { get; set; }
        public string keywords { get; set; }
        public string keywords_pinyin { get; set; }
        public string likes_count { get; set; }
        public string name { get; set; }
        public string name_pinyin { get; set; }
        public string post_ids { get; set; }
        public string price { get; set; }
        public string price_str { get; set; }
        public string purchase_id { get; set; }
        public string purchase_shop_id { get; set; }
        public string purchase_status { get; set; }
        public string purchase_type { get; set; }
        public string purchase_url { get; set; }
        public string selection { get; set; }
        public string shop_title { get; set; }
        public string short_description { get; set; }
        public string short_description_ik { get; set; }
        public string short_description_pinyin { get; set; }
        public string subcategory_id { get; set; }
        public string updated_at { get; set; }
        public string url { get; set; }
        public string weight { get; set; }
        public string shop_type { get; set; }
        public string item_id { get; set; }
        public string template { get; set; }
        public Cover_image cover_image { get; set; }
        public List<Images> images { get; set; }
        public Title title { get; set; }

    }

    public class entityOne
    {
        public List<Items> items { get; set; }
    }
}
