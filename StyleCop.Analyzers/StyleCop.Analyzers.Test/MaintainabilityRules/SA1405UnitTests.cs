namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Analyzers.MaintainabilityRules;
    using TestHelper;

    [TestClass]
    public class SA1405UnitTests : DebugMessagesUnitTestsBase<SA1405DebugAssertMustProvideMessageText>
    {
        protected override string DiagnosticId
        {
            get
            {
                return SA1405DebugAssertMustProvideMessageText.DiagnosticId;
            }
        }

        protected override string MethodName
        {
            get
            {
                return nameof(Debug.Assert);
            }
        }

        protected override IEnumerable<string> InitialArguments
        {
            get
            {
                yield return "true";
            }
        }

        [TestMethod]
        public async Task TestWrongOverload()
        {
            var testCode = @"using System.Diagnostics;
public class Foo
{
    public void Bar()
    {
        Debug.Assert(true);
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = string.Format("Debug.Assert must provide message text", MethodName),
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);
        }
    }
}