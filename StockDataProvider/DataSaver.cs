using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using stock;
using Stock.Common;

namespace Stock.DataProvider
{
    public static class DataSaver
    {
        static DataSaver()
        {
            DataSaver.history_second_fields.Add("tick", "Tick");
        }

        static string EscapeString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            //var arr = s.ToCharArray();
            if (s.Contains('\t') || s.Contains('\n') || s.Contains('\\'))
            {
                var arr = s.ToCharArray();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == '\t')
                        sb.Append("\\t");
                    else if (arr[i] == '\n')
                        sb.Append("\\n");
                    else if (arr[i] == '\\')
                        sb.Append(@"\\");
                    else
                        sb.Append(arr[i]);
                }
                return sb.ToString();
            }
            else
                return s;
        }

        private static Dictionary<string,string>  history_second_fields =new Dictionary<string, string>();

        static TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
        public static int SaveSecondData(String Symbol, IEnumerable<StockHistoryData> stockData)
        {
            var dlist = stockData
                .Where(obj=>obj.Volume>0)//get volume>0
                .Select(obj => new stock_history_second_bar()
            {
                symbol = Symbol,
                close = (decimal) obj.Close,
                open = (decimal) obj.Open,
                wap = (decimal) obj.Wap,
                high = (decimal) obj.High,
                low = (decimal) obj.Low,
                volume = obj.Volume,
                count = obj.Count,
                hasgap = obj.HasGaps ? 1 : 0,
                tick = (uint)obj.Tick//(uint) (Util.ConvertDateTimeInt(obj.EndTime, tzi))

            }).ToList();
            var index = 0;
            while (dlist.Count>0)
            {
                if (dlist.Count >= 200)
                {
                    stockDB.GetInstance().BatchInsert(dlist.GetRange(0, 200));
                    dlist.RemoveRange(0, 200);
                }
                else
                {
                    stockDB.GetInstance().BatchInsert(dlist);
                    dlist.Clear();
                }                        
            }
            return index;

        }

        public static stock_history_min_bar FormatStockDataMinuteBar(String Symbol, StockHistoryData obj)
        {
            return new stock_history_min_bar()
            {
                symbol = Symbol,
                close = (decimal) obj.Close,
                open = (decimal) obj.Open,
                wap = (decimal) obj.Wap,
                high = (decimal) obj.High,
                low = (decimal) obj.Low,
                volume = obj.Volume,
                count = obj.Count,
                hasgap = obj.HasGaps ? 1 : 0,
                tick = (uint) obj.Tick //(uint) (Util.ConvertDateTimeInt(obj.EndTime, tzi))

            };
        }
        public static stock_history_second_bar FormatStockDataSecondBar(String Symbol, StockHistoryData obj)
        {
            return new stock_history_second_bar()
            {
                symbol = Symbol,
                close = (decimal)obj.Close,
                open = (decimal)obj.Open,
                wap = (decimal)obj.Wap,
                high = (decimal)obj.High,
                low = (decimal)obj.Low,
                volume = obj.Volume,
                count = obj.Count,
                hasgap = obj.HasGaps ? 1 : 0,
                tick = (uint)obj.Tick //(uint) (Util.ConvertDateTimeInt(obj.EndTime, tzi))

            };
        }
        public static stock_history_second_bar FormatStockDataSecondBar(String Symbol, uint tick, double open, double high, double low, double close,
            int volume, int count, double wap, bool hasGaps)
        {
            return new stock_history_second_bar()
            {
                symbol = Symbol,
                close = (decimal)close,
                open = (decimal)open,
                wap = (decimal)wap,
                high = (decimal)high,
                low = (decimal)low,
                volume =volume,
                count = count,
                hasgap = hasGaps ? 1 : 0,
                tick =tick//(uint) (Util.ConvertDateTimeInt(obj.EndTime, tzi))

            };
        }
        public static stock_history_15second_bar FormatStockData15SecondBar(String Symbol, uint tick, double open, double high, double low, double close,
          int volume, int count, double wap, bool hasGaps)
        {
            return new stock_history_15second_bar()
            {
                symbol = Symbol,
                close = (decimal)close,
                open = (decimal)open,
                wap = (decimal)wap,
                high = (decimal)high,
                low = (decimal)low,
                volume = volume,
                count = count,
                hasgap = hasGaps ? 1 : 0,
                tick = tick//(uint) (Util.ConvertDateTimeInt(obj.EndTime, tzi))

            };
        }
        public static stock_history_min_bar FormatStockDataMinuteBar(String Symbol, uint tick, double open, double high, double low, double close,
            int volume, int count, double wap, bool hasGaps)
        {
            return new stock_history_min_bar()
            {
                symbol = Symbol,
                close = (decimal)close,
                open = (decimal)open,
                wap = (decimal)wap,
                high = (decimal)high,
                low = (decimal)low,
                volume = volume,
                count = count,
                hasgap = hasGaps ? 1 : 0,
                tick = tick//(uint) (Util.ConvertDateTimeInt(obj.EndTime, tzi))

            };
        }
        public static int SaveHistoryData(DataTask dtask,uint tick, double open, double high, double low, double close,
            int volume, int count, double wap, bool hasGaps)
        {
            if (dtask.DataRequestType == DataRequestType.HISTORYBAR)
            {
                Func<string, uint, double, double, double, double, int, int, double, bool, object> func;
                if (dtask.BarSize == IBStandardHistoryBarSize.Sec01)
                    func = FormatStockDataSecondBar;
                else if (dtask.BarSize == IBStandardHistoryBarSize.Sec15)
                    func = FormatStockData15SecondBar;
                else
                    func = FormatStockDataMinuteBar;
                object obj=func(dtask.StockSymbol, tick, open, high, low, close, volume, count,wap, hasGaps);
                stockDB.GetInstance().Save(obj);
            }
            return 0;
        }

        public static int SaveMinuteData(String Symbol, IEnumerable<StockHistoryData> stockData)
        {
            var dlist = stockData
                .Where(obj => obj.Volume > 0)//get volume>0
                .Select(obj => FormatStockDataMinuteBar(Symbol,obj)).ToList();
            var index = 0;
            while (dlist.Count > 0)
            {
                if (dlist.Count >= 200)
                {
                    stockDB.GetInstance().BatchInsert(dlist.GetRange(0, 200));
                    dlist.RemoveRange(0, 200);
                }
                else
                {
                    stockDB.GetInstance().BatchInsert(dlist);
                    dlist.Clear();
                }
            }
            return index;

        }
        //public static int SaveDataToFile(String Symbol,List<StockHistoryData> stockData,string fileName)
        //{
        //    StreamWriter sw = File.CreateText(fileName);
        //    try
        //    {
        //        //return db.ExecuteReader<int>(
        //        //r =>
        //        //{
        //        var r = stockData.GetEnumerator();
        //            object[] objs = new object[r.FieldCount];
        //            int rowCount = 0;
        //            while (r.MoveNext())
        //            {
        //                var current = r.Current;
        //                rowCount += 1;
        //                r.GetValues(objs);
        //                for (int i = 0; i < r.FieldCount; i++)
        //                //foreach (object obj in objs)
        //                {
        //                    var obj = objs[i];
        //                    if (obj is DBNull)
        //                        objs[i] = @"\N";
        //                    else if (obj is string)
        //                    {
        //                        var t = obj as string;

        //                        objs[i] = EscapeString(t);
        //                    }
        //                }
        //                sw.Write(string.Join("\t", objs) + "\n");
        //            }
        //            sw.Flush();
        //            return rowCount;
        //        //}, source_sql, new object[] { execDate });
        //    }
        //    finally
        //    {
        //        sw.Close();
        //    }

        //}
    }
}
