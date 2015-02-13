using System.Collections.Generic;
using System.Linq;

namespace MvcBootEx.Grid
{
    public static class WebTableEx
    {
        public static WebTable<T> Grid<T>(this BootEx bootEx,
            IQueryable<T> source = null,
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
        {
            return new WebTable<T>(source, columnNames, defaultSort, rowsPerPage, canPage, canSort,
                ajaxUpdateContainerId, ajaxUpdateCallback, fieldNamePrefix, pageFieldName, selectionFieldName,
                sortFieldName, sortDirectionFieldName, striped, bordered, hovered, responsive, condensed);
        }
    }
}
    