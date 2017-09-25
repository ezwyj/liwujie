using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet.entity
{
    public class ProductItem
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string TaobaoUrl { get; set; }

        public string TaobaoUID { get; set; }

        public float Price { get; set; }
    }
}
