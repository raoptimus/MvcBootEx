using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;

namespace MvcBootEx.Grid
{
    public class Table<T> : WebTable
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
            bool canPage = true,
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
                (IEnumerable<object>) source, columnNames, defaultSort, rowsPerPage, canPage, canSort,
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

            return base.GetHtml(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle, selectedRowStyle,
                caption,
                displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions, mode, firstText, previousText,
                nextText, lastText, numericLinksCount, htmlAttributes);
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
    }
}
