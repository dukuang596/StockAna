using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IBApi;
using Stock.Common;

namespace Stock.DataProvider
{
    public enum DataRequestType
    {
        HISTORYBAR,
        TICKSNAPSHOT,
        RTVOLUME
    }

    public class IBDataProvider : IStockDataProvider
    {
        internal class DataTask
        {
            public DataRequestType DataRequestType { get; set; }
            public string StockSymbol { get; set; }
            public IBStandardHistoryDataRange Range { get; set; }
            public IBStandardHistoryBarSize BarSize { get; set; }
            public DateTime EndDate { get; set; }
            public int ReqId { get; set; }
        }

        readonly Object _lockObj = new Object();
        int _sessionid = 10000;
        static Contract GetUsStock(string stockSymol)
        {
            Contract contract = new Contract();
            contract.Symbol = stockSymol;
            contract.SecType = "STK";
            contract.Currency = "USD";
            contract.Exchange = "SMART";
            return contract;
        }

        readonly EWrapperImpl _testImpl;

        int GetNextSessionId()
        {
            int reqid = 0;
            lock (_lockObj)
            {
                reqid = _sessionid++;
            }
            return reqid;
        }

        private readonly ConcurrentQueue<DataTask> _tq = new ConcurrentQueue<DataTask>();

        public IBDataProvider()
        {
            _testImpl = new EWrapperImpl();

            ThreadPool.QueueUserWorkItem(obj =>
            {
                while (true)
                {
                    DataTask dtask;
                    if (_tq.TryDequeue(out dtask))
                    {
                        if (dtask.DataRequestType == DataRequestType.HISTORYBAR)
                        {
                            Console.WriteLine(String.Format("reqHistoricalData("+dtask.ReqId+","+dtask.StockSymbol
                                + "," + dtask.EndDate.ToString("yyyyMMdd HH:mm:ss EST")
                                + "," + EnumDescriptionAttribute.GetEnumDescription(dtask.Range)
                                + "," + EnumDescriptionAttribute.GetEnumDescription(dtask.BarSize)
                                + "," + "TRADES" + "," + 0 + "," + 2 + "," + "null)"));
                            _testImpl.ClientSocket.reqHistoricalData(dtask.ReqId, GetUsStock(dtask.StockSymbol)
                          , dtask.EndDate.ToString("yyyyMMdd HH:mm:ss EST")
                          , EnumDescriptionAttribute.GetEnumDescription(dtask.Range),
                          EnumDescriptionAttribute.GetEnumDescription(dtask.BarSize)/* "1 day"*/, "TRADES", 0, 2, null);
                        }
                      
                        else if(dtask.DataRequestType == DataRequestType.TICKSNAPSHOT)
                            _testImpl.ClientSocket.reqMktData(dtask.ReqId, GetUsStock(dtask.StockSymbol),"233",false,null);

                    }
                    // ib grateway limitation
                    Thread.Sleep((int)(1000 * 10.5));
                }
            }, null);
        }


        public  void Connect()
        {
            //_testImpl = new EWrapperImpl();
            _testImpl.ClientSocket.eConnect("127.0.0.1", 7496, 0, false);

        }

        public  int ReqTickData(string stockSymol, DateTime startDate, DateTime enddate,
            bool fewRequest = true)
        {
            int reqid = GetNextSessionId();

            _tq.Enqueue(new DataTask()
            {
                DataRequestType = DataRequestType.TICKSNAPSHOT,
                StockSymbol = stockSymol,
                ReqId = reqid
            });
            return reqid;

        }
        public IEnumerable<StockHistoryData> GetSecondHistarySpan(string stockSymol, DateTime start,DateTime enddate)
        {
            var result=new List<StockHistoryData>();
            DateTime off = enddate;
            while(off >= start)
            {
                var b = GetSecondHistaryData(stockSymol, off);
                if(b!=null&&b.Count()>0)
                    result.AddRange(b);
                off =off.AddMinutes(-30);
               
            }
            return result;
        }
        IEnumerable<StockHistoryData> GetSecondHistaryData(string stockSymol, DateTime enddate)
        {
            return GetUsHistoryData(RequestHistoryDataDaily(stockSymol, enddate, IBStandardHistoryDataRange.HalfHour,
                IBStandardHistoryBarSize.Sec01));
        }

        public  IEnumerable<StockHistoryData> GetDailyHistoryData(string stockSymol, DateTime startDate, DateTime enddate, bool fewRequest = true)
        {
            //yyyy-MM-dd 23:59:59 :get trade  data
            startDate = startDate.AddHours(23 - startDate.Hour).AddMinutes(59 - startDate.Minute).AddSeconds(59 - startDate.Second);//=enddate.Hour
            enddate = enddate.AddHours(23 - enddate.Hour).AddMinutes(59 - enddate.Minute).AddSeconds(59 - enddate.Second);//=enddate.Hour

            if (!fewRequest)
                GetUsHistoryData(stockSymol, startDate, enddate);

            List<StockHistoryData> result = new List<StockHistoryData>();
            if (enddate.AddYears(-1).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyHistoryData(stockSymol, startDate, enddate.AddYears(-1), fewRequest));

                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Year, IBStandardHistoryBarSize.Day));

            }
            else if (enddate.AddMonths(-6).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Year, IBStandardHistoryBarSize.Day));
            }
            else if (enddate.AddMonths(-3).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.HalfYear, IBStandardHistoryBarSize.Day));
            }
            else if (enddate.AddMonths(-1).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Quater, IBStandardHistoryBarSize.Day));
            }
            else if (enddate.AddDays(-7).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Month, IBStandardHistoryBarSize.Day));
            }
            else if (enddate.AddDays(-1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Week, IBStandardHistoryBarSize.Day));
            }
            else if (enddate.AddDays(-1) == startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.TwoDay, IBStandardHistoryBarSize.Day));
            }
            else if (enddate == startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Day, IBStandardHistoryBarSize.Day));
            }
            return result.ToList();

        }

        IEnumerable<StockHistoryData> GetUsHistoryData(string stockSymol, DateTime startDate, DateTime enddate)
        {

            List<StockHistoryData> result = new List<StockHistoryData>();
            if (enddate.AddYears(-1).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Year, IBStandardHistoryBarSize.Day));
                result.AddRange(GetUsHistoryData(stockSymol, startDate, enddate.AddYears(-1)));
            }
            else if (enddate.AddMonths(-6).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.HalfYear, IBStandardHistoryBarSize.Day));
                result.AddRange(GetUsHistoryData(stockSymol, startDate, enddate.AddMonths(-6)));
            }
            else if (enddate.AddMonths(-3).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Quater, IBStandardHistoryBarSize.Day));
                result.AddRange(GetUsHistoryData(stockSymol, startDate, enddate.AddMonths(-3)));
            }
            else if (enddate.AddMonths(-1).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Month, IBStandardHistoryBarSize.Day));
                result.AddRange(GetUsHistoryData(stockSymol, startDate, enddate.AddMonths(-1)));
            }
            else if (enddate.AddDays(-7).AddDays(1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Week, IBStandardHistoryBarSize.Day));
                result.AddRange(GetUsHistoryData(stockSymol, startDate, enddate.AddDays(-7)));
            }
            else// if (enddate.AddDays(-1) > startDate)
            {
                result.AddRange(GetDailyData(stockSymol, enddate, IBStandardHistoryDataRange.Week, IBStandardHistoryBarSize.Day));
            }
            //else if (enddate.AddDays(-1) == startDate)
            //{
            //    result.AddRange(GetUsDailyHistoryDataDaily(stockSymol, enddate, IBStandardHistoryDataRange.TwoDay));
            //}
            //else if (enddate == startDate)
            //{
            //    result.AddRange(GetUsDailyHistoryDataDaily(stockSymol, enddate, IBStandardHistoryDataRange.Day));
            //}
            return result;

        }

        private  IEnumerable<StockHistoryData> GetDailyData(string stockSymol, DateTime enddate,
            IBStandardHistoryDataRange range, IBStandardHistoryBarSize barsize)
        {
            return GetUsHistoryData(RequestHistoryDataDaily(stockSymol, enddate, range, barsize));
        }

        IEnumerable<StockHistoryData> GetUsHistoryData(int reqid)
        {
            IEnumerable<StockHistoryData> result = new List<StockHistoryData>();
            while (!_testImpl.GetHistoryData(reqid, out result))
            {
                Thread.Sleep(1000 * 5);

            }
            return result;
        }

        int RequestHistoryDataDaily(string stockSymol, DateTime enddate, IBStandardHistoryDataRange range, IBStandardHistoryBarSize barsize)
        {

            //IEnumerable<StockHistoryData> result=new List<StockHistoryData>();
            int reqid = GetNextSessionId();

            _tq.Enqueue(new DataTask()
            {
                DataRequestType = DataRequestType.HISTORYBAR,
                StockSymbol = stockSymol,
                EndDate = enddate,
                Range = range,
                BarSize = barsize,
                ReqId = reqid
            });



            return reqid;

        }
        public  void Disconnet()
        {
            _testImpl.ClientSocket.Close();
        }
    }
}