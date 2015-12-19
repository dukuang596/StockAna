using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.Activities.Rules;

namespace TestRuleEngine
{
    public  class StrategyBasket
    {
        public decimal TotalMoney { get; set; }
        public decimal TradingLimitRatio { get; set; }
        public decimal HardStopLimitRatio { get; set; }

        public string EnterStrategyExpression { get; set; }
        public string StopStrategyExpression { get; set; }
        public string LimitStrategyExpression { get; set; }

       // IEnumerable<Stock>  _stocks { get; set; }

        public StrategyBasket(string enter, string stop, string limit, decimal money, decimal limitratio, decimal hardstopratio)
        {
            //_stocks = ss;
            EnterStrategyExpression = enter;
            StopStrategyExpression = stop;
            LimitStrategyExpression = limit;
        }

        public StrategyBasket(string enter, string stop, string limit)
            : this(enter, stop, limit, 0, 0, 0)
        {
        }


    }
}
