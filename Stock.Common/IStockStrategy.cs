using System;
using System.Collections.Generic;

namespace Stock.Common
{
    public interface IStockStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisData"></param>
        /// <returns>int 0:no pattern ,1     </returns>
        Tuple<int, decimal, decimal, decimal> CalculatePattern(IEnumerable<StockHistoryData>  hisData);
    }
}
