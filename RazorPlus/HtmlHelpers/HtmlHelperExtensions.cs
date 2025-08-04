using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace RazorPlus.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RP_BasicTable<T>(this IHtmlHelper htmlHelper, List<T> modelList, string id = "", string @class = "")
        {
            Type modelType = typeof(T);
            PropertyInfo[] propertyInfos = modelType.GetProperties();

            TagBuilder table = new TagBuilder("table");
            TagBuilder thead = GetTableHead(modelType, propertyInfos);
            TagBuilder tbody = GetBasicTableBody(modelList, modelType, propertyInfos);

            if (!string.IsNullOrEmpty(id)) table.Attributes.Add("id", id);
            if (!string.IsNullOrEmpty(@class)) table.Attributes.Add("class", @class);

            table.InnerHtml.AppendHtml(thead);
            table.InnerHtml.AppendHtml(tbody);

            return table;
        }

        #region Table Aux Methods

        private static TagBuilder GetTableHead(Type modelType, PropertyInfo[] propertyInfos)
        {
            string[] columns = propertyInfos.Select(prop => prop.Name).ToArray();

            TagBuilder thead = new TagBuilder("thead");
            TagBuilder tr = new TagBuilder("tr");

            foreach (string col in columns)
            {
                TagBuilder th = new TagBuilder("th");

                th.InnerHtml.Append(col);
                tr.InnerHtml.AppendHtml(th);
            }

            thead.InnerHtml.AppendHtml(tr);

            return thead;
        }

        private static TagBuilder GetBasicTableBody<T>(List<T> modelList, Type modelType, PropertyInfo[] propertyInfos)
        {
            TagBuilder tbody = new TagBuilder("tbody");

            for (int i = 0; i < modelList.Count; i++)
            {
                TagBuilder tr = new TagBuilder("tr");

                foreach (PropertyInfo prop in propertyInfos)
                {
                    TagBuilder td = new TagBuilder("td");

                    var cellText = prop.GetValue(modelList[i]);
                    td.InnerHtml.Append(cellText?.ToString() ?? "");
                    tr.InnerHtml.AppendHtml(td);
                }

                tbody.InnerHtml.AppendHtml(tr);
            }

            return tbody;
        }

        #endregion
    }
}
