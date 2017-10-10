using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get
{
    public static class GlobalVariable
    {
        public static List<KeyValuePair<string,string>> TargetUrlList { get; set; }
        public static List<KeyValuePair<string, string>> SceneUrlList { get; set; }
        public static List<KeyValuePair<string, string>> PersonalityUrlList { get; set; }


        public static List<string> PostList { get; set; }
    }
}
