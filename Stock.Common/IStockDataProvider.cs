﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Common
{
    public interface IStockDataProvider
    {
        void ReqSecondHistaryData(string stockSymol, DateTime start, DateTime enddate);
        void ReqMinuteHistaryData(string stockSymol, DateTime start, DateTime enddate);

        //IEnumerable<StockHistoryData> GetMinuteHistarySpan(string stockSymol, DateTime startDate, DateTime enddate);
        //IEnumerable<StockHistoryData> GetSecondHistarySpan(string stockSymol, DateTime startDate, DateTime enddate);
        IEnumerable<StockHistoryData> GetDailyHistoryData(string stockSymol, DateTime startDate, DateTime enddate,
            bool fewRequest = true);
        int ReqTickData(string stockSymol, DateTime startDate, DateTime enddate,
            bool fewRequest = true);
        void Connect();
        void Disconnet();

    }
}
