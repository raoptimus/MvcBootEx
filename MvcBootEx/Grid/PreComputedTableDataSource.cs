using System.Collections.Generic;

namespace MvcBootEx.Grid
{
    internal class PreComputedTableDataSource<T> : IWebTableDataSource<T>
    {
        private readonly int _totalRows;
        private readonly IList<WebTableRow<T>> _rows;

        public long TotalRowCount
        {
            get { return _totalRows; }
        }

        public PreComputedTableDataSource(WebTable<T> grid, IEnumerable<T> values, int totalRows)
        {
            _totalRows = totalRows;
            _rows = new List<WebTableRow<T>>();
            int i = 0;
            foreach (var value in values)
            {
                _rows.Add(new WebTableRow<T>(grid, (T)value, i));
                i++;
            }
        }

        public IList<WebTableRow<T>> GetRows(SortInfo sortInfo, int pageIndex)
        {
            return _rows;
        }
    }
}
