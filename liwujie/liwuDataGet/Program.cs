using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace liwuDataGet
{
    class Program
    {
      
        static void Main(string[] args)
        {
            //
            Task.Factory.StartNew(CheckOutContent1);

            //Task.Factory.StartNew(CheckOutContent34);



            Console.ReadLine();
        }

        static void CheckOutContent1()
        {

            GetLeiwuPage analysisChannels1 = new GetLeiwuPage();
            analysisChannels1.Start("http://www.liwushuo.com/api/channels/1/items?limit=50");
        }
        static void CheckOutContent34()
        {
            GetLeiwuPage analysisChannels34 = new GetLeiwuPage();
            analysisChannels34.Start("http://www.liwushuo.com/api/channels/34/items?limit=50");
        }




    }
}
