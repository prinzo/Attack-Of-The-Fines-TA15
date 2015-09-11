using System;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using FineBot.API.DI;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using NUnit.Framework;
using SharpTestsEx;

namespace FineBot.Tests.DependencyTests
{
    public class DependencyTests
    {
        private ValidationResult CheckForPotentiallyMisconfiguredComponents(IWindsorContainer container)
        {
            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var diagnostics = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>();

            var handlers = diagnostics.Inspect();

            ValidationResult result = new ValidationResult();

            if (handlers.Any())
            {
                var message = new StringBuilder();
                var inspector = new DependencyInspector(message);

                foreach (IExposeDependencyInfo handler in handlers)
                {
                    handler.ObtainDependencyDetails(inspector);
                }

                result.AddMessage(Severity.Error, message.ToString());
                Console.Write(message.ToString());
            }

            return result;
        }

        [Test]
        [Ignore("Need to exclude things which should not be registered")]
        public void ApiDependenciesResolve()
        {
            //Arrange:
            IWindsorContainer container = Bootstrapper.Init();

            //Act:
            var result  = this.CheckForPotentiallyMisconfiguredComponents(container);

            //Assert:
            result.HasErrors.Should().Be.False();
        }
    }
}