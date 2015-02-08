using System.Collections.Generic;

namespace MvcBootEx.Grid
{
    internal class PreComputedTableDataSource : IWebTableDataSource
    {
        private readonly int _totalRows;
        private readonly IList<WebTableRow> _rows;

        public int TotalRowCount
        {
            get { return this._totalRows; }
        }

        public PreComputedTableDataSource(WebTable grid, IEnumerable<object> values, int totalRows)
        {
            this._totalRows = totalRows;
            this._rows = new List<WebTableRow>();
            int i = 0;
            foreach (var value in values)
            {
                this._rows.Add(new WebTableRow(grid, value, i));
                i++;
            }
//            this._rows = values.Select((Func<object, int, WebGridRow>) ((value, index) =>
//            {
//                // ISSUE: reference to a compiler-generated field
//                if (PreComputedTableDataSource.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
//                {
//                    // ISSUE: reference to a compiler-generated field
//                    PreComputedTableDataSource.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 =
//                        CallSite<Func<CallSite, Type, WebGrid, object, int, WebGridRow>>.Create(
//                            Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (PreComputedTableDataSource),
//                                (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
//                                {
//                                    CSharpArgumentInfo.Create(
//                                        CSharpArgumentInfoFlags.UseCompileTimeType |
//                                        CSharpArgumentInfoFlags.IsStaticType, (string) null),
//                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
//                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "value"),
//                                    CSharpArgumentInfo.Create(
//                                        CSharpArgumentInfoFlags.UseCompileTimeType |
//                                        CSharpArgumentInfoFlags.NamedArgument, "rowIndex")
//                                }));
//                }
//                // ISSUE: reference to a compiler-generated field
//                // ISSUE: reference to a compiler-generated field
//                return
//                    PreComputedTableDataSource.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target(
//                        (CallSite) PreComputedTableDataSource.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1,
//                        typeof (WebGridRow), grid, value, index);
//            })).ToList();
        }

        public IList<WebTableRow> GetRows(SortInfo sortInfo, int pageIndex)
        {
            return this._rows;
        }
    }
}
