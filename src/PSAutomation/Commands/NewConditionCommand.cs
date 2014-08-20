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
        public string Property { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "PropertyAndValue")]
        public object Value { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public string Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Name")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ControlType")]
        public string ControlType { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "And")]
        public Condition[] And { get; set; }

        protected override void ProcessRecord()
        {
            Condition condition = null;
            switch (this.ParameterSetName)
            {
                case "PropertyAndValue":
                    condition = FromPropertyNameAndValue(this.Property, this.Value);
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
                case "And":
                    condition = new AndCondition(this.And);
                    break;
            }

            if (condition != null)
                this.WriteObject(condition);
        }

        private static Condition GetControlType(string controlType)
        {
            var fieldInfo = typeof(ControlType).GetFields().FirstOrDefault(fi => string.Equals(fi.Name, controlType, StringComparison.InvariantCultureIgnoreCase));
            if (fieldInfo == null)
            {
                throw new Exception(string.Format("Could not find control type '{0}'", controlType));
            }

            return new PropertyCondition(AutomationElement.ControlTypeProperty, fieldInfo.GetValue(null));
        }

        private Condition FromPropertyNameAndValue(string name, object value)
        {
            string lookupName = name;
            if (!lookupName.EndsWith("Property"))
                lookupName += "Property";

            var fieldInfo = typeof(AutomationElement).GetFields().FirstOrDefault(fi => string.Equals(fi.Name, lookupName, StringComparison.InvariantCultureIgnoreCase));
            if (fieldInfo == null)
            {
                throw new Exception(string.Format("Could not find automation property '{0}'", name));
            }
            else
            {
                var condition = new PropertyCondition((AutomationProperty)fieldInfo.GetValue(null), value);
                return condition;
            }
        }
    }
}
