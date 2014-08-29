using Microsoft.PowerShell.Commands;
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
        [Parameter]
        public ScriptBlock Action { get; set; }

        protected override void ProcessRecord()
        {
            var regCmd = new RegisterObjectEventCommand()
            {
                InputObject = new PSObject(PSAutomationEvent.Instance),
                EventName = "FocusChanged",
                Action = this.Action
            };
            this.WriteObject(regCmd.Invoke(), true);
        }
    }

    public class PSAutomationEvent
    {
        private static readonly PSAutomationEvent _instance = new PSAutomationEvent();
        
        private PSAutomationEvent()
        {
            Automation.AddAutomationFocusChangedEventHandler(this.FocusChangedHandler);
        }

        public static PSAutomationEvent Instance { get { return _instance; } }

        public event AutomationFocusChangedEventHandler FocusChanged;

        private void FocusChangedHandler(object sender, AutomationFocusChangedEventArgs e)
        {
            var handler = FocusChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }        
    }
}
