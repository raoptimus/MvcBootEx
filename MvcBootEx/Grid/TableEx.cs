using System.Collections.Generic;

namespace MvcBootEx.Grid
{
    public static class TableEx
    {
        public static Table<T> Grid<T>(this BootEx bootEx,
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
        {
            return new Table<T>(source, columnNames, defaultSort, rowsPerPage, canSort,
                ajaxUpdateContainerId, ajaxUpdateCallback, fieldNamePrefix, pageFieldName, selectionFieldName,
                sortFieldName, sortDirectionFieldName, striped, bordered, hovered, responsive, condensed);
        }
    }
}
    