using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using Demo.Web.Models;
using Sail.Common;

namespace Demo.Web.Apis
{
    public class CourseController : BaseController<Course>
    {

        /// <summary>
        /// ≈≈–Ú
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<Course>(x => x.ExamDate.Asc() /*&& x.Probability.Asc()*/);

        protected override Clip MakeWhere(string key)
        {
            Clip where = null;
            if (key.IsNotNull()) where &= Clip.Where<Course>(x => x.Name.Like(key) || x.TeacherName.Like(key));
            return where;
        }
        public static List<KeyValuePair<string, string>> GetAll()
        {
            using (var db = new DataContext())
            {
                return db.GetList<Course>() 
                    .OrderBy(x => x.Name)
                    .Select(x => new KeyValuePair<string, string>(x.CourseId, x.Name))
                    .ToList();
            }

        }
    }
}