using Common.Excel.Management;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestExcelAddin.Export;

namespace TestExcelAddin
{
    public partial class PvUvSelector : Form
    {
        private static IExcelManagement excelManager;
        private static SheetParameter sheetParameter;
        private static readonly Color sheetBackGroundColor = Color.FromArgb(231, 246, 255);
        private static readonly Color exportTypeStringFontColor = Color.FromArgb(0, 112, 192);
        private static readonly Color detailInfoFontColor = Color.FromArgb(128, 128, 128);
        private static readonly Color buttonPanelBorderColor = Color.FromArgb(216, 216, 216);
        private static readonly Color buttonPanelBackGround = Color.FromArgb(255, 255, 255);
        private static ExportParameters exportParameters;
        public PvUvSelector()
        {
            InitializeComponent();
            excelManager = ExcelManagementFactory.GetExcelManagement();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            BuildExportTitle();
        }
        private static SheetParameter GetSheetParameter(string sheetName)
        {
            SheetParameter sheetParams = new SheetParameter();
            excelManager.GetCurrentWorkbook();//确保有workbook
            object workSheet = excelManager.CreateNewSheet(sheetName, true);
            sheetParams.SheetName = excelManager.GetSheetName(workSheet);
            sheetParams.CurrentSheet = workSheet;
            return sheetParams;
        }
        private static string VIEW_GOOGLE_CURRENTVIEW = "ExportTableView";
        private static string GetStyleViewName(string style)
        {
            string language = "zh-CN";
            return string.Concat(style, ",", language);
        }
        public static object[,] BuildCurrentViewHeader(object[,] array, int startRow, TemplateBase templateHeaders, ExportParameters para)
        {
            for (int i = 0; i < templateHeaders.ObjectPerfStartColumn; i++)
            {
                array[startRow, i] = templateHeaders.TemplateProperty[i];
            }

            if (para.ShowPerformance)
            {
                for (int i = templateHeaders.ObjectPerfStartColumn; i < templateHeaders.TemplateProperty.Count; i++)
                {
                    array[startRow, i] = templateHeaders.TemplateProperty[i];
                }
            }
            return array;
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
        //        Array firstRow = excelGetFirstRow(data);
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
        private static void BuildExportTableHeader(int columnCount, int startRowIndex, TemplateBase template, bool isIgnoredObjects)
        {
            object[,] header = new object[1, columnCount];
            header = BuildCurrentViewHeader(header, 0, template, exportParameters);
            if (isIgnoredObjects)
            {
                header.SetValue("错误信息", 0, columnCount - 4);
            }
            ///ttt
            //ExportAndImportUtils.AddFormulaColumnForEdit(header);
            //object tableRange = excelManager.BuildExportTable(sheetParameter.CurrentSheet, GetStyleViewName(VIEW_GOOGLE_CURRENTVIEW), 1, startRowIndex, header);


            //change table header style
            object tableHeaderRange = excelManager.GetRange(1, startRowIndex, columnCount, 1);
            excelManager.ChangeBackgroundColor(tableHeaderRange, Color.FromArgb(165, 165, 165));
            excelManager.ApplayDefaultBorders(tableHeaderRange, true, true, true, true, true, true, Color.FromArgb(165, 165, 165));
            excelManager.SetRangeHeight(tableHeaderRange, 14.25);

            //change status table header style
            object statusHeaderRange = excelManager.GetRange(1, startRowIndex, 2, 1);
            excelManager.ChangeBackgroundColor(statusHeaderRange, Color.FromArgb(237, 246, 249));
            excelManager.ApplayDefaultBorders(statusHeaderRange, true, true, true, true, true, true, Color.FromArgb(237, 246, 249));
            excelManager.ChangeFontColor(statusHeaderRange, Color.FromArgb(237, 246, 249));

            //change enable columns table header style
            int enabledCount = template.EditablePropertyCount;
            object enabledRange = excelManager.GetRange(3, startRowIndex, enabledCount, 1);
            excelManager.ApplayDefaultBorders(enabledRange, true, true, true, true, true, true, Color.FromArgb(0, 112, 192));
            excelManager.ChangeBackgroundColor(enabledRange, Color.FromArgb(0, 112, 192));
            excelManager.ChangeFontColor(tableHeaderRange, Color.White);

            //change data range style
            object currentViewRange = excelManager.GetRange(1, startRowIndex + 1, columnCount, 1);
            excelManager.ChangeBackgroundColor(currentViewRange, Color.FromArgb(242, 242, 242));
            excelManager.ApplayDefaultBorders(currentViewRange, true, true, true, true, true, true, Color.FromArgb(216, 216, 216));

            object enabledViewRange = excelManager.GetRange(3, startRowIndex + 1, enabledCount, 1);
            excelManager.ApplayDefaultBorders(enabledViewRange, false, true, true, false, false, false, Color.FromArgb(147, 205, 221));
            excelManager.ApplayDefaultBorders(enabledViewRange, true, false, false, true, true, true, Color.FromArgb(219, 229, 241));

            int index = 1;
            excelManager.SetColumnWidth(excelManager.GetColumnRange(sheetParameter.CurrentSheet, index, index++), 2);
            excelManager.SetColumnWidth(excelManager.GetColumnRange(sheetParameter.CurrentSheet, index, index++), 2);
            excelManager.SetColumnWidth(excelManager.GetColumnRange(sheetParameter.CurrentSheet, index, index++), 18);
        }
        public static string GetFontName()
        {
            string fontName = "微软雅黑";
            return fontName;
        }
        private static void BuildExportTitle() {

            string sheetName = "";
            sheetName = "赶集流量";
            sheetParameter = GetSheetParameter(sheetName);
            //Build Title
            excelManager.BeginUpdate();
            object allCell = BuildExportStyle();
            BuildExportTypeString();
            BuildExportDetailinfo();
            //BuildExportButtonPanel();
            //SetTitleColumnWidth();


            excelManager.FreezePanes(1, currentRowIndex + 1);
            excelManager.SelectRange(1, 1);
            excelManager.ChangeFont(allCell, GetFontName());
            excelManager.EndUpdate();
        }
        private static object BuildExportStyle()
        {
            excelManager.SetGridLineDisplay(false);
            excelManager.ChangeSheetBackGroundColor(sheetParameter.CurrentSheet, sheetBackGroundColor);
            object allCell = excelManager.GetAllCell(sheetParameter.CurrentSheet);
            excelManager.ChangeFontSize(allCell, 9);
            return allCell;
        }
        static int currentRowIndex = 0;
        private static void BuildExportTypeString()
        {
            currentRowIndex = 1;
            excelManager.MergeRange(sheetParameter.CurrentSheet, 1, currentRowIndex, 3, 3, false);
            object typeRange = excelManager.DrawArray(sheetParameter.CurrentSheet, new string[,] { { "赶集流量" } }, 1, currentRowIndex);
            excelManager.ChangeFontSize(typeRange, 20);
            excelManager.SetCenterAlignment(typeRange, 1, 1, true, true);
            excelManager.ChangeFontColor(typeRange, exportTypeStringFontColor);
        }
        private static void BuildExportDetailinfo()
        {
            object[,] title =BuildTitle();
            object detailInfoRange = excelManager.DrawArray(sheetParameter.CurrentSheet, title, 4, currentRowIndex);
            excelManager.ChangeFontColor(detailInfoRange, detailInfoFontColor);
            excelManager.SetRangeHeight(detailInfoRange, 16);
            currentRowIndex += 3;
        }
        public static object[,] BuildTitle()
        {
            object[,] array = new object[3, 1];
            array[0, 0] = "时间范围:";
            array[1, 0] ="城市:";
            array[2, 0] = "类目";
         
            return array;
        }

        private static void CreateNewObject(int row, int selectRow, SheetParameter para/*, ExportTitleObject title*/)
        {
            IExcelManagement excelManager = ExcelManagementFactory.GetExcelManagement();
            excelManager.StartExport();
            try
            {
                excelManager.SetAutoExtentTable(true);
                excelManager.BeginUpdate();
                //BuildCurrentView(exportFrame);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                excelManager.EndUpdate();
                excelManager.EndExport();
               
            }

        }

        private void PvUvSelector_Load(object sender, EventArgs e)
        {

        }
    }
}
