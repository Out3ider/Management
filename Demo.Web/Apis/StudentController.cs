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
    public class StudentController : BaseController<Student>
    {
        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<Student>(x => x.Number.Asc() /*&& x.Probability.Asc()*/);

        /// <summary>
        /// 创建默认where条件，可以被覆盖
        /// </summary>
        //<param name = "key" > The key.</param>
        //<returns>Clip.</returns>
        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where &= Clip.Where<Student>(x => x.Name.Like(key) || x.Number.Like(key));

            return where;
        }
        [HttpGet]
        public AjaxResult GetList(int pageIndex, int pageSize, string key, string grade, string gender)
        {
            return HandleHelper.TryAction(db =>
            {
                var clip = MakeWhere(key);
                var sgender = gender.ToInt();
                var sgrade = grade.ToInt();     
                //if (county.IsNotNull()) clip &= nameof(Community.County).ToField("") == county;
                if (gender.IsNotNull()) clip &= Clip.Where<Student>(x => x.Gender == (Gender)sgender);
                if (grade.IsNotNull()) clip &= Clip.Where<Student>(x => x.Grade == (Grade)sgrade);
                //clip &= Clip.Where<Community>(x => x.Name == county);
                return db.GetPageList<Student>(pageIndex, pageSize, clip, OrderBy);
            });
        }
        public static List<KeyValuePair<string,string>> GetAll()
        {
            using (var db = new DataContext())
            {
                return db.GetList<Student>()
                    .OrderBy(x => x.Number)
                    .Select(x=>new KeyValuePair<string,string>(x.StudentId,x.Name))
                    .ToList();
            }

        }

    };


}
