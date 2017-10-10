using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liwuDataGet.entity
{
    [TableName("LiewushuoSourcePage")]
    [PrimaryKey("ID", autoIncrement = true)]
    public class SourcePage
    {
        public int ID { get; set; }
        public string Taget { get; set; }
        public string Scene { get; set; }
        public string Personality { get; set; }
        public string Page { get; set; }
        public string LocalPage { get; set; }
        public string Status { get; set; }
    }
}
