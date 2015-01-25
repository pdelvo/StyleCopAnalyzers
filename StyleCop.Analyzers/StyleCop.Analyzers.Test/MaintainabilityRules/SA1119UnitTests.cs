﻿namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Xunit;
    using Analyzers.MaintainabilityRules;
    using TestHelper;
    
    public class SA1119UnitTests : CodeFixVerifier
    {
        private const string DiagnosticId = SA1119StatementMustNotUseUnnecessaryParenthesis.DiagnosticId;

        [Fact]
        public async Task TestEmptySource()
        {
            var testCode = string.Empty;
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestLiteral()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestLiteralParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = (1);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 19)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = 1;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestLiteralDoubleParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = ((1));
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 18)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 18)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
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
        int x = 1;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task FactCall()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task FactCallParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (ToString());
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 31)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        string x = ToString();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestLocalMember()
        {
            var testCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        int x = Local + Local.IndexOf('x');
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestLocalMemberMemberAccess()
        {
            var testCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        string x = (Local).ToString() + Local.IndexOf(('x'));
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 26)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 55)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 55)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 59)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        string x = Local.ToString() + Local.IndexOf('x');
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestLocalMemberAssignment()
        {
            var testCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        this.Local = Local;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestLocalMemberAssignmentParenthesis()
        {
            var testCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        (this.Local) = Local;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 9)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 9)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 20)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        this.Local = Local;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestLocalMemberParenthesis()
        {
            var testCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        int x = (Local);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 23)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public int Local { get; set; }
    public void Bar()
    {
        int x = Local;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestCast()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = (int)3;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestCastParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = ((int)3);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 24)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = (int)3;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestCastAssignment()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x;
        x = (int)3;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestCastAssignmentParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x;
        x = ((int)3);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 20)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x;
        x = (int)3;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestCastMemberAccess()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = ((int)3).ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestCastMemberAccessAssignment()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x;
        x = ((int)3).ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestUnaryOperators()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 3;
        x = x++;
        x = x--;
        x = ++x;
        x = ~x;
        x = +x;
        x = -x;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestUnaryOperatorsParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 3;
        x = (x++);
        x = (x--);
        x = (++x);
        x = (~x);
        x = (+x);
        x = (-x);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 7, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 7, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 7, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 8, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 8, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 8, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 9, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 9, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 9, 16)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 10, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 10, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 10, 16)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 11, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 11, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 11, 16)
                        }
                },
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = 3;
        x = x++;
        x = x--;
        x = ++x;
        x = ~x;
        x = +x;
        x = -x;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestCheckedUnchecked()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 3 * checked(5);
        x = 3 * unchecked(5);
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestCheckedUncheckedParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int x = 3 * (checked(5));
        x = 3 * (unchecked(5));
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 21)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 21)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 32)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 30)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int x = 3 * checked(5);
        x = 3 * unchecked(5);
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestNameOf()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = nameof(Foo) + ""Bar"";
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestNameOfParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (nameof(Foo)) + ""Bar"";
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 32)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        string x = nameof(Foo) + ""Bar"";
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestIsExpression()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        bool x = "" is string;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestIsExpressionMemberAccess()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = ("""" is string).ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestIsExpressionParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        bool x = ("""" is string);
        x = ("""" is string);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 18)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 18)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 31)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 13)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 26)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        bool x = """" is string;
        x = """" is string;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestAssignment()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string y;
        string x = y = ""foo"";
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestAssignmentParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string y;
        string x = (y = ""foo"");
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 30)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        string y;
        string x = y = ""foo"";
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestInnerAssignment()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string y;
        string x = (y = ""foo"") + ""bar"";
        x = (y = ""foo"").ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestConditional()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = true ? ""foo"" : ""bar"";
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestConditionalParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (true ? ""foo"" : ""bar"");
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 41)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        string x = true ? ""foo"" : ""bar"";
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestConditionalInner()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (true ? ""foo"" : ""bar"") + ""test"";
        string x = (true ? ""foo"" : ""bar"").ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestCoalesce()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = ""foo"" ?? ""bar"";
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestCoalesceParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (""foo"" ?? ""bar"");
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 20)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 35)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        string x = ""foo"" ?? ""bar"";
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestCoalesceInner()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (""foo"" ?? ""bar"") + ""test"";
        string x = (""foo"" ?? ""bar"").ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestLambda()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        System.Func<string, string> x = v => v;
        System.Func<string, string> y = (v) => v;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestLambdaParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        System.Func<string, string> x = (v => v);
        System.Func<string, string> y = ((v) => v);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 41)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 41)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 48)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 41)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 41)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 50)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        System.Func<string, string> x = v => v;
        System.Func<string, string> y = (v) => v;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestLambdaInner()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (v => v)(""foo"");
        string y = ((v) => v)(""foo"");
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestArray()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int[] x = new int[10];
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestArrayParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        int[] x = (new int[10]);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 19)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 19)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 31)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        int[] x = new int[10];
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestArrayInner()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        string x = (new int[10]).ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestQuery()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        var x = from y in new int[10] select y + 1;
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestQueryParenthesis()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        var x = (from y in new int[10] select y + 1);
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 17)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId + "_p",
                    Message = "Statement must not use unnecessary parenthesis",
                    Severity = DiagnosticSeverity.Hidden,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 5, 52)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);

            var fixedCode = @"public class Foo
{
    public void Bar()
    {
        var x = from y in new int[10] select y + 1;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestQueryInner()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        var x = (from y in new int[10] select y + 1).ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestAwaitInner()
        {
            var testCode = @"public class Foo
{
    public async void Bar()
    {
        var x = (await System.Threading.Tasks.Task.Delay(10).ContinueWith(() => System.Threading.Tasks.Task.FromResult(1))).ToString();
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestBinary()
        {
            var testCode = @"public class Foo
{
    public void Bar()
    {
        var x = 1 + 1 + 1;
        var y = (1 + 1) + (1 + 1);
        var z = 1 * ~(1 + 1);
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestInnerTriviaCodeFix()
        {
            var testCode = @"public class Foo
{
        private bool Test()
        {
            string x = """";
            return (x == null || x.GetType() == typeof(object)
#if !(NET35 || NET20 || PORTABLE40)
                    || x.GetType() == typeof(string)
#endif
                );
        }
}";
            var fixedCode = @"public class Foo
{
        private bool Test()
        {
            string x = """";
            return x == null || x.GetType() == typeof(object)
#if !(NET35 || NET20 || PORTABLE40)
                    || x.GetType() == typeof(string)
#endif
                ;
        }
}";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestCodeFixWorksInTrivia()
        {
            var testCode = @"#if (NET20 || NET35)
// Foo
#endif";
            var fixedCode = @"#if NET20 || NET35
// Foo
#endif";
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SA1119StatementMustNotUseUnnecessaryParenthesis();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SA1119CodeFixProvider();
        }
    }
}
