using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using Sail.Common;
using Sail.Web;

namespace Demo.Web.Apis
{
    public class UserController : ApiController
    {
        public static List<HMenuItem> LoadMenu()
        {
            // var user = WebHelper.CheckUser<User>();
            var menutext = FileHelper.ReadTextFile("~/config/menu.json".FullFileName());
            var list = menutext.JsonToObj<List<HMenuItem>>().ToList();
            //HMenuItem.Filter(list, "", null);
            return list;
        }
    }
}
