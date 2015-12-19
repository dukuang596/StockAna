using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.Runtime;

namespace TestRuleEngine
{
 

    public class InterceptingActivity : Activity
    {
        // Some caching variables (ExecutionEvent)
        private static FieldInfo _argsFieldInfo;
        // Some caching variables (InjectAllTheNeededHandlers)
        private static ConstructorInfo _executorCtr;
        private static PropertyInfo _currActivity;
        private static Delegate _handlerDelegate;
        private static FieldInfo _workflowExecutionEventFieldInfo;
        private static FieldInfo _workflowCoreRuntimeFieldInfo;

        // Static event for tracking rules
        public static event EventHandler<TrackingEventArgs> Track;

        public InterceptingActivity()
            : base("InterceptingActivity")
        {
            InjectAllTheNeededHandlers();
        }

        private void InjectAllTheNeededHandlers()
        {
            if (_handlerDelegate == null)
            {
                // Get the type of the WorkflowExecutor
                Type executorType =
                    typeof(WorkflowEventArgs).Assembly.GetType("System.Workflow.Runtime.WorkflowExecutor");
                // Get eventargs type, the event and the handler type
                Type eventTypeType =
                    typeof(WorkflowEventArgs).Assembly.GetType(
                        "System.Workflow.Runtime.WorkflowExecutor+WorkflowExecutionEventArgs");
                EventInfo evt = executorType.GetEvent("WorkflowExecutionEvent",
                                                      BindingFlags.Instance | BindingFlags.NonPublic);
                Type handlerType = TypeProvider.GetEventHandlerType(evt);
                // Get current activity of WorkflowExecutor
                _currActivity = executorType.GetProperty("CurrentActivity",
                                                        BindingFlags.Instance | BindingFlags.NonPublic);
                // Get the constructor
                _executorCtr = executorType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                          new[] { typeof(Guid) }, null);

                // Get field which has the event handler
                _workflowExecutionEventFieldInfo = executorType.GetField("_workflowExecutionEvent",
                                                                        BindingFlags.Instance | BindingFlags.NonPublic);
                // Get workflowCoreRuntime field of activity
                _workflowCoreRuntimeFieldInfo = typeof(Activity).GetField("workflowCoreRuntime",
                                                                          BindingFlags.Instance | BindingFlags.NonPublic);

                // Create dynamic method in module of workflow
                Module m = typeof(WorkflowEventArgs).Assembly.GetModules()[0];
                DynamicMethod dm = new DynamicMethod("MyHandler", null, new[] { typeof(object), eventTypeType }, m, true);
                MethodInfo execMethod = GetType().GetMethod("ExecutionEvent");

                // Generate method body
                ILGenerator ilgen = dm.GetILGenerator();
                ilgen.Emit(OpCodes.Nop);
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldarg_1);
                ilgen.Emit(OpCodes.Call, execMethod);
                ilgen.Emit(OpCodes.Nop);
                ilgen.Emit(OpCodes.Ret);

                // Create delegate
                _handlerDelegate = dm.CreateDelegate(handlerType);
            }

            // Create instance of WorkflowExecutor
            object executor = _executorCtr.Invoke(new object[] { Guid.NewGuid() });
            // Set current activity of WorkflowExecutor
            _currActivity.SetValue(executor, this, null);

            // Attach delegate to event
            _workflowExecutionEventFieldInfo.SetValue(executor, _handlerDelegate);

            // Set executor as workflowCoreRuntime
            _workflowCoreRuntimeFieldInfo.SetValue(this, executor);
        }

        public static void ExecutionEvent(object sender, EventArgs eventArgs)
        {
            if (Track != null)
            {
                if (_argsFieldInfo == null)
                {
                    _argsFieldInfo = eventArgs.GetType().GetField("_args",
                                                                  BindingFlags.NonPublic | BindingFlags.Instance);
                }
                var argsValue = _argsFieldInfo.GetValue(eventArgs);
                // Extract args
                RuleActionTrackingEvent args = (RuleActionTrackingEvent)argsValue;
                Track(sender, new TrackingEventArgs(args));
            }
        }

    }
}
