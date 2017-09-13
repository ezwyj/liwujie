using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet
{
    public class targetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public string Tag { get; set; }
        public DateTime InputTime { get; set; }
        public string ImgUrl { get; set; }
        public string taobaoUrl { get; set; }

        public string SourceUrl { get; set; }
        public int SourceId { get; set; }
        public string SourceProductId { get; set; }
        
    }
}
