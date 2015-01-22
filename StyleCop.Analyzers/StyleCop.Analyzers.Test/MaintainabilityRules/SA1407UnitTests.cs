﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCop.Analyzers.MaintainabilityRules;
using TestHelper;

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    [TestClass]
    public class SA1407UnitTests : CodeFixVerifier<SA1407ArithmeticExpressionsMustDeclarePrecedence, SA1407SA1408CodeFixProvider>
    {
        private const string DiagnosticId = SA1407ArithmeticExpressionsMustDeclarePrecedence.DiagnosticId;
        protected static readonly DiagnosticResult[] EmptyDiagnosticResults = { };

        [TestMethod]
        public async Task TestEmptySource()
        {
            var testCode = @"";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [TestMethod]
        public async Task TestAdditionAndSubtraction()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 - 1 + 1 - 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [TestMethod]
        public async Task TestMultiplicationAndDivision()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 / 1 * 1 / 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [TestMethod]
        public async Task TestLeftShiftRightShift()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 >> 1 << 1 >> 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [TestMethod]
        public async Task TestAdditionMultiplication()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 + 1 * 1;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Arithmetic expressions must declare precedence",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 21)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 + (1 * 1);
    }
}";

            await VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestMultiplicationAddition()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 * 1 + 1;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Arithmetic expressions must declare precedence",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = (1 * 1) + 1;
    }
}";

            await VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestAdditionMultiplicationParenthesized()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 + (1 * 1);
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [TestMethod]
        public async Task TestMultiplicationAdditionParenthesized()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = (1 * 1) * 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [TestMethod]
        public async Task TestMultipleViolations()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 * 1 + 1 * 1;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Arithmetic expressions must declare precedence",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Arithmetic expressions must declare precedence",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 25)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = (1 * 1) + (1 * 1);
    }
}";

            await VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestSubViolations()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 << 1 + 1 * 1;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Arithmetic expressions must declare precedence",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 22)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Arithmetic expressions must declare precedence",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 26)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 << (1 + (1 * 1));
    }
}";

            await VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestCodeFix()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1 * 1 + 1 * 1;
        int y = 5 + y * b / 6 % z - 2;
        // the following test makes sure the code fix doesn't alter spacing
        int z = z ? 4*3+-1 :false;
    }
}";
            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = (1 * 1) + (1 * 1);
        int y = 5 + ((y * b / 6) % z) - 2;
        // the following test makes sure the code fix doesn't alter spacing
        int z = z ? (4*3)+-1 :false;
    }
}";

            await VerifyCSharpFixAsync(testCode, fixedCode);
        }
    }
}