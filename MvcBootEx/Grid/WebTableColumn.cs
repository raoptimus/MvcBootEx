using System;
using System.Web;

namespace MvcBootEx.Grid
{
    /// <summary>
    /// Represents a column in a <see cref="T:System.Web.Helpers.WebGrid"/> instance.
    /// </summary>
    public class WebTableColumn<T>
    {
        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="T:System.Web.Helpers.WebGrid"/> column can be sorted.
        /// </summary>
        /// 
        /// <returns>
        /// true to indicate that the column can be sorted; otherwise, false.
        /// </returns>
        public bool CanSort { get; set; }

        /// <summary>
        /// Gets or sets the name of the data item that is associated with the <see cref="T:System.Web.Helpers.WebGrid"/> column.
        /// </summary>
        /// 
        /// <returns>
        /// The name of the data item.
        /// </returns>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets a function that is used to format the data item that is associated with the <see cref="T:System.Web.Helpers.WebGrid"/> column.
        /// </summary>
        /// 
        /// <returns>
        /// The function that is used to format that data item that is associated with the column.
        /// </returns>
        public Func<T, object> Format { get; set; }

        /// <summary>
        /// Gets or sets the text that is rendered in the header of the <see cref="T:System.Web.Helpers.WebGrid"/> column.
        /// </summary>
        /// 
        /// <returns>
        /// The text that is rendered to the column header.
        /// </returns>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the CSS class attribute that is rendered as part of the HTML table cells that are associated with the <see cref="T:System.Web.Helpers.WebGrid"/> column.
        /// </summary>
        /// 
        /// <returns>
        /// The CSS class attribute that is applied to cells that are associated with the column.
        /// </returns>
        public string Style { get; set; }
    }
}
