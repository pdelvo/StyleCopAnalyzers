using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCop.Analyzers.MaintainabilityRules;
using TestHelper;

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    [TestClass]
    public class SA1403UnitTests : FileMayOnlyContainTestBase<SA1403FileMayOnlyContainASingleNamespace>
    {
        public override string Keyword
        {
            get
            {
                return "namespace";
            }
        }

        public override string DiagnosticId
        {
            get
            {
                return SA1403FileMayOnlyContainASingleNamespace.DiagnosticId;
            }
        }

        [TestMethod]
        public async Task TestNestedNamespaces()
        {
            var testCode = @"namespace Foo
{
    namespace Bar
    {

    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = Message,
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 3, 15)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

        }
    }
}