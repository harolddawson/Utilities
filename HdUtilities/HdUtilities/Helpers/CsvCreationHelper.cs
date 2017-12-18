using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HdUtilities.Attributes;

namespace HdUtilities.Helpers
{
    public static class CsvCreationHelper
    {
        public static void CreateCsvFile<T>(IEnumerable<T> enumerable, string destinationFilePath, bool includeHeaders)
        {
            List<string> lines = new List<string>();
            var props =
                typeof (T).GetProperties()
                    .Where(prop => Attribute.IsDefined(prop, typeof (CsvColumnAttribute)))
                    .OrderBy(p =>p.GetCustomAttributes(typeof (CsvColumnAttribute), true).Cast<CsvColumnAttribute>().First().ColumnIndex)
                    .ToList();
//            typeof(T).GetProperties().GetCustomAttributes(typeof (CsvColumnAttribute), true).FirstOrDefault();
            if (includeHeaders)
            {
                var headerLine = new StringBuilder();
                foreach (var a in props.Select(prop => prop.GetCustomAttributes(typeof (CsvColumnAttribute), true).Cast<CsvColumnAttribute>().First()))
                {
                    if (headerLine.Length > 0)
                    {
                        headerLine.Append(",");
                    }
                    headerLine.Append(a.ColumnHeader);
                }
                lines.Add(headerLine.ToString());
            }

            foreach (var item in enumerable)
            {
                var currentLine = new StringBuilder();
                foreach (var property in props)
                {
                    if (currentLine.Length > 0)
                    {
                        currentLine.Append(",");
                    }
                    currentLine.Append(property.GetValue(item));
                    //currentLine.Append(a.ColumnHeader);

                }
                lines.Add(currentLine.ToString());
            }

            File.WriteAllLines(destinationFilePath, lines);
        }
         
    }
}