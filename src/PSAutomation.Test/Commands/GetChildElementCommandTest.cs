using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Test.Commands
{
    [TestFixture]
    public class GetChildElementCommandTest : PSTestBase
    {
        [Test]
        public void GetChildElementUsesRoot()
        {
            var elements = RunCommandCollection<AutomationElement>("Get-ChildElement");
            Assert.AreEqual(AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition), elements);
        }

        [Test]
        public void GetChildElementRunsFilterCondition()
        {
            var elements = RunCommandCollection<AutomationElement>("Get-ChildElement -Condition (New-Condition -ControlType Window)");
            var cond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
            Assert.AreEqual(AutomationElement.RootElement.FindAll(TreeScope.Children, cond), elements);
        }

        [Test]
        public void GetChildElementRunsFilterConditions()
        {
            var elements = RunCommandCollection<AutomationElement>("Get-ChildElement -Condition (New-Condition -ControlType Window), (New-Condition -ControlType Pane)");
            var cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
            var cond2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);
            var results1 = AutomationElement.RootElement.FindAll(TreeScope.Children, cond1).Cast<AutomationElement>();
            var results2 = AutomationElement.RootElement.FindAll(TreeScope.Children, cond2).Cast<AutomationElement>();
            var results = results1.Concat(results2);
            Assert.AreEqual(results, elements);
        }
    }
}
