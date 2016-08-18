using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.UI.WebControls.Expressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Sail.Common;
using Demo.Web.Models;


// ReSharper disable once CheckNamespace
namespace System.Web.WebPages
{
    /// <summary>
    /// 创建表单必备
    /// </summary>
    public class Element
    {
        /// <summary>
        /// 标签内容
        /// </summary>
        public string Label { set; get; }

        /// <summary>
        /// 控件id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tips { set; get; }

        /// <summary>
        /// 自定义验证
        /// </summary>
        public string ValidateCustom { set; get; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { set; get; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { set; get; }

        /// <summary>
        /// css类
        /// </summary>
        public string Class { set; get; }

        /// <summary>
        /// data参数
        /// </summary>
        public Dictionary<string, string> Data { set; get; }

        /// <summary>
        /// 输出data-*=*
        /// </summary>
        /// <returns></returns>
        public HtmlString RenderData()
        {
            var sb = new StringBuilder();
            if (Data.IsNull()) return new HtmlString("");
            foreach (var data in Data)
            {
                sb.Append($" data-{data.Key}='{data.Value}' ");
            }
            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// 输出id
        /// </summary>
        /// <returns></returns>
        public HtmlString RenderId()
        {
            var element = this;
            return new HtmlString($"id='{element.Id}'");
        }



        /// <summary>
        /// 输出class
        /// </summary>
        /// <param name="isControl"></param>
        /// <returns></returns>
        public IHtmlString RenderClass(bool isControl = true)
        {
            var element = this;
            return new HtmlString($"class='{(isControl ? (" form-control " + element.Class) : element.Class)}'");
        }

        /// <summary>
        /// 输出全部属性
        /// </summary>
        /// <param name="isControl"></param>
        /// <returns></returns>
        public IHtmlString Attr(bool isControl = true)
        {
            var str = $"{RenderId()} {RenderClass(isControl)} {RenderData()} {(IsReadOnly ? "readonly='readonly'" : "")} ";
            return new HtmlString(str);
        }


        /// <summary>
        /// 构造
        /// </summary>
        public Element()
        {
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        /// 构造
        /// </summary>
        public Element(string label, string id) : this()
        {
            Label = label;
            Id = id;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public Element(string label, string id, string className) : this(label, id)
        {
            Class = className;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="label"></param>
        /// <param name="id"></param>
        /// <param name="className"></param>
        /// <param name="tip"></param>
        public Element(string label, string id, string className, string tip) : this(label, id, className)
        {
            Tips = tip;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public Element(string label, string id, bool isrequired, string className) : this(label, id, className)
        {
            IsRequired = isrequired;
        }
 

       

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="expression"></param>
        /// <param name="customlabel"></param>
        /// <returns></returns>
        public static Element Create<TModel>(Expression<Func<TModel, Object>> expression, string customlabel = "")
        {


            if (expression == null) throw (new ArgumentNullException(nameof(expression)));
            MemberExpression operation = null;

            if (expression.Body is MemberExpression)
                operation = (MemberExpression)expression.Body;
            else if (expression.Body is UnaryExpression)
                operation = (MemberExpression)(((UnaryExpression)expression.Body).Operand);

            
            var typeName = expression.Parameters[0].Type.FullName + "_" + expression.Body;

            return typeName.GetCache(() =>
            {
                var prefix = "";
                var preFixLable = "";
                var memberName = operation?.Member.Name;
                var modelType = operation?.Member.DeclaringType;

                if (operation?.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    var ma = (operation.Expression) as MemberExpression;
                    prefix = ma?.Member.Name + ".";
                    var piPrefix = ReflectionExtensions.GetProperties(ma?.Member.DeclaringType)
                            .Find(x => x.Name.Equals(ma?.Member.Name));
                    preFixLable = piPrefix.GetSummary();
                }

                var ps = ReflectionExtensions.GetProperties(modelType);
                var pi = ps.Find(x => x.Name.Equals(memberName));
                var mc = pi.GetAttribute<FormAttribute>();
                var lable = mc?.Label;
                if (lable.IsNull()) lable = pi.GetSummary();
                var result = new Element
                {
                    Label = customlabel.IsNotNull() ? customlabel : (preFixLable + lable),
                    Id = prefix + pi.Name,
                    IsRequired = mc?.IsRequired ?? false,
                    Class = mc?.Class ?? "", 
                    Tips = mc?.Tips ?? "",
                    ValidateCustom = mc?.CustomValidate,
                    IsReadOnly = mc?.IsReadOnly ?? false,
                };
                var validate = new List<string>();
                if (result.IsRequired) validate.Add("required");
                if ((mc?.CustomValidate ?? "").IsNotNull()) validate.Add($"custom[{mc?.CustomValidate}]");
                if (validate.Any()) result.Class += " validate[" + validate.JoinToString() + "]";
                var datas = pi.GetCustomAttributes(typeof(ModelDataAttribute), true).Cast<ModelDataAttribute>().ToList();
                if (datas.Any()) datas.ForEach(x =>
                {
                    result.Data[x.Key] = x.Value;

                });
                return result;
            });
        }
    }

}