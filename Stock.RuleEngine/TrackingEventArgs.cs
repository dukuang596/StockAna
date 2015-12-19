using System;
using System.Workflow.Activities.Rules;

namespace Stock.RuleEngine
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
