using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Excel.Management
{
    public static class ExcelManagementFactory
    {
        private static IExcelManagement instance;
        private static object syncObject = new object();

        public static IExcelManagement GetExcelManagement()
        {
            if (instance == null)
            {
                lock (syncObject)
                {
                    if (instance == null)
                    {
                        switch (ExcelUtils.GetExcelVersion())
                        {
                            case ExcelVersion.V2003:
                            case ExcelVersion.V2007:
                                instance = new Excel2007Management();
                                break;
                        
                            case ExcelVersion.V2010:
                                instance = new Excel2010Management();
                                break;
                        }
                    }
                }
            }
            return instance;
        }
    }

}
