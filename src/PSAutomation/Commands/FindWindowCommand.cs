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
    [Cmdlet(VerbsCommon.Find, "Window")]
    public sealed class FindWindowCommand : PSCmdlet
    {
        private static readonly Condition WindowCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
        
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ParameterSetName = "Name")]
        public string[] Name { get; set; }

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ParameterSetName = "Process")]
        public Process[] Process { get; set; }

        //[Parameter]
        public SwitchParameter Recurse { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "Name")
            {
                var walker = new TreeWalker(WindowCondition);
                var elements = new Queue<AutomationElement>();
                elements.Enqueue(AutomationElement.RootElement);
                while(elements.Count > 0)
                {
                    var element = elements.Dequeue();

                    InspectElement(walker, elements, element);
                    
                    while((element = walker.GetNextSibling(element)) != null)
                    {
                        InspectElement(walker, elements, element);
                    }    
                }
            }
            else if (this.ParameterSetName == "Process")
            {
                var conditions = this.Process.Select(p => new PropertyCondition(AutomationElement.ProcessIdProperty, p.Id)).ToArray();
                Condition processCond;
                if (conditions.Length > 1)
                {
                    processCond = new OrCondition(conditions);
                }
                else
                {
                    processCond = conditions[0];
                }
                var and = new AndCondition(WindowCondition, processCond);
                TreeScope scope = this.Recurse.IsPresent ? TreeScope.Descendants : TreeScope.Children;
                var windows = AutomationElement.RootElement.FindAll(scope, and);
                this.WriteObject(windows, true);
            }
            else
            {
                TreeScope scope = this.Recurse.IsPresent ? TreeScope.Descendants : TreeScope.Children;
                var windows = AutomationElement.RootElement.FindAll(scope, WindowCondition);
                this.WriteObject(windows, true);
            }
        }

        private void InspectElement(TreeWalker walker, Queue<AutomationElement> elements, AutomationElement element)
        {
            if (this.IsNameMatch(element.Current.Name))
            {
                this.WriteObject(element);
            }

            if (this.Recurse.IsPresent)
            {
                var child = walker.GetFirstChild(element);
                if (child != null)
                {
                    elements.Enqueue(child);
                }
            }
        }

        private bool IsNameMatch(string name)
        {
            return this.Name.Any(target => string.Equals(target, name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
