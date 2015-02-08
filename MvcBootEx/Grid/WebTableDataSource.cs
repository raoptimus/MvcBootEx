using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Helpers;
using Microsoft.CSharp.RuntimeBinder;

namespace MvcBootEx.Grid
{
    public class WebTableDataSource : IWebTableDataSource
    {
        private readonly WebTable _grid;
        private readonly Type _elementType;
        private readonly IEnumerable<object> _values;
        private readonly bool _canPage;
        private readonly bool _canSort;

        public SortInfo DefaultSort { get; set; }

        public int RowsPerPage { get; set; }

        public int TotalRowCount
        {
            get { return _values.Count(); }
        }

        public WebTableDataSource(WebTable grid, IEnumerable<object> values, Type elementType, bool canPage,
            bool canSort)
        {
            this._grid = grid;
            this._values = values;
            this._elementType = elementType;
            this._canPage = canPage;
            this._canSort = canSort;
        }

        public IList<WebTableRow> GetRows(SortInfo sortInfo, int pageIndex)
        {
            IEnumerable<object> data = this._values;
            if (this._canSort)
                data = this.Sort(this._values.AsQueryable(), sortInfo);
            IEnumerable<object> source1 = this.Page(data, pageIndex);
            IEnumerable<object> source2;
            try
            {
                source2 = source1.ToList();
            }
            catch (ArgumentException)
            {
                source2 = this.Page(this._values.AsQueryable(), pageIndex);
            }
            var rows = new List<WebTableRow>();
            var i = 0;
            foreach (var v in source2)
            {
                rows.Add(new WebTableRow(this._grid, v, i));
                i++;
            }
            return rows;
//            return
//                (IList<WebGridRow>)
//                    source2.Select((Func<object, int, WebGridRow>) ((value, index) =>
//                    {
//                        // ISSUE: reference to a compiler-generated field
//                        if (WebTableDataSource.\u003CGetRows\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
//                        {
//                            // ISSUE: reference to a compiler-generated field
//                            WebTableDataSource.\u003CGetRows\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 =
//                                CallSite<Func<CallSite, Type, WebGrid, object, int, WebGridRow>>.Create(
//                                    Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None,
//                                        typeof (WebTableDataSource),
//                                        (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
//                                        {
//                                            CSharpArgumentInfo.Create(
//                                                CSharpArgumentInfoFlags.UseCompileTimeType |
//                                                CSharpArgumentInfoFlags.IsStaticType, (string) null),
//                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType,
//                                                (string) null),
//                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "value"),
//                                            CSharpArgumentInfo.Create(
//                                                CSharpArgumentInfoFlags.UseCompileTimeType |
//                                                CSharpArgumentInfoFlags.NamedArgument, "rowIndex")
//                                        }));
//                        }
//                        // ISSUE: reference to a compiler-generated field
//                        // ISSUE: reference to a compiler-generated field
//                        return
//                            WebTableDataSource.\u003CGetRows\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target(
//                                (CallSite)
//                                    WebTableDataSource.\u003CGetRows\u003Eo__SiteContainer0.\u003C\u003Ep__Site1,
//                                typeof (WebGridRow), this._grid, value, index);
//                    })).ToList();
        }

        private IQueryable<object> Sort(IQueryable<object> data, SortInfo sortInfo)
        {
            if (!string.IsNullOrEmpty(sortInfo.SortColumn) ||
                this.DefaultSort != null && !string.IsNullOrEmpty(this.DefaultSort.SortColumn))
                return this.Sort(data, this._elementType, sortInfo);
            else
                return data;
        }

        private IEnumerable<object> Page(IEnumerable<object> data, int pageIndex)
        {
            if (this._canPage)
                return data.Skip(pageIndex*this.RowsPerPage).Take(this.RowsPerPage);
            else
                return data;
        }

        private IQueryable<object> Sort(IQueryable<object> data, Type elementType, SortInfo sort)
        {
            if (typeof (IDynamicMetaObjectProvider).IsAssignableFrom(elementType))
            {
                CallSiteBinder member = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None,
                    sort.SortColumn, typeof (WebGrid), new CSharpArgumentInfo[1]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                    });
                ParameterExpression parameterExpression = Expression.Parameter(typeof (IDynamicMetaObjectProvider), "o");
                DynamicExpression dynamicExpression = Expression.Dynamic(member, typeof (object),
                    parameterExpression);
                return
                    SortGenericExpression<IDynamicMetaObjectProvider, object>(data,
                        dynamicExpression, parameterExpression, sort.SortDirection);
            }
            else
            {
                ParameterExpression parameterExpression = Expression.Parameter(elementType, "o");
                var expression = (Expression) parameterExpression;
                Type type = elementType;
                string sortColumn = sort.SortColumn;
                var chArray = new char[1]
                {
                    '.'
                };
                foreach (string name in sortColumn.Split(chArray))
                {
                    PropertyInfo property = type.GetProperty(name);
                    if (property == null)
                    {
                        if (this.DefaultSort != null && !sort.Equals(this.DefaultSort) &&
                            !string.IsNullOrEmpty(this.DefaultSort.SortColumn))
                            return this.Sort(data, elementType, this.DefaultSort);
                        return data;
                    }
                    expression = Expression.Property(expression, property);
                    type = property.PropertyType;
                }
                return
                    (IQueryable<object>)
                        this.GetType()
                            .GetMethod("SortGenericExpression", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(elementType, expression.Type)
                            .Invoke((object) null, new object[4]
                            {
                                (object) data,
                                (object) expression,
                                (object) parameterExpression,
                                (object) sort.SortDirection
                            });
            }
        }

        private static IQueryable<TElement> SortGenericExpression<TElement, TProperty>(IQueryable<object> data,
            Expression body, ParameterExpression param, SortDirection sortDirection)
        {
            IQueryable<TElement> source = data.Cast<TElement>();
            Expression<Func<TElement, TProperty>> keySelector = Expression.Lambda<Func<TElement, TProperty>>(body,
                new ParameterExpression[1]
                {
                    param
                });
            return sortDirection == SortDirection.Descending 
                ? source.OrderByDescending(keySelector) 
                : source.OrderBy(keySelector);
        }
    }
}
