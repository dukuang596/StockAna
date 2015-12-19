using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
//using Ganji.ExcelManagement.Config;
//using GanJi.SEM.UICommon;
//using GanJi.SEM.UICommon.Config;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Drawing;
using Core = Microsoft.Office.Core;

namespace Common.Excel.Management
{
    public class Excel2007Management : IExcelManagement
    {
        #region Members

        private event EventHandler<HyperlinkEventArg> hyperlinkClicked;
        static Regex regex = new Regex(@"(\+|-)?[0-9][0-9]*(\.[0-9]*)?", RegexOptions.Compiled);
        public event System.Action<bool> ExportingChanged;
        private object currentApplication;
        private bool isExporting = false;
        public readonly int MaxChars = 8203;
        public readonly int MaxRow = 1048576;
        public readonly int MaxColumn = 16384;
        public readonly int SheetNameMaxLength = 31;
        private readonly string SheetNameMore = "...";
        private readonly string DefaultSheetName = "DefaultName";
        private readonly string TablePrefix = "Table_";
        private readonly string Connector = " - ";
        private readonly ExcelInfo excelInfo = new ExcelInfo();
        private readonly static char[] SheetNameInvalidCharacters = new char[] { '\t', '\r', '\n', '\\', '/', '?', '*', '[', ']', ':', '：', '？', '／', '＼', '/', '＊' };
        
        #endregion

        #region Get Excel Object

        public object GetCurrentWorkbook(bool isCreate = true)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = (Microsoft.Office.Interop.Excel.Application)CurrentApplication;
            return excelApp.ActiveWorkbook ?? (isCreate ? CreateNewWorkbook() : null);
        }

        public object GetCurrentSheet()
        {
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)CurrentApplication;
            if (app.ActiveWorkbook != null)
                return app.ActiveWorkbook.ActiveSheet;
            else
                return null;
        }

        public object GetSheetByName(string sheetName)
        {
            return GetSheetByName(GetCurrentWorkbook(), sheetName);
        }

        public object GetSheetByName(object workbook, string sheetName)
        {
            if (workbook == null || string.IsNullOrEmpty(sheetName))
            {
                return null;
            }

            Workbook wb = workbook as Workbook;
            foreach (object sheet in wb.Sheets)
            {
                if (string.Compare(GetSheetName(sheet), sheetName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return sheet;
                }
            }
            return null;
        }

        #endregion

        #region Create Excel Object

        public object CreateNewSheet(string sheetName, bool beforeCurrentSheet = true)
        {
            sheetName = ExcelUtils.RemoveCharsInString(sheetName.Trim(), SheetNameInvalidCharacters) ?? string.Empty;
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = DefaultSheetName;
            }

            if (sheetName[0] == '\'')
            {
                sheetName = string.Concat(' ', sheetName);
            }

            if (sheetName[sheetName.Length - 1] == '\'')
            {
                sheetName = string.Concat(sheetName, ' ');
            }

            if (sheetName.Length > SheetNameMaxLength - 9)
            {
                sheetName = string.Concat(sheetName.Substring(0, SheetNameMaxLength - 9), SheetNameMore);
            }

            Workbook currentWorkbook = (Workbook)GetCurrentWorkbook();
            Dictionary<string, string> names = new Dictionary<string, string>();

            foreach (var sheet in currentWorkbook.Worksheets)
            {
                Worksheet ws = (Worksheet)sheet;
                names.Add(ws.Name.ToLower(), string.Empty);
            }
            Worksheet newSheet;
            int index = 0;
            string newname = sheetName;
            while (true)
            {
                if (names.ContainsKey(newname.ToLower()))
                {
                    newname = string.Concat(sheetName, Connector, index);
                    index++;
                    continue;
                }

                if (newname.Length > SheetNameMaxLength)
                {
                    throw new ExcelException(SR.ExcelError_FailedToCreateNewSheet);
                }
                newSheet = beforeCurrentSheet
                    ? (Worksheet)currentWorkbook.Worksheets.Add(currentWorkbook.ActiveSheet, Type.Missing, Type.Missing, Type.Missing)
                    : (Worksheet)currentWorkbook.Worksheets.Add(Type.Missing, currentWorkbook.ActiveSheet, Type.Missing, Type.Missing);
                newSheet.Name = newname;
                names = null;
                break;
            }
            return newSheet;
        }

        public object CreateNewWorkbook()
        {
            Microsoft.Office.Interop.Excel.Application excelApp = (Microsoft.Office.Interop.Excel.Application)this.CurrentApplication;
            Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            return workbook;
        }

        #endregion

        #region Event

        public event EventHandler<HyperlinkEventArg> HyperlinkClicked
        {
            add { hyperlinkClicked += value; }
            remove { hyperlinkClicked -= value; }
        }

        #endregion

        public string GetSheetName(object workSheet)
        {
            string _sheetName = string.Empty;
            if (workSheet == null) return _sheetName;

            if (workSheet is Worksheet)
            {
                Worksheet wsheet = workSheet as Worksheet;
                _sheetName = wsheet.Name;
            }
            else if (workSheet is Chart)
            {
                Chart csheet = workSheet as Chart;
                _sheetName = csheet.Name;
            }
            else
            {
                Type itemType = workSheet.GetType();
                PropertyInfo porpInfo = itemType.GetProperty("Name");
                _sheetName = porpInfo.GetValue(workSheet, null).ToString();
            }

            return _sheetName;
        }

        public string GetSheetName(int sheetIndex)
        {
            var book = GetCurrentWorkbook() as Workbook;
            if (book == null) return string.Empty;

            var sheet = book.Sheets[sheetIndex] as Worksheet;
            if (sheet == null) return string.Empty;
            return sheet.Name;
        }

        public string GetCurrentSheetName()
        {
            return GetSheetName(GetCurrentSheet());
        }

        public void StartExport()
        {
            this.isExporting = true;
            RiseExportingChangedEvent();
        }

        public void EndExport()
        {
            this.isExporting = false;
            RiseExportingChangedEvent();
        }

        public void BeginUpdate()
        {
            if (!excelInfo.IsInUsed)
            {
                lock (excelInfo)
                {
                    if (!excelInfo.IsInUsed)
                    {
                        Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)CurrentApplication;
                        excelInfo.IsInUsed = true;
                        excelInfo.ScreenUpdating = app.ScreenUpdating;
                        excelInfo.Calculation = app.Calculation;
                        try
                        {
                            app.ScreenUpdating = false;
                            app.Calculation = XlCalculation.xlCalculationManual;//手动计算
                        }
                        catch
                        {
                            excelInfo.CannotChangeCalculation = true;
                        }
                    }
                }
            }
        }

        public void EndUpdate()
        {
            if (excelInfo.IsInUsed)
            {
                lock (excelInfo)
                {
                    if (excelInfo.IsInUsed)
                    {
                        Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)this.CurrentApplication;
                        app.ScreenUpdating = excelInfo.ScreenUpdating;
                        if (!excelInfo.CannotChangeCalculation)
                        {
                            try
                            {
                                app.Calculation = excelInfo.Calculation;
                            }
                            catch
                            { }
                        }
                        excelInfo.IsInUsed = false;
                        app.Visible = true;
                    }
                }
            }
        }

        public void SetAutoExtentTable(bool extend)
        {
            Microsoft.Office.Interop.Excel.Application app = CurrentApplication as Microsoft.Office.Interop.Excel.Application;
            app.AutoCorrect.AutoExpandListRange = extend;
        }

        public object GetSelectedSingleRange()
        {
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)CurrentApplication;
            Range range = app.Selection as Range;
            if (range != null)
            {
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;

                if (rowCount > 1 || colCount > 1)
                {
                    return range.get_Resize(1, 1);
                }
            }
            return range;
        }

        public int GetColumnIndex(object cell)
        {
            Range cellRange = cell as Range;
            return cellRange == null ? 0 : cellRange.Column;
        }

        public int GetRowIndex(object cell)
        {
            Range cellRange = cell as Range;
            return cellRange == null ? 0 : cellRange.Row;
        }

        public object CurrentApplication
        {
            get
            {
                return this.currentApplication;
            }
            set
            {
                this.currentApplication = value;
            }
        }

        public bool IsExporting
        {
            get { return this.isExporting; }
        }

        #region Function

        private void RiseExportingChangedEvent()
        {
            if (ExportingChanged != null)
            {
                ExportingChanged(this.isExporting);
            }
        }

        private void Normalize(Array dataArray)
        {
            if (dataArray == null || dataArray.Rank != 2)
            {
                return;
            }

            int start0 = dataArray.GetLowerBound(0);
            int start1 = dataArray.GetLowerBound(1);

            int row = dataArray.GetLength(0);
            int column = dataArray.GetLength(1);
            int len1 = row + start0;
            int len2 = column + start1;
            for (int i = start0; i < len1; i++)
            {
                for (int j = start1; j < len2; j++)
                {
                    bool reset = false;
                    string str = dataArray.GetValue(i, j) as string;

                    if (str != null)
                    {
                        if (str.Length > 1 && ((str[0] == '=' && !regex.IsMatch(str.Substring(1))) || str[0] == '\''))
                        {
                            str = string.Concat('\'', str);
                            reset = true;
                        }
                        if (str.Length > MaxChars)
                        {
                            str = str.Substring(0, MaxChars);
                            reset = true;
                        }
                    }
                    if (reset)
                    {
                        dataArray.SetValue(str, i, j);
                    }
                }
            }
        }

        public void ReleaseComObject(object obj)
        {
            int count = 0;
            try
            {
                if (obj != null)
                {
                    count = System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                }
            }
            catch
            {}
            finally
            {
                obj = null;
            }
        }

        public void ApplyGradientCell(object rangeObj, string[] colors, double degree)
        {
            if (colors.Length < 2) return;
            Range range = rangeObj as Range;
            Interior interior = range.Interior;
            interior.Pattern = XlPattern.xlPatternLinearGradient;
            LinearGradient gradient = (interior.Gradient as LinearGradient);
            gradient.Degree = degree;
            gradient.ColorStops.Clear();
            double position = 1.0 / (colors.Length - 1);
            for (int i = 0; i < colors.Length; i++)
            {
                gradient.ColorStops.Add(i == colors.Length - 1 ? 1 : (i * position));
                gradient.ColorStops[gradient.ColorStops.Count].Color = GetColor(colors[i]);
                gradient.ColorStops[gradient.ColorStops.Count].TintAndShade = 0;
            }
        }

        public int GetColor(string color)
        {
            try
            {
                Color c = ExcelUtils.GetColor(color);
                return 0x00FFFFFF & ((c.B << 16) | (c.G << 8) | c.R);
            }
            catch (Exception e)
            {
                throw new ExcelException(e.Message);
            }
        }

        #endregion

        public void ChangeTableOption(string sheetName, int tableIndex, bool showTableStyleColumnStrips, bool showTableStyleFirstColumn, bool showTableStyleLastColumn, bool showTableStyleRowStripes, bool showTotals)
        {
            Worksheet sheet = this.GetSheetByName(sheetName) as Worksheet;
            if (sheet != null && sheet.ListObjects.Count > 0)
            {
                sheet.ListObjects[tableIndex].ShowTableStyleColumnStripes = showTableStyleColumnStrips;
                sheet.ListObjects[tableIndex].ShowTableStyleFirstColumn = showTableStyleFirstColumn;
                sheet.ListObjects[tableIndex].ShowTableStyleLastColumn = showTableStyleLastColumn;
                sheet.ListObjects[tableIndex].ShowTableStyleRowStripes = showTableStyleRowStripes;
                sheet.ListObjects[tableIndex].ShowTotals = showTotals;
            }
        }

        public object DrawArray(object sheet, Array array, int x, int y, string dataFormat = null)
        {
            Range startCell = null;
            Range outputRange = null;
            try
            {
                Normalize(array);
                int startRowIndex = 0;
                int rows = array == null ? 0 : array.GetLength(0);
                int row = y;
                startCell = (Range)((Worksheet)sheet).Cells[y, x];
                outputRange = startCell;
                Array arr = ExcelUtils.GetSubDataTableArray(array, 0, (row + rows > this.MaxRow) ? (this.MaxRow - row + 1) : rows);
                if (arr != null)
                {
                    int arrRow = arr.GetLength(0);
                    int columnRow = arr.GetLength(1);
                    if (arrRow > 0 && columnRow > 0)
                    {
                        outputRange = startCell.get_Resize(arrRow, columnRow);
                        if (!string.IsNullOrEmpty(dataFormat))
                        {
                            outputRange.NumberFormat = dataFormat;
                        }
                        startRowIndex = arrRow;
                        outputRange.Value2 = arr;
                    }
                }
                while (startRowIndex < rows)
                {
                    Array arrMore = ExcelUtils.GetSubDataTableArray(array, startRowIndex, this.MaxRow);
                    outputRange = (Range)DisplayDataToNewSheet(this.GetCurrentSheetName(), arrMore);
                    startRowIndex += this.MaxRow - 1;
                }
                return outputRange;
            }
            catch (Exception ex)
            {
                throw new ExcelException(ex.Message, ex);
            }
            finally
            {
                ReleaseComObject(startCell);
                startCell = null;
            }
        }

        private Range DisplayDataToNewSheet(string p, Array arrMore)
        {
            throw new NotImplementedException();
        }

        public void SetGridLineDisplay(bool show)
        {
            ((Microsoft.Office.Interop.Excel.Application)this.CurrentApplication).ActiveWindow.DisplayGridlines = show;
        }

        public void ChangeSheetBackGroundColor(object sheet, Color color)
        {
            Worksheet worksheet = sheet as Worksheet;
            ChangeBackgroundColor(worksheet.Application.Cells, color);
        }

        public void ChangeBackgroundColor(object range, Color color, int startX = 0, int startY = 0, int rows = 0, int columns = 0)
        {
           Range r = range as Range;
            if (r == null)
            {
                return;
            }

            if (columns > 0 || rows > 0)// resize first
            {
                r = r.get_Resize(rows <= 0 ? r.Rows.Count : rows, columns <= 0 ? r.Columns.Count : columns);
            }

            if (startX != 0 || startY != 0)
            {
                r = r.get_Offset(startY, startX);
            }

            Interior interior = r.Interior;
            interior.Pattern = XlPattern.xlPatternSolid;
            interior.PatternColorIndex = XlColorIndex.xlColorIndexAutomatic;
            interior.PatternTintAndShade = 0;
            interior.TintAndShade = 0;

            interior.Color = GetColor(color);
        }

        public int GetColor(Color color)
        {
            try
            {
                return 0x00FFFFFF & ((color.B << 16) | (color.G << 8) | color.R);
            }
            catch (Exception ex)
            {
                throw new ExcelException(ex.Message, ex);
            }
        }

        public void ChangeBackgroundColor(object range, string color, int startX = 0, int startY = 0, int rows = 0, int columns = 0)
        {
            Range r = range as Range;
            if (r == null)
            {
                return;
            }

            // resize first
            if (columns > 0 || rows > 0)
            {
                r = r.get_Resize(rows <= 0 ? r.Rows.Count : rows, columns <= 0 ? r.Columns.Count : columns);
            }

            if (startX != 0 || startY != 0)
            {
                r = r.get_Offset(startY, startX);
            }

            Interior interior = r.Interior;
            interior.Pattern = XlPattern.xlPatternSolid;
            interior.PatternColorIndex = XlColorIndex.xlColorIndexAutomatic;
            interior.PatternTintAndShade = 0;
            interior.TintAndShade = 0;


            interior.Color = GetColor(color);
        }

        public object GetAllCell(object sheet)
        {
            Worksheet sheetObj = sheet as Worksheet;
            if (sheetObj != null)
            {
                return sheetObj.Cells;
            }
            return null;
        }

        public void ChangeFontSize(object range, int pt)
        {
            Range cells = range as Range;
            if (cells != null)
            {
                cells.Font.Size = pt;
            }
        }

        public void ChangeFont(object range, string fontName)
        {
            Range cells = range as Range;
            if (cells != null)
            {
                cells.Font.Name = fontName;
            }
        }

        public void MergeRange(object sheet, int x, int y, int col, int row, bool across)
        {
            Range startCell = (Range)((Worksheet)sheet).Cells[y, x];
            Range outputRange = startCell.get_Resize(row, col);
            outputRange.Merge(across);
        }

        public void SetCenterAlignment(object range, int width = 0, int height = 0, bool isH = true, bool isV = true)
        {
            Range cells = range as Range;
            if (cells != null)
            {
                if (width > 0 && height > 0)
                {
                    cells = cells.get_Resize(height, width);
                }
                if (isH)
                    cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                if (isV)
                    cells.VerticalAlignment = XlVAlign.xlVAlignCenter;
            }
        }

        public void ChangeFontColor(object range, Color color, int width = 0, int height = 0)
        {
            Range cells = range as Range;
            if (cells != null)
            {
                if (width > 0 && height > 0)
                {
                    cells = cells.get_Resize(height, width);
                }
                cells.Font.Color = ColorTranslator.ToOle(color);
            }
        }

        public void SetRangeHeight(object rangeObj, object height)
        {
            Range range = rangeObj as Range;
            range.EntireRow.RowHeight = height;
        }

        public object GetRange(int x, int y, int w, int h)
        {
            return GetRange(GetCurrentSheet(), x, y, w, h);
        }

        public object GetRange(object sheetObj, int x, int y, int w, int h)
        {
            Worksheet sheet = sheetObj as Worksheet;
            if (sheet != null)
            {
                Range range = sheet.Cells[y, x] as Range;
                range = range.get_Resize(h < 1 ? 1 : h, w < 1 ? 1 : w);
                return range;
            }
            return null;
        }

        public void ApplayDefaultBorders(object range, bool top, bool left, bool right, bool bottom, bool insideVertical, bool insideHorizontal, Color? color)
        {
            Range destRange = range as Range;
            Borders borders = destRange.Borders;
            bool automatic = !color.HasValue;
            int oleColor = 0;
            if (!automatic)
            {
                oleColor = GetColor(color.Value);
            }
            if (top)
            {
                Border topBorder = borders.get_Item(XlBordersIndex.xlEdgeTop);
                topBorder.LineStyle = XlLineStyle.xlContinuous;
                if (automatic)
                {
                    topBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                }
                else
                {
                    topBorder.Color = oleColor;
                }
                topBorder.TintAndShade = 0;
                topBorder.Weight = XlBorderWeight.xlThin;
            }
            if (left)
            {
                Border leftBorder = borders.get_Item(XlBordersIndex.xlEdgeLeft);
                leftBorder.LineStyle = XlLineStyle.xlContinuous;
                if (automatic)
                {
                    leftBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                }
                else
                {
                    leftBorder.Color = oleColor;
                }
                leftBorder.TintAndShade = 0;
                leftBorder.Weight = XlBorderWeight.xlThin;
            }
            if (right)
            {
                Border rightBorder = borders.get_Item(XlBordersIndex.xlEdgeRight);
                rightBorder.LineStyle = XlLineStyle.xlContinuous;
                if (automatic)
                {
                    rightBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                }
                else
                {
                    rightBorder.Color = oleColor;
                }
                rightBorder.TintAndShade = 0;
                rightBorder.Weight = XlBorderWeight.xlThin;
            }
            if (bottom)
            {
                Border bottomBorder = borders.get_Item(XlBordersIndex.xlEdgeBottom);
                bottomBorder.LineStyle = XlLineStyle.xlContinuous;
                if (automatic)
                {
                    bottomBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                }
                else
                {
                    bottomBorder.Color = oleColor;
                }
                bottomBorder.TintAndShade = 0;
                bottomBorder.Weight = XlBorderWeight.xlThin;
            }
            if (insideHorizontal)
            {
                Border insideHorizontalBorder = borders.get_Item(XlBordersIndex.xlInsideHorizontal);
                insideHorizontalBorder.LineStyle = XlLineStyle.xlContinuous;
                if (automatic)
                {
                    insideHorizontalBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                }
                else
                {
                    insideHorizontalBorder.Color = oleColor;
                }
                insideHorizontalBorder.TintAndShade = 0;
                insideHorizontalBorder.Weight = XlBorderWeight.xlThin;
            }
            if (insideVertical)
            {
                Border insideVerticalBorder = borders.get_Item(XlBordersIndex.xlInsideVertical);
                insideVerticalBorder.LineStyle = XlLineStyle.xlContinuous;
                if (automatic)
                {
                    insideVerticalBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                }
                else
                {
                    insideVerticalBorder.Color = oleColor;
                }
                insideVerticalBorder.TintAndShade = 0;
                insideVerticalBorder.Weight = XlBorderWeight.xlThin;
            }
        }

        public void SetColumnWidth(object rangeObj, object width)
        {
            Range range = rangeObj as Range;
            if (range != null)
            {
                range.ColumnWidth = width;
            }
        }

        public object GetColumnRange(object sheetObj, int start, int end)
        {
            Worksheet sheet = sheetObj as Worksheet;
            string startAddr = ExcelUtils.GetRangeAdress(1, start);
            if (end != start)
            {
                string endAddr = ExcelUtils.GetRangeAdress(1, end);
                return sheet.get_Range(startAddr, endAddr).EntireColumn;
            }
            return sheet.get_Range(startAddr, Type.Missing).EntireColumn;
        }

        public void FreezePanes(int startX, int startY)
        {
            Worksheet sheet = this.GetCurrentSheet() as Worksheet;
            if (sheet == null)
            {
                return;
            }
            Range range = sheet.Cells[startY, startX] as Range;
            range.Select();
            ((Microsoft.Office.Interop.Excel.Application)this.CurrentApplication).ActiveWindow.FreezePanes = true;
        }

        public void SelectRange(int startX, int startY, int width = 1, int height = 1)
        {
            Worksheet sheet = this.GetCurrentSheet() as Worksheet;
            if (sheet == null)
            {
                return;
            }

            Range range = sheet.Cells[startY, startX] as Range;
            range.get_Resize(height < 1 ? 1 : height, width < 1 ? 1 : width);
            range.Select();
        }

        public void SelectRange(object rangeObj)
        {
            Range range = rangeObj as Range;
            if (range != null)
            {
                range.Select();
            }
        }

        //public object BuildExportTable(object sheet, string style, int startx, int starty, object[,] data)
        //{
        //    Worksheet ws = sheet as Worksheet;
        //    if (ws == null) throw new ArgumentException("sheet object is null!");
        //    Range startCell = ws.Cells[starty, startx] as Range;
        //    Range range = null;
        //    if (startCell != null)
        //    {
        //        range = startCell.get_Resize(data.GetLength(0), data.GetLength(1));
        //        string[] styles = style.Split(',');
        //        string viewName = styles[0];

        //        if (data.GetLength(0) < 2)
        //        {
        //            range = range.get_Resize(range.Rows.Count + 1, range.Columns.Count);
        //        }
        //        Array firstRow = GetFirstRow(data);
        //        range.Value2 = firstRow;
        //        ConfigService configService = ConfigService.GetConfigService();
        //        ExcelSettingsSection execlSection = configService.GetConfig<ExcelSettingsSection>();
        //        TableElement conf = execlSection.Views.All[viewName].Table;

        //        this.ApplyColumnNumberFormat(range, conf, "CNY");
        //        this.Normalize(data);
        //        range.Value2 = data;
        //        if (data.GetLength(0) <= 1)
        //        {
        //            object[,] newArray = RangeToArray(range) as object[,];
        //            for (int i = 1; i <= newArray.GetLength(1); i++)
        //            {
        //                newArray[2, i] = string.Empty;
        //            }
        //            range.Value2 = newArray;
        //        }

        //        this.FormatRangeAsTable(range, conf.TableStyle);
        //    }
        //    return range;
        //}

        //public void ApplyColumnNumberFormat(Range range, TableElement conf, string currencyCode)
        //{
        //    if (conf == null)
        //    {
        //        return;
        //    }
        //    Range ran = range as Range;
        //    int rowCount = ran.Rows.Count;
        //    for (int i = 0; i < ran.Columns.Count; i++)
        //    {
        //        Range cell = ran.get_Resize(rowCount, 1);
        //        Range firstcell = cell = cell.get_Offset(0, i);
        //        firstcell = firstcell.get_Resize(1, 1);
        //        string colName = Convert.ToString(firstcell.Text);
        //        ColumnElement col = conf.Columns == null
        //            ? null
        //            : conf.Columns.FindMatchedColumn(colName);
        //        if (col != null)
        //        {
        //            string DataFormatStyle = col.GetColumnFormatStyle(currencyCode);

        //            if (!string.IsNullOrEmpty(DataFormatStyle))
        //            {
        //                firstcell.EntireColumn.NumberFormat = DataFormatStyle;
        //            }

        //        }
        //        else
        //        {
        //            firstcell.EntireColumn.NumberFormat = "@";
        //        }
        //    }
        //}

        public object FormatRangeAsTable(object range, string style)
        {
            string guid = TablePrefix + ExcelUtils.GenerateGuid();
            return FormatRangeAsTable(range, guid, style);
        }

        public object FormatRangeAsTable(object range, string tableName, string style)
        {
            Range r = range as Range;
            if (r == null)
            {
                return new ArgumentException("Range is null!");
            }
            Worksheet sheet = r.Worksheet;
            sheet.ListObjects.Add(XlListObjectSourceType.xlSrcRange, range, Missing.Value, XlYesNoGuess.xlYes, Missing.Value).Name = tableName;
            if (!string.IsNullOrEmpty(style))
                sheet.ListObjects[tableName].TableStyle = style;
            return sheet.ListObjects[tableName];
        }

        public Array GetFirstRow(object[,] data)
        {
            int columnLength = data.GetLength(1);
            object[,] firstRow = new object[1, columnLength];

            for (int i = 0; i < columnLength; i++)
            {
                firstRow[0, i] = data[0, i];
            }
            return firstRow;
        }

        public Array RangeToArray(object rangeObj)
        {
            Worksheet sheet = null;
            object tempRange = null;
            try
            {
                if (rangeObj == null)
                    return new object[0, 0];
                Range range = (Range)rangeObj;
                int rows = range.Rows.Count;
                int columns = range.Columns.Count;


                if (rows > 1 || columns > 1)
                {
                    if (columns == 1)
                        return (Array)range.Value2;
                    int maxRow = 5000;
                    if (rows > maxRow)
                    {
                        object[,] totalArray = Array.CreateInstance(typeof(object), new int[] { rows, columns }, new int[] { 1, 1 }) as object[,];
                        Array currentArray = null;
                        int startIndex = 0;
                        sheet = range.Parent as Worksheet;
                        if (sheet == null)
                        {
                            sheet = GetCurrentSheet() as Worksheet;
                        }
                        while (startIndex < totalArray.Length)
                        {
                            tempRange = GetRange(sheet, range.Column, range.Row + startIndex / columns, columns, rows - startIndex / columns > maxRow ? maxRow : range.Rows.Count - startIndex / columns);
                            currentArray = RangeToArray(tempRange);
                            Array.Copy(currentArray, 1, totalArray, startIndex + 1, currentArray.Length);
                            startIndex += currentArray.Length;
                        }
                        return totalArray;
                    }
                    else
                    {
                        return (Array)range.Value2;
                    }
                }
                else
                {
                    object[,] array = new object[1, 1];
                    array[0, 0] = range.Value2;
                    return array;
                }
            }
            finally
            {
                ReleaseComObject(tempRange);
                ReleaseComObject(sheet);
            }
        }

        public object GetTableLastRow(int tableIndex)
        {
            ListObject listObject = GetTableByIndex(GetCurrentSheet(), tableIndex);
            if (listObject != null)
            {
                Range sourceCell = listObject.Range;
                return
                    sourceCell.get_Resize(1, 1).get_Offset(
                        listObject.ListRows.Count < 1 ? 1 : listObject.ListRows.Count, 0);
            }
            return null;
        }

        private ListObject GetTableByIndex(object sheetObj, int tableIndex)
        {
            Worksheet sheet = sheetObj as Worksheet;
            if (sheet == null)
            {
                throw new ArgumentException("Error sheet!");
            }
            return sheet.ListObjects.Count < tableIndex ? null : sheet.ListObjects[tableIndex];
        }

        public string GetTableName(object sheetObj, int tableIndex)
        {
            ListObject table = GetTableByIndex(sheetObj, tableIndex);
            return table.Name;
        }

        public string ProductCode
        {
            get { return ((Microsoft.Office.Interop.Excel.Application)this.currentApplication).ProductCode; }
        }

        public void SetLink(object range, string callBackFuncName, string displayText, string tooltip)
        {
            Range r = range as Range;
            string callBack = null;

            if (!string.IsNullOrEmpty(callBackFuncName))
            {
                callBack = callBackFuncName;
            }
            r.Worksheet.Hyperlinks.Add(r, callBack, Type.Missing, string.IsNullOrEmpty(tooltip) ? Type.Missing : tooltip, string.IsNullOrEmpty(displayText) ? Type.Missing : displayText);
        }

        public void ChangeFontStyle(object range, FontStyle fontStyle, int width = 0, int height = 0)
        {
            Range cells = range as Range;
            if (cells != null)
            {
                if (width > 0 && height > 0)
                {
                    cells = cells.get_Resize(height, width);
                }

                cells.Font.Bold = (fontStyle & FontStyle.Bold) > 0;
                cells.Font.Italic = (fontStyle & FontStyle.Italic) > 0;
                cells.Font.Underline = (fontStyle & FontStyle.Underline) > 0;
                cells.Font.Strikethrough = (fontStyle & FontStyle.Strikeout) > 0;
            }
        }

        public void UpdateHyperlinkStyle(string fontName, double fontSize)
        {
            Workbook workbook = GetCurrentWorkbook() as Workbook;
            Styles styles = workbook.Styles;
            foreach (Style existedStyle in styles)
            {
                if (existedStyle.Name == "Hyperlink" || existedStyle.Name == "超链接")
                {
                    Microsoft.Office.Interop.Excel.Font font = existedStyle.Font;
                    string name = (string)font.Name;
                    if (name != fontName)
                    {
                        font.Name = fontName;
                    }
                    double size = (double)font.Size;
                    if (size != fontSize)
                    {
                        font.Size = fontSize;
                    }
                    break;
                }
            }
        }

        public void DrawButtonStyleShape(object sheetObj, string text, string action, System.Drawing.Font font, string chineseFontName, float x, float y, float width, float height)
        {
            Worksheet sheet = sheetObj as Worksheet;
            Shape newShape = sheet.Shapes.AddShape(Microsoft.Office.Core.MsoAutoShapeType.msoShapeRoundedRectangle, x, y, width, height);
            newShape.Placement = XlPlacement.xlFreeFloating;
            LineFormat line = newShape.Line;
            line.Weight = 0.75f;
            line.ForeColor.RGB = GetColor(Color.FromArgb(166,166,166));
            ShadowFormat shadow = newShape.Shadow;
            shadow.Style = Core.MsoShadowStyle.msoShadowStyleOuterShadow;
            shadow.Blur = 2;
            shadow.Transparency = 0.88f;
            shadow.OffsetY = 1;
            FillFormat fill = newShape.Fill;
            fill.TwoColorGradient(Core.MsoGradientStyle.msoGradientHorizontal, 2);
            Core.GradientStops gradientStops = fill.GradientStops;
            if (gradientStops.Count == 2)
            {
                Core.GradientStop gradientStop1 = gradientStops[1];
                gradientStop1.Position = 1;
                gradientStop1.Color.RGB = GetColor(Color.FromArgb(255,255,255));
                gradientStop1.Transparency = 0;
                Core.GradientStop gradientStop2 = gradientStops[2];
                gradientStop2.Position = 0;
                gradientStop2.Color.RGB = GetColor(Color.FromArgb(230,230,230));
                gradientStop2.Transparency = 0;
            }
            newShape.TextFrame.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            newShape.TextFrame.VerticalAlignment = XlVAlign.xlVAlignCenter;
            Characters characters = newShape.TextFrame.Characters(Type.Missing, Type.Missing);
            Microsoft.Office.Interop.Excel.Font cFont = characters.Font;
            cFont.Name = font.Name;
            cFont.Color = GetColor(Color.FromArgb(0,112,192));
            cFont.Bold = font.Bold;
            cFont.Size = font.Size;
            characters.Caption = characters.Text = text;

            newShape.TextFrame2.TextRange.Font.NameFarEast = chineseFontName;
            newShape.OnAction = action;
        }

        public object GetTableRange(object sheetObj, int tableIndex)
        {
            return GetTableByIndex(sheetObj, tableIndex).Range;
        }

        //public void ApplyColumnFormat(object range, TableElement conf, bool formatCondition, bool autoFit, string languageCode)
        //{
        //    if (conf == null)
        //    {
        //        return;
        //    }
        //    Range ran = range as Range;
        //    int rowCount = ran.Rows.Count;
        //    int columnCount = ran.Columns.Count;
        //    for (int i = 0; i < columnCount; i++)
        //    {
        //        Range cell = ran.get_Resize(rowCount, 1).get_Offset(0, i);
        //        Range firstcell = cell.get_Resize(1, 1);
        //        string colName = Convert.ToString(firstcell.Text);
        //        ColumnElement col = conf.Columns == null
        //            ? null
        //            : conf.Columns.FindMatchedColumn(colName);
        //        if (col != null)
        //        {
        //            Range colRange = cell.get_Resize(rowCount - 1, 1).get_Offset(1, 0);

        //            if (!string.IsNullOrEmpty(col.BackgroundColor))
        //            {
        //                ChangeBackgroundColor(colRange, col.BackgroundColor);
        //            }

        //            if (!string.IsNullOrEmpty(col.FontColor))
        //            {
        //                ChangeFontColor(colRange, ExcelUtils.GetColor(col.FontColor));
        //            }
        //            if (!string.IsNullOrEmpty(col.GradientColors))
        //            {
        //                ApplyGradientCell(colRange, col.GradientColors.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries), 90);
        //            }

        //            if (!string.IsNullOrEmpty(col.ColumnWidth))
        //            {
        //                colRange.EntireColumn.ColumnWidth = int.Parse(col.ColumnWidth);
        //            }

        //            if (!string.IsNullOrEmpty(col.Formula))
        //            {
        //                colRange.Formula = col.Formula;
        //            }
        //            cell = ran.get_Resize(ran.Rows.Count, 1).get_Offset(0, i);
        //            string DataFormatStyle = col.GetColumnFormatStyle("CNY");

        //            if (!string.IsNullOrEmpty(DataFormatStyle))
        //            {
        //                cell.NumberFormat = DataFormatStyle;
        //            }

        //            #region set format conditions
        //            if (formatCondition && conf.SupportFormatCondition)
        //            {
        //                switch (col.FormatCondition.TypeName)
        //                {
        //                    case FormatTypeEnum.ColorScale:
        //                        {
        //                            ColorScale cs = (ColorScale)cell.FormatConditions.AddColorScale(2);
        //                            cs.ColorScaleCriteria[1].FormatColor.Color
        //                                = ColorTranslator.ToOle(ExcelUtils.GetColor(
        //                                    col.FormatCondition.Parameters["lightColor"].ParameterValue));
        //                            cs.ColorScaleCriteria[2].FormatColor.Color
        //                                = ColorTranslator.ToOle(ExcelUtils.GetColor(
        //                                    col.FormatCondition.Parameters["darkColor"].ParameterValue));
        //                            break;
        //                        }
        //                    case FormatTypeEnum.DupeUnique:
        //                        {
        //                            UniqueValues uv = (UniqueValues)cell.FormatConditions.AddUniqueValues();
        //                            uv.DupeUnique = XlDupeUnique.xlDuplicate;
        //                            uv.Interior.Color = ColorTranslator.ToOle(ExcelUtils.GetColor(
        //                                    col.FormatCondition.Parameters["color"].ParameterValue));
        //                            break;
        //                        }

        //                    case FormatTypeEnum.DataBar:
        //                        {
        //                            Databar db = (Databar)cell.FormatConditions.AddDatabar();
        //                            FormatColor fc = (FormatColor)db.BarColor;
        //                            fc.Color = ColorTranslator.ToOle(ExcelUtils.GetColor(
        //                                    col.FormatCondition.Parameters["color"].ParameterValue));
        //                            db.MinPoint.Modify(XlConditionValueTypes.xlConditionValueNumber, 0);
        //                            break;
        //                        }
        //                    case FormatTypeEnum.IconSet:
        //                        {
        //                            string name = col.FormatCondition.IconCriteria.IconSet;
        //                            XlIconSet iconSet = ExcelUtils.GuessEnumValue<XlIconSet>(name);

        //                            IconSetCondition isc
        //                                = (IconSetCondition)cell.FormatConditions.AddIconSetCondition();
        //                            isc.IconSet
        //                                = ((Microsoft.Office.Interop.Excel.Application)this.CurrentApplication)
        //                                    .ActiveWorkbook.IconSets[iconSet];
        //                            isc.ReverseOrder = col.FormatCondition.IconCriteria.ReverseOrder;
        //                            isc.ShowIconOnly = col.FormatCondition.IconCriteria.ShowIconOnly;

        //                            if (col.FormatCondition.IconCriteria.All.Count > 0)
        //                            {
        //                                int startIndex = 2;
        //                                foreach (IconCriterionElement ice in col.FormatCondition.IconCriteria.All)
        //                                {
        //                                    int idx = ice.Index > startIndex ? ice.Index : startIndex++;
        //                                    isc.IconCriteria[idx].Type = ExcelUtils.GuessEnumValue<XlConditionValueTypes>(ice.ValueType);
        //                                    isc.IconCriteria[idx].Value = ice.Value;
        //                                    isc.IconCriteria[idx].Operator =
        //                                        (int)ExcelUtils.GuessEnumValue<XlFormatConditionOperator>(ice.Operator);
        //                                }
        //                            }
        //                            break;
        //                        }
        //                    case FormatTypeEnum.ColorFont:
        //                        {
        //                            foreach (ParameterElement ele in col.FormatCondition.Parameters)
        //                            {
        //                                FormatCondition con = (FormatCondition)cell.FormatConditions.Add(XlFormatConditionType.xlCellValue, XlFormatConditionOperator.xlEqual, ele.ParameterName,
        //                                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //                                con.Font.Color = ColorTranslator.ToOle(ExcelUtils.GetColor(ele.ParameterValue));
        //                                if (!string.IsNullOrEmpty(ele.ParameterBGValue))
        //                                {
        //                                    con.Interior.Color = ColorTranslator.ToOle(ExcelUtils.GetColor(ele.ParameterValue));
        //                                }
        //                            }
        //                            break;
        //                        }
        //                    default:
        //                        break;
        //                }
        //            }
        //            #endregion

        //            #region deal with validation
        //            if (col.Validation != null && !string.IsNullOrEmpty(col.Validation.ValidationType))
        //            {
        //                colRange.Validation.Delete();
        //                colRange.Validation.Add(ExcelUtils.GuessEnumValue<XlDVType>(col.Validation.ValidationType),
        //                    ExcelUtils.GuessEnumValue<XlDVAlertStyle>(col.Validation.AlertStyle),
        //                    ExcelUtils.GuessEnumValue<XlFormatConditionOperator>(col.Validation.Operator),
        //                    col.Validation.GetColumnFormula1Style(languageCode), col.Validation.Formula2);
        //                colRange.Validation.IgnoreBlank = true;
        //                colRange.Validation.InCellDropdown = true;
        //                if (!string.IsNullOrEmpty(col.Validation.InputTitle))
        //                {
        //                    colRange.Validation.InputTitle = col.Validation.InputTitle;
        //                }

        //                if (!string.IsNullOrEmpty(col.Validation.InputMessage))
        //                {
        //                    colRange.Validation.InputMessage = col.Validation.InputMessage;
        //                }

        //                if (!string.IsNullOrEmpty(col.Validation.ErrorTitle))
        //                {
        //                    colRange.Validation.ErrorTitle = col.Validation.ErrorTitle;
        //                }

        //                if (!string.IsNullOrEmpty(col.Validation.ErrorMessage))
        //                {
        //                    colRange.Validation.ErrorMessage = col.Validation.ErrorMessage;
        //                }

        //                colRange.Validation.ShowInput = col.Validation.ShowInput;
        //                colRange.Validation.ShowError = col.Validation.ShowError;
        //            }
        //            #endregion

        //            #region deal link

        //            if (!string.IsNullOrEmpty(col.Link))
        //            {
        //                string link = string.Compare(col.Link.Trim(), "null", StringComparison.CurrentCultureIgnoreCase) == 0 ? null : col.Link;
        //                SetLink(colRange, link, null, null);
        //            }

        //            #endregion

        //            if (autoFit && conf.SupportColumnAutoFit && col.AutoFit)
        //            {
        //                cell.EntireColumn.AutoFit();
        //            }
        //        }
        //        else
        //        {
        //            //treat as text cell
        //            cell.NumberFormat = "@";

        //            if (autoFit && conf.SupportColumnAutoFit)
        //            {
        //                // by default all columns should be auto fit
        //                cell.EntireColumn.AutoFit();
        //            }
        //        }
        //    }
        //}

        public object CurrentRibbon
        {
            get;
            set;
        }

        public void OnHyperlinkClicked(HyperlinkEventArg e)
        {
            if (hyperlinkClicked != null)
                hyperlinkClicked(this, e);
        }

        public object Evaluate(string vba)
        {
            return (currentApplication as Microsoft.Office.Interop.Excel.Application).Evaluate(vba);
        }

        public bool EnsureUDF(string name)
        {
            if (IsAddinInstalled(name))
            {
                object obj = Evaluate("GetStringHash(\"\")");
                if (obj == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool IsAddinInstalled(string addinName)
        {
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)this.currentApplication;
            try
            {
                app.AddIns.Add(addinName, Type.Missing);
            }
            catch (Exception) { }
            try
            {
                app.AddIns[addinName].Installed = true;
                return true;
            }
            catch (Exception) { return false; }

        }

        public void DeleteCurrentSheet()
        {
            DeleteSheet(GetCurrentSheet());
        }

        public void DeleteSheet(object sheetObj)
        {
            Worksheet sheet = sheetObj as Worksheet;
            if (sheet != null)
            {
                using (new DisplayAlertDisabler((Microsoft.Office.Interop.Excel.Application)this.CurrentApplication))
                {
                    sheet.Delete();
                }
            }
        }

        public Array GetSheetData(object sheet, int startx, int starty, int endx, int endy)
        {
            return GetSheetData(sheet, startx, starty, endx, endy, startx);
        }

        public Array GetSheetData(object sheet, int startx, int starty, int endx, int endy, int keyCol)
        {
            Worksheet ws;
            if (sheet == null)
            {
                Microsoft.Office.Interop.Excel.Application excelApp = (Microsoft.Office.Interop.Excel.Application)CurrentApplication;
                ws = (Worksheet)excelApp.ActiveWorkbook.ActiveSheet;
            }
            else
                ws = (Worksheet)sheet;
            Range startCell = (Range)ws.Cells[starty, startx];

            int height;
            int width;
            width = endx - startx + 1;

            if (endy >= starty)
            {
                height = endy - starty + 1;
            }
            else
            {
                int row = starty;
                keyCol = keyCol < startx ? startx : keyCol;
                keyCol = keyCol > endx ? endx : keyCol;

                // we need to guess height here
                // to improve performance, never invoke VSTO/PIA method too many times
                while (true)
                {
                    int step = 1000;
                    if (row + step > this.MaxRow)
                    {
                        step = this.MaxRow - row;
                    }

                    Array data = GetSheetData(sheet, keyCol, row, keyCol, row + step);

                    if (data != null)
                    {
                        bool stopped = false;
                        for (int i = data.GetLowerBound(0); i < data.GetUpperBound(0); i++)
                        {
                            //string str = Convert.ToString(data.GetValue(i, data.GetLowerBound(1)));
                            //str = str.Trim();
                            if (IsFullRowEmpty(data, i))
                            {
                                row += i - data.GetLowerBound(0);
                                stopped = true;
                                break;
                            }
                        }

                        if (!stopped)
                        {
                            row += step;
                        }

                        if (stopped || row + step >= this.MaxRow)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                height = row - starty;
            }

            Range outputRange = startCell.get_Resize(height < 1 ? 1 : height, width);
            return RangeToArray(outputRange);
        }

        public bool IsFullRowEmpty(Array array, int rowIndex)
        {
            bool isEmpty = true;
            if (GetIsOneCell(array))
            {
                if (array.GetValue(0, 0) != null)
                    isEmpty = false;
            }
            else
            {
                for (int i = 1; i <= array.GetLength(1); i++)
                {
                    if (array.GetValue(rowIndex, i) != null)
                    {
                        isEmpty = false;
                    }
                }
            }
            return isEmpty;
        }

        public bool GetIsOneCell(Array array)
        {
            if (array.GetLength(0) == 1 && array.GetLength(1) == 1)
            {
                return true;
            }
            else
                return false;
        }

        public int InsertTableRow(int tableIndex, int height)
        {
            ListObject listObject = GetTableByIndex(GetCurrentSheet(), tableIndex);
            InsertTableRow(listObject);
            Range sourceCell = listObject.Range;
            return sourceCell.get_Resize(height, 1).get_Offset(listObject.ListRows.Count, 0).Row;
            //sourceCell.Select();
        }

        public void InsertTableRow(object listObj)
        {
            ListObject listObject = listObj as ListObject;
            if (listObject != null)
            {
                listObject.ListRows.Add(Type.Missing);
            }
        }

        public object GetTableByIndex(string sheetName, int tableIndex)
        {
            Worksheet sheet = GetSheetByName(sheetName) as Worksheet;
            if (sheet == null || sheet.ListObjects.Count < 1)
                return null;
            else
                return sheet.ListObjects[tableIndex];
        }

        public string GetTableName(string sheetName, int tableIndex)
        {
            Worksheet sheet = GetSheetByName(sheetName) as Worksheet;
            if (sheet == null || sheet.ListObjects.Count < 1)
                return null;
            else
                return sheet.ListObjects[tableIndex].Name;
        }

        public int GetTableColumnCount(string sheetName, int colIndex)
        {
            object table = GetTableByIndex(sheetName, colIndex);
            return table == null ? -1 : (table as ListObject).ListColumns.Count;
        }

        public int GetTableRowCount(object sheetObj, int tableIndex)
        {
            Worksheet sheet = sheetObj as Worksheet;
            ListObject listObject = sheet.ListObjects[tableIndex];
            if (listObject != null)
            {
                Range dataRange = listObject.Range;
                if (dataRange != null)
                {
                    return dataRange.Rows.Count;
                }
            }
            return -1;
        }

        public int GetTableColumnIndexByName(object table, string name)
        {
            ListObject listObject = table as ListObject;
            if (listObject != null && !string.IsNullOrEmpty(name))
            {
                for (int i = 1; i < listObject.ListColumns.Count + 1; i++)
                {
                    if (string.Compare(listObject.ListColumns[i].Name, name, true) == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void HideColumns(object sheet, int colIndex, int colCount)
        {
            Worksheet ws = sheet as Worksheet;
            if (ws == null)
            {
                return;
            }

            try
            {
                Range range = ws.Cells[1, colIndex] as Range;

                if (range != null)
                {
                    range = range.get_Resize(1, colCount);
                    range.EntireColumn.Hidden = true;
                }
            }
            catch (Exception) { }
        }

        public void HideSheet(object sheet)
        {
            Worksheet ws = sheet as Worksheet;
            if (ws == null)
            {
                return;
            }

            ws.Visible = XlSheetVisibility.xlSheetVeryHidden;
        }

        public object GetRange(string sheetName, int startX, int startY, int endX, int endY)
        {
            var sheet = GetSheetByName(sheetName) as Worksheet;
            if (sheet != null)
            {
                ActiveSheet(sheet);
                string start = ExcelUtils.GetRangeAdress(startX, startY);
                if (start != "")
                {
                    if (startX != endX || startY != endY)
                    {
                        string end = ExcelUtils.GetRangeAdress(endX, endY);
                        return sheet.get_Range(start, end);
                    }
                    return sheet.get_Range(start, Type.Missing);
                }
            }

            return null;
        }

        public void ActiveSheet(object sheet)
        {
            _Worksheet ws = sheet as _Worksheet;
            if (ws == null)
            {
                return;
            }
            ws.Activate();

            Window win = ((Workbook)ws.Parent).Windows[1];
            if (win.WindowState == XlWindowState.xlMinimized)
            {
                win.WindowState = XlWindowState.xlMaximized;
            }
        }

        public string GetTableColumnNameByIndex(object table, int colIndex)
        {
            ListObject listObject = table as ListObject;
            if (listObject != null
                && colIndex >= 0
                && colIndex <= listObject.ListColumns.Count)
            {
                return listObject.ListColumns[colIndex].Name;
            }
            else
            {
                return null;
            }
        }

        public string OpenWorkbook(string workBookName)
        {
            Microsoft.Office.Interop.Excel.Application app = this.CurrentApplication as Microsoft.Office.Interop.Excel.Application;

            if (IsWorkbookOpened(workBookName))
            {
                app.Workbooks.Open(workBookName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            object obj = GetCurrentWorkbook();
            Workbook workbook = obj as Workbook;
            return workbook.Name;
        }

        public bool IsWorkbookOpened(string workBookName)
        {
            bool isExist = false;
            if (!string.IsNullOrEmpty(workBookName))
            {
                Microsoft.Office.Interop.Excel.Application app = this.CurrentApplication as Microsoft.Office.Interop.Excel.Application;
                foreach (_Workbook workbook in app.Workbooks)
                {
                    if (workbook.FullName == workBookName)
                    {
                        isExist = true;
                        workbook.Activate();
                    }

                }
                if (!isExist)
                {
                    return true;
                }
            }
            return false;
        }

        public object DrawArray(string sheetName, Array array, int x, int y)
        {
            Worksheet sheet = null;
            Range startCell = null;
            Range outputRange = null;
            try
            {

                sheet = GetSheetByName(GetCurrentWorkbook(), sheetName) as Worksheet;
                ActiveSheet(sheet);
                if (null == sheet) throw new ArgumentException("Error sheet name!");
                if (null == array) throw new ArgumentNullException("array");
                if (y + array.GetLength(0) - 1 > MaxRow) throw new ExcelException("Draw data too long!");

                Normalize(array);

                startCell = (Range)sheet.Cells[y, x];
                outputRange = startCell.get_Resize(array.GetLength(0), array.GetLength(1));

                outputRange.Value2 = array;

                return outputRange;
            }
            catch (Exception exp)
            {
                throw new ExcelException(exp.Message, exp);
            }
            finally
            {
                ReleaseComObject(startCell);
                startCell = null;
                ReleaseComObject(sheet);
                sheet = null;
            }
        }

        public string GetTableName(object listObj)
        {
            ListObject list = listObj as ListObject;
            if (list != null)
            {
                return list.Name;
            }

            return null;
        }

        public void RefreshCurrentWorkbook(object workBook)
        {
            Workbook wb = workBook as Workbook;
            if (wb != null)
                wb.RefreshAll();
        }
        
        public void ActiveAllChart(object workBook, string sheetName)
        {
            Workbook wb = workBook as Workbook;
            Worksheet sheet = GetSheetByName(wb, sheetName) as Worksheet;
            ChartObjects charts = sheet.ChartObjects(Type.Missing) as ChartObjects;
            if (charts == null) return;

            foreach (ChartObject chart in charts)
            {
                try
                {
                    if (chart.Visible)
                        chart.Select(false);
                    //chart.Activate();
                }
                catch
                {//just ignore
                }
            }
        }

        public void ApplayCellBorderStyle(object range, bool top, bool left, bool right, bool bottom, string color)
        {
            bool automaticColor = string.IsNullOrEmpty(color);
            StringBuilder styleNameBuilder = new StringBuilder();
            styleNameBuilder.Append(top ? '1' : '0')
                            .Append(left ? '1' : '0')
                            .Append(right ? '1' : '0')
                            .Append(bottom ? '1' : '0')
                            .Append(',')
                            .Append(automaticColor ? "0" : color);
            string styleName = styleNameBuilder.ToString();
            Workbook workbook = GetCurrentWorkbook() as Workbook;
            Styles styles = workbook.Styles;
            bool styleExisted = false;
            foreach (Style existedStyle in styles)
            {
                if (existedStyle.Name == styleName)
                {
                    styleExisted = true;
                    break;
                }
            }
            if (!styleExisted)
            {
                Style style = styles.Add(styleName, Missing.Value);
                int oleColor = 0;
                if (!automaticColor)
                {
                    oleColor = ColorTranslator.ToOle(ExcelUtils.GetColor(color));
                }
                style.IncludeBorder = true;
                style.IncludeNumber = false;
                style.IncludeProtection = false;
                style.IncludeFont = false;
                style.IncludeAlignment = false;
                style.IncludePatterns = false;
                if (top)
                {
                    Border topBorder = style.Borders[(XlBordersIndex)Constants.xlTop];
                    topBorder.LineStyle = XlLineStyle.xlContinuous;
                    if (automaticColor)
                    {
                        topBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                    }
                    else
                    {
                        topBorder.ColorIndex = XlColorIndex.xlColorIndexNone;
                        topBorder.Color = oleColor;
                    }
                    topBorder.TintAndShade = 0;
                    topBorder.Weight = XlBorderWeight.xlThin;
                }
                if (left)
                {
                    Border leftBorder = style.Borders[(XlBordersIndex)Constants.xlLeft];
                    leftBorder.LineStyle = XlLineStyle.xlContinuous;
                    if (automaticColor)
                    {
                        leftBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                    }
                    else
                    {
                        leftBorder.ColorIndex = XlColorIndex.xlColorIndexNone;
                        leftBorder.Color = oleColor;
                    }
                    leftBorder.TintAndShade = 0;
                    leftBorder.Weight = XlBorderWeight.xlThin;
                }
                if (right)
                {
                    Border rightBorder = style.Borders[(XlBordersIndex)Constants.xlRight];
                    rightBorder.LineStyle = XlLineStyle.xlContinuous;
                    if (automaticColor)
                    {
                        rightBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                    }
                    else
                    {
                        rightBorder.ColorIndex = XlColorIndex.xlColorIndexNone;
                        rightBorder.Color = oleColor;
                    }
                    rightBorder.TintAndShade = 0;
                    rightBorder.Weight = XlBorderWeight.xlThin;
                }
                if (bottom)
                {
                    Border bottomBorder = style.Borders[(XlBordersIndex)Constants.xlBottom];
                    bottomBorder.LineStyle = XlLineStyle.xlContinuous;
                    if (automaticColor)
                    {
                        bottomBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                    }
                    else
                    {
                        bottomBorder.ColorIndex = XlColorIndex.xlColorIndexNone;
                        bottomBorder.Color = oleColor;
                    }
                    bottomBorder.TintAndShade = 0;
                    bottomBorder.Weight = XlBorderWeight.xlThin;
                }
            }
            Range destRange = range as Range;
            destRange.Style = styleName;
        }

        public object GetTableDataRange(object table)
        {
            ListObject list = table as ListObject;
            if (null == list) return null;
            return list.Range.get_Resize(list.Range.Rows.Count - 1, list.Range.Columns.Count).get_Offset(1, 0);
        }

        internal delegate void ChangeControlVisiblity();

        public void SelectArray(System.Windows.Forms.Control sender, ref string address, out string sheetName)
        {
            sheetName = string.Empty;
            SelectArray(sender, true, ref address, out sheetName);
        }

        public void SelectArray(System.Windows.Forms.Control sender, bool isDisableExcelInput, ref string address, out string sheetName)
        {

            //Array array = null;
            sheetName = string.Empty;
            //address = string.Empty;
            try
            {
                if (sender != null)
                {
                    if (sender.InvokeRequired)
                    {
                        sender.Invoke(new ChangeControlVisiblity(delegate()
                        {
                            sender.Hide();
                            //sender.Visible = false;
                        }));
                    }
                    else
                    {
                        sender.Hide();
                    }
                }
                Range row = null;
                Range col = null;

                if (DoSelect(out row, out col, address, isDisableExcelInput))
                {
                    if (col != null)
                    {
                        address = col.get_Address(Type.Missing, Type.Missing, XlReferenceStyle.xlA1, Type.Missing, Type.Missing);
                        sheetName = col.Worksheet.Name;
                        //array = RangeToArray(col);
                        //object[,] arr = (object[,])col.Value2;
                        return;
                    }
                }

                return;
            }
            finally
            {
                if (sender != null)
                {
                    if (sender.InvokeRequired)
                    {
                        sender.Invoke(new ChangeControlVisiblity(delegate()
                        {
                            sender.Show();
                        }));
                    }
                    else
                    {
                        sender.Show();
                    }
                }
            }
        }

        private bool DoSelect(out Range row, out Range col, string initText, bool isDisableExcelInput)
        {

            row = null;
            col = null;

            Microsoft.Office.Interop.Excel.Application app = this.CurrentApplication as Microsoft.Office.Interop.Excel.Application;
            if (app == null)
            {
                return false;
            }
            app.Visible = true; // workaround to unexpected minimized issue

            object obj = app.InputBox(SR.SelectRange_Dialog_Label, SR.SelectRange_Dialog_Title,
                                      initText, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 8);
            if (isDisableExcelInput)
            {
                EnableWindow(new IntPtr(app.Hwnd), false);
            }
            if (obj is bool)
            {
                return (bool)obj;
            }

            Range range = obj as Range;
            col = range;
            if (range != null)
            {
                this.ActiveSheet(range.Worksheet);
            }

            return true;



        }

        [DllImport("User32")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        private bool DoSelect(out Range row, out Range col)
        {
            row = null;
            col = null;

            Microsoft.Office.Interop.Excel.Application app = this.CurrentApplication as Microsoft.Office.Interop.Excel.Application;
            if (app == null)
            {
                return false;
            }
            app.Visible = true; // workaround to unexpected minimized issue

            object obj = app.InputBox(SR.SelectRange_Dialog_Label, SR.SelectRange_Dialog_Title,
                                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 8);
            EnableWindow(new IntPtr(app.Hwnd), false);
            if (obj is bool)
            {
                return (bool)obj;
            }

            Range range = obj as Range;
            col = range;
            if (range != null)
            {
                this.ActiveSheet(range.Worksheet);
            }

            return true;
        }
    }
}
