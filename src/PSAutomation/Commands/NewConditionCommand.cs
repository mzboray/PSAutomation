using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Commands
{
    [Cmdlet(VerbsCommon.New, "Condition")]
    [OutputType(typeof(Condition))]
    public sealed class NewConditionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string AutomationProperty { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public object Value { get; set; }

        protected override void ProcessRecord()
        {
            var propertyInfo = typeof(AutomationElement).GetProperty(this.AutomationProperty + "Property");
            if (propertyInfo == null)
            {
                throw new Exception(string.Format(string.Format("Could not find property named {0}", this.AutomationProperty)));
            }
            else
            {
                var condition = new PropertyCondition((AutomationProperty)propertyInfo.GetValue(null), Value);
                this.WriteObject(condition);
            }
        }
    }
}
