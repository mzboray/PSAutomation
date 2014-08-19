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
        public void GetChildElement()
        {
            var elements = RunCommandCollection<AutomationElement>("Get-ChildElement");
            Assert.AreEqual(AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition), elements);
        }
    }
}
