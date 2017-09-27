using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet.entity
{
    [TableName("ProductItem")]
    [PrimaryKey("ID", autoIncrement = true)]
    public class ProductItem
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string TaobaoUrl { get; set; }

        public string TaobaoUID { get; set; }

        public string Image { get; set; }

        public string EndUrl { get; set; }

        public float Price { get; set; } 

        public string Tag { get; set; }

        public string SourcePage { get; set; }

        public string ClassPage { get; set; }

        public DateTime InputTime { get; set; }

        public string Scene { get; set; }
    }

   
}
