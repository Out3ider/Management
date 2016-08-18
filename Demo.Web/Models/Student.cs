using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Sail.Common;

namespace Demo.Web.Models
{
    /// <summary>
    /// 年级
    /// </summary>
    public enum Grade
    {
        大一,
        大二,
        大三,
        大四,
    }
    
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        男,
        女
    }

    /// <summary>
    /// 学生信息
    /// </summary>
    public class Student : IModel
    {
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string StudentId { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        [HColumn(Length = 20)]
        public string Name { set; get; }

        /// <summary>
        /// 学号
        /// </summary>
        [HColumn(Length = 20)]
        public string Number { set; get; }

        /// <summary>
        /// 年级
        /// </summary>
        [HColumn]
        public Grade Grade { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string GradeStr => Grade.ToString();
        /// <summary>
        /// 性别
        /// </summary>
        [HColumn]
        public Gender Gender { set; get; }
        public string GenderStr => Gender.ToString();
        /// <summary>
        /// 年龄
        /// </summary>
        [HColumn]
        public int Age { set; get; }

    }
}