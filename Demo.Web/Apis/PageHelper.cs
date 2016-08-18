using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Sail.Common;
using Sail.Web;


namespace System.Web.WebPages
{
    /// <summary>
    /// 页面帮助方法
    /// </summary>
    public static class PageHelper
    {
        /// <summary>
        /// 菜单id
        /// </summary>
        public const string ItemId = nameof(ItemId);
        /// <summary>
        /// 页面标题
        /// </summary>
        public const string PageTitle = nameof(PageTitle);
        /// <summary>
        /// 页面副标题
        /// </summary>
        public const string PageTips = nameof(PageTips);
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public const string CurrentUser = nameof(CurrentUser);



        /// <summary>
        /// 设定页面菜单、标题什么的
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemId"></param>
        /// <param name="title"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        public static IUser SetPage(this WebPageBase page, string itemId, string title, string tips)
        {
            page.PageData[ItemId] = itemId;
            page.PageData[PageTitle] = title;
            page.PageData[PageTips] = tips;
            return WebHelper.CurrentUser;
        }

        private static TagBuilder BuilderOption(string text, string value)
        {
            var tag = new TagBuilder("option");
            tag.SetInnerText(text);
            tag.MergeAttribute("value", value);
            return tag;
        }

        /// <summary>
        /// 输出select的options
        /// </summary>
        /// <param name="items"></param>
        /// <param name="defaultText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static HtmlString RenderOptions(this IList<KeyValuePair<string, string>> items, string defaultText = "", string defaultValue = "")
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(defaultText)) sb.Append(BuilderOption(defaultText, defaultValue));
            if (items.IsNotNull()) items.ForEach(item => { sb.Append(BuilderOption(item.Value, item.Key).ToString()); });
            return new HtmlString(sb.ToString());
        }


        /// <summary>
        /// 生成枚举的列表数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> EnumList(this Type type)
        {
            return type.GetEnumItems().OrderBy(x => x.Key).Select(x => new KeyValuePair<string, string>(x.Key.ToString(), x.Value)).ToList();
        }

        /// <summary>
        /// 输出枚举到options
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static HtmlString RenderEnum(this Type type, string defaultText = "", string defaultValue = "")
        {
            return
                type.GetEnumItems()
                    .OrderBy(x => x.Key)
                    .Select(x => new KeyValuePair<string, string>(x.Key.ToString(), x.Value))
                    .ToList()
                    .RenderOptions(defaultText, defaultValue);
        }


        /// <summary>
        /// 生成数字
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static HtmlString NumberTo(this int start, int end)
        {
            var items = new List<KeyValuePair<string, string>>();
            for (var i = start; i <= end; i++)
            {
                items.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString()));
            }
            return items.RenderOptions();
        }


    }

}