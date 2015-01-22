namespace StyleCop.Analyzers.Test.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Analyzers.DocumentationRules;
    using TestHelper;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1601PartialElementsMustBeDocumented"/>-
    /// </summary>
    [TestClass]
    public class SA1601UnitTests : DiagnosticVerifier<SA1601PartialElementsMustBeDocumented>
    {
        public string DiagnosticId { get; } = SA1601PartialElementsMustBeDocumented.DiagnosticId;

        [TestMethod]
        public async Task TestEmptySource()
        {
            var testCode = @"";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestPartialTypeWithDocumentation()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial {0} TypeName
{{
}}";
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "class"), EmptyDiagnosticResults);
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "struct"), EmptyDiagnosticResults);
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "interface"), EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestPartialTypeWithoutDocumentation()
        {
            var testCode = @"
public partial {0}
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
                        Message = "Partial elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 3, 1)
                            }
                    }
                };
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "class"), expected);
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "struct"), expected);
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "interface"), expected);
        }

        [TestMethod]
        public async Task TestPartialClassWithEmptyDocumentation()
        {
            var testCode = @"
/// <summary>
/// 
/// </summary>
public partial {0} 
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
                        Message = "Partial elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 6, 1)
                            }
                    }
                };
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "class"), expected);
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "struct"), expected);
            await VerifyCSharpDiagnosticAsync(string.Format(testCode, "interface"), expected);
        }

        [TestMethod]
        public async Task TestPartialMethodWithDocumentation()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{{
    /// <summary>
    /// Some Documentation
    /// </summary>
    partial void MemberName();
}}";
            await VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestPartialMethodWithoutDocumentation()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{{
    partial void MemberName();
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Partial elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 7, 18)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(testCode, expected);
        }

        [TestMethod]
        public async Task TestPartialMethodWithEmptyDocumentation()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{{
    /// <summary>
    /// 
    /// </summary>
    partial void MemberName();
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Partial elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 10, 18)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(testCode, expected);
        }
    }
}
