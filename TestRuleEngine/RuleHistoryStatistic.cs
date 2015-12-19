using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.Activities.Rules;
using Stock.RuleEngine;

namespace TestRuleEngine
{
 

    public class RuseHistoryStatistic
    {
        private StrategyBasket _strategeb;
    
        DateTime _start;
        DateTime _end;
        IEnumerable<Stock> _slist;

        private Dictionary<string, List<RuseResult>> RuseResultDict;
        
        //RuleSet _lose_ruleSet;
         
        public RuseHistoryStatistic(StrategyBasket  strategeb,IEnumerable<Stock> slist) {
 
            _slist = slist;
        }

        public void Execute() {
            RuleValidation validation = new RuleValidation(typeof(Stock), null);
            RuleParser parser = new RuleParser(validation);

            // in rule
            RuleCondition condition = parser.ParseCondition(_strategeb.EnterStrategyExpression);
            RuleAction thenAction = parser.ParseAction("EngineResult = 100");
            RuleAction elseAction = parser.ParseAction("EngineResult =-100");
            RuleSet inRuleSet = new RuleSet();
            var inRule = new Rule("InRule", condition, new List<RuleAction> { thenAction }, new List<RuleAction> { elseAction });
            inRuleSet.Rules.Add(inRule);
            

            //stop out rule
             condition = parser.ParseCondition(_strategeb.StopStrategyExpression);
             thenAction = parser.ParseAction("EngineResult = 100");
             elseAction = parser.ParseAction("EngineResult =-100");
             RuleSet stopout=new RuleSet();
             var stopoutrule = new Rule("stopoutrule", condition, new List<RuleAction> {thenAction},
                new List<RuleAction> {elseAction});
            stopout.Rules.Add(stopoutrule);
             //limit out rule

            condition = parser.ParseCondition(_strategeb.LimitStrategyExpression);
            thenAction = parser.ParseAction("EngineResult = 100");
            elseAction = parser.ParseAction("EngineResult =-100");
            RuleSet limitout = new RuleSet();
            var limitoutrule = new Rule("limitoutrule", condition, new List<RuleAction> { thenAction },
               new List<RuleAction> { elseAction });
            limitout.Rules.Add(limitoutrule);
            RuleHelper<Stock> rhelper = new RuleHelper<Stock>();
            
            foreach (var s in _slist) {
                for (DateTime dt = _start; dt <= _end; dt=dt.AddDays(1))
                {
                    rhelper.SetRules(inRuleSet);
                    //
                    s.RuseDate = _start;
                    rhelper.Execute(s, true);

                    if (s.EngineResult == 100)
                    {

                        do
                        {
                            _start = _start.AddDays(1);
                            s.RuseDate = _start;
                            rhelper.SetRules(stopout);
                            rhelper.Execute(s, true);
                            if (s.EngineResult == 100)
                            {
                                break;
                            }
                            //
                            rhelper.SetRules(limitout);
                            rhelper.Execute(s, true);
                            if (s.EngineResult == 100)
                            {
                                break;
                            }              
                        } while (_start <= _end);
                    }
                   

                  
                }
        
            }
         
        
        }
        
    }
}
