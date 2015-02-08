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
    public class WebTableRow : DynamicObject, IEnumerable<object>, IEnumerable
    {
        private const string ROW_INDEX_MEMBER_NAME = "ROW";

        private const BindingFlags BindFlags =
            BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        private WebTable _grid;
        private IDynamicMetaObjectProvider _dynamic;
        private int _rowIndex;
        private object _value;
        private IEnumerable<object> _values;

        /// <summary>
        /// Gets an object that contains a property member for each value in the row.
        /// </summary>
        /// 
        /// <returns>
        /// An object that contains each value in the row as a property.
        /// </returns>
        public object Value
        {
            get { return this._value; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.Helpers.WebGrid"/> instance that the row belongs to.
        /// </summary>
        /// 
        /// <returns>
        /// The <see cref="T:System.Web.Helpers.WebGrid"/> instance that contains the row.
        /// </returns>
        public WebTable WebGrid
        {
            get { return this._grid; }
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
                    throw new ArgumentException("Argument_Cannot_Be_Null_Or_Empty", "name");
                object result = (object) null;
                if (this.TryGetMember(name, out result))
                    return result;
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "WebGrid_ColumnNotFound",
                    new object[1]
                    {
                        (object) name
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
                if (index < 0 || index >= this._grid.ColumnNames.Count())
                    throw new ArgumentOutOfRangeException("index");
                return this.Skip(index).First();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Helpers.WebGridRow"/> class using the specified <see cref="T:System.Web.Helpers.WebGrid"/> instance, row value, and index.
        /// </summary>
        /// <param name="webGrid">The <see cref="T:System.Web.Helpers.WebGrid"/> instance that contains the row.</param><param name="value">An object that contains a property member for each value in the row.</param><param name="rowIndex">The index of the row.</param>
        public WebTableRow(WebTable webGrid, object value, int rowIndex)
        {
            this._grid = webGrid;
            this._value = value;
            this._rowIndex = rowIndex;
            this._dynamic = value as IDynamicMetaObjectProvider;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the values of the <see cref="T:System.Web.Helpers.WebGridRow"/> instance.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerator that can be used to iterate through the values of the row.
        /// </returns>
        public IEnumerator<object> GetEnumerator()
        {
            if (this._values == null)
                this._values = this._grid.ColumnNames.Select((Func<string, object>) (c => WebTable.GetMember(this, c)));
            return this._values.GetEnumerator();
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
            return WebTableRenderer.GridLink(this._grid, this.GetSelectUrl(), text);
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
            queryString[this.WebGrid.SelectionFieldName] =
                ((long) this._rowIndex + 1L).ToString((IFormatProvider) CultureInfo.CurrentCulture);
            return this.WebGrid.GetPath(queryString, new string[0]);
        }

        /// <summary>
        /// Returns the value of a <see cref="T:System.Web.Helpers.WebGridRow"/> member that is described by the specified binder.
        /// </summary>
        /// 
        /// <returns>
        /// true if the value of the item was successfully retrieved; otherwise, false.
        /// </returns>
        /// <param name="binder">The getter of the bound property member.</param><param name="result">When this method returns, contains an object that holds the value of the item described by <paramref name="binder"/>. This parameter is passed uninitialized.</param>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = (object) null;
            if (this.TryGetRowIndex(binder.Name, out result) ||
                this._dynamic != null && DynamicHelper.TryGetMemberValue((object) this._dynamic, binder, out result))
                return true;
            else
                return WebTableRow.TryGetComplexMember(this._value, binder.Name, out result);
        }

        internal bool TryGetMember(string memberName, out object result)
        {
            result = (object) null;
            if (this.TryGetRowIndex(memberName, out result) ||
                this._dynamic != null && DynamicHelper.TryGetMemberValue((object) this._dynamic, memberName, out result))
                return true;
            else
                return WebTableRow.TryGetComplexMember(this._value, memberName, out result);
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
            return this._value.ToString();
        }

        private bool TryGetRowIndex(string memberName, out object result)
        {
            result = (object) null;
            if (string.IsNullOrEmpty(memberName) || memberName != "ROW")
                return false;
            result = (object) this._rowIndex;
            return true;
        }

        private static bool TryGetComplexMember(object obj, string name, out object result)
        {
            result = (object) null;
            string str = name;
            char[] chArray = new char[1]
            {
                '.'
            };
            foreach (string name1 in str.Split(chArray))
            {
                if (obj == null || !WebTableRow.TryGetMember(obj, name1, out result))
                {
                    result = (object) null;
                    return false;
                }
                else
                    obj = result;
            }
            return true;
        }

        private static bool TryGetMember(object obj, string name, out object result)
        {
            PropertyInfo property = obj.GetType()
                .GetProperty(name,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
            if (property != (PropertyInfo) null && property.GetIndexParameters().Length == 0)
            {
                result = property.GetValue(obj, (object[]) null);
                return true;
            }
            else
            {
                result = (object) null;
                return false;
            }
        }
    }
}
