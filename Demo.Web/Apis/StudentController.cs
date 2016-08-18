using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Demo.Web.Models;
using Sail.Common;

namespace Demo.Web.Apis
{
    public class StudentController:BaseController<Student>
    {
        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<Student>(x => x.Number.Asc() /*&& x.Probability.Asc()*/);

        /// <summary>创建默认where条件，可以被覆盖</summary>
        /// <param name="key">The key.</param>
        /// <returns>Clip.</returns>
        //protected override Clip MakeWhere(string key)
        //{
        //    Clip where = null;
        //    if (key.IsNotNull()) where &= Clip.Where<>(x => x.Like(key));
        //    return where;
        //}

    };
        
    
}
