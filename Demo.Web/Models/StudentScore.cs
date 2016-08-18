using Sail.Common;

namespace Demo.Web.Models
{
    /// <summary>
    /// 学生成绩
    /// </summary>
    public class StudentScore : IModel
    {
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string ScoreId { set; get; }

        /// <summary>
        /// 学生
        /// </summary>
        [HColumn]
        public Student Student { set; get; }

        /// <summary>
        /// 课程
        /// </summary>
        [HColumn]
        public Course Course { set; get; }

        /// <summary>
        /// 成绩
        /// </summary>
        [HColumn]
        public int Score { set; get; }
    }
}