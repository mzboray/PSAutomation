using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Commands
{
    public enum ConditionOperator
    {
        And, Or
    }

    [Cmdlet(VerbsCommon.New, "Condition", DefaultParameterSetName = "PropertyAndValue")]
    [OutputType(typeof(Condition))]
    public sealed class NewConditionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PropertyAndValue")]
        public string Property { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "PropertyAndValue")]
        public object Value { get; set; }

        [Parameter(ParameterSetName = "CommonProperties")]
        public string Id { get; set; }

        [Parameter(ParameterSetName = "CommonProperties")]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "CommonProperties")]
        public string ControlType { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "And")]
        public Condition[] And { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Or")]
        public Condition[] Or { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ConditionHashtable")]
        public System.Collections.Hashtable Conditions { get; set; }

        [Parameter(ParameterSetName = "ConditionHashtable")]
        public ConditionOperator Operator { get; set; }

        protected override void ProcessRecord()
        {
            Condition condition = null;
            switch (this.ParameterSetName)
            {
                case "PropertyAndValue":
                    condition = FromPropertyNameAndValue(this.Property, this.Value);
                    break;
                case "CommonProperties":
                    condition = FromCommonPropeties();
                    break;
                case "And":
                    condition = new AndCondition(this.And);
                    break;
                case "Or":
                    condition = new OrCondition(this.Or);
                    break;
                case "ConditionHashtable":
                    var conditions = new List<Condition>(this.Conditions.Count);
                    foreach(System.Collections.DictionaryEntry entry in this.Conditions)
                    {
                        conditions.Add(FromPropertyNameAndValue((string)entry.Key, entry.Value));
                    }
                    condition = this.Operator == ConditionOperator.Or ? (Condition)new OrCondition(conditions.ToArray()) : new AndCondition(conditions.ToArray());
                    break;
                default:
                    Debug.Fail(string.Format("Missing case for '{0}", this.ParameterSetName));
                    break;
            }

            if (condition != null)
                this.WriteObject(condition);
        }

        private Condition FromCommonPropeties()
        {
            List<Condition> conditions = new List<Condition>();
            if (this.Name != null)
            {
                conditions.Add(new PropertyCondition(AutomationElement.NameProperty, this.Name));
            }

            if (this.Id != null)
            {
                conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, this.Id));
            }

            if (this.ControlType != null)
            {
                conditions.Add(GetControlTypeCondition(this.ControlType));
            }

            if (conditions.Count == 1)
            {
                return conditions[0];
            }
            else
            {
                return new AndCondition(conditions.ToArray());
            }
        }

        private static ControlType GetControlType(string controlType)
        {
            var fieldInfo = typeof(ControlType).GetFields().FirstOrDefault(fi => string.Equals(fi.Name, controlType, StringComparison.InvariantCultureIgnoreCase));
            if (fieldInfo == null)
            {
                throw new Exception(string.Format("Could not find control type '{0}'", controlType));
            }

            return (ControlType)fieldInfo.GetValue(null);
        }

        private static Condition GetControlTypeCondition(string controlType)
        {
            ControlType controlTypeObj = GetControlType(controlType);
            return new PropertyCondition(AutomationElement.ControlTypeProperty, controlTypeObj);
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
                var property = (AutomationProperty)fieldInfo.GetValue(null);
                if (property.Id == AutomationElement.ControlTypeProperty.Id)
                {
                    if (value is string)
                    {
                        value = GetControlType((string)value);
                    }
                }
                var condition = new PropertyCondition(property, value);
                return condition;
            }
        }
    }
}
