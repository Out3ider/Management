using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sail.Common;

namespace Demo.Web.Models
{
    public class DictBaseController : IDict
    {
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }

        /// <summary>
        /// 县区名称
        /// </summary>
        [HColumn(Length = 50, IsUniqueness = true, Remark = "名称")]
        public string Name { set; get; }

        /// <summary>
        /// 排序号
        /// </summary>
        [HColumn(Length = 14, Precision = 2)]
        public decimal OrderByNo { set; get; }

    }
}