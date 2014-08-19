using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Commands
{
    [Cmdlet(VerbsCommon.New, "Condition", DefaultParameterSetName = "PropertyAndValue")]
    [OutputType(typeof(Condition))]
    public sealed class NewConditionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PropertyAndValue")]
        public string AutomationProperty { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "PropertyAndValue")]
        public object Value { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public string Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Name")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ControlType")]
        public string ControlType { get; set; }

        protected override void ProcessRecord()
        {
            Condition condition = null;
            switch (this.ParameterSetName)
            {
                case "PropertyAndValue":
                    condition = FromPropertyNameAndValue(this.AutomationProperty, this.Value);
                    break;
                case "Id":
                    condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.Id);
                    break;
                case "Name":
                    condition = new PropertyCondition(AutomationElement.NameProperty, this.Name);
                    break;
                case "ControlType":
                    condition = GetControlType(this.ControlType);
                    break;
            }

            if (condition != null)
                this.WriteObject(condition);
        }

        private static Condition GetControlType(string controlType)
        {
            var propertyInfo = typeof(ControlType).GetProperty(controlType);
            if (propertyInfo == null)
            {
                throw new Exception(string.Format(string.Format("Could not find property named '{0}'", controlType)));
            }

            return new PropertyCondition(AutomationElement.ControlTypeProperty, propertyInfo.GetValue(null));
        }

        private Condition FromPropertyNameAndValue(string name, object value)
        {
            string lookupName = name;
            if (!lookupName.EndsWith("Property"))
                lookupName += "Property";

            var propertyInfo = typeof(AutomationElement).GetProperty(lookupName);
            if (propertyInfo == null)
            {
                throw new Exception(string.Format(string.Format("Could not find property named '{0}'", name)));
            }
            else
            {
                var condition = new PropertyCondition((AutomationProperty)propertyInfo.GetValue(null), value);
                return condition;
            }
        }
    }
}
