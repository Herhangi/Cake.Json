using System;
using Cake.Core.IO;
using Cake.Core;
using Cake.Core.Tooling;
using Cake.Testing;
using Microsoft.DotNet.InternalAbstractions;

namespace Cake.Json.Tests
{
    public class FakeCakeContext
    {
        ICakeContext context;
        FakeLog log;
        DirectoryPath testsDir;


        public FakeCakeContext ()
        {
            testsDir = new DirectoryPath (
                System.IO.Path.GetFullPath(
                    System.IO.Path.Combine (ApplicationEnvironment.ApplicationBasePath, "../../../")));

            var environment = FakeEnvironment.CreateUnixEnvironment (false);

            var fileSystem = new FakeFileSystem (environment);
            var globber = new Globber (fileSystem, environment);
            log = new FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, log);
            var registry = new WindowsRegistry ();
            var configuration = new FakeConfiguration();

            var toolRepository = new ToolRepository(environment);
            var toolResolutionStrategy = new ToolResolutionStrategy(fileSystem, environment, globber, configuration);
            var toolLocator = new ToolLocator(environment, toolRepository, toolResolutionStrategy);

            context = new CakeContext (fileSystem, environment, globber, log, args, processRunner, registry, toolLocator);
            context.Environment.WorkingDirectory = testsDir;
        }

        public DirectoryPath WorkingDirectory {
            get { return testsDir; }
        }

        public ICakeContext CakeContext {
            get { return context; }
        }

        public string GetLogs ()
        {
            return string.Join(Environment.NewLine, log.Entries);
        }

        public void DumpLogs ()
        {
            foreach (var m in log.Entries)
                Console.WriteLine (m);
        }
    }
}
