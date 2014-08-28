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
    public class GetRootElementCommandTest : PSTestBase
    {
        [Test]
        public void GetRootElementReturnsRootElement()
        {
            var root = RunCommand<AutomationElement>("Get-RootElement");
            Assert.AreEqual(AutomationElement.RootElement, root);
        }

        [Test]
        public void GetRootElementHasExtendedProperties()
        {
            var element = RunCommandRaw("Get-RootElement");
            var propertyNames = element.Properties.Select(p => p.Name).ToList();
            Assert.GreaterOrEqual(propertyNames.Count, 3);
            foreach (var prop in ((AutomationElement)element.BaseObject).GetSupportedProperties())
            {
                Assert.Contains(Automation.PropertyName(prop), propertyNames);
            }
        }
    }
}
