using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sail.Common;

namespace Demo.Web.Models
{
    /// <summary>
    /// 字典类接口
    /// </summary>
    public interface IDict: IModel
    {
        int Id { set; get; }

        string Name { set; get; }

        decimal OrderByNo { set; get; }
    }
}
