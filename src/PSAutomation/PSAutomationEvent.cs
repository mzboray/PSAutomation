using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation
{
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
