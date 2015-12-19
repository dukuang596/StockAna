using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.Activities.Rules;

namespace TestRuleEngine
{
    public class TrackingEventArgs : EventArgs
    {

        public TrackingEventArgs(RuleActionTrackingEvent args)
        {
            Args = args;
            RulesetName = args.RuleName;
        }

        public RuleActionTrackingEvent Args { get; private set; }

        public string RulesetName { get; private set; }

    }
}
