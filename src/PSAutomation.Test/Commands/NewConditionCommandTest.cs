using NUnit.Framework;
using PSAutomation.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace PSAutomation.Test.Commands
{
    [TestFixture]
    public class NewConditionCommandTest : PSTestBase
    {
        [Test]
        public void PropertyAndValueSetShouldUseSpecifiedParameters()
        {
            var result = RunCommand<PropertyCondition>("New-Condition ProcessId 101");
            Assert.AreEqual(AutomationElement.ProcessIdProperty, result.Property);
            Assert.AreEqual(101, result.Value);
        }

        [Test]
        public void PropertyAndValueSetShouldUseSpecifiedParametersCaseInsensitive()
        {
            var result = RunCommand<PropertyCondition>("New-Condition processid 101");
            Assert.AreEqual(AutomationElement.ProcessIdProperty, result.Property);
            Assert.AreEqual(101, result.Value);
        }

        [Test]
        public void PropertyAndValueSetShouldUseSpecifiedParametersExplicit()
        {
            var result = RunCommand<PropertyCondition>("New-Condition -Property ProcessId -Value 101");
            Assert.AreEqual(AutomationElement.ProcessIdProperty, result.Property);
            Assert.AreEqual(101, result.Value);
        }

        [Test]
        public void PropertyAndValueSetShouldUseSpecifiedParametersLongNameAndValue()
        {
            var result = RunCommand<PropertyCondition>("New-Condition ProcessIdProperty 101");
            Assert.AreEqual(AutomationElement.ProcessIdProperty, result.Property);
            Assert.AreEqual(101, result.Value);
        }

        [Test]
        public void IdShouldUseSpecifiedValue()
        {
            var result = RunCommand<PropertyCondition>("New-Condition -Id abcd");
            Assert.AreEqual(AutomationElement.AutomationIdProperty, result.Property);
            Assert.AreEqual("abcd", result.Value);
        }

        [Test]
        public void NameUseSpecifiedValue()
        {
            var result = RunCommand<PropertyCondition>("New-Condition -Name abcd");
            Assert.AreEqual(AutomationElement.NameProperty, result.Property);
            Assert.AreEqual("abcd", result.Value);
        }

        [Test]
        public void ControlTypeShouldUseSpecifiedValue()
        {
            var result = RunCommand<PropertyCondition>("New-Condition -ControlType Window");
            Assert.AreEqual(AutomationElement.ControlTypeProperty, result.Property);
            Assert.AreEqual(ControlType.Window.Id, result.Value);
        }

        [Test]
        public void ControlTypeShouldUseSpecifiedValueCaseInsensitive()
        {
            var result = RunCommand<PropertyCondition>("New-Condition -ControlType button");
            Assert.AreEqual(AutomationElement.ControlTypeProperty, result.Property);
            Assert.AreEqual(ControlType.Button.Id, result.Value);
        }
    }
}
