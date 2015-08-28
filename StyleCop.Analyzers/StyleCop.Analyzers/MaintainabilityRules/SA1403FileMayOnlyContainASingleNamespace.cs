﻿namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// A C# code file contains more than one namespace.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when a C# file contains more than one namespace. To increase long-term
    /// maintainability of the code-base, each file should contain at most one namespace.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1403FileMayOnlyContainASingleNamespace : StyleCopDiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1403FileMayOnlyContainASingleNamespace"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1403";
        private const string Title = "File may only contain a single namespace";
        private const string MessageFormat = "File may only contain a single namespace";
        private const string Description = "A C# code file contains more than one namespace.";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1403.md";

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
            this.RegisterSyntaxTreeActionHonorExclusions(context, this.HandleSyntaxTree);
        }

        private void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);

            var descentNodes = syntaxRoot.DescendantNodes(descendIntoChildren: node => node != null && !node.IsKind(SyntaxKind.ClassDeclaration));

            bool foundNode = false;

            foreach (var node in descentNodes)
            {
                if (node.IsKind(SyntaxKind.NamespaceDeclaration))
                {
                    if (foundNode)
                    {
                        var location = NamedTypeHelpers.GetNameOrIdentifierLocation(node);
                        if (location != null)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
                        }
                    }
                    else
                    {
                        foundNode = true;
                    }
                }
            }
        }
    }
}
