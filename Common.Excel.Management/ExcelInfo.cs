using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Office.Interop.Excel;

namespace Common.Excel.Management
{
    sealed class ExcelInfo
    {
        private Mutex inUsedMutex;
        private bool isInUsed;
        private const string InUseMutexLockString = "ExcelManagement.InUsed.Mutex";
        public bool ScreenUpdating = false;
        public XlCalculation Calculation = XlCalculation.xlCalculationAutomatic;
        public bool CannotChangeCalculation = false;

        public bool IsInUsed
        {
            get
            {
                if (isInUsed) return true;
                bool isCreated = false;
                Mutex testMutex = new Mutex(false, InUseMutexLockString, out isCreated);
                testMutex.Close();
                testMutex = null;
                return !isCreated;
            }
            set
            {
                if (isInUsed != value)
                {
                    isInUsed = value;
                    if (inUsedMutex == null && isInUsed)
                    {
                        inUsedMutex = new Mutex(false, InUseMutexLockString);
                    }
                    else if (!isInUsed)
                    {
                        inUsedMutex.Close();
                        inUsedMutex = null;
                    }
                }
            }
        }
    }
}
