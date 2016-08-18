using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using Sail.Common;
using Sail.Web;

namespace Demo.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 开始请求时更新Session时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            SailSession.Session.UpdateTime();
        }

        /// <summary>
        /// 注册webapi路由、bundle、初始化数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseScheme.InitDatabase();


        }
    }
}
