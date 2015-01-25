namespace StyleCop.Analyzers.Test.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Xunit;
    using StyleCop.Analyzers.DocumentationRules;
    using TestHelper;
    using static StyleCop.Analyzers.DocumentationRules.SA1642ConstructorSummaryDocumentationMustBeginWithStandardText;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1642ConstructorSummaryDocumentationMustBeginWithStandardText"/>-
    /// </summary>
    public class SA1642UnitTests : CodeFixVerifier
    {
        public string DiagnosticId { get; } = SA1642ConstructorSummaryDocumentationMustBeginWithStandardText.DiagnosticId;

        [Fact]
        public async Task TestEmptySource()
        {
            var testCode = string.Empty;
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestNoDocumentation()
        {
            var testCode = @"namespace FooNamespace
{
    public class Foo<TFoo, TBar>
    {                                                                                                 
        public Foo()
        {

        }
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }


        private async Task TestEmptyConstructor(string modifiers)
        {
            var testCode = @"namespace FooNamespace
{
    public class Foo<TFoo, TBar>
    {{
        /// 
        /// 
        ///                                                                                                 
        {0} 
        Foo()
        {{

        }}
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestEmptyPublicConstructor()
        {
            await TestEmptyConstructor("public");
        }

        [Fact]
        public async Task TestEmptyNonPublicConstructor()
        {
            await TestEmptyConstructor("private");
        }

        [Fact]
        public async Task TestEmptyStaticConstructor()
        {
            await TestEmptyConstructor("static");
        }

        private async Task TestConstructorCorrectDocumentationSimple(string modifiers, string part1, string part2, bool generic)
        {
            var testCode = @"namespace FooNamespace
{{
    public class Foo{0}
    {{
        /// <summary>
        /// {2}<see cref=""Foo{1}""/>{3}.
        /// </summary>
        {4} Foo()
        {{

        }}
    }}
}}";


            await VerifyCSharpDiagnosticAsync(string.Format(testCode, generic ? "<T1, T2>" : string.Empty, generic ? "{T1, T2}" : string.Empty, part1, part2, modifiers), EmptyDiagnosticResults, CancellationToken.None);
        }

        private async Task TestConstructorCorrectDocumentationCustomized(string modifiers, string part1, string part2, bool generic)
        {
            var testCode = @"namespace FooNamespace
{{
    public class Foo{0}
    {{
        /// <summary>
        /// {2}<see cref=""Foo{1}""/>{3} with A and B.
        /// </summary>
        {4} Foo()
        {{

        }}
    }}
}}";


            await VerifyCSharpDiagnosticAsync(string.Format(testCode, generic ? "<T1, T2>" : string.Empty, generic ? "{T1, T2}" : string.Empty, part1, part2, modifiers), EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task TestNonPrivateConstructorCorrectDocumentationSimple()
        {
            await TestConstructorCorrectDocumentationSimple("public", NonPrivateConstructorStandardText[0], NonPrivateConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestNonPrivateConstructorCorrectDocumentationCustomized()
        {
            await TestConstructorCorrectDocumentationCustomized("public", NonPrivateConstructorStandardText[0], NonPrivateConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestNonPrivateConstructorCorrectDocumentationGenericSimple()
        {
            await TestConstructorCorrectDocumentationSimple("public", NonPrivateConstructorStandardText[0], NonPrivateConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestNonPrivateConstructorCorrectDocumentationGenericCustomized()
        {
            await TestConstructorCorrectDocumentationCustomized("public", NonPrivateConstructorStandardText[0], NonPrivateConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestPrivateConstructorCorrectDocumentationSimple()
        {
            await TestConstructorCorrectDocumentationSimple("private", PrivateConstructorStandardText[0], PrivateConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestPrivateConstructorCorrectDocumentationCustomized()
        {
            await TestConstructorCorrectDocumentationCustomized("private", PrivateConstructorStandardText[0], PrivateConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestPrivateConstructorCorrectDocumentationGenericSimple()
        {
            await TestConstructorCorrectDocumentationSimple("private", PrivateConstructorStandardText[0], PrivateConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestPrivateConstructorCorrectDocumentationGenericCustomized()
        {
            await TestConstructorCorrectDocumentationCustomized("private", PrivateConstructorStandardText[0], PrivateConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestStaticConstructorCorrectDocumentationSimple()
        {
            await TestConstructorCorrectDocumentationSimple("static", StaticConstructorStandardText[0], StaticConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestStaticConstructorCorrectDocumentationCustomized()
        {
            await TestConstructorCorrectDocumentationCustomized("static", StaticConstructorStandardText[0], StaticConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestStaticConstructorCorrectDocumentationGenericSimple()
        {
            await TestConstructorCorrectDocumentationSimple("static", StaticConstructorStandardText[0], StaticConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestStaticConstructorCorrectDocumentationGenericCustomized()
        {
            await TestConstructorCorrectDocumentationCustomized("static", StaticConstructorStandardText[0], StaticConstructorStandardText[1], true);
        }

        private async Task TestConstructorMissingDocumentation(string modifiers, string part1, string part2, bool generic)
        {
            var testCode = @"namespace FooNamespace
{{
    public class Foo{0}
    {{
        /// <summary>
        /// </summary>
        {1} 
        Foo()
        {{

        }}
    }}
}}";
            testCode = string.Format(testCode, generic ? "<T1, T2>" : string.Empty, modifiers);

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Constructor summary documentation must begin with standard text",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 5, 13)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(testCode,
                expected, CancellationToken.None);


            var fixedCode = @"namespace FooNamespace
{{
    public class Foo{0}
    {{
        /// <summary>
        /// {2}<see cref=""Foo{1}""/>{3}.
        /// </summary>
        {4} 
        Foo()
        {{

        }}
    }}
}}";
            fixedCode = string.Format(fixedCode, generic ? "<T1, T2>" : string.Empty, generic ? "{T1, T2}" : string.Empty, part1, part2, modifiers);
            await VerifyCSharpFixAsync(testCode, fixedCode);
        }

        [Fact]
        public async Task TestNonPrivateConstructorMissingDocumentation()
        {
            await TestConstructorMissingDocumentation("public", NonPrivateConstructorStandardText[0], NonPrivateConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestNonPrivateConstructorMissingDocumentationGeneric()
        {
            await TestConstructorMissingDocumentation("public", NonPrivateConstructorStandardText[0], NonPrivateConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestPrivateConstructorMissingDocumentation()
        {
            await TestConstructorMissingDocumentation("private", PrivateConstructorStandardText[0], PrivateConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestPrivateConstructorMissingDocumentationGeneric()
        {
            await TestConstructorMissingDocumentation("private", PrivateConstructorStandardText[0], PrivateConstructorStandardText[1], true);
        }

        [Fact]
        public async Task TestStaticConstructorMissingDocumentation()
        {
            await TestConstructorMissingDocumentation("static", StaticConstructorStandardText[0], StaticConstructorStandardText[1], false);
        }

        [Fact]
        public async Task TestStaticConstructorMissingDocumentationGeneric()
        {
            await TestConstructorMissingDocumentation("static", StaticConstructorStandardText[0], StaticConstructorStandardText[1], true);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SA1642ConstructorSummaryDocumentationMustBeginWithStandardText();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SA1642CodeFixProvider();
        }
    }
}
