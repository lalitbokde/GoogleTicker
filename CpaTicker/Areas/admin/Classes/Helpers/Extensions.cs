using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Return a SelectList from an Enum object. Convert an enum object into a SelectList ready
        /// to create a Dropdownlist list. This feature is used in the card type ddl.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            //foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
            //{
            //    var field = item.GetType().GetField(item.ToString());
            //    var display = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            //}

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.DisplayValue() }; //EnumHelper<TEnum>.GetDisplayValue(e)
            return new SelectList(values, "Id", "Name", enumObj);
        }

        public static string DisplayValue<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            try
            {
                var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

                // display attributes
                var displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (displayAttributes != null && displayAttributes.Length > 0)
                    return displayAttributes[0].Name;

                // description attributes
                var description = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                if (description != null)
                {
                    return (description as DescriptionAttribute).Description;
                }

                return enumObj.ToString();
            }
            catch
            { }

            return string.Empty;
        }



        public static string ExternalLink(this HtmlHelper helper, string URI, string label)
        {
            return string.Format("<a href='{0}'>{1}</a>", URI, label);
        }

        public static MvcHtmlString Hyperlink(this HtmlHelper helper, string url, string linkText)
        {
            return MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>", url, linkText));
        }

        public static T GetValue<T>(this NameValueCollection collection, string key)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            var value = collection[key];

            if (value == null)
            {
                throw new ArgumentOutOfRangeException("key");
            }

            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new ArgumentException(String.Format("Cannot convert '{0}' to {1}", value, typeof(T)));
            }

            return (T)converter.ConvertFrom(value);
        }

        /// <summary>
        /// Convert an Enum to checkbox
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="htmlhelper"></param>
        /// <param name="name">Name of the checkbox property</param>
        /// <param name="modelItems"></param>
        /// <returns></returns>
        //public static IHtmlString CheckboxListForEnum<T>(this HtmlHelper html, string name, T modelItems) where T : struct
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (T item in Enum.GetValues(typeof(T)).Cast<T>())
        //    {
        //        TagBuilder builder = new TagBuilder("input");
        //        long targetValue = Convert.ToInt64(item);
        //        long flagValue = Convert.ToInt64(modelItems);

        //        if ((targetValue & flagValue) == targetValue)
        //            builder.MergeAttribute("checked", "checked");

        //        builder.MergeAttribute("type", "checkbox");
        //        builder.MergeAttribute("value", item.ToString());
        //        builder.MergeAttribute("name", name);
        //        builder.InnerHtml = item.ToString();

        //        sb.Append(builder.ToString(TagRenderMode.Normal));
        //    }

        //    return new HtmlString(sb.ToString());
        //}

        public static IHtmlString CheckboxListForEnum<T>(this HtmlHelper htmlhelper, string name, T modelItems) where T : struct
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder aux = new StringBuilder();
            var penum = modelItems as Enum;

            foreach (T item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var ti = htmlhelper.ViewData.TemplateInfo;
                var id = ti.GetFullHtmlFieldId(item.ToString());

                var builder = new TagBuilder("input");
                if (penum.HasFlag(item as Enum))
                    builder.MergeAttribute("checked", "checked");
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", item.ToString());
                builder.MergeAttribute("name", name);
                sb.AppendLine(builder.ToString(TagRenderMode.SelfClosing));

                var label = new TagBuilder("label");
                label.Attributes["for"] = id;
                label.SetInnerText(item.ToString());
                sb.AppendLine(label.ToString());

                var li = new TagBuilder("li");
                li.InnerHtml = sb.ToString();
                sb.Clear();
                aux.AppendLine(li.ToString(TagRenderMode.Normal));
            }

            var ol = new TagBuilder("ol");
            ol.InnerHtml = aux.ToString();

            return new HtmlString(ol.ToString(TagRenderMode.Normal));
        }

        // this is the one that i was using before apply dynamic permissions
        //public static IHtmlString CustomCheckboxListForEnum<T>(this HtmlHelper htmlhelper, string name, T modelItems, bool isAffiliate) where T : struct
        //{
        //    StringBuilder sb = new StringBuilder();
        //    StringBuilder aux = new StringBuilder();
        //    var penum = modelItems as Enum;
        //    ViewPermission affiliatep = CpaTickerConfiguration.DefaultAffiliateRestrictions;

        //    foreach (T item in Enum.GetValues(typeof(T)).Cast<T>())
        //    {
        //        // if is affiliate and i'm in a affiliate restriccion not put in so
        //        // negate the before condition for continue
        //        if (!(isAffiliate && affiliatep.HasFlag(item as Enum)))
        //        {
        //            var ti = htmlhelper.ViewData.TemplateInfo;
        //            var id = ti.GetFullHtmlFieldId(item.ToString());

        //            var builder = new TagBuilder("input");
        //            if (penum.HasFlag(item as Enum))
        //                builder.MergeAttribute("checked", "checked");
        //            builder.MergeAttribute("type", "checkbox");
        //            builder.MergeAttribute("value", item.ToString());
        //            builder.MergeAttribute("name", name);
        //            sb.AppendLine(builder.ToString(TagRenderMode.SelfClosing));

        //            var label = new TagBuilder("label");
        //            label.Attributes["for"] = id;

        //            // this is for grab the display attribute
        //            var field = item.GetType().GetField(item.ToString());
        //            var display = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
        //            if (display == null)
        //                label.SetInnerText(item.ToString());
        //            else
        //                label.SetInnerText(display.Name);
        //            sb.AppendLine(label.ToString());

        //            var li = new TagBuilder("li");
        //            li.InnerHtml = sb.ToString();
        //            sb.Clear();
        //            aux.AppendLine(li.ToString(TagRenderMode.Normal));
        //        }
        //    }

        //    var ol = new TagBuilder("ol");
        //    ol.InnerHtml = aux.ToString();

        //    return new HtmlString(ol.ToString(TagRenderMode.Normal));
        //}


        // this is the one that i actually using ******
        public static IHtmlString CustomCheckboxListForDynamicEnum(this HtmlHelper htmlhelper, string name, long modelItems, bool isAffiliate)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder aux = new StringBuilder();
            //var penum = modelItems as Enum;
            //ViewPermission affiliatep = CpaTickerConfiguration.DefaultAffiliateRestrictions;
            long affiliatep = CpaTickerConfiguration.DefaultAffiliateDynamicRestrictions;

            //get the enum from the assembly
            Type enumtype = CPAHelper.GetEnumType();

            foreach (object item in Enum.GetValues(enumtype))
            {
                long targetValue = Convert.ToInt64(item);

                // if is affiliate and i'm in a affiliate restriccion not put in so
                // negate the before condition for continue
                //if (!(isAffiliate && affiliatep.HasFlag(item as Enum)))
                if (!(isAffiliate && (affiliatep & targetValue) == targetValue))
                {
                    var ti = htmlhelper.ViewData.TemplateInfo;
                    var id = ti.GetFullHtmlFieldId(item.ToString());

                    var builder = new TagBuilder("input");

                    //if (penum.HasFlag(item as Enum))
                    if ((targetValue & modelItems) == targetValue)
                        builder.MergeAttribute("checked", "checked");

                    builder.MergeAttribute("type", "checkbox");
                    builder.MergeAttribute("value", item.ToString());
                    builder.MergeAttribute("name", name);
                    sb.AppendLine(builder.ToString(TagRenderMode.SelfClosing));

                    var label = new TagBuilder("label");
                    label.Attributes["for"] = id;

                    // this is for grab the display attribute
                    var field = item.GetType().GetField(item.ToString());
                    var display = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
                    if (display == null)
                        label.SetInnerText(item.ToString());
                    else
                        label.SetInnerText(display.Name);
                    sb.AppendLine(label.ToString());

                    var li = new TagBuilder("li");
                    li.InnerHtml = sb.ToString();
                    sb.Clear();
                    aux.AppendLine(li.ToString(TagRenderMode.Normal));
                }
            }

            var ol = new TagBuilder("ol");
            ol.InnerHtml = aux.ToString();

            return new HtmlString(ol.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString CheckboxListForEnumBootStrap<T>(this HtmlHelper htmlhelper, string name, T modelItems) where T : struct
        {
            StringBuilder sb = new StringBuilder();
            var penum = modelItems as Enum;

            foreach (T item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var ti = htmlhelper.ViewData.TemplateInfo;
                var id = ti.GetFullHtmlFieldId(item.ToString());

                var builder = new TagBuilder("input");
                if (penum.HasFlag(item as Enum))
                    builder.MergeAttribute("checked", "checked");
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", item.ToString());
                builder.MergeAttribute("name", name);

                var label = new TagBuilder("label");
                label.Attributes["class"] = "checkbox";

                // this is for grab the display attribute
                var field = item.GetType().GetField(item.ToString());
                var display = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
                if (display == null)
                    label.SetInnerText(item.ToString());
                else
                    label.SetInnerText(display.Name);

                // set the builder iside the label
                label.InnerHtml = builder.ToString(TagRenderMode.SelfClosing) + "<i></i>" + item.ToString();
                sb.AppendLine(label.ToString());
            }

            return new HtmlString(sb.ToString());
        }

        public static IHtmlString CustomCheckboxListForDynamicEnumBootStrap(this HtmlHelper htmlhelper, string name, long modelItems, bool isAffiliate, string enumtypeName)
        {

            EFCpatickerRepository repo = new EFCpatickerRepository();

            var ListPermissionsView = repo.GetPermissionNames();
            var ListPermissionsView1 = repo.GetPermissionNames1();
            StringBuilder sb = new StringBuilder();
            //StringBuilder aux = new StringBuilder();
            //var penum = modelItems as Enum;
            //ViewPermission affiliatep = CpaTickerConfiguration.DefaultAffiliateRestrictions;
            long affiliatep = CpaTickerConfiguration.DefaultAffiliateDynamicRestrictionsCopy(enumtypeName);

            //get the enum from the assembly
            //Type enumtype = CPAHelper.GetEnumType();
            var assembly = System.Reflection.Assembly.Load("EnumAssembly");
            Type enumtype = assembly.GetType(enumtypeName);

            foreach (object item in Enum.GetValues(enumtype))
            {
                long targetValue = Convert.ToInt64(item);

                // if is affiliate and i'm in a affiliate restriccion not put in so
                // negate the before condition for continue
                //if (!(isAffiliate && affiliatep.HasFlag(item as Enum)))
                if (!(isAffiliate && (affiliatep & targetValue) == targetValue))
                {
                    var ti = htmlhelper.ViewData.TemplateInfo;
                    var id = ti.GetFullHtmlFieldId(item.ToString());

                    // builder is the input type=checkbox
                    var builder = new TagBuilder("input");

                    //if (penum.HasFlag(item as Enum))
                    if ((targetValue & modelItems) == targetValue)
                        builder.MergeAttribute("checked", "checked");

                    builder.MergeAttribute("type", "checkbox");
                    builder.MergeAttribute("value", item.ToString());
                    builder.MergeAttribute("name", name);

                    //sb.AppendLine(builder.ToString(TagRenderMode.SelfClosing));

                    //// label

                    var label = new TagBuilder("label");
                    //label.Attributes["for"] = id; bootstrap doesn't function with for attrib
                    label.Attributes["class"] = "checkbox";

                    // this is for grab the display attribute
                    var field = item.GetType().GetField(item.ToString());
                    var display = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
                    if (display == null)
                        label.SetInnerText(item.ToString());
                    else
                        label.SetInnerText(display.Name);


                    string htmldispName = "";
                    string itemname = item.ToString();
                    if (name == "views") { htmldispName = ListPermissionsView.Where(u => u.value == itemname).Select(u => u.name).FirstOrDefault(); }
                    else if (name == "views1") { htmldispName = ListPermissionsView1.Where(u => u.value == itemname).Select(u => u.name).FirstOrDefault(); }

                    if(String.IsNullOrEmpty(htmldispName))
                    { htmldispName = itemname; }
                    //// set the builder iside the label
                    //label.InnerHtml = builder.ToString(TagRenderMode.SelfClosing) + "<i></i>" +  item.ToString();
                    label.InnerHtml = builder.ToString(TagRenderMode.SelfClosing) + "<i></i>" + htmldispName;

                    sb.AppendLine(label.ToString());

                }
            }

            return new HtmlString(sb.ToString());
        }

        public static bool checkpermissions(long modelItems, bool isAffiliate, string enumtypeName, string pagename)
        {

            //ViewPermission affiliatep = CpaTickerConfiguration.DefaultAffiliateRestrictions;
           // long affiliatep = CpaTickerConfiguration.DefaultAffiliateDynamicRestrictions;
            long affiliatep = CpaTickerConfiguration.DefaultAffiliateDynamicRestrictionsCopy(enumtypeName);

            //get the enum from the assembly
            //Type enumtype = CPAHelper.GetEnumType();
            var assembly = System.Reflection.Assembly.Load("EnumAssembly");
            Type enumtype = assembly.GetType(enumtypeName);

            System.Reflection.FieldInfo enumItem = enumtype.GetField(pagename);
            long targetValue = (long)enumItem.GetValue(enumtype);

           // long targetValue = Convert.ToInt64(pagename);

            // if is affiliate and i'm in a affiliate restriccion not put in so
            // negate the before condition for continue
            //if (!(isAffiliate && affiliatep.HasFlag(item as Enum)))
            //if (!(isAffiliate && (affiliatep & targetValue) == targetValue))
            //{

            //if (penum.HasFlag(item as Enum))
            //if(isAffiliate)
            //{
            //if (!(isAffiliate && (affiliatep & targetValue) == targetValue))
            //{
            //    return false;
            //}
            if ((targetValue & modelItems) == targetValue)
            {
                return false;
            }
            if (!(isAffiliate && (affiliatep & targetValue) == targetValue))
            {
                return true;
            }
            else
            { return false; }
            //}


            //}
            return true;
        }

        private static void temp<T>(T enumtype) where T : struct
        {
            foreach (T item in Enum.GetValues(typeof(T)).Cast<T>())
            {

            }
        }

        public static MvcHtmlString CheckBoxSmart(this HtmlHelper helper, string name, string text, bool check)
        {
            var sb = new StringBuilder();

            sb.Append("<label class=\"checkbox\">");

            var checkbox = new TagBuilder("input");
            checkbox.Attributes["id"] = name;
            checkbox.Attributes["name"] = name;
            checkbox.Attributes["type"] = "checkbox";
            if (check)
            {
                checkbox.Attributes["checked"] = "checked";
            }

            sb.Append(checkbox.ToString(TagRenderMode.SelfClosing));
            sb.AppendFormat("<i></i>{0}</label>", text);

            return MvcHtmlString.Create(sb.ToString());
        }

        public static IHtmlString CheckBoxSmart(this HtmlHelper helper, string name, string text, bool check, object htmlAttributes)
        {
            var sb = new StringBuilder();

            sb.Append("<label class=\"checkbox\">");

            var checkbox = new TagBuilder("input");

            checkbox.Attributes["id"] = name;
            checkbox.Attributes["name"] = name;
            checkbox.Attributes["type"] = "checkbox";
            //checkbox.Attributes["value"] = "13"; why th' fuck is this not working ??

            if (check)
            {
                checkbox.Attributes["checked"] = "checked";
            }
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            foreach (var key in attrs.Keys)
            {
                checkbox.Attributes.Add(key, attrs[key].ToString());
            }



            sb.Append(checkbox.ToString(TagRenderMode.SelfClosing));
            sb.AppendFormat("<i></i>{0}</label>", text);

            return new HtmlString(sb.ToString());
            //return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioBoxSmart(this HtmlHelper helper, string name, string text, string value, bool check)
        {
            var sb = new StringBuilder();

            sb.Append("<label class=\"radio\">");

            var checkbox = new TagBuilder("input");
            checkbox.Attributes["name"] = name;
            checkbox.Attributes["value"] = value;
            checkbox.Attributes["type"] = "radio";
            if (check)
            {
                checkbox.Attributes["checked"] = "checked";
            }

            sb.Append(checkbox.ToString(TagRenderMode.SelfClosing));
            sb.AppendFormat("<i></i>{0}</label>", text);

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString CheckBoxSmart(this HtmlHelper helper, string name, string text, string value, bool check, IDictionary<string, string> htmlAttributes = null)
        {
            var sb = new StringBuilder();

            sb.Append("<label class=\"checkbox\">");

            var checkbox = new TagBuilder("input");
            checkbox.Attributes["value"] = value;
            checkbox.Attributes["name"] = name;
            checkbox.Attributes["type"] = "checkbox";
            if (check)
            {
                checkbox.Attributes["checked"] = "checked";
            }

            if (htmlAttributes != null)
            {
                foreach (var key in htmlAttributes.Keys)
                {
                    checkbox.Attributes[key] = htmlAttributes[key];
                }
            }


            sb.Append(checkbox.ToString(TagRenderMode.SelfClosing));
            sb.AppendFormat("<i></i>{0}</label>", text);

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// TModel must be an Enum otherwise exception. Convert enum to checkbox
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxesForEnumModel<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            if (!typeof(TModel).IsEnum)
            {
                throw new ArgumentException("this helper can only be used with enums");
            }
            var sb = new StringBuilder();
            foreach (Enum item in Enum.GetValues(typeof(TModel)))
            {
                var ti = htmlHelper.ViewData.TemplateInfo;
                var id = ti.GetFullHtmlFieldId(item.ToString());
                var name = ti.GetFullHtmlFieldName(string.Empty);
                var label = new TagBuilder("label");
                label.Attributes["for"] = id;
                label.SetInnerText(item.ToString());
                sb.AppendLine(label.ToString());

                var checkbox = new TagBuilder("input");
                checkbox.Attributes["id"] = id;
                checkbox.Attributes["name"] = name;
                checkbox.Attributes["type"] = "checkbox";
                checkbox.Attributes["value"] = item.ToString();
                var model = htmlHelper.ViewData.Model as Enum;
                if (model.HasFlag(item))
                {
                    checkbox.Attributes["checked"] = "checked";
                }
                sb.AppendLine(checkbox.ToString());
            }

            return new HtmlString(sb.ToString());
        }

    }
}