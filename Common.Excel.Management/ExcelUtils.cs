using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Common.Excel.Management
{
    public static class ExcelUtils
    {
        public static ExcelVersion GetExcelVersion()
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            int productMajorPart = process.MainModule.FileVersionInfo.ProductMajorPart;
            if (productMajorPart > 12)
            {
                return ExcelVersion.V2010;
            }
            else if (productMajorPart == 12)
            {
                return ExcelVersion.V2007;
            }
            else
            {
                return ExcelVersion.V2003;
            }
        }

        public static string RemoveCharsInString(string str, char[] chars)
        {
            if (chars == null || str == null)
            {
                return str;
            }

            StringBuilder sb = new StringBuilder(str.Length);
            foreach (char c in str)
            {
                bool matched = false;
                foreach (char ch in chars)
                {
                    if (c == ch)
                    {
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Trim();
        }

        public static Array GetSubDataTableArray(Array dataTableArray, int startRowIndex, int rows, bool includeHeader = true)
        {
            if (dataTableArray == null || rows == 0)
            {
                return new object[0, 0];
            }

            if (dataTableArray.Rank != 2)
            {
                throw new ArgumentException("two-dimension array required!");
            }

            if (startRowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startRowIndex should be greater than or equal zero!");
            }

            if (rows < 0)
            {
                throw new ArgumentOutOfRangeException("rows should be greater than or equal zero!");
            }

            int maxRow = dataTableArray.GetLength(0);
            if (startRowIndex >= maxRow)
            {
                throw new ArgumentOutOfRangeException("startRowIndex should be less than" + maxRow + "!");
            }

            if (includeHeader)
            {
                startRowIndex = startRowIndex == 0 ? 1 : startRowIndex;
            }
            rows = rows + startRowIndex > maxRow ? (maxRow - startRowIndex + 1) : rows;

            if (maxRow == 0 || (rows == maxRow && startRowIndex <= 1))
            {
                return dataTableArray;
            }

            int cols = dataTableArray.GetLength(1);

            object[,] subArr = new object[rows, cols];

            if (includeHeader)
            {
                for (int i = 0; i < cols; i++)
                {
                    subArr[0, i] = dataTableArray.GetValue(0, i);
                }
            }

            for (int i = includeHeader ? 1 : 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    subArr[i, j] = dataTableArray.GetValue(startRowIndex, j);
                }
                startRowIndex++;
            }

            return subArr;
        }

        public static string GetRangeAdress(int rowIndex, int columnIndex)
        {
            if (rowIndex < 1 || columnIndex < 1)
            {
                return "";
            }
            int div, mod;
            div = columnIndex;
            string columnStr = "";
            while (div > 26)
            {
                mod = div % 26;
                if (mod == 0)
                {
                    mod = 26;
                    div--;
                }
                columnStr = (char)(64 + mod) + columnStr;
                div = div / 26;
            }
            columnStr = (char)(64 + div) + columnStr;
            return columnStr + rowIndex.ToString();
        }

        public static object GenerateGuid()
        {
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] arr = new byte[16];
            provider.GetNonZeroBytes(arr);
            return new Guid(arr).ToString();
        }

        [DllImport("Msi.dll", EntryPoint = "MsiQueryFeatureStateA", CharSet = CharSet.Ansi)]
        internal extern static MsiInstallState MsiQueryFeatureState(string productCode, string featureName);
        public static bool IsVBAInstalled()
        {
            IExcelManagement excelManager = ExcelManagementFactory.GetExcelManagement();
            MsiInstallState state = MsiQueryFeatureState(excelManager.ProductCode, "VBAFiles");
            return state == MsiInstallState.Advertised || state == MsiInstallState.Local || state == MsiInstallState.Source;
        }

        public static Color GetColor(string color, string alpha = null)
        {
            // normalize input parameters
            if (color != null)
            {
                color = color.Trim();
            }
            if (alpha != null)
            {
                alpha = alpha.Trim();
            }

            if (string.IsNullOrEmpty(color))
            {
                return Color.Empty;
            }

            Color c = Color.Empty;
            //hzq 7.6 for FxCop MSInternalRules
            //if (color.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) || color.StartsWith("0X", StringComparison.CurrentCultureIgnoreCase))
            if (color[0] == '0' && (color[1] == 'x' || color[1] == 'X'))
            {
                try
                {
                    int num = Convert.ToInt32(color, 16);
                    c = Color.FromArgb((0x00FF0000 & num) >> 16,
                            (0x0000FF00 & num) >> 8, 0x000000FF & num);
                }
                catch
                {
                }
            }

            if (c == Color.Empty && color.IndexOf(',') > 0)
            {
                string[] rgb = color.Split(',');
                if (rgb != null && rgb.Length == 3)
                {
                    try
                    {
                        c = Color.FromArgb(int.Parse(rgb[0].Trim()),
                                int.Parse(rgb[1].Trim()), int.Parse(rgb[2].Trim()));
                    }
                    catch
                    {
                    }
                }
            }

            if (c == Color.Empty)
            {
                c = Color.FromName(color);
            }


            if (!string.IsNullOrEmpty(alpha))
            {
                try
                {
                    c = Color.FromArgb(int.Parse(alpha), c);
                }
                catch
                {
                }
            }
            return c;
        }

        public static T GuessEnumValue<T>(string value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Enum type required");
            }

            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            // assume the underlying type is UInt32
            int num = 0;
            int.TryParse(value, out num);
            T t = num > 0
                    ? (T)Convert.ChangeType(num, typeof(T))
                    : (T)Enum.Parse(typeof(T), value);
            return t;
        }

        public static object GetRangeByHyperLink(object target)
        {
            object result = null;
             Hyperlink hl = target as Hyperlink;
             if (hl != null)
             {
                 result = hl.Range;
             }
             return result;
        }
    }

    public enum ExcelVersion
    {
        V2003,
        V2007,
        V2010
    }

    internal enum MsiInstallState : int
    {
        /// <summary>component disabled</summary>
        NotUsed = -7,
        /// <summary>configuration data corrupt</summary>
        BadConfig = -6,
        /// <summary>installation suspended or in progress</summary>
        Incomplete = -5,
        /// <summary>run from source, source is unavailable</summary>
        SourceAbsent = -4,
        /// <summary>return buffer overflow</summary>
        MoreData = -3,
        /// <summary>invalid function argument</summary>
        InvalidArg = -2,
        /// <summary>unrecognized product or feature</summary>
        Unknown = -1,
        /// <summary>broken</summary>
        Broken = 0,
        /// <summary>advertised feature</summary>
        Advertised = 1,
        ///// <summary>component being removed (action state, not settable)</summary>
        //Removed = 1,
        /// <summary>uninstalled (or action state absent but clients remain)</summary>
        Absent = 2,
        /// <summary>installed on local drive</summary>
        Local = 3,
        /// <summary>run from source, CD or net</summary>
        Source = 4,
        /// <summary>use default, local or source</summary>
        Default = 5,
    }
}
