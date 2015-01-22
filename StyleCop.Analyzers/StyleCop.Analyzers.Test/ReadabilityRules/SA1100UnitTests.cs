﻿namespace StyleCop.Analyzers.Test.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Analyzers.ReadabilityRules;
    using TestHelper;

    [TestClass]
    public class SA1100UnitTests : CodeFixVerifier<SA1100DoNotPrefixCallsWithBaseUnlessLocalImplementationExists, SA1100CodeFixProvider>
    {
        private const string DiagnosticId = SA1100DoNotPrefixCallsWithBaseUnlessLocalImplementationExists.DiagnosticId;

        [TestMethod]
        public async Task TestChildClassUsesBaseButNoOverride()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar();
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseButMethodIsNotVirtual()
        {
            var testCode = @"
public class Foo
{
    protected void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar();
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseBaseAndChildHaveMethodWithSameName()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected  void Baz()
    {
        base.Bar();
    }
    protected void Bar()
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodWithSameNameButDifferentParametersExist()
        {
            var testCode = @"
public class Foo
{
    protected void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar();
    }
    protected void Bar(string param)
    {

    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar();
    }
    protected void Bar(string param)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseBaseIsVirtualChildHidesBase()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected  void Baz()
    {
        base.Bar();
    }
    protected void Bar()
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestChildClassUsesBasePropertyButNoOverride()
        {
            var testCode = @"
public class Foo
{
    protected virtual string Bar
    {
        get;set;
    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        var s = base.Bar;
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 17)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual string Bar
    {
        get;set;
    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        var s = this.Bar;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodMethodWithSameNameButDifferentParametersExist()
        {
            var testCode = @"
public class Foo
{
    protected void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar();
    }
    protected void Bar(string param)
    {

    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar();
    }
    protected void Bar(string param)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodMethodWithSameNameButDifferentParameterType()
        {
            var testCode = @"
public class Foo
{
    protected void Bar(string s)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar("""");
    }
    protected void Bar(long l)
    {

    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Bar(string s)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar("""");
    }
    protected void Bar(long l)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodMethodWithSameNameRefUsed()
        {
            var testCode = @"
public class Foo
{
    protected void Bar(string s)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar("""");
    }
    protected void Bar(ref string s)
    {

    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Bar(string s)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar("""");
    }
    protected void Bar(ref string s)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodMethodWithSameNameOutUsed()
        {
            var testCode = @"
public class Foo
{
    protected void Bar(string s)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar("""");
    }
    protected void Bar(out string s)
    {
        s = string.Empty;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Bar(string s)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar("""");
    }
    protected void Bar(out string s)
    {
        s = string.Empty;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodOverrideWithDifferentParametersExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar();
    }
    protected void Bar(string param)
    {

    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar();
    }
    protected void Bar(string param)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodOverrideWithRefExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar(string.Empty, 5);
    }
    protected void Bar(ref string s, int i)
    {

    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar(string.Empty, 5);
    }
    protected void Bar(ref string s, int i)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodOverrideWithOutExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected  void Baz()
    {
        base.Bar(string.Empty, 5);
    }
    protected void Bar(out string s, int i)
    {
        s = string.Empty;
    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected  void Baz()
    {
        this.Bar(string.Empty, 5);
    }
    protected void Bar(out string s, int i)
    {
        s = string.Empty;
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseMethodOverrideWithDifferentParameterTypeExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected  void Baz()
    {
        base.Bar(string.Empty, 5);
    }
    protected void Bar(int i, string s)
    {

    }
}";
            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected  void Baz()
    {
        this.Bar(string.Empty, 5);
    }
    protected void Bar(int i, string s)
    {

    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseOverrideExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected override void Baz()
    {
        base.Bar();
    }
    protected override void Bar()
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);

        }

        [TestMethod]
        public async Task TestChildClassUsesBaseWithFewParametersOverrideExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar(string s, int i)
    {

    }
}

public class FooChild : Foo
{
    protected override void Baz()
    {
        base.Bar(string.Empty,5);
    }
    protected override void Bar(string s, int i)
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);

        }

        [TestMethod]
        public async Task TestChildClassUsesBaseWithFewParametersHidingMethodExists()
        {
            var testCode = @"
public class Foo
{
    protected void Bar(string s, ref int i)
    {

    }
}

public class FooChild : Foo
{
    protected override void Baz()
    {
        base.Bar(string.Empty,5);
    }
    protected void Bar(string s, ref int i)
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);

        }

        [TestMethod]
        public async Task TestChildClassUsesBasePropertyOverrideExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual string Bar
    {
        get;set;
    }
}

public class FooChild : Foo
{
    protected override void Baz()
    {
        var s = base.Bar;
    }

    protected override string Bar
    {
        get;set;
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestChildEventNoOverride()
        {
            var testCode = @"
public class Foo
{
    protected event Action MyEvent;
}

public class FooChild : Foo
{
    protected void Baz()
    {
        if(base.MyEvent != null)
        {

        }
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 11, 12)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected event Action MyEvent;
}

public class FooChild : Foo
{
    protected void Baz()
    {
        if(this.MyEvent != null)
        {

        }
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildEventOverrideExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual event Action MyEvent;
}

public class FooChild : Foo
{
    protected override event Action MyEvent;

    protected void Baz()
    {
        if(base.MyEvent != null)
        {

        }
    }
}";


            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestChildEventHidingEventExists()
        {
            var testCode = @"
public class Foo
{
    protected virtual event Action MyEvent;
}

public class FooChild : Foo
{
    protected new event Action MyEvent;

    protected void Baz()
    {
        if(base.MyEvent != null)
        {

        }
    }
}";


            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestStruct()
        {
            var testCode = @"
public struct S
{
    public string Baz()
    {
        return base.ToString();
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 6, 16)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public struct S
{
    public string Baz()
    {
        return this.ToString();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseButNoOverrideTwoIssues()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }

    protected virtual void Bar2()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base.Bar();
    }

    protected void Baz2()
    {
        base.Bar2();
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 19, 9)
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 24, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar()
    {

    }

    protected virtual void Bar2()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this.Bar();
    }

    protected void Baz2()
    {
        this.Bar2();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseWithExtraLinesButNoOverrideTwoIssues()
        {
            var testCode = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        base

        .Bar();
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 14, 9)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected virtual void Bar()
    {

    }
}

public class FooChild : Foo
{
    protected void Baz()
    {
        this

        .Bar();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }

        [TestMethod]
        public async Task TestChildClassUsesBaseWithPreprocessorDirectivesButNoOverrideTwoIssues()
        {
            var testCode = @"
public class Foo
{
    protected void Baz()
    {
        #if true
                base
        #else
            this
        #endif
        .ToString();
    }
}";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Message = "Do not prefix calls with base unless local implementation exists",
                    Severity =  DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 7, 17)
                        }
                }
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected);

            var fixedTest = @"
public class Foo
{
    protected void Baz()
    {
        #if true
                this
        #else
            this
        #endif
        .ToString();
    }
}";
            await VerifyCSharpFixAsync(testCode, fixedTest, cancellationToken: CancellationToken.None);
        }
    }
}
