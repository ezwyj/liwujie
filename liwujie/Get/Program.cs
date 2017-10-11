using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Get
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            GlobalVariable.TargetUrlList = new List<KeyValuePair<string, string>>();
            GlobalVariable.SceneUrlList = new List<KeyValuePair<string, string>>();
            GlobalVariable.PersonalityUrlList = new List<KeyValuePair<string, string>>();
            GlobalVariable.PostList = new List<string>();

            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=34&limit=33", "自己")); //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=9&limit=33", "男票"));  //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=10&limit=33", "女票"));  //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=5&limit=33", "闺蜜"));   //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=26&limit=33", "基友"));  //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=6&limit=33", "爸妈"));   //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=24&limit=33", "小朋友"));  //
            GlobalVariable.TargetUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?target_id=17&limit=33", "同事"));  //

            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=39&limit=33", "新年"));//
            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=30&limit=33", "生日"));//
            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=32&limit=33", "情人节"));//
            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=31&limit=33", "纪念日"));//
            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=33&limit=33", "结婚"));//
            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=36&limit=33", "乔迁"));//
            GlobalVariable.SceneUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?scene_id=40&limit=33", "圣诞节"));//

            GlobalVariable.PersonalityUrlList .Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=2&limit=33", "美物"));//
            GlobalVariable.PersonalityUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=3&limit=33", "手工"));//
            GlobalVariable.PersonalityUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=27&limit=33", "吃货"));//
            GlobalVariable.PersonalityUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=11&limit=33", "萌萌哒"));//
            GlobalVariable.PersonalityUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=29&limit=33", "动漫迷"));//
            GlobalVariable.PersonalityUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=14&limit=33", "小清新"));//
            GlobalVariable.PersonalityUrlList.Add(new KeyValuePair<string, string>("http://www.liwushuo.com/api/search/post_by_type?personality_id=28&limit=33", "科技范"));//
            Application.Run(new Form1());
        }
    }
}
