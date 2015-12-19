using System;

namespace TestRuleEngine
{
    public enum RuseStatus
    {
        Hoding,
        Finish
    }
    public class RuseResult
    {
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }

        public decimal InPrice { get; set; }
        public decimal OutPrice { get; set; }

        public int Amount { get; set; }
        //1 long -1 short
        public int Direction { get; set; }

        public RuseStatus Status { get; set; }
        public bool IsSuccessful{
            get { return GainAmount > 0; }
        }

        public decimal GainAmount
        {

            get { return (OutPrice - InPrice)*Direction*Amount; }
        }
    }
}