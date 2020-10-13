namespace SofTrust.Report.Core.Generator.DataAdapter
{
    using System.Collections.Generic;

    public static class ListDictionaryAdapter
    {
        public static List<Dictionary<string, object>> ToListDictionaryAdapt(this IData data)
        {
            var datas = new List<Dictionary<string, object>>();
            while (data.Read())
            {
                var dataRow = new Dictionary<string, object>();
                var unnamedColumnIndex = 1;
                for (int i = 0; i < data.FieldCount; i++)
                {
                    var fieldName = data.GetName(i);
                    if (string.IsNullOrWhiteSpace(fieldName))
                    {
                        fieldName = $"Column{unnamedColumnIndex++}";
                    }
                    dataRow.Add(dataRow.ContainsKey(fieldName) ? $"{fieldName}{i}" : fieldName, data.GetValue(i));
                }
                datas.Add(dataRow);
            }

            return datas;
        }
    }
}
