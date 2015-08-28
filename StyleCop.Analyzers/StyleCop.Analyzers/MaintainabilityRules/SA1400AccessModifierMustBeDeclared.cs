namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// The access modifier for a C# element has not been explicitly defined.
    /// </summary>
    /// <remarks>
    /// <para>C# allows elements to be defined without an access modifier. Depending upon the type of element, C# will
    /// automatically assign an access level to the element in this case.</para>
    ///
    /// <para>This rule requires an access modifier to be explicitly defined for every element. This removes the need
    /// for the reader to make assumptions about the code, improving the readability of the code.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1400AccessModifierMustBeDeclared : StyleCopDiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1400AccessModifierMustBeDeclared"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1400";
        private const string Title = "Access modifier must be declared";
        private const string MessageFormat = "Element '{0}' must declare an access modifier";
        private const string Description = "The access modifier for a C# element has not been explicitly defined.";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1400.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.MaintainabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

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
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleBaseTypeDeclarationSyntax, SyntaxKind.ClassDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleBaseTypeDeclarationSyntax, SyntaxKind.InterfaceDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleBaseTypeDeclarationSyntax, SyntaxKind.EnumDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleBaseTypeDeclarationSyntax, SyntaxKind.StructDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleDelegateDeclarationSyntax, SyntaxKind.DelegateDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleEventDeclarationSyntax, SyntaxKind.EventDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleMethodDeclarationSyntax, SyntaxKind.MethodDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandlePropertyDeclarationSyntax, SyntaxKind.PropertyDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleBaseFieldDeclarationSyntax, SyntaxKind.EventFieldDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleBaseFieldDeclarationSyntax, SyntaxKind.FieldDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleOperatorDeclarationSyntax, SyntaxKind.OperatorDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleConversionOperatorDeclarationSyntax, SyntaxKind.ConversionOperatorDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleIndexerDeclarationSyntax, SyntaxKind.IndexerDeclaration);
            this.RegisterSyntaxNodeActionHonorExclusions(context, this.HandleConstructorDeclarationSyntax, SyntaxKind.ConstructorDeclaration);
        }

        private void HandleBaseTypeDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (BaseTypeDeclarationSyntax)context.Node;
            this.CheckAccessModifiers(context, syntax.Identifier, syntax.Modifiers);
        }

        private void HandleDelegateDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (DelegateDeclarationSyntax)context.Node;
            this.CheckAccessModifiers(context, syntax.Identifier, syntax.Modifiers);
        }

        private void HandleEventDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (EventDeclarationSyntax)context.Node;
            if (syntax.ExplicitInterfaceSpecifier != null)
            {
                return;
            }

            if (syntax.Parent.IsKind(SyntaxKind.InterfaceDeclaration))
            {
                return;
            }

            this.CheckAccessModifiers(context, syntax.Identifier, syntax.Modifiers);
        }

        private void HandleMethodDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (MethodDeclarationSyntax)context.Node;
            if (syntax.ExplicitInterfaceSpecifier != null)
            {
                return;
            }

            if (syntax.Parent.IsKind(SyntaxKind.InterfaceDeclaration))
            {
                return;
            }

            this.CheckAccessModifiers(context, syntax.Identifier, syntax.Modifiers);
        }

        private void HandlePropertyDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (PropertyDeclarationSyntax)context.Node;
            if (syntax.ExplicitInterfaceSpecifier != null)
            {
                return;
            }

            if (syntax.Parent.IsKind(SyntaxKind.InterfaceDeclaration))
            {
                return;
            }

            this.CheckAccessModifiers(context, syntax.Identifier, syntax.Modifiers);
        }

        private void HandleBaseFieldDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (BaseFieldDeclarationSyntax)context.Node;
            if (syntax.Parent.IsKind(SyntaxKind.InterfaceDeclaration))
            {
                // this can occur for event field declarations
                return;
            }

            VariableDeclarationSyntax declarationSyntax = syntax.Declaration;
            if (declarationSyntax == null)
            {
                return;
            }

            VariableDeclaratorSyntax declarator = declarationSyntax.Variables.FirstOrDefault(i => !i.Identifier.IsMissing);
            if (declarator == null)
            {
                return;
            }

            this.CheckAccessModifiers(context, declarator.Identifier, syntax.Modifiers, declarator);
        }

        private void HandleOperatorDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (OperatorDeclarationSyntax)context.Node;
            this.CheckAccessModifiers(context, syntax.OperatorToken, syntax.Modifiers);
        }

        private void HandleConversionOperatorDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (ConversionOperatorDeclarationSyntax)context.Node;
            this.CheckAccessModifiers(context, syntax.Type.GetLastToken(), syntax.Modifiers);
        }

        private void HandleIndexerDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (IndexerDeclarationSyntax)context.Node;
            if (syntax.ExplicitInterfaceSpecifier != null)
            {
                return;
            }

            if (syntax.Parent.IsKind(SyntaxKind.InterfaceDeclaration))
            {
                return;
            }

            this.CheckAccessModifiers(context, syntax.ThisKeyword, syntax.Modifiers);
        }

        private void HandleConstructorDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            var syntax = (ConstructorDeclarationSyntax)context.Node;
            this.CheckAccessModifiers(context, syntax.Identifier, syntax.Modifiers);
        }

        private void CheckAccessModifiers(SyntaxNodeAnalysisContext context, SyntaxToken identifier, SyntaxTokenList modifiers, SyntaxNode declarationNode = null)
        {
            if (identifier.IsMissing)
            {
                return;
            }

            foreach (SyntaxToken token in modifiers)
            {
                switch (token.Kind())
                {
                case SyntaxKind.PublicKeyword:
                case SyntaxKind.ProtectedKeyword:
                case SyntaxKind.InternalKeyword:
                case SyntaxKind.PrivateKeyword:
                    return;

                case SyntaxKind.StaticKeyword:
                    if (context.Node is ConstructorDeclarationSyntax)
                    {
                        return;
                    }

                    break;

                case SyntaxKind.PartialKeyword:
                    // the access modifier might be declared on another part, which isn't handled at this time
                    return;

                default:
                    break;
                }
            }

            // missing access modifier
            ISymbol symbol = context.SemanticModel.GetDeclaredSymbol(declarationNode ?? context.Node, context.CancellationToken);
            string name = symbol?.Name;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, identifier.GetLocation(), name));
        }
    }
}
