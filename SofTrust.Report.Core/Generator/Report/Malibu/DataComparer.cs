namespace SofTrust.Report.Core.Generator.Report.Malibu
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    internal class DataComparer : IEqualityComparer<object>
    {
        private IEnumerable<string> fields;

        public DataComparer(IEnumerable<string> fields)
        {
            this.fields = fields;
        }

        public bool Equals([AllowNull] object x, [AllowNull] object y)
        {
            var xD = x as Dictionary<string, object>;
            var yD = y as Dictionary<string, object>;
            return fields.All(f => xD[f].ToString() == yD[f].ToString());
        }

        public int GetHashCode([DisallowNull] object obj)
        {
            var objD = obj as Dictionary<string, object>;
            return string.Join("", fields.Select(x => objD[x])).GetHashCode();
        }
    }
}
