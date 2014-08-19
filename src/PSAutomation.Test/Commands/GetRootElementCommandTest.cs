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
    }
}
