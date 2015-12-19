using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Common
{
    public interface IStockDataProvider
    {
        IEnumerable<StockHistoryData> GetDailyHistoryData(string stockSymol, DateTime startDate, DateTime enddate,
            bool fewRequest = true);

        void Connect();
        void Disconnet();

    }
}
