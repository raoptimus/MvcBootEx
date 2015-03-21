using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcBootEx.Grid
{
    public class WebTableRenderer<T> : HelperPage
    {
        private static HelperResult GridInitScript(WebTable<T> webGrid, HttpContextBase httpContext)
        {
            return new HelperResult(razorHelperWriter =>
            {
                if (!webGrid.IsAjaxEnabled)
                {
                    return;
                }

                if (IsGridScriptRendered(httpContext))
                {
                    return;
                }

                SetGridScriptRendered(httpContext, true);

                WriteLiteralTo(razorHelperWriter,
                    "        <script type=\"text/javascript\">\r\n        (function($) {\r\n            $.fn" +
                    ".swhgLoad = function(url, containerId, callback) {\r\n                url = url + " +
                    "(url.indexOf(\'?\') == -1 ? \'?\' : \'&\') + \'__swhg=\' + new Date().getTime();\r\n\r\n    " +
                    "            $(\'<div/>\').load(url + \' \' + containerId, function(data, status, xhr" +
                    ") {\r\n                    $(containerId).replaceWith($(this).html());\r\n          " +
                    "          if (typeof(callback) === \'function\') {\r\n                        callba" +
                    "ck.apply(this, arguments);\r\n                    }\r\n                });\r\n        " +
                    "        return this;\r\n            }\r\n\r\n            $(function() {\r\n             " +
                    "   $(\'table[data-swhgajax=\"true\"],span[data-swhgajax=\"true\"]\').each(function() {" +
                    "\r\n                    var self = $(this);\r\n                    var containerId =" +
                    " \'#\' + self.data(\'swhgcontainer\');\r\n                    var callback = getFuncti" +
                    "on(self.data(\'swhgcallback\'));\r\n\r\n                    $(containerId).parent().de" +
                    "legate(containerId + \' a[data-swhglnk=\"true\"]\', \'click\', function() {\r\n         " +
                    "               $(containerId).swhgLoad($(this).attr(\'href\'), containerId, callba" +
                    "ck);\r\n                        return false;\r\n                    });\r\n          " +
                    "      })\r\n            });\r\n\r\n            function getFunction(code, argNames) {\r" +
                    "\n                argNames = argNames || [];\r\n                var fn = window, pa" +
                    "rts = (code || \"\").split(\".\");\r\n                while (fn && parts.length) {\r\n  " +
                    "                  fn = fn[parts.shift()];\r\n                }\r\n                if" +
                    " (typeof (fn) === \"function\") {\r\n                    return fn;\r\n               " +
                    " }\r\n                argNames.push(code);\r\n                return Function.constr" +
                    "uctor.apply(null, argNames);\r\n            }\r\n        })(jQuery);\r\n        </scri" +
                    "pt>\r\n");
            });
        }

        public static HelperResult Table(WebTable<T> webGrid,
            HttpContextBase httpContext,
            string tableStyle,
            string headerStyle,
            string footerStyle,
            string rowStyle,
            string alternatingRowStyle,
            string selectedRowStyle,
            string caption,
            bool displayHeader,
            bool fillEmptyRows,
            string emptyRowCellValue,
            IEnumerable<WebTableColumn<T>> columns,
            IEnumerable<string> exclusions,
            Func<object, IHtmlString> footer,
            object htmlAttributes)
        {
            return new HelperResult(razorHelperWriter =>
            {
                if (emptyRowCellValue == null)
                {
                    emptyRowCellValue = "&nbsp;";
                }

                WriteTo(razorHelperWriter, GridInitScript(webGrid, httpContext));

                var htmlAttributeDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                if (webGrid.IsAjaxEnabled)
                {
                    htmlAttributeDictionary["data-swhgajax"] = "true";
                    htmlAttributeDictionary["data-swhgcontainer"] = webGrid.AjaxUpdateContainerId;
                    htmlAttributeDictionary["data-swhgcallback"] = webGrid.AjaxUpdateCallback;
                }

                WriteLiteralTo(razorHelperWriter, "    <table");
                WriteTo(razorHelperWriter, tableStyle.IsEmpty() ? null : Raw(" class=\"" + HttpUtility.HtmlAttributeEncode(tableStyle) + "\""));
                WriteTo(razorHelperWriter, PrintAttributes(htmlAttributeDictionary));
                WriteLiteralTo(razorHelperWriter, ">\r\n");

                if (!caption.IsEmpty())
                {
                    WriteLiteralTo(razorHelperWriter, "        <caption>");
                    WriteTo(razorHelperWriter, caption);
                    WriteLiteralTo(razorHelperWriter, "</caption>\r\n");
                }

                var webTableColumns = columns as IList<WebTableColumn<T>> ?? columns.ToList();

                if (displayHeader)
                {
                    WriteLiteralTo(razorHelperWriter, "    <thead>\r\n        <tr");
                    WriteTo(razorHelperWriter, CssClass(headerStyle));
                    WriteLiteralTo(razorHelperWriter, ">\r\n");

                    foreach (var column in webTableColumns)
                    {
                        WriteLiteralTo(razorHelperWriter, "            <th scope=\"col\">\r\n");

                        if (ShowSortableColumnHeader(webGrid, column))
                        {
                            var text = column.Header.IsEmpty() ? column.ColumnName : column.Header;

                            WriteTo(razorHelperWriter,
                                GridLink(webGrid, webGrid.GetSortUrl(column.ColumnName), text));
                        }
                        else
                        {
                            WriteTo(razorHelperWriter, column.Header ?? column.ColumnName);
                        }

                        WriteLiteralTo(razorHelperWriter, "            </th>\r\n");
                    }

                    WriteLiteralTo(razorHelperWriter, "        </tr>\r\n    </thead>\r\n");
                }

                if (footer != null)
                {
                    WriteLiteralTo(razorHelperWriter, "    <tfoot>\r\n        <tr ");
                    WriteTo(razorHelperWriter, CssClass(footerStyle));
                    WriteLiteralTo(razorHelperWriter, ">\r\n            <td colspan=\"");
                    WriteTo(razorHelperWriter, webTableColumns.Count());
                    WriteLiteralTo(razorHelperWriter, "\">");
                    WriteTo(razorHelperWriter, Format(footer));
                    WriteLiteralTo(razorHelperWriter, "</td>\r\n        </tr>\r\n    </tfoot>\r\n");
                }

                WriteLiteralTo(razorHelperWriter, "    <tbody>\r\n");

                int rowIndex = 0;

                foreach (var row in webGrid.Rows)
                {
                    string style = GetRowStyle(webGrid, rowIndex++, rowStyle, alternatingRowStyle, selectedRowStyle);
                    WriteLiteralTo(razorHelperWriter, "        <tr");
                    WriteTo(razorHelperWriter, CssClass(style));
                    WriteLiteralTo(razorHelperWriter, ">\r\n");

                    foreach (var column in webTableColumns)
                    {
                        var value = (column.Format == null)
                            ? HttpUtility.HtmlEncode(row[column.ColumnName])
                            : Format(column.Format, row.Value).ToString();

                        WriteLiteralTo(razorHelperWriter, "            <td");
                        WriteTo(razorHelperWriter, CssClass(column.Style));
                        WriteLiteralTo(razorHelperWriter, ">");
                        WriteTo(razorHelperWriter, Raw(value));
                        WriteLiteralTo(razorHelperWriter, "</td>\r\n");
                    }

                    WriteLiteralTo(razorHelperWriter, "        </tr>\r\n");
                }

                if (fillEmptyRows)
                {
                    rowIndex = webGrid.Rows.Count;
                    while (rowIndex < webGrid.RowsPerPage)
                    {
                        string style = GetRowStyle(webGrid, rowIndex++, rowStyle, alternatingRowStyle, null);

                        WriteLiteralTo(razorHelperWriter, "            <tr");
                        WriteTo(razorHelperWriter, CssClass(style));
                        WriteLiteralTo(razorHelperWriter, ">\r\n");

                        foreach (var column in webTableColumns)
                        {
                            WriteLiteralTo(razorHelperWriter, "                    <td");
                            WriteTo(razorHelperWriter, CssClass(column.Style));
                            WriteLiteralTo(razorHelperWriter, ">");
                            WriteTo(razorHelperWriter, Raw(emptyRowCellValue));
                            WriteLiteralTo(razorHelperWriter, "</td>\r\n");
                        }

                        WriteLiteralTo(razorHelperWriter, "            </tr>\r\n");
                    }
                }

                WriteLiteralTo(razorHelperWriter, "    </tbody>\r\n    </table>\r\n");
            });
        }

        public static HelperResult Pager(
            WebTable<T> webGrid,
            HttpContextBase httpContext,
            WebGridPagerModes mode,
            string firstText,
            string previousText,
            string nextText,
            string lastText,
            int numericLinksCount,
            bool renderAjaxContainer)
        {
            return new HelperResult(razorHelperWriter =>
            {
                int currentPage = webGrid.PageIndex;
                int totalPages = webGrid.PageCount;
                int lastPage = totalPages - 1;

                var nav = new TagBuilder("nav");
                WriteLiteralTo(razorHelperWriter, nav.ToString(TagRenderMode.StartTag));

                var ul = new TagBuilder("ul");
                ul.AddCssClass("pagination");
                WriteLiteralTo(razorHelperWriter, ul.ToString(TagRenderMode.StartTag));

                WriteTo(razorHelperWriter, GridInitScript(webGrid, httpContext));

                if (renderAjaxContainer && webGrid.IsAjaxEnabled)
                {
                    WriteLiteralTo(razorHelperWriter, "        ");
                    WriteLiteralTo(razorHelperWriter, "<span data-swhgajax=\"true\" data-swhgcontainer=\"");
                    WriteTo(razorHelperWriter, webGrid.AjaxUpdateContainerId);
                    WriteLiteralTo(razorHelperWriter, "\" data-swhgcallback=\"");
                    WriteTo(razorHelperWriter, webGrid.AjaxUpdateCallback);
                    WriteLiteralTo(razorHelperWriter, "\">\r\n");
                }

                if (ModeEnabled(mode, WebGridPagerModes.FirstLast) && currentPage > 1)
                {
                    if (String.IsNullOrEmpty(firstText))
                    {
                        firstText = "<<";
                    }

                    WriteTo(razorHelperWriter, GridLiLink(webGrid, webGrid.GetPageUrl(0), firstText, currentPage == 0));
                    WriteTo(razorHelperWriter, Raw(" "));
                }

                if (ModeEnabled(mode, WebGridPagerModes.NextPrevious) && currentPage > 0)
                {
                    if (String.IsNullOrEmpty(previousText))
                    {
                        previousText = "<";
                    }

                    WriteTo(razorHelperWriter, GridLiLink(webGrid, webGrid.GetPageUrl(currentPage - 1), previousText, currentPage == 0));
                    WriteTo(razorHelperWriter, Raw(" "));
                }

                if (ModeEnabled(mode, WebGridPagerModes.Numeric) && (totalPages > 1))
                {
                    int last = currentPage + (numericLinksCount/2);
                    int first = last - numericLinksCount + 1;
                    
                    if (last > lastPage)
                    {
                        first -= last - lastPage;
                        last = lastPage;
                    }

                    if (first < 0)
                    {
                        last = Math.Min(last + (0 - first), lastPage);
                        first = 0;
                    }

                    for (int i = first; i <= last; i++)
                    {
                        var pageText = (i + 1).ToString(CultureInfo.InvariantCulture);
                        WriteTo(razorHelperWriter, GridLiLink(webGrid, webGrid.GetPageUrl(i), pageText, i == currentPage));
                        WriteTo(razorHelperWriter, Raw(" "));
                    }
                }

                if (ModeEnabled(mode, WebGridPagerModes.NextPrevious) && (currentPage < lastPage))
                {
                    if (String.IsNullOrEmpty(nextText))
                    {
                        nextText = ">";
                    }

                    WriteTo(razorHelperWriter, GridLiLink(webGrid, webGrid.GetPageUrl(currentPage + 1), nextText, currentPage == lastPage));
                    WriteTo(razorHelperWriter, Raw(" "));
                }

                if (ModeEnabled(mode, WebGridPagerModes.FirstLast) && (currentPage < lastPage - 1))
                {
                    if (String.IsNullOrEmpty(lastText))
                    {
                        lastText = ">>";
                    }

                    WriteTo(razorHelperWriter, GridLiLink(webGrid, webGrid.GetPageUrl(lastPage), lastText, currentPage == lastPage));
                }

                if (renderAjaxContainer && webGrid.IsAjaxEnabled)
                {
                    WriteLiteralTo(razorHelperWriter, "        ");
                    WriteLiteralTo(razorHelperWriter, "</span>\r\n");
                }

                WriteLiteralTo(razorHelperWriter, ul.ToString(TagRenderMode.EndTag));
                WriteLiteralTo(razorHelperWriter, nav.ToString(TagRenderMode.EndTag));
            });
        }

        private static readonly object _gridScriptRenderedKey = new object();

        private static bool IsGridScriptRendered(HttpContextBase context)
        {
            bool? value = (bool?) context.Items[_gridScriptRenderedKey];
            return value.HasValue && value.Value;
        }

        private static void SetGridScriptRendered(HttpContextBase context, bool value)
        {
            context.Items[_gridScriptRenderedKey] = value;
        }

        private static bool ShowSortableColumnHeader(WebTable<T> grid, WebTableColumn<T> column)
        {
            return grid.CanSort && column.CanSort && !column.ColumnName.IsEmpty();
        }

        public static IHtmlString GridLiLink(WebTable<T> webGrid, string url, string text, bool disabled = false)
        {
            var li = new TagBuilder("li")
            {
                InnerHtml = GridLink(webGrid, url, text).ToHtmlString()
            };

            if (disabled)
            {
                li.MergeAttribute("class", "disabled");
            }

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }
        public static IHtmlString GridLink(WebTable<T> webGrid, string url, string text)
        {
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.MergeAttribute("href", url);

            if (webGrid.IsAjaxEnabled)
            {
                builder.MergeAttribute("data-swhglnk", "true");
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        private static IHtmlString Raw(string text)
        {
            return new HtmlString(text);
        }

        private static IHtmlString RawJs(string text)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(text));
        }

        private static IHtmlString CssClass(string className)
        {
            return
                new HtmlString((!className.IsEmpty())
                    ? " class=\"" + HttpUtility.HtmlAttributeEncode(className) + "\""
                    : String.Empty);
        }

        private static string GetRowStyle(WebTable<T> webGrid, int rowIndex, string rowStyle, string alternatingRowStyle,
            string selectedRowStyle)
        {
            var style = new StringBuilder();

            if (rowIndex%2 == 0)
            {
                if (!String.IsNullOrEmpty(rowStyle))
                {
                    style.Append(rowStyle);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(alternatingRowStyle))
                {
                    style.Append(alternatingRowStyle);
                }
            }

            if (String.IsNullOrEmpty(selectedRowStyle) || (rowIndex != webGrid.SelectedIndex))
            {
                return style.ToString();
            }

            if (style.Length > 0)
            {
                style.Append(" ");
            }
            style.Append(selectedRowStyle);
            return style.ToString();
        }

        private static HelperResult Format(Func<object, object> format)
        {
            var result = format(null);
            return new HelperResult(tw =>
            {
                var helper = result as HelperResult;
                if (helper != null)
                {
                    helper.WriteTo(tw);
                    return;
                }

                var obj = result as IHtmlString;
                if (obj != null)
                {
                    tw.Write(obj);
                    return;
                }

                tw.Write(HttpUtility.HtmlEncode(result));
            });
        }

        private static HelperResult Format(Func<T, object> format, T arg)
        {
            var result = format(arg);
            return new HelperResult(tw =>
            {
                var helper = result as HelperResult;
                if (helper != null)
                {
                    helper.WriteTo(tw);
                    return;
                }

                var obj = result as IHtmlString;
                if (obj != null)
                {
                    tw.Write(obj);
                    return;
                }

                tw.Write(HttpUtility.HtmlEncode(result));
            });
        }

        private static IHtmlString PrintAttributes(IEnumerable<KeyValuePair<string, object>> attributes)
        {
            var builder = new StringBuilder();

            foreach (var item in attributes)
            {
                var value = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                builder.Append(' ')
                    .Append(HttpUtility.HtmlEncode(item.Key))
                    .Append("=\"")
                    .Append(HttpUtility.HtmlAttributeEncode(value))
                    .Append('"');
            }

            return new HtmlString(builder.ToString());
        }

        private static bool ModeEnabled(WebGridPagerModes mode, WebGridPagerModes modeCheck)
        {
            return (mode & modeCheck) == modeCheck;
        }

        protected static HttpApplication ApplicationInstance
        {
            get { return ((Context.ApplicationInstance)); }
        }
    }
}
