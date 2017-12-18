using System;

namespace HdUtilities.Attributes
{
    public class CsvColumnAttribute : Attribute
    {
        public int ColumnIndex { get; }
        public string ColumnHeader { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex">1-based index</param>
        /// <param name="columnHeader"></param>
        public CsvColumnAttribute(int columnIndex, string columnHeader)
        {
            if (columnIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            ColumnHeader = columnHeader;
            ColumnIndex = columnIndex;
        }
    }
}