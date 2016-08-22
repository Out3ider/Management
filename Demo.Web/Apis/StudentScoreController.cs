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
    public class StudentScoreController:BaseController<StudentScore>
    {
        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<StudentScore>(x=> x.Score.Asc() /*&& x.Probability.Asc()*/);

        /// <summary>创建默认where条件，可以被覆盖</summary>
        /// <param name="key">The key.</param>
        /// <returns>Clip.</returns>
        //protected override Clip MakeWhere(string key)
        //{
        //    Clip where = null;
        //    if (key.IsNotNull()) where &= Clip.Where<>(x => x.Like(key));
        //    return where;
        //}
        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where &= Clip.Where<StudentScore>(x => x.Student.Name.Like(key) || x.Student.Number.Like(key));
            return where;
        }

        [HttpGet]
        public AjaxResult GetList(int pageIndex, int pageSize, string key, string course)
        {
            return HandleHelper.TryAction(db =>
            {
                var clip = MakeWhere(key);
               
                //if (county.IsNotNull()) clip &= nameof(Community.County).ToField("") == county;
               if (course.IsNotNull()) clip &= Clip.Where<StudentScore>(x => x.Course.Name.Like(course));
                //clip &= Clip.Where<Community>(x => x.Name == county);
                return db.GetPageList<StudentScore>(pageIndex, pageSize, clip, OrderBy);
            });
        }

    };
        
    
}
