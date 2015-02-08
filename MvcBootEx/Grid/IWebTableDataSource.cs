using System.Collections.Generic;

namespace MvcBootEx.Grid
{
    internal interface IWebTableDataSource
    {
        int TotalRowCount { get; }

        IList<WebTableRow> GetRows(SortInfo sortInfo, int pageIndex);
    }
}
