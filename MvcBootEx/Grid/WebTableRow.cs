using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MvcBootEx.Grid
{
    public class WebTableRow<T> : IEnumerable<T>
    {
        private const string ROW_INDEX_MEMBER_NAME = "ROW";

        private const BindingFlags BIND_FLAGS =
            BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        private readonly WebTable<T>  _grid;
        private readonly int _rowIndex;
        private readonly T _value;
        private IEnumerable<T> _values;

        /// <summary>
        /// Gets an object that contains a property member for each value in the row.
        /// </summary>
        /// 
        /// <returns>
        /// An object that contains each value in the row as a property.
        /// </returns>
        public T Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.Helpers.WebGrid"/> instance that the row belongs to.
        /// </summary>
        /// 
        /// <returns>
        /// The <see cref="T:System.Web.Helpers.WebGrid"/> instance that contains the row.
        /// </returns>
        public WebTable<T> WebGrid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Returns the value that has the specified name in the <see cref="T:System.Web.Helpers.WebGridRow"/> instance.
        /// </summary>
        /// 
        /// <returns>
        /// The specified value.
        /// </returns>
        /// <param name="name">The name of the value in the row to return.</param><exception cref="T:System.ArgumentException"><paramref name="name"/> is null or empty.</exception><exception cref="T:System.InvalidOperationException"><paramref name="name"/> specifies a value that does not exist.</exception>
        public object this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Argument Cannot Be Null Or Empty", "name");
                object result;

                if (TryGetMember(name, out result))
                    return result;

                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "WebGrid_ColumnNotFound",
                    new object[1]
                    {
                        name
                    }));
            }
        }

        /// <summary>
        /// Returns the value at the specified index in the <see cref="T:System.Web.Helpers.WebGridRow"/> instance.
        /// </summary>
        /// 
        /// <returns>
        /// The value at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the value in the row to return.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or greater than or equal to the number of values in the row.</exception>
        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= _grid.ColumnNames.Count())
                    throw new ArgumentOutOfRangeException("index");

                return this.Skip(index).First();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Helpers.WebGridRow"/> class using the specified <see cref="T:System.Web.Helpers.WebGrid"/> instance, row value, and index.
        /// </summary>
        /// <param name="webGrid">The <see cref="T:System.Web.Helpers.WebGrid"/> instance that contains the row.</param><param name="value">An object that contains a property member for each value in the row.</param><param name="rowIndex">The index of the row.</param>
        public WebTableRow(WebTable<T> webGrid, T value, int rowIndex)
        {
            _grid = webGrid;
            _value = value;
            _rowIndex = rowIndex;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the values of the <see cref="T:System.Web.Helpers.WebGridRow"/> instance.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerator that can be used to iterate through the values of the row.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (_values == null)
                _values = _grid.ColumnNames.Select((Func<string, T>) (c => WebTable<T>.GetMember(this, c)));

            return _values.GetEnumerator();
        }

        /// <summary>
        /// Returns an HTML element (a link) that users can use to select the row.
        /// </summary>
        /// 
        /// <returns>
        /// The link that users can click to select the row.
        /// </returns>
        /// <param name="text">The inner text of the link element. If <paramref name="text"/> is empty or null, "Select" is used.</param>
        public IHtmlString GetSelectLink(string text = null)
        {
            if (string.IsNullOrEmpty(text))
                text = "Select";

            return WebTableRenderer<T>.GridLink(_grid, GetSelectUrl(), text);
        }

        /// <summary>
        /// Returns the URL that can be used to select the row.
        /// </summary>
        /// 
        /// <returns>
        /// The URL that is used to select a row.
        /// </returns>
        public string GetSelectUrl()
        {
            var queryString = new NameValueCollection(1);
            queryString[WebGrid.SelectionFieldName] = (_rowIndex + 1L).ToString(CultureInfo.CurrentCulture);

            return WebGrid.GetPath(queryString, new string[0]);
        }

        /// <summary>
        /// Returns the value of a <see cref="T:System.Web.Helpers.WebGridRow"/> member that is described by the specified binder.
        /// </summary>
        /// 
        /// <returns>
        /// true if the value of the item was successfully retrieved; otherwise, false.
        /// </returns>
        /// <param name="binder">The getter of the bound property member.</param><param name="result">When this method returns, contains an object that holds the value of the item described by <paramref name="binder"/>. This parameter is passed uninitialized.</param>
        public bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (TryGetRowIndex(binder.Name, out result) || ( _value != null && TryGetMember(_value, binder.Name, out result)))
                return true;

            return TryGetComplexMember(_value, binder.Name, out result);
        }

        internal bool TryGetMember(string memberName, out object result)
        {
            result = null;
            if (TryGetRowIndex(memberName, out result) || (_value != null &&  TryGetMember(_value, memberName, out result)))
                return true;

            return TryGetComplexMember(_value, memberName, out result);
        }

        /// <summary>
        /// Returns a string that represents all of the values of the <see cref="T:System.Web.Helpers.WebGridRow"/> instance.
        /// </summary>
        /// 
        /// <returns>
        /// A string that represents the row's values.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        private bool TryGetRowIndex(string memberName, out object result)
        {
            result = null;
            if (string.IsNullOrEmpty(memberName) || memberName != "ROW")
                return false;
            result = _rowIndex;

            return true;
        }

        private static bool TryGetComplexMember(T obj, string name, out object result)
        {
            result = null;
            string str = name;
            foreach (string name1 in str.Split('.'))
            {
                if (obj != null && TryGetMember(obj, name1, out result)) 
                    continue;

                result = null;
                return false;
            }
            return true;
        }

        private static bool TryGetMember(T obj, string name, out object result)
        {
            PropertyInfo property = obj.GetType()
                .GetProperty(name,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

            if (property != null && property.GetIndexParameters().Length == 0)
            {
                result = property.GetValue(obj, null);
                return true;
            }

            result = null;
            return false;
        }
    }
}
