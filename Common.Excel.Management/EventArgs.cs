using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace Common.Excel.Management
{
    public class HyperlinkEventArg : EventArgs
    {

        public HyperlinkEventArg() { }

        public HyperlinkEventArg(string semobjID)
        {
            LinkTag = semobjID;
        }

        public string LinkTag
        {
            get;
            set;
        }

        public object Parameter
        {
            get;
            set;
        }

    }

    public class DisplayAlertDisabler : IDisposable
    {
        Microsoft.Office.Interop.Excel.Application application;

        public DisplayAlertDisabler(Microsoft.Office.Interop.Excel.Application app)
        {
            application = app;
            if (application != null)
            {
                application.DisplayAlerts = false;
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        #endregion

        void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (application != null)
                {
                    application.DisplayAlerts = true;
                }

            }
        }
    }

    public class ExcelTable
    {
        Worksheet sheet;
        ListObject table;
        //Array tableValue;
        IExcelManagement excelManager;
        Dictionary<string, int> tableColumns;
        int rowStart;
        int columnStart;
        public string Name
        {
            get
            {
                if (table != null)
                    return table.Name;
                return String.Empty;
            }
        }

        private int columnCount;

        Dictionary<string, int> TableColumns
        {
            get
            {
                if (tableColumns == null)
                {
                    tableColumns = new Dictionary<string, int>();
                    if (table != null)
                    {
                        for (int i = 1; i <= columnCount; i++)
                        {
                            tableColumns.Add(table.ListColumns[i].Name, i);
                        }

                    }
                }
                return tableColumns;
            }
        }

        public void SetColumnWidth(int columnIndex, int width)
        {
            ListColumn column = table.ListColumns[columnIndex];
            column.Range.EntireColumn.ColumnWidth = width;
        }
        public ExcelTable(string sheetName, int tableIndex)
        {
            excelManager = ExcelManagementFactory.GetExcelManagement();
            sheet = excelManager.GetSheetByName(sheetName) as Worksheet;
            table = excelManager.GetTableByIndex(sheetName, tableIndex) as ListObject;
            if (table != null)
            {
                columnCount = table.ListColumns.Count;
                rowStart = table.Range.Row - 1;
                columnStart = table.Range.Column - 1;
                //tableValue = excelManager.RangeToArray(table.Range);
            }
            else
            {
                columnCount = -1;
                rowStart = 1;
                columnStart = 1;
            }
        }

        public object GetRange()
        {
            return table.Range;
        }

        private int getColumnIndex(string columnName)
        {
            try
            {
                return TableColumns[columnName];
            }
            catch (KeyNotFoundException)
            {

                return -1;
            }
        }

        private ListColumn GetColumn(string columnName)
        {
            int columnIndex = getColumnIndex(columnName);
            if (columnIndex > 0)
            {
                return table.ListColumns[columnIndex];
            }
            return null;

        }


        public Array GetColumnData(string columnName, bool withHeader)
        {
            ListColumn column = null;
            Range range = null;
            try
            {
                if (withHeader)
                {
                    column = GetColumn(columnName);
                    if (column == null) return null;
                    range = column.Range;
                    return excelManager.RangeToArray(range);
                }
                else
                {
                    column = GetColumn(columnName);
                    if (column == null) return null;
                    range = column.DataBodyRange;
                    return excelManager.RangeToArray(range);
                }
            }
            finally
            {
                excelManager.ReleaseComObject(range);
                excelManager.ReleaseComObject(column);
            }
        }

        private Range GetCell(int row, string columnName)
        {
            int columnIndex = getColumnIndex(columnName);
            if (columnIndex >= 0)
            {
                Range cell = sheet.Cells[row + rowStart, columnIndex + columnStart] as Range;
                return cell;
            }
            return null;
        }

        /// <summary>
        /// this method will retuan a specified cell value in excel
        /// the performance of this method is low , please take care of using this method
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object GetValueAt(int row, string columnName)
        {
            Range cell = GetCell(row, columnName);
            if (cell != null) return cell.Value2;
            return null;
            //if (tableValue == null) return null;
            //int rowStart = tableValue.GetLowerBound(0);
            //int columnStart = tableValue.GetLowerBound(1);
            //int columnIndex = getColumnIndex(columnName);
            //if (columnIndex < 0) return null;   //the specified column is not there
            //return tableValue.GetValue(row + rowStart - 1, columnIndex + columnStart - 1);

        }

        /// <summary>
        /// this method will retuan a specified cell value in excel
        /// the performance of this method is low , please take care of using this method
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object GetFormulaAt(int row, string columnName)
        {
            Range cell = GetCell(row, columnName);
            if (cell != null) return cell.Formula;
            return null;
            //if (tableValue == null) return null;
            //int rowStart = tableValue.GetLowerBound(0);
            //int columnStart = tableValue.GetLowerBound(1);
            //int columnIndex = getColumnIndex(columnName);
            //if (columnIndex < 0) return null;   //the specified column is not there
            //return tableValue.GetValue(row + rowStart - 1, columnIndex + columnStart - 1);

        }

        public void SetValueAt(int row, string columnName, object value)
        {
            Range cell = GetCell(row, columnName);
            if (cell != null) cell.Value2 = value;
        }

        public void ConditionalFormat(string formula1, int themeColor)
        {
            //int rowCount = table.Range.Rows.Count;
            Range range = null;
            FormatConditions conditions = null;
            FormatCondition cond = null;
            try
            {
                range = table.Range;
                conditions = range.FormatConditions;
                conditions.Add(XlFormatConditionType.xlExpression, XlFormatConditionOperator.xlEqual, formula1,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing);

                int length = conditions.Count;
                cond = conditions[length] as FormatCondition;
                cond.Interior.Pattern = XlPattern.xlPatternSolid;
                cond.Interior.PatternColorIndex = XlColorIndex.xlColorIndexAutomatic;
                cond.Interior.Color = themeColor;
                cond.Interior.TintAndShade = 0;
                cond.Interior.PatternTintAndShade = 0;
                cond.StopIfTrue = false;
            }
            finally
            {
                excelManager.ReleaseComObject(cond);
                excelManager.ReleaseComObject(conditions);
                excelManager.ReleaseComObject(range);
            }
        }

        public string GetColumnAddress(string columnName)
        {
            if (table != null)
            {
                ListColumn column = null;
                Range columnDataRange = null;
                try
                {
                    column = GetColumn(columnName);
                    if (column != null)
                    {
                        columnDataRange = column.DataBodyRange.get_Resize(1, 1);
                        return columnDataRange.get_Address(false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing);

                    }
                }
                finally
                {
                    excelManager.ReleaseComObject(columnDataRange);
                    excelManager.ReleaseComObject(column);
                }
            }
            return string.Empty;
        }

        public void SetStyle(string style)
        {
            table.TableStyle = style;
        }

        public void SetStyleForColumn(string columnName, string columnFormula, string columnFormat)
        {
            if (table != null)
            {
                ListColumn column = null;
                Range columnDataRange = null;
                try
                {
                    column = GetColumn(columnName);
                    if (column != null)
                    {
                        columnDataRange = column.DataBodyRange;
                        if (columnFormat != null)
                        {
                            columnDataRange.NumberFormat = columnFormat;
                        }
                        if (columnFormula != null)
                        {
                            columnDataRange.Formula = columnFormula;
                        }
                    }
                }
                finally
                {
                    excelManager.ReleaseComObject(columnDataRange);
                    excelManager.ReleaseComObject(column);
                }
            }

        }

        public void SetColumn(string columnName, Array data, bool withHeader)
        {
            ListColumn column = GetColumn(columnName);
            if (column != null)
            {
                if (withHeader)
                    column.Range.Value2 = data;
                else
                {
                    if (column.DataBodyRange != null)
                    {
                        column.DataBodyRange.Value2 = data;
                    }
                }

            }

        }

        public void SetTableData(Array data, bool dataWithHeader)
        {
            if (table != null)
            {
                table.DataBodyRange.Clear();
                excelManager.DrawArray(sheet, data, 1, dataWithHeader ? 1 : 2);
            }
        }

        public int GetColumnCount()
        {
            return columnCount;
        }
    }
}
