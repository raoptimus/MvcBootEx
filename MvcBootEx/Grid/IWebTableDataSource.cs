using System.Collections.Generic;

namespace MvcBootEx.Grid
{
    internal interface IWebTableDataSource<T>
    {
        long TotalRowCount { get; }

        IList<WebTableRow<T>> GetRows(SortInfo sortInfo, int pageIndex);
    }
}
