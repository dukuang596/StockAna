using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Excel.Management
{
    public class SheetParameter
    {
        public Array SheetData
        {
            get;
            set;
        }

        public string SheetName
        {
            get;
            set;
        }

        public object CurrentSheet
        {
            get;
            set;
        }

        public static bool IsTitle(Array array)
        {
            try
            {
                if (array != null
                    && array.GetValue(1, 1) != null && !string.IsNullOrEmpty(array.GetValue(1, 1).ToString())
                    && array.GetValue(2, 1) != null && !string.IsNullOrEmpty(array.GetValue(2, 1).ToString())
                   )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
