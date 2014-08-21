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

        [Test]
        public void AndShouldAndConditionsTogether()
        {
            var result = RunCommand<AndCondition>("New-Condition -And (New-Condition ProcessId 2), (New-Condition -Name me)");
            var conditions = result.GetConditions();
            Assert.AreEqual(2, conditions.Length);

            var condition1 = (PropertyCondition)conditions[0];
            Assert.AreEqual(AutomationElement.ProcessIdProperty, condition1.Property, "proc id prop");
            Assert.AreEqual(2, condition1.Value);

            var condition2 = (PropertyCondition)conditions[1];
            Assert.AreEqual(AutomationElement.NameProperty, condition2.Property, "name prop");
            Assert.AreEqual("me", condition2.Value);
        }

        [Test]
        public void OrShouldOrConditionsTogether()
        {
            var result = RunCommand<OrCondition>("New-Condition -Or (New-Condition ProcessId 3), (New-Condition -Name meme)");
            var conditions = result.GetConditions();
            Assert.AreEqual(2, conditions.Length);

            var condition1 = (PropertyCondition)conditions[0];
            Assert.AreEqual(AutomationElement.ProcessIdProperty, condition1.Property, "proc id prop");
            Assert.AreEqual(3, condition1.Value);

            var condition2 = (PropertyCondition)conditions[1];
            Assert.AreEqual(AutomationElement.NameProperty, condition2.Property, "name prop");
            Assert.AreEqual("meme", condition2.Value);
        }

        [Test]
        public void HashtableInputDefault()
        {
            var result = RunCommand<AndCondition>("New-Condition @{ ProcessId = 4; Name = 'hello' }");
            var conditions = result.GetConditions();
            Assert.AreEqual(2, conditions.Length);

            var name = new PropertyCondition(AutomationElement.NameProperty, "hello");
            AssertConditionEquals(name, (PropertyCondition)conditions[0]);

            var procId = new PropertyCondition(AutomationElement.ProcessIdProperty, 4);
            AssertConditionEquals(procId, (PropertyCondition)conditions[1]);
        }

        [Test]
        public void CanConvertStringToControlType()
        {
            var result = RunCommand<PropertyCondition>("New-Condition ControlType 'Window'");
            Assert.AreEqual(AutomationElement.ControlTypeProperty, result.Property);
            Assert.AreEqual(ControlType.Window.Id, result.Value);
        }

        [Test]
        public void HashtableInputOr()
        {
            var result = RunCommand<OrCondition>("New-Condition -Operator Or @{ ProcessId = 3; Name = 'hello' }");
            var conditions = result.GetConditions();
            Assert.AreEqual(2, conditions.Length);

            var name = new PropertyCondition(AutomationElement.NameProperty, "hello");
            AssertConditionEquals(name, (PropertyCondition)conditions[0]);

            var procId = new PropertyCondition(AutomationElement.ProcessIdProperty, 3);
            AssertConditionEquals(procId, (PropertyCondition)conditions[1]);
        }

        [Test]
        public void HashtableInputAnd()
        {
            var result = RunCommand<AndCondition>("New-Condition -Operator And @{ ProcessId = 4; Name = 'hello' }");
            var conditions = result.GetConditions();
            Assert.AreEqual(2, conditions.Length);

            var name = new PropertyCondition(AutomationElement.NameProperty, "hello");
            AssertConditionEquals(name, (PropertyCondition)conditions[0]);

            var procId = new PropertyCondition(AutomationElement.ProcessIdProperty, 4);
            AssertConditionEquals(procId, (PropertyCondition)conditions[1]);
        }

        private static void AssertConditionEquals(PropertyCondition condition, PropertyCondition actual)
        {
            Assert.AreEqual(condition.Property, actual.Property);
            Assert.AreEqual(condition.Value, actual.Value);
            Assert.AreEqual(condition.Flags, actual.Flags);
        }
    }
}
