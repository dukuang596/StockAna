using System;
using System.Reflection;
using System.Workflow.Activities.Rules;

namespace Stock.RuleEngine
{
   
    //public class RuleConditionComponent:RuleCondition {
    //    public override bool Evaluate(RuleExecution execution)
    //    {
    //        execution.ActivityExecutionContext.TrackData
    //        base.Evaluate(execution);
    //        //execution.ThisObject;
    //        throw new NotImplementedException();
    //    }
    //}

    

    public class RuleParser
    {
        private static ConstructorInfo _ctor = null;
        private object _parser = null;
        private const BindingFlags _flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        static RuleParser() {
            Type type = Assembly.GetAssembly(typeof(RuleValidation)).GetType("System.Workflow.Activities.Rules.Parser");
            _ctor = type.GetConstructor(_flags, null, new Type[] { typeof(RuleValidation) }, null);
            if (_ctor == null) throw new NotSupportedException();
        }
        public RuleParser(RuleValidation validation)
        {
            _parser = _ctor.Invoke(new object[] { validation });
           
        }

        public RuleExpressionCondition ParseCondition(string statement)
        {

            MethodInfo mi = _parser.GetType().GetMethod("ParseCondition", _flags);
            return (RuleExpressionCondition)mi.Invoke(_parser, new object[] { statement });
        }

        public RuleAction ParseAction(string statement)
        {
            MethodInfo mi = _parser.GetType().GetMethod("ParseSingleStatement", _flags);
            return (RuleAction)mi.Invoke(_parser, new object[] { statement });
        }
    }
}
