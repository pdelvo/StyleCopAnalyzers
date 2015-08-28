namespace StyleCop.Analyzers.DocumentationRules
{
    using System.Collections.Immutable;
    using System.Linq;
    using Helpers;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// A C# code element is missing a documentation header.
    /// </summary>
    /// <remarks>
    /// <para>C# syntax provides a mechanism for inserting documentation for classes and elements directly into the
    /// code, through the use of Xml documentation headers. For an introduction to these headers and a description of
    /// the header syntax, see the following article:
    /// <see href="http://msdn.microsoft.com/en-us/magazine/cc302121.aspx"/>.</para>
    ///
    /// <para>A violation of this rule occurs if an element is completely missing a documentation header, or if the
    /// header is empty. In C# the following types of elements can have documentation headers: classes, constructors,
    /// delegates, enums, events, finalizers, indexers, interfaces, methods, properties, and structs.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1600ElementsMustBeDocumented : StyleCopDiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1600ElementsMustBeDocumented"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1600";
        private const string Title = "Elements must be documented";
        private const string MessageFormat = "Elements must be documented";
        private const string Description = "A C# code element is missing a documentation header.";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1600.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.DocumentationRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly ImmutableArray<DiagnosticDescriptor> SupportedDiagnosticsValue =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return SupportedDiagnosticsValue;
            }
        }

        /// <inheritdoc/>
        protected override void InitializeOnCompilationStart(CompilationStartAnalysisContext context)
        {
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleTypeDeclaration, SyntaxKind.ClassDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleTypeDeclaration, SyntaxKind.StructDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleTypeDeclaration, SyntaxKind.InterfaceDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleTypeDeclaration, SyntaxKind.EnumDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleMethodDeclaration, SyntaxKind.MethodDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleDestructorDeclaration, SyntaxKind.DestructorDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandlePropertyDeclaration, SyntaxKind.PropertyDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleIndexerDeclaration, SyntaxKind.IndexerDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleFieldDeclaration, SyntaxKind.FieldDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleDelegateDeclaration, SyntaxKind.DelegateDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleEventDeclaration, SyntaxKind.EventDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleEventFieldDeclaration, SyntaxKind.EventFieldDeclaration);
        }

        private void HandleTypeDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            BaseTypeDeclarationSyntax declaration = context.Node as BaseTypeDeclarationSyntax;

            bool isNestedInClassOrStruct = this.IsNestedType(declaration);

            if (declaration != null && this.NeedsComment(declaration.Modifiers, isNestedInClassOrStruct ? SyntaxKind.PrivateKeyword : SyntaxKind.InternalKeyword))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandleMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            MethodDeclarationSyntax declaration = context.Node as MethodDeclarationSyntax;
            SyntaxKind defaultVisibility = SyntaxKind.PrivateKeyword;

            if (this.IsInterfaceMemberDeclaration(declaration) || declaration.ExplicitInterfaceSpecifier != null)
            {
                defaultVisibility = SyntaxKind.PublicKeyword;
            }

            if (declaration != null && this.NeedsComment(declaration.Modifiers, defaultVisibility))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandleConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            ConstructorDeclarationSyntax declaration = context.Node as ConstructorDeclarationSyntax;

            if (declaration != null && this.NeedsComment(declaration.Modifiers, SyntaxKind.PrivateKeyword))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandleDestructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            DestructorDeclarationSyntax declaration = context.Node as DestructorDeclarationSyntax;

            if (declaration != null)
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandlePropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            PropertyDeclarationSyntax declaration = context.Node as PropertyDeclarationSyntax;
            SyntaxKind defaultVisibility = SyntaxKind.PrivateKeyword;

            if (this.IsInterfaceMemberDeclaration(declaration) || declaration.ExplicitInterfaceSpecifier != null)
            {
                defaultVisibility = SyntaxKind.PublicKeyword;
            }

            if (declaration != null && this.NeedsComment(declaration.Modifiers, defaultVisibility))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandleIndexerDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            IndexerDeclarationSyntax declaration = context.Node as IndexerDeclarationSyntax;
            SyntaxKind defaultVisibility = SyntaxKind.PrivateKeyword;

            if (this.IsInterfaceMemberDeclaration(declaration) || declaration.ExplicitInterfaceSpecifier != null)
            {
                defaultVisibility = SyntaxKind.PublicKeyword;
            }

            if (declaration != null && this.NeedsComment(declaration.Modifiers, defaultVisibility))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.ThisKeyword.GetLocation()));
                }
            }
        }

        private void HandleFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            FieldDeclarationSyntax declaration = context.Node as FieldDeclarationSyntax;
            var variableDeclaration = declaration?.Declaration;

            if (variableDeclaration != null && this.NeedsComment(declaration.Modifiers, SyntaxKind.PrivateKeyword))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    var locations = variableDeclaration.Variables.Select(v => v.Identifier.GetLocation());
                    foreach (var location in locations)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
                    }
                }
            }
        }

        private void HandleDelegateDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            DelegateDeclarationSyntax declaration = context.Node as DelegateDeclarationSyntax;

            bool isNestedInClassOrStruct = this.IsNestedType(declaration);

            if (declaration != null && this.NeedsComment(declaration.Modifiers, isNestedInClassOrStruct ? SyntaxKind.PrivateKeyword : SyntaxKind.InternalKeyword))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandleEventDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            EventDeclarationSyntax declaration = context.Node as EventDeclarationSyntax;
            SyntaxKind defaultVisibility = SyntaxKind.PrivateKeyword;

            if (declaration.ExplicitInterfaceSpecifier != null)
            {
                defaultVisibility = SyntaxKind.PublicKeyword;
            }

            if (declaration != null && this.NeedsComment(declaration.Modifiers, defaultVisibility))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, declaration.Identifier.GetLocation()));
                }
            }
        }

        private void HandleEventFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.GetDocumentationMode() != DocumentationMode.Diagnose)
            {
                return;
            }

            EventFieldDeclarationSyntax declaration = context.Node as EventFieldDeclarationSyntax;
            SyntaxKind defaultVisibility = SyntaxKind.PrivateKeyword;

            if (this.IsInterfaceMemberDeclaration(declaration))
            {
                defaultVisibility = SyntaxKind.PublicKeyword;
            }

            var variableDeclaration = declaration?.Declaration;

            if (variableDeclaration != null && this.NeedsComment(declaration.Modifiers, defaultVisibility))
            {
                if (!XmlCommentHelper.HasDocumentation(declaration))
                {
                    var locations = variableDeclaration.Variables.Select(v => v.Identifier.GetLocation());
                    foreach (var location in locations)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
                    }
                }
            }
        }

        private bool NeedsComment(SyntaxTokenList modifiers, SyntaxKind defaultModifier)
        {
            if (!(modifiers.Any(SyntaxKind.PublicKeyword)
                || modifiers.Any(SyntaxKind.ProtectedKeyword)
                || modifiers.Any(SyntaxKind.InternalKeyword)
                || defaultModifier == SyntaxKind.PublicKeyword
                || defaultModifier == SyntaxKind.ProtectedKeyword
                || defaultModifier == SyntaxKind.InternalKeyword))
            {
                return false;
            }

            // Also ignore partial classes because they get reported as SA1601
            return !modifiers.Any(SyntaxKind.PartialKeyword);
        }

        private bool IsNestedType(BaseTypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration?.Parent is BaseTypeDeclarationSyntax;
        }

        private bool IsNestedType(DelegateDeclarationSyntax delegateDeclaration)
        {
            return delegateDeclaration?.Parent is BaseTypeDeclarationSyntax;
        }

        private bool IsInterfaceMemberDeclaration(SyntaxNode declaration)
        {
            return declaration?.Parent is InterfaceDeclarationSyntax;
        }
    }
}
