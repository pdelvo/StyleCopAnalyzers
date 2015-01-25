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

    /// <summary>
    /// This class contains unit tests for <see cref="SA1605PartialElementDocumentationMustHaveSummary"/>-
    /// </summary>
    public class SA1605UnitTests : CodeFixVerifier
    {
        public string DiagnosticId { get; } = SA1605PartialElementDocumentationMustHaveSummary.DiagnosticId;

        [Fact]
        public async Task TestEmptySource()
        {
            var testCode = string.Empty;
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        private async Task TestTypeNoDocumentation(string typeName)
        {
            var testCode = @"
partial {0} TypeName
{{
}}";
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, typeName), EmptyDiagnosticResults, CancellationToken.None);
        }

        private async Task TestTypeWithSummaryDocumentation(string typeName)
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
partial {0} TypeName
{{
}}";
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, typeName), EmptyDiagnosticResults, CancellationToken.None);
        }

        private async Task TestTypeWithContentDocumentation(string typeName)
        {
            var testCode = @"
/// <content>
/// 
/// </content>
partial {0} TypeName
{{
}}";
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, typeName), EmptyDiagnosticResults, CancellationToken.None);
        }

        private async Task TestTypeWithInheritedDocumentation(string typeName)
        {
            var testCode = @"
/// <inheritdoc/>
partial {0} TypeName
{{
}}";
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, typeName), EmptyDiagnosticResults, CancellationToken.None);
        }

        private async Task TestTypeWithoutDocumentation(string typeName)
        {
            var testCode = @"
///
partial {0}
TypeName
{{
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Partial element documentation must have summary",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 4, 1)
                            }
                    }
                };
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, typeName), expected, CancellationToken.None);
        }

        [Fact]
        public async Task TestClassWithDocumentation()
        {
            await TestTypeWithSummaryDocumentation("class");
        }

        [Fact]
        public async Task TestStructWithDocumentation()
        {
            await TestTypeWithSummaryDocumentation("struct");
        }

        [Fact]
        public async Task TestInterfaceWithDocumentation()
        {
            await TestTypeWithSummaryDocumentation("interface");
        }

        [Fact]
        public async Task TestClassWithContentDocumentation()
        {
            await TestTypeWithContentDocumentation("class");
        }

        [Fact]
        public async Task TestStructWithContentDocumentation()
        {
            await TestTypeWithContentDocumentation("struct");
        }

        [Fact]
        public async Task TestInterfaceWithContentDocumentation()
        {
            await TestTypeWithContentDocumentation("interface");
        }

        [Fact]
        public async Task TestClassWithInheritedDocumentation()
        {
            await TestTypeWithInheritedDocumentation("class");
        }

        [Fact]
        public async Task TestStructWithInheritedDocumentation()
        {
            await TestTypeWithInheritedDocumentation("struct");
        }

        [Fact]
        public async Task TestInterfaceWithInheritedDocumentation()
        {
            await TestTypeWithInheritedDocumentation("interface");
        }

        [Fact]
        public async Task TestClassWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("class");
        }

        [Fact]
        public async Task TestStructWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("struct");
        }

        [Fact]
        public async Task TestInterfaceWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("interface");
        }

        [Fact]
        public async Task TestEnumNoDocumentation()
        {
            await TestTypeNoDocumentation("enum");
        }

        [Fact]
        public async Task TestClassNoDocumentation()
        {
            await TestTypeNoDocumentation("class");
        }

        [Fact]
        public async Task TestStructNoDocumentation()
        {
            await TestTypeNoDocumentation("struct");
        }

        [Fact]
        public async Task TestInterfaceNoDocumentation()
        {
            await TestTypeNoDocumentation("interface");
        }

        [Fact]
        public async Task FactNoDocumentation()
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
public class ClassName
{
    partial void Test();
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task FactWithSummaryDocumentation()
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
public class ClassName
{
    /// <summary>
    ///
    /// </summary>
    partial void Test();
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task FactWithContentDocumentation()
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
public class ClassName
{
    /// <content>
    ///
    /// </content>
    partial void Test();
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task FactWithInheritedDocumentation()
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
public class ClassName
{
    /// <inheritdoc/>
    partial void Test();
}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        [Fact]
        public async Task FactWithoutDocumentation()
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
public class ClassName
{
    ///
    partial void Test();
}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Partial element documentation must have summary",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 18)
                            }
                    }
                };
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);
        }
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SA1605PartialElementDocumentationMustHaveSummary();
        }
    }
}
