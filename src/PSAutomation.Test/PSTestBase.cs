using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace PSAutomation.Test
{
    [TestFixture]
    public abstract class PSTestBase
    {
        protected static T RunCommand<T>(string command)
        {
            var state = InitialSessionState.CreateDefault();
            state.ImportPSModule(new[] { Path.Combine(Environment.CurrentDirectory, "PSAutomation.psd1") });
            using (var runspace = RunspaceFactory.CreateRunspace(state))
            {
                runspace.Open();
                using (var p = runspace.CreatePipeline(command))
                {
                    var results = p.Invoke();
                    Assert.AreEqual(1, results.Count);
                    return (T)results[0].BaseObject;
                }
            }
        }

        protected static T[] RunCommandCollection<T>(string command)
        {
            var state = InitialSessionState.CreateDefault();
            state.ImportPSModule(new[] { Path.Combine(Environment.CurrentDirectory, "PSAutomation.dll") });
            using (var runspace = RunspaceFactory.CreateRunspace(state))
            {
                runspace.Open();
                using (var p = runspace.CreatePipeline(command))
                {
                    var results = p.Invoke();
                    return results.Select(r => (T)r.BaseObject).ToArray();
                }
            }
        }
    }
}
