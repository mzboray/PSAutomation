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
        private static readonly InitialSessionState State = CreateState();
        private static readonly Runspace DefaultRunspace = CreateRunspace();

        private static InitialSessionState CreateState()
        {
            var state = InitialSessionState.CreateDefault();
            state.ImportPSModule(new[] { Path.Combine(Environment.CurrentDirectory, "PSAutomation.psd1") });
            return state;
        }

        private static Runspace CreateRunspace()
        {
            var runspace = RunspaceFactory.CreateRunspace(State);
            runspace.Open();
            return runspace;
        }

        protected static T RunCommand<T>(string command)
        {
            using (var p = DefaultRunspace.CreatePipeline(command))
            {
                var results = p.Invoke();
                Assert.AreEqual(1, results.Count);
                return (T)results[0].BaseObject;
            }
        }

        protected static T[] RunCommandCollection<T>(string command)
        {
            using (var p = DefaultRunspace.CreatePipeline(command))
            {
                var results = p.Invoke();
                return results.Select(r => (T)r.BaseObject).ToArray();
            }
        }
    }
}
