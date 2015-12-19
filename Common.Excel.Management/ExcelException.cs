using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Excel.Management
{
    class ExcelException : ApplicationException
    {
        public ExcelException() : base()
        {
        }

        public ExcelException(string message) : base(message)
        {
        }

        public ExcelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
