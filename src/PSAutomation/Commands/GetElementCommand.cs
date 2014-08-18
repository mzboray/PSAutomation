using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Commands
{
    [Cmdlet(VerbsCommon.Get, "RootElement")]
    [OutputType(typeof(AutomationElement))]
    public sealed class GetRootElementCommand : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(AutomationElement.RootElement);
        }
    }
}
