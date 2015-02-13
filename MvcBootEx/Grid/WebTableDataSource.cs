using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Helpers;

namespace MvcBootEx.Grid
{
    public class WebTableDataSource<T> : IWebTableDataSource<T>
    {
        private readonly WebTable<T> _grid;
        private readonly Type _elementType;
        private readonly IQueryable<T> _values;
        private readonly bool _canPage;
        private readonly bool _canSort;
        private readonly long _totalRowCount;

        public SortInfo DefaultSort { get; set; }

        public int RowsPerPage { get; set; }

        public long TotalRowCount
        {
            get { return _totalRowCount; }
        }

        public WebTableDataSource(WebTable<T> grid, IQueryable<T> values, Type elementType, bool canPage,
            bool canSort)
        {
            _grid = grid;
            _values = values;
            _elementType = elementType;
            _canPage = canPage;
            _canSort = canSort;
            _totalRowCount = _values.Count();
        }

        public IList<WebTableRow<T>> GetRows(SortInfo sortInfo, int pageIndex)
        {
            var data = _values;

            if (_canSort)
            {
                data = Sort(data.AsQueryable(), sortInfo);
            }

            if (_canPage)
            {
                data = Page(data, pageIndex);
            }

            var rows = new List<WebTableRow<T>>();
            var i = 0;

            try
            {
                foreach (var v in data)
                {
                    rows.Add(new WebTableRow<T>(_grid, v, i));
                    i++;
                }
            }
            catch (ArgumentException)
            {
            }

            return rows;
        }

        private IQueryable<T> Page(IQueryable<T> data, int pageIndex)
        {
            if (!_canPage)
            {
                return data;
            }

            return data.Skip(pageIndex * RowsPerPage).Take(RowsPerPage);
        }

        private IQueryable<T> Sort(IQueryable<T> data, SortInfo sortInfo)
        {
            if (!string.IsNullOrEmpty(sortInfo.SortColumn) ||
                DefaultSort != null && !string.IsNullOrEmpty(DefaultSort.SortColumn))
            {
                var direct = sortInfo.SortDirection == SortDirection.Descending ? "OrderByDescending" : "OrderBy";
                var property = _elementType.GetProperty(sortInfo.SortColumn);
                var parameter = Expression.Parameter(_elementType, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                MethodCallExpression resultExp = Expression.Call(typeof (Queryable), direct
                    ,
                    new Type[] { _elementType, property.PropertyType }, data.Expression, Expression.Quote(orderByExp));
                return data.Provider.CreateQuery<T>(resultExp);
            }

            return data;
        }
    }
}
