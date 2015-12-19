using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
//using Ganji.ExcelManagement.Config;
using Microsoft.Office.Interop.Excel;

namespace Common.Excel.Management
{
    public interface IExcelManagement
    {
        object CurrentApplication { get; set; }
        object GetCurrentWorkbook(bool isCreate = true);
        object GetCurrentSheet();
        object CreateNewSheet(string sheetName, bool beforeCurrentSheet = true);
        string GetSheetName(object workSheet);
        string GetSheetName(int sheetIndex);
        void StartExport();
        void EndExport();
        bool IsExporting{ get; }
        void SetAutoExtentTable(bool extend);
        void BeginUpdate();
        void EndUpdate();
        object GetSelectedSingleRange();
        int GetColumnIndex(object cell);
        int GetRowIndex(object cell); 
        object CreateNewWorkbook();
        void ReleaseComObject(object obj);
        void ChangeTableOption(
            string sheetName
            , int tableIndex
            , bool showTableStyleColumnStrips
            , bool showTableStyleFirstColumn
            , bool showTableStyleLastColumn
            , bool showTableStyleRowStripes
            , bool showTotals);
        object GetSheetByName(string sheetName);
        object GetSheetByName(object workbook, string sheetName);
        string GetCurrentSheetName();
        object DrawArray(object sheet, Array array, int x, int y, string dataFormat = null);
        object DrawArray(string sheetName, Array array, int x, int y);
        void SetGridLineDisplay(bool show);
        void ChangeSheetBackGroundColor(object sheet, Color color);
        void ChangeBackgroundColor(object range, Color color, int startX = 0, int startY = 0, int rows = 0, int columns = 0);
        object GetAllCell(object sheet);
        void ChangeFontSize(object range, int pt);
        void MergeRange(object sheet, int x, int y, int col, int row, bool across);
        void SetCenterAlignment(object range, int width = 0, int height = 0, bool isH = true, bool isV = true);
        void ChangeFontColor(object range, Color color, int width = 0, int height = 0);
        void SetRangeHeight(object range, object height);
        void ChangeFont(object range, string fontName);
        object GetRange(int x, int y, int w, int h);
        object GetRange(object sheet, int x, int y, int w, int h);
        object GetRange(string sheetName, int startX, int startY, int endX, int endY);
        void ApplayDefaultBorders(object range, bool top, bool left, bool right, bool bottom, bool insideVertical, bool insideHorizontal, Color? color);
        void SetColumnWidth(object range, object width);
        object GetColumnRange(object sheetObj, int start, int end);
        void FreezePanes(int startX, int startY);
        void SelectRange(int startX, int startY, int width = 1, int height = 1);
        void SelectRange(object rangeObj);
        //object BuildExportTable(object sheet, string style, int startx, int starty, object[,] data);

        object GetTableLastRow(int tableIndex);
        string ProductCode { get; }
        void SetLink(object range, string callBackFuncName, string displayText, string tooltip);
        void ChangeFontStyle(object range, FontStyle fontStyle, int width = 0, int height = 0);
        void UpdateHyperlinkStyle(string fontName, double fontSize);
        void DrawButtonStyleShape(object sheetObj, string text, string action, System.Drawing.Font font, string chineseFontName, float x, float y, float width, float height);
        object GetTableByIndex(string sheetName, int tableIndex);
        string GetTableName(object sheetObj, int tableIndex);
        string GetTableName(string sheetName, int tableIndex);
        object GetTableRange(object sheetObj, int tableIndex);
       // void ApplyColumnFormat(object range, TableElement conf, bool formatCondition, bool autoFit, string languageCode);
        object CurrentRibbon { get; set; }
        event EventHandler<HyperlinkEventArg> HyperlinkClicked;
        void OnHyperlinkClicked(HyperlinkEventArg e);
        bool EnsureUDF(string addinName);
        void DeleteSheet(object sheetObj);
        void DeleteCurrentSheet();
        Array GetSheetData(object sheet, int startx, int starty, int endx, int endy);
        bool IsFullRowEmpty(Array array, int rowIndex);
        int InsertTableRow(int tableIndex, int height);
        int GetTableColumnCount(string sheetName, int colIndex); 
        Array RangeToArray(object rangeObj);
        bool IsAddinInstalled(string addinName);
        int GetTableRowCount(object sheetObj, int tableIndex);
        int GetTableColumnIndexByName(object table, string name);
        object FormatRangeAsTable(object range, string style);
        void HideColumns(object sheet, int colIndex, int colCount);
        void HideSheet(object sheet);
        void ActiveSheet(object sheet);
        string GetTableColumnNameByIndex(object table, int colIndex);
        string OpenWorkbook(string workBookName);
        string GetTableName(object listObj);
        void RefreshCurrentWorkbook(object workBook);
        void ActiveAllChart(object workBook, string sheetName);
        void ApplayCellBorderStyle(object range, bool top, bool left, bool right, bool bottom, string color);
        object GetTableDataRange(object table);
        void SelectArray(Control sender, ref string address, out string sheetName);
        void SelectArray(Control sender, bool isDisableExcelInput, ref string address, out string sheetName);
    }
}
