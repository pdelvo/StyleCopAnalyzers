﻿namespace StyleCop.Analyzers.Test.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Analyzers.DocumentationRules;
    using TestHelper;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1600ElementsMustBeDocumented"/>-
    /// </summary>
    [TestClass]
    public class SA1600UnitTests : DiagnosticVerifier<SA1600ElementsMustBeDocumented>
    {
        public string DiagnosticId { get; } = SA1600ElementsMustBeDocumented.DiagnosticId;

        [TestMethod]
        public async Task TestEmptySource()
        {
            var testCode = @"";
            await VerifyCSharpDiagnosticIsEmptyAsync(testCode);
        }

        private async Task TestTypeDeclarationDocumentation(string type, string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"
{0} {1}
TypeName
{{
}}";
            var testCodeWithDocumentation = @"/// <summary> A summary. </summary>
{0} {1}
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
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 3, 1)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers, type), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestNestedTypeDeclarationDocumentation(string type, string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0} {1}
    TypeName
    {{
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0} {1}
    TypeName
    {{
    }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 5)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers, type), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestDelegateDeclarationDocumentation(string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"
{0} delegate void
DelegateName()
{{
}}";
            var testCodeWithDocumentation = @"/// <summary> A summary. </summary>
{0} delegate void
DelegateName()
{{
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 3, 1)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestNestedDelegateDeclarationDocumentation(string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0} delegate void
    DelegateName()
    {{
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0} delegate void
    DelegateName()
    {{
    }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 5)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestMethodDeclarationDocumentation(string modifiers, bool isExplicitInterfaceMethod, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0} void{1}
    MemberName()
    {{
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0} void{1}
    MemberName()
    {{
    }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 5)
                            }
                    }
                };

            string explicitInterfaceText = isExplicitInterfaceMethod ? " IInterface." : string.Empty;
            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers, explicitInterfaceText), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestInterfaceMethodDeclarationDocumentation(bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{

    void
    MemberName()
    {{
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{
    /// <summary>A summary.</summary>
    void
    MemberName()
    {
    }
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 5)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, !hasDocumentation ? expected : EmptyDiagnosticResults);
        }

        private async Task TestInterfacePropertyDeclarationDocumentation(bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{

    
    string MemberName
    {
        get; set;
    }
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{
    /// <summary>A summary.</summary>
    
    string MemberName
    {
        get; set;
    }
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 12)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, !hasDocumentation ? expected : EmptyDiagnosticResults);
        }

        private async Task TestInterfaceEventDeclarationDocumentation(bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{

    
    event System.Action MemberName;
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{
    /// <summary>A summary.</summary>
    
    event System.Action MemberName;
}}";


            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 25)
                            }
                    },
                    // Workaround because the diagnostic is called twice for the SyntaxNode
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 25)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, !hasDocumentation ? expected : EmptyDiagnosticResults);
        }

        private async Task TestInterfaceIndexerDeclarationDocumentation(bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{

    string
    this[string key] { get; set; }
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public interface InterfaceName
{{
    /// <summary>A summary.</summary>
    string
    this[string key] { get; set; }
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 5)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, !hasDocumentation ? expected : EmptyDiagnosticResults);
        }

        private async Task TestConstructorDeclarationDocumentation(string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0}
    OuterClass()
    {{
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0}
    OuterClass()
    {{
    }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 5)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestDestructorDeclarationDocumentation(bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    ~OuterClass()
    {{
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    ~OuterClass()
    {{
    }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 7, 6)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestPropertyDeclarationDocumentation(string modifiers, bool isExplicitInterfaceProperty, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0}
    string{1}
    MemberName {{ get; set; }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0}
    string{1}
    MemberName {{ get; set; }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 9, 5)
                            }
                    }
                };

            string explicitInterfaceText = isExplicitInterfaceProperty ? " IInterface." : string.Empty;
            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers, explicitInterfaceText), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestIndexerDeclarationDocumentation(string modifiers, bool isExplicitInterfaceIndexer, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0}
    string{1}
    this[string key] {{ get {{ return ""; }} set {{ }} }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0}
    string{1}
    this[string key] {{ get {{ return ""; }} set {{ }} }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 9, 5)
                            }
                    }
                };

            string explicitInterfaceText = isExplicitInterfaceIndexer ? " IInterface." : string.Empty;
            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers, explicitInterfaceText), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestEventDeclarationDocumentation(string modifiers, bool isExplicitInterfaceEvent, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    System.Action _myEvent;

    {0}
    event System.Action{1}
    MyEvent
    {{
        add
        {{
            _myEvent += value;
        }}
        remove
        {{
            _myEvent -= value;
        }}
    }}
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    System.Action _myEvent;
    /// <summary>A summary.</summary>
    {0}
    event System.Action{1}
    MyEvent
    {{
        add
        {{
            _myEvent += value;
        }}
        remove
        {{
            _myEvent -= value;
        }}
    }}
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 10, 5)
                            }
                    }
                };

            string explicitInterfaceText = isExplicitInterfaceEvent ? " IInterface." : string.Empty;
            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers, explicitInterfaceText), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestFieldDeclarationDocumentation(string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0}
    System.Action Action;
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0}
    System.Action Action;
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 19)
                            }
                    },
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 19)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestEventFieldDeclarationDocumentation(string modifiers, bool requiresDiagnostic, bool hasDocumentation)
        {
            var testCodeWithoutDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{

    {0} event
    System.Action Action;
}}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{{
    /// <summary>A summary.</summary>
    {0} event
    System.Action Action;
}}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 19)
                            }
                    },
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 8, 19)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(string.Format(hasDocumentation ? testCodeWithDocumentation : testCodeWithoutDocumentation, modifiers), requiresDiagnostic ? expected : EmptyDiagnosticResults);
        }

        private async Task TestTypeWithoutDocumentation(string type)
        {
            await TestTypeDeclarationDocumentation(type, string.Empty, true, false);
            await TestTypeDeclarationDocumentation(type, "internal", true, false);
            await TestTypeDeclarationDocumentation(type, "public", true, false);

            await TestNestedTypeDeclarationDocumentation(type, string.Empty, false, false);
            await TestNestedTypeDeclarationDocumentation(type, "private", false, false);
            await TestNestedTypeDeclarationDocumentation(type, "protected", true, false);
            await TestNestedTypeDeclarationDocumentation(type, "internal", true, false);
            await TestNestedTypeDeclarationDocumentation(type, "protected internal", true, false);
            await TestNestedTypeDeclarationDocumentation(type, "public", true, false);
        }

        private async Task TestTypeWithDocumentation(string type)
        {
            await TestTypeDeclarationDocumentation(type, string.Empty, false, true);
            await TestTypeDeclarationDocumentation(type, "internal", false, true);
            await TestTypeDeclarationDocumentation(type, "public", false, true);

            await TestNestedTypeDeclarationDocumentation(type, string.Empty, false, true);
            await TestNestedTypeDeclarationDocumentation(type, "private", false, true);
            await TestNestedTypeDeclarationDocumentation(type, "protected", false, true);
            await TestNestedTypeDeclarationDocumentation(type, "internal", false, true);
            await TestNestedTypeDeclarationDocumentation(type, "protected internal", false, true);
            await TestNestedTypeDeclarationDocumentation(type, "public", false, true);
        }


        [TestMethod]
        public async Task TestClassWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("class");
        }
        [TestMethod]
        public async Task TestStructWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("struct");
        }

        [TestMethod]
        public async Task TestEnumWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("enum");
        }

        [TestMethod]
        public async Task TestInterfaceWithoutDocumentation()
        {
            await TestTypeWithoutDocumentation("interface");
        }

        [TestMethod]
        public async Task TestClassWithDocumentation()
        {
            await TestTypeWithDocumentation("class");
        }

        [TestMethod]
        public async Task TestStructWithDocumentation()
        {
            await TestTypeWithDocumentation("struct");
        }

        [TestMethod]
        public async Task TestEnumWithDocumentation()
        {
            await TestTypeWithoutDocumentation("enum");
        }

        [TestMethod]
        public async Task TestInterfaceWithDocumentation()
        {
            await TestTypeWithoutDocumentation("interface");
        }

        [TestMethod]
        public async Task TestDelegateWithoutDocumentation()
        {
            await TestDelegateDeclarationDocumentation(string.Empty, true, false);
            await TestDelegateDeclarationDocumentation("internal", true, false);
            await TestDelegateDeclarationDocumentation("public", true, false);

            await TestNestedDelegateDeclarationDocumentation(string.Empty, false, false);
            await TestNestedDelegateDeclarationDocumentation("private", false, false);
            await TestNestedDelegateDeclarationDocumentation("protected", true, false);
            await TestNestedDelegateDeclarationDocumentation("internal", true, false);
            await TestNestedDelegateDeclarationDocumentation("protected internal", true, false);
            await TestNestedDelegateDeclarationDocumentation("public", true, false);
        }

        [TestMethod]
        public async Task TestDelegateWithDocumentation()
        {
            await TestDelegateDeclarationDocumentation(string.Empty, false, true);
            await TestDelegateDeclarationDocumentation("internal", false, true);
            await TestDelegateDeclarationDocumentation("public", false, true);

            await TestDelegateDeclarationDocumentation(string.Empty, false, true);
            await TestDelegateDeclarationDocumentation("private", false, true);
            await TestDelegateDeclarationDocumentation("protected", false, true);
            await TestDelegateDeclarationDocumentation("internal", false, true);
            await TestDelegateDeclarationDocumentation("protected internal", false, true);
            await TestDelegateDeclarationDocumentation("public", false, true);
        }

        [TestMethod]
        public async Task TestMethodWithoutDocumentation()
        {
            await TestMethodDeclarationDocumentation(string.Empty, false, false, false);
            await TestMethodDeclarationDocumentation(string.Empty, true, true, false);
            await TestMethodDeclarationDocumentation("private", false, false, false);
            await TestMethodDeclarationDocumentation("protected", false, true, false);
            await TestMethodDeclarationDocumentation("internal", false, true, false);
            await TestMethodDeclarationDocumentation("protected internal", false, true, false);
            await TestMethodDeclarationDocumentation("public", false, true, false);

            await TestInterfaceMethodDeclarationDocumentation(false);
        }

        [TestMethod]
        public async Task TestMethodWithDocumentation()
        {
            await TestMethodDeclarationDocumentation(string.Empty, false, false, true);
            await TestMethodDeclarationDocumentation(string.Empty, true, false, true);
            await TestMethodDeclarationDocumentation("private", false, false, true);
            await TestMethodDeclarationDocumentation("protected", false, false, true);
            await TestMethodDeclarationDocumentation("internal", false, false, true);
            await TestMethodDeclarationDocumentation("protected internal", false, false, true);
            await TestMethodDeclarationDocumentation("public", false, false, true);

            await TestInterfaceMethodDeclarationDocumentation(true);
        }

        [TestMethod]
        public async Task TestConstructorWithoutDocumentation()
        {
            await TestConstructorDeclarationDocumentation(string.Empty, false, false);
            await TestConstructorDeclarationDocumentation("private", false, false);
            await TestConstructorDeclarationDocumentation("protected", true, false);
            await TestConstructorDeclarationDocumentation("internal", true, false);
            await TestConstructorDeclarationDocumentation("protected internal", true, false);
            await TestConstructorDeclarationDocumentation("public", true, false);
        }

        [TestMethod]
        public async Task TestConstructorWithDocumentation()
        {
            await TestConstructorDeclarationDocumentation(string.Empty, false, true);
            await TestConstructorDeclarationDocumentation("private", false, true);
            await TestConstructorDeclarationDocumentation("protected", false, true);
            await TestConstructorDeclarationDocumentation("internal", false, true);
            await TestConstructorDeclarationDocumentation("protected internal", false, true);
            await TestConstructorDeclarationDocumentation("public", false, true);
        }

        [TestMethod]
        public async Task TestDestructorWithoutDocumentation()
        {
            await TestDestructorDeclarationDocumentation(true, false);
        }

        [TestMethod]
        public async Task TestDestructorWithDocumentation()
        {
            await TestDestructorDeclarationDocumentation(false, true);
        }

        [TestMethod]
        public async Task TestFieldWithoutDocumentation()
        {
            await TestFieldDeclarationDocumentation(string.Empty, false, false);
            await TestFieldDeclarationDocumentation("private", false, false);
            await TestFieldDeclarationDocumentation("protected", true, false);
            await TestFieldDeclarationDocumentation("internal", true, false);
            await TestFieldDeclarationDocumentation("protected internal", true, false);
            await TestFieldDeclarationDocumentation("public", true, false);
        }

        [TestMethod]
        public async Task TestFieldWithDocumentation()
        {
            await TestFieldDeclarationDocumentation(string.Empty, false, true);
            await TestFieldDeclarationDocumentation("private", false, true);
            await TestFieldDeclarationDocumentation("protected", false, true);
            await TestFieldDeclarationDocumentation("internal", false, true);
            await TestFieldDeclarationDocumentation("protected internal", false, true);
            await TestFieldDeclarationDocumentation("public", false, true);
        }

        [TestMethod]
        public async Task TestPropertyWithoutDocumentation()
        {
            await TestPropertyDeclarationDocumentation(string.Empty, false, false, false);
            await TestPropertyDeclarationDocumentation(string.Empty, true, true, false);
            await TestPropertyDeclarationDocumentation("private", false, false, false);
            await TestPropertyDeclarationDocumentation("protected", false, true, false);
            await TestPropertyDeclarationDocumentation("internal", false, true, false);
            await TestPropertyDeclarationDocumentation("protected internal", false, true, false);
            await TestPropertyDeclarationDocumentation("public", false, true, false);

            await TestInterfacePropertyDeclarationDocumentation(false);
        }

        [TestMethod]
        public async Task TestPropertyWithDocumentation()
        {
            await TestPropertyDeclarationDocumentation(string.Empty, false, false, true);
            await TestPropertyDeclarationDocumentation(string.Empty, true, false, true);
            await TestPropertyDeclarationDocumentation("private", false, false, true);
            await TestPropertyDeclarationDocumentation("protected", false, false, true);
            await TestPropertyDeclarationDocumentation("internal", false, false, true);
            await TestPropertyDeclarationDocumentation("protected internal", false, false, true);
            await TestPropertyDeclarationDocumentation("public", false, false, true);

            await TestInterfacePropertyDeclarationDocumentation(true);
        }

        [TestMethod]
        public async Task TestIndexerWithoutDocumentation()
        {
            await TestIndexerDeclarationDocumentation(string.Empty, false, false, false);
            await TestIndexerDeclarationDocumentation(string.Empty, true, true, false);
            await TestIndexerDeclarationDocumentation("private", false, false, false);
            await TestIndexerDeclarationDocumentation("protected", false, true, false);
            await TestIndexerDeclarationDocumentation("internal", false, true, false);
            await TestIndexerDeclarationDocumentation("protected internal", false, true, false);
            await TestIndexerDeclarationDocumentation("public", false, true, false);

            await TestInterfaceIndexerDeclarationDocumentation(false);
        }

        [TestMethod]
        public async Task TestIndexerWithDocumentation()
        {
            await TestIndexerDeclarationDocumentation(string.Empty, false, false, true);
            await TestIndexerDeclarationDocumentation(string.Empty, true, false, true);
            await TestIndexerDeclarationDocumentation("private", false, false, true);
            await TestIndexerDeclarationDocumentation("protected", false, false, true);
            await TestIndexerDeclarationDocumentation("internal", false, false, true);
            await TestIndexerDeclarationDocumentation("protected internal", false, false, true);
            await TestIndexerDeclarationDocumentation("public", false, false, true);

            await TestInterfaceIndexerDeclarationDocumentation(true);
        }

        [TestMethod]
        public async Task TestEventWithoutDocumentation()
        {
            await TestEventDeclarationDocumentation(string.Empty, false, false, false);
            await TestEventDeclarationDocumentation(string.Empty, true, true, false);
            await TestEventDeclarationDocumentation("private", false, false, false);
            await TestEventDeclarationDocumentation("protected", false, true, false);
            await TestEventDeclarationDocumentation("internal", false, true, false);
            await TestEventDeclarationDocumentation("protected internal", false, true, false);
            await TestEventDeclarationDocumentation("public", false, true, false);

            await TestInterfaceEventDeclarationDocumentation(false);
        }

        [TestMethod]
        public async Task TestEventWithDocumentation()
        {
            await TestEventDeclarationDocumentation(string.Empty, false, false, true);
            await TestEventDeclarationDocumentation(string.Empty, true, false, true);
            await TestEventDeclarationDocumentation("private", false, false, true);
            await TestEventDeclarationDocumentation("protected", false, false, true);
            await TestEventDeclarationDocumentation("internal", false, false, true);
            await TestEventDeclarationDocumentation("protected internal", false, false, true);
            await TestEventDeclarationDocumentation("public", false, false, true);

            await TestInterfaceEventDeclarationDocumentation(true);
        }

        [TestMethod]
        public async Task TestEventFieldWithoutDocumentation()
        {
            await TestEventFieldDeclarationDocumentation(string.Empty, false, false);
            await TestEventFieldDeclarationDocumentation("private", false, false);
            await TestEventFieldDeclarationDocumentation("protected", true, false);
            await TestEventFieldDeclarationDocumentation("internal", true, false);
            await TestEventFieldDeclarationDocumentation("protected internal", true, false);
            await TestEventFieldDeclarationDocumentation("public", true, false);
        }

        [TestMethod]
        public async Task TestEventFieldWithDocumentation()
        {
            await TestEventFieldDeclarationDocumentation(string.Empty, false, true);
            await TestEventFieldDeclarationDocumentation("private", false, true);
            await TestEventFieldDeclarationDocumentation("protected", false, true);
            await TestEventFieldDeclarationDocumentation("internal", false, true);
            await TestEventFieldDeclarationDocumentation("protected internal", false, true);
            await TestEventFieldDeclarationDocumentation("public", false, true);
        }

        [TestMethod]
        public async Task TestEmptyXmlComments()
        {
            var testCodeWithEmptyDocumentation = @"    /// <summary>
    /// </summary>
public class OuterClass
{
}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// A summary
    /// </summary>
public class OuterClass
{
}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 3, 14)
                            }
                    }
                };

            await VerifyCSharpDiagnosticIsEmptyAsync(testCodeWithDocumentation);
            await VerifyCSharpDiagnosticAsync(testCodeWithEmptyDocumentation, expected);
        }

        [TestMethod]
        public async Task TestCDataXmlComments()
        {
            var testCodeWithEmptyDocumentation = @"/// <summary>
    /// <![CDATA[]]>
    /// </summary>
public class OuterClass
{
}";
            var testCodeWithDocumentation = @"    /// <summary>
    /// <![CDATA[A summary.]]>
    /// </summary>
public class OuterClass
{
}";

            DiagnosticResult[] expected;

            expected =
                new[]
                {
                    new DiagnosticResult
                    {
                        Id = DiagnosticId,
                        Message = "Elements must be documented",
                        Severity = DiagnosticSeverity.Warning,
                        Locations =
                            new[]
                            {
                                new DiagnosticResultLocation("Test0.cs", 4, 14)
                            }
                    }
                };

            await VerifyCSharpDiagnosticAsync(testCodeWithDocumentation, EmptyDiagnosticResults);
            await VerifyCSharpDiagnosticAsync(testCodeWithEmptyDocumentation, expected);
        }

        [TestMethod]
        public async Task TestEmptyElementXmlComments()
        {
            var testCodeWithDocumentation = @"/// <inheritdoc/>
public class OuterClass
{
}";

            await VerifyCSharpDiagnosticAsync(testCodeWithDocumentation, EmptyDiagnosticResults);
        }

        [TestMethod]
        public async Task TestMultiLineDocumentation()
        {
            var testCodeWithDocumentation = @"/**
 * <summary>This is a documentation comment summary.</summary>
 */
public void SomeMethod() { }";

            await VerifyCSharpDiagnosticAsync(testCodeWithDocumentation, EmptyDiagnosticResults);
        }
    }
}