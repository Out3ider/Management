using System;
using Sail.Common;

namespace Demo.Web.Models
{
    /// <summary>
    /// 课程信息
    /// </summary>
    public class Course : IModel
    {

        [HColumn(IsPrimary = true, IsGuid = true)]
        public string CourseId { set; get; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [HColumn(Length = 20)]
        public string Name { set; get; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        [HColumn(Length = 20)]
        public string TeacherName { set; get; }


        /// <summary>
        /// 考试日期
        /// </summary>
        [HColumn]
        public DateTime ExamDate { set; get; }

    }
}