using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRuleEngine
{
    public class Stock
    {
        public string StockCode { get; set; }
        public DateTime RuseDate { get; set; }

        RuseResult CurrentRuse { get; set; }
        public decimal LYR { get; set; }
        public decimal TTM { get; set; }
        public decimal MRQ { get; set; }
        public Dictionary<string, decimal> Indictors = new Dictionary<string, decimal>();
        public int EngineResult { get; set; }

        public Dictionary<string, Dictionary<string, object>> FuncParams = new Dictionary<string, Dictionary<string, object>>();

        public static Dictionary<string, Func<string, Dictionary<string, object>, bool>> ComplexFunc = new Dictionary<string, Func<string, Dictionary<string,object>, bool>>();
        //in operation 
        private decimal op_in_priceLimit { get; set; }
        private int op_direction { get; set; }
        private int op_amount { get; set; }

        private decimal op_price { get; set; }

        public void TradeIn()
        {
            CurrentRuse = new RuseResult
            {
                InPrice = op_in_priceLimit, Direction = op_direction, Status = RuseStatus.Hoding,InTime = RuseDate
                ,Amount = op_amount
            };
        }

        //
        public void TradeOut()
        {
            if (CurrentRuse != null)
            {
                CurrentRuse.OutTime = RuseDate;
                CurrentRuse.OutPrice = op_price;
            }
           
        }


        public bool CallFunc(string funcname) {
            if (!ComplexFunc.ContainsKey(funcname))
                throw new ApplicationException(string.Format("Func not exists",funcname));
            if (FuncParams.ContainsKey(funcname))
                return ComplexFunc[funcname](StockCode, FuncParams[funcname]);
            return ComplexFunc[funcname](StockCode,null);
        }
    }
}
