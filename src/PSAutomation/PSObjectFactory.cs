using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation
{
    static class PSObjectFactory
    {
        public static PSObject WrapElement(AutomationElement element)
        {
            var psObj = new PSObject(element);
            foreach (var prop in element.GetSupportedProperties())
            {
                if (prop.Id == AutomationElement.NameProperty.Id ||
                    prop.Id == AutomationElement.ControlTypeProperty.Id)
                    continue;

                string name = Automation.PropertyName(prop);
                string script = string.Format("$this.GetCurrentPropertyValue([System.Windows.Automation.AutomationProperty]::LookupById({0}))", prop.Id);
                var psProp = new PSScriptProperty(name, ScriptBlock.Create(script));
                psObj.Properties.Add(psProp);
            }

            foreach(var pattern in element.GetSupportedPatterns())
            {
                string name = Automation.PatternName(pattern) + "Pattern";
                string script = string.Format("$this.GetCurrentPattern([System.Windows.Automation.AutomationPattern]::LookupById({0}))", pattern.Id);
                var psProp = new PSScriptProperty(name, ScriptBlock.Create(script));
                psObj.Properties.Add(psProp);
            }
            return psObj;
        }

        public static IEnumerable<PSObject> WrapElements(IEnumerable<AutomationElement> elements)
        {
            foreach (var elem in elements)
            {
                yield return WrapElement(elem);
            }
        }
    }

}
