using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Commands
{
    [Cmdlet("Register", "FocusChangedEvent")]
    public sealed class RegisterFocusChangedEventCommand : PSCmdlet
    {
        private readonly static List<ScriptBlock> handlers = new List<ScriptBlock>();
        private static AutomationFocusChangedEventHandler psHandler;

        [Parameter(Mandatory = true, Position = 0)]
        public ScriptBlock Action { get; set; }

        protected override void ProcessRecord()
        {
            if (psHandler == null)
            {
                psHandler = FocusChangedHandler;
                Automation.AddAutomationFocusChangedEventHandler(psHandler);
            }

            lock (handlers)
            {
                handlers.Add(this.Action);
            }
        }

        private static void FocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            ScriptBlock[] localHandlers;
            lock(handlers)
            {
                localHandlers = handlers.ToArray();
            }

            foreach(var handler in localHandlers)
            {
                thi
                handler.Invoke(src, args);
            }
        }
    }
}
