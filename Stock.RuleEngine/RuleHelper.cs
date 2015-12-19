using System;
using System.Collections.Generic;
using System.Reflection;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;

namespace Stock.RuleEngine
{
    public class RuleHelper<T>
    {
        private RuleSet _ruleSet;
        private System.Workflow.Activities.Rules.RuleEngine _ruleEngine;
        private static readonly object SyncRoot = new object();
        private List<TrackingEventArgs> _ruleMessages;

        public RuleHelper()
        {
            _ruleMessages = new List<TrackingEventArgs>();
        }

        public void SetRules(RuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        public void Execute(T objectToRunRulesOn)
        {
            _ruleEngine = new System.Workflow.Activities.Rules.RuleEngine(_ruleSet, typeof(T));
            _ruleEngine.Execute(objectToRunRulesOn);
        }

        public void Execute(T objectToRunRulesOn, bool trackData)
        {
            if (trackData)
            {
                // Create dummy ActivityExecutionContext and see that trackData is intercepted.
                // Initialize with own activity and insert IWorkflowCoreRuntime

                Type activityExecutionContext = typeof(Activity).Assembly.GetType("System.Workflow.ComponentModel.ActivityExecutionContext");
                var ctor = activityExecutionContext.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                                   new[] { typeof(Activity) }, null);
                var activity = new InterceptingActivity();
                var context = ctor.Invoke(new object[] { activity });

                _ruleEngine = new System.Workflow.Activities.Rules.RuleEngine(_ruleSet, typeof(T));
                lock (SyncRoot)
                {
                    InterceptingActivity.Track += InterceptingActivity_Track;
                    _ruleEngine.Execute(objectToRunRulesOn, (ActivityExecutionContext)context);
                    InterceptingActivity.Track -= InterceptingActivity_Track;
                }
            }
            else
            {
                Execute(objectToRunRulesOn);
            }
        }

        public List<TrackingEventArgs> GetRuleMessages()
        {
            return _ruleMessages;
        }

        private void InterceptingActivity_Track(object sender, TrackingEventArgs e)
        {
#if DEBUG
            Console.WriteLine("{0} Rule result of {1} = {2}", e.RulesetName, e.Args.RuleName, e.Args.ConditionResult);
#endif
            _ruleMessages.Add(e);
        }
    }
}
