using System;

namespace Stock.Common
{
    public class StockHistoryData{
    
        public  DateTime StartTime {get;set;}
        public DateTime EndTime { get; set; }
        public double Open{get;set;}
        public double High{get;set;}
        public double Low {get;set;}
        public double Close{get;set;}
        public  int Volume{get;set;}
        public int Count{get;set;}
        public double Wap{get;set;}
        public bool HasGaps{get;set;}
    
    }
}