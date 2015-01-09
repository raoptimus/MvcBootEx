using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcBootEx.Grid
{
    public class Table<T> : WebGrid
    {
        public bool Striped { get; private set; } //= true;// C# 6 or higher
        public bool Bordered { get; private set; }
        public bool Hovered { get; private set; }
        public bool Responsive { get; private set; }
        public bool Condensed { get; private set; }

        public Table(
            IEnumerable<T> source = null,
            IEnumerable<string> columnNames = null,
            string defaultSort = null,
            int rowsPerPage = 10,
            bool canSort = true,
            string ajaxUpdateContainerId = null,
            string ajaxUpdateCallback = null,
            string fieldNamePrefix = null,
            string pageFieldName = null,
            string selectionFieldName = null,
            string sortFieldName = null,
            string sortDirectionFieldName = null,
            bool striped = true,
            bool bordered = false,
            bool hovered = true,
            bool responsive = true,
            bool condensed = true
            )
            : base(
                (IEnumerable<object>) source, columnNames, defaultSort, rowsPerPage, true, canSort,
                ajaxUpdateContainerId, ajaxUpdateCallback, fieldNamePrefix, pageFieldName, selectionFieldName,
                sortFieldName, sortDirectionFieldName)
        {
            Striped = striped;
            Bordered = bordered;
            Hovered = hovered;
            Responsive = responsive;
            Condensed = condensed;
        }

        /// <summary>
        /// Gets the HTML for a table with a pager.
        /// </summary>
        /// <param name="tableStyle">Table class for styling.</param>
        /// <param name="headerStyle">Header row class for styling.</param>
        /// <param name="footerStyle">Footer row class for styling.</param>
        /// <param name="rowStyle">Row class for styling (odd rows only).</param>
        /// <param name="alternatingRowStyle">Row class for styling (even rows only).</param>
        /// <param name="selectedRowStyle">Selected row class for styling.</param>
        /// <param name="displayHeader">Whether the header row should be displayed.</param>
        /// <param name="caption">The string displayed as the table caption</param>
        /// <param name="fillEmptyRows">Whether the table can add empty rows to ensure the rowsPerPage row count.</param>
        /// <param name="emptyRowCellValue">Value used to populate empty rows. This property is only used when <paramref name="fillEmptyRows"/> is set</param>
        /// <param name="columns">Column model for customizing column rendering.</param>
        /// <param name="exclusions">Columns to exclude when auto-populating columns.</param>
        /// <param name="mode">Modes for pager rendering.</param>
        /// <param name="firstText">Text for link to first page.</param>
        /// <param name="previousText">Text for link to previous page.</param>
        /// <param name="nextText">Text for link to next page.</param>
        /// <param name="lastText">Text for link to last page.</param>
        /// <param name="numericLinksCount">Number of numeric links that should display.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        public new IHtmlString GetHtml(
            string tableStyle = null,
            string headerStyle = null,
            string footerStyle = null,
            string rowStyle = null,
            string alternatingRowStyle = null,
            string selectedRowStyle = null,
            string caption = null,
            bool displayHeader = true,
            bool fillEmptyRows = false,
            string emptyRowCellValue = null,
            IEnumerable<WebGridColumn> columns = null,
            IEnumerable<string> exclusions = null,
            WebGridPagerModes mode = WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric,
            string firstText = null,
            string previousText = null,
            string nextText = null,
            string lastText = null,
            int numericLinksCount = 5,
            object htmlAttributes = null)
        {
            var tableStyles = new HashSet<string>((tableStyle ?? "").Split(' ')) {"table"};
            if (Striped)
            {
                tableStyles.Add("table-striped");
            }
            if (Bordered)
            {
                tableStyles.Add("table-bordered");
            }
            if (Hovered)
            {
                tableStyles.Add("table-hover");
            }
            if (Condensed)
            {
                tableStyles.Add("table-condensed");
            }
            if (Responsive)
            {
                tableStyles.Add("table-responsive");
            }
            tableStyle = String.Join(" ", tableStyles);

            Func<dynamic, object> footer = null;

            if (PageCount > 1)
            {
                footer = item => PagerList(mode, firstText, previousText, nextText, lastText, numericLinksCount, false, footerStyle);
            }

            return Table(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle, selectedRowStyle, caption, displayHeader,
                         fillEmptyRows, emptyRowCellValue, columns, exclusions, footer: footer,
                         htmlAttributes: htmlAttributes);
        }

        public WebGridColumn Column(
            string columnName = null,
            string header = null,
            Func<T, object> format = null,
            string style = null,
            bool canSort = true)
        {
            Func<dynamic, object> wrappedFormat = null;
            if (format != null)
            {
                wrappedFormat = o => format((T) o.Value);
            }
            WebGridColumn column = base.Column(
                columnName, header,
                wrappedFormat, style, canSort);
            return column;
        }

        public Table<T> Bind(
            IEnumerable<T> source,
            IEnumerable<string> columnNames = null,
            bool autoSortAndPage = true,
            int rowCount = -1)
        {
            Bind(
                (IEnumerable<object>) source,
                columnNames,
                autoSortAndPage,
                rowCount);
            return this;
        }

        public new HelperResult Pager(
            WebGridPagerModes mode = WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric,
            string firstText = null,
            string previousText = null,
            string nextText = null,
            string lastText = null,
            int numericLinksCount = 5
            )
        {
            return PagerList(mode, firstText, previousText, nextText, lastText, numericLinksCount, true, null);
        }

        private HelperResult PagerList(
            WebGridPagerModes mode,
            string firstText,
            string previousText,
            string nextText,
            string lastText,
            int numericLinksCount,
            bool explicitlyCalled,
            string paginationStyle
            )
        {
            int currentPage = PageIndex;
            int totalPages = PageCount;
            int lastPage = totalPages - 1;

            var nav = new TagBuilder("nav");
            nav.AddCssClass(paginationStyle);
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var li = new List<TagBuilder>();

            if (totalPages <= 1)
            {
                return new HelperResult(writer => writer.Write(string.Empty));
            }

            if (ModeEnabled(mode, WebGridPagerModes.FirstLast))
            {
                if (String.IsNullOrEmpty(firstText))
                {
                    firstText = "First";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(this, GetPageUrl(0), firstText)
                };

                if (currentPage == 0)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious))
            {
                if (String.IsNullOrEmpty(previousText))
                {
                    previousText = "Prev";
                }

                int page = currentPage == 0 ? 0 : currentPage - 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(this, GetPageUrl(page), previousText)
                };

                if (currentPage == 0)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.Numeric) && (totalPages > 1))
            {
                int last = currentPage + (numericLinksCount / 2);
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
                    var part = new TagBuilder("li")
                    {
                        InnerHtml = GridLink(this, GetPageUrl(i), pageText)
                    };

                    if (i == currentPage)
                    {
                        part.MergeAttribute("class", "active");
                    }

                    li.Add(part);

                }
            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious))
            {
                if (String.IsNullOrEmpty(nextText))
                {
                    nextText = "Next";
                }

                int page = currentPage == lastPage ? lastPage : currentPage + 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(this, GetPageUrl(page), nextText)
                };

                if (currentPage == lastPage)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            if (ModeEnabled(mode, WebGridPagerModes.FirstLast))
            {
                if (String.IsNullOrEmpty(lastText))
                {
                    lastText = "Last";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(this, GetPageUrl(lastPage), lastText)
                };

                if (currentPage == lastPage)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);

            }

            ul.InnerHtml = string.Join("", li);

            string html;

            if (explicitlyCalled && IsAjaxEnabled)
            {
                nav.MergeAttribute("data-swhgajax", "true");
                nav.MergeAttribute("data-swhgcontainer", AjaxUpdateContainerId);
                nav.MergeAttribute("data-swhgcallback", AjaxUpdateCallback);

                nav.InnerHtml = ul.ToString();
                html = nav.ToString();

            }
            else
            {
                nav.InnerHtml = ul.ToString();
                html = nav.ToString();
            }

            return new HelperResult(writer => writer.Write(html));
        }
        private static bool ModeEnabled(WebGridPagerModes mode, WebGridPagerModes modeCheck)
        {
            return (mode & modeCheck) == modeCheck;
        }
        private static String GridLink(WebGrid webGrid, string url, string text)
        {
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.MergeAttribute("href", url);
            if (webGrid.IsAjaxEnabled)
            {
                builder.MergeAttribute("data-swhglnk", "true");
            }
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}
