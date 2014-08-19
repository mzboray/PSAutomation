using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Commands
{
    [Cmdlet(VerbsCommon.Get, "ChildElement")]
    [OutputType(typeof(AutomationElement))]
    [Alias("gce")]
    public sealed class GetChildElementCommand : PSCmdlet
    {
        [Parameter]
        public AutomationElement Root { get; set; }

        [Parameter]
        public SwitchParameter Recurse { get; set; }

        [Parameter]
        public Condition Condition { get; set; }

        protected override void ProcessRecord()
        {
            AutomationElement root = this.Root ?? AutomationElement.RootElement;
            TreeScope scope = this.Recurse.IsPresent ? TreeScope.Descendants : TreeScope.Children;
            Condition condition = this.Condition ?? Condition.TrueCondition;
            var results = root.FindAll(scope, condition);
            this.WriteObject(results, true);
        }
    }
}
