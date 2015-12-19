using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Common.Excel.Management;

namespace TestExcelAddin
{
   
    public partial class ThisAddIn
    {
        [ThreadStatic]
        private static ThisAddIn instance = null;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            IExcelManagement excelManagement = ExcelManagementFactory.GetExcelManagement();
            excelManagement.HyperlinkClicked += excelManagement_HyperlinkClicked;
            instance = this;
            excelManagement.CurrentApplication = this.Application;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }
        void excelManagement_HyperlinkClicked(object sender, HyperlinkEventArg e)
        {
            string text = e.LinkTag;
            if (string.IsNullOrEmpty(text))
                return;
            //if (UIUtils.Hyperlinks.Contains(text))
            //{
            //    UIUtils.OnHyperlinkClicked(sender, e);
            //}
        }
        #region VSTO 生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
      
        #endregion
        internal static ThisAddIn GetInstanceObject()
        {
            return instance;
        }

    }
}
