using System;
using System.Web.Helpers;

namespace MvcBootEx.Grid
{
    public class SortInfo : IEquatable<SortInfo>
    {
        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }

        public bool Equals(SortInfo other)
        {
            if (other != null && string.Equals(this.SortColumn, other.SortColumn, StringComparison.OrdinalIgnoreCase))
                return this.SortDirection == other.SortDirection;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            SortInfo other = obj as SortInfo;
            return other != null ? this.Equals(other) : base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.SortColumn.GetHashCode();
        }
    }
}
